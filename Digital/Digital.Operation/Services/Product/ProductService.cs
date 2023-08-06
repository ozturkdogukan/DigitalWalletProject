using AutoMapper;
using Digital.Base.Response;
using Digital.Data.Model;
using Digital.Data.UnitOfWork;
using Digital.Operation.Base;
using Digital.Schema.Dto.Category;
using Digital.Schema.Dto.Product;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digital.Operation.Services
{
    public class ProductService : BaseService<Product, ProductRequest, ProductResponse>, IProductService
    {

        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        public ProductService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }


        public override ApiResponse Insert(ProductRequest request)
        {
            // Product nesnesini oluştur
            var product = mapper.Map<Product>(request);

            // İlişkili kategorilerin ID'lerini al
            var categoryIds = request.CategoryIds;

            // Kategorileri veritabanından al
            var categories = unitOfWork.GetRepository<Category>().GetAll(x => categoryIds.Contains(x.Id));


            // Kategorileri ürüne ekle
            foreach (var category in categories)
            {
                product.Categories.Add(new ProductCategoryMap
                {
                    Product = product,
                    Category = category
                });
            }
            // Ürünü veritabanına ekle
            unitOfWork.GetRepository<Product>().Add(product);
            if (unitOfWork.SaveChanges() > 0)
            {
                return new ApiResponse();

            }
            return new ApiResponse("Internal Server Error");

        }

        public override ApiResponse Update(int id, ProductRequest request)
        {
            var productToUpdate = unitOfWork.GetRepository<Product>().GetAll(x => x.Id.Equals(id))?.Include(x => x.Categories)?.FirstOrDefault();
            if (productToUpdate == null)
            {
                return new ApiResponse("Product not found");
            }

            // AutoMapper kullanarak güncelleme işlemini gerçekleştir
            mapper.Map(request, productToUpdate);

            // İlişkili kategorileri güncelle
            var updatedCategoryIds = request.CategoryIds;
            var existingCategoryIds = productToUpdate.Categories.Select(pc => pc.CategoryId).ToList();

            // Eklenecek kategorileri bul
            var categoriesToAdd = updatedCategoryIds.Except(existingCategoryIds).ToList();

            // Çıkarılacak kategorileri bul
            var categoriesToRemove = existingCategoryIds.Except(updatedCategoryIds).ToList();
            // Eklenecek kategorileri ürüne ekle
            foreach (var categoryId in categoriesToAdd)
            {
                productToUpdate.Categories.Add(new ProductCategoryMap
                {
                    CategoryId = categoryId
                });
            }

            // Çıkarılacak kategorileri üründen çıkar
            foreach (var categoryId in categoriesToRemove)
            {
                var productCategoryMapToRemove = productToUpdate.Categories.FirstOrDefault(pc => pc.CategoryId == categoryId);
                if (productCategoryMapToRemove != null)
                {
                    productToUpdate.Categories.Remove(productCategoryMapToRemove);
                }
            }
            // İlişkide olmayan kategorileri üründen sil
            var categoriesToRemoveCompletely = unitOfWork.GetRepository<ProductCategoryMap>().GetAll(pc => pc.ProductId == id && !updatedCategoryIds.Contains(pc.CategoryId)).ToList();
            foreach (var categoryToRemoveCompletely in categoriesToRemoveCompletely)
            {
                unitOfWork.GetRepository<ProductCategoryMap>().Delete(categoryToRemoveCompletely);
            }

            unitOfWork.GetRepository<Product>().Update(productToUpdate);
            if (unitOfWork.SaveChanges() > 0)
            {
                return new ApiResponse();
            }
            return new ApiResponse("Internal Server Error");

        }
        // Aktif veya pasif olan ürünleri getirir.
        public ApiResponse<List<ProductResponse>> GetAllProduct(bool isActive)
        {
            var products = unitOfWork.GetRepository<Product>().GetAll(x => x.IsActive.Equals(isActive)).Include(x => x.Categories).ThenInclude(pc => pc.Category).ToList();
            var productsResponse = mapper.Map<List<ProductResponse>>(products);
            return new ApiResponse<List<ProductResponse>>(productsResponse);
        }

        // Bütün ürünleri getirir.
        public override ApiResponse<List<ProductResponse>> GetAll()
        {
            var products = unitOfWork.GetRepository<Product>().GetAll().Include(x => x.Categories).ThenInclude(pc => pc.Category).ToList();
            var productsResponse = mapper.Map<List<ProductResponse>>(products);
            return new ApiResponse<List<ProductResponse>>(productsResponse);
        }

        // id'ye göre ürün getirir.
        public override ApiResponse<ProductResponse> GetById(int id)
        {
            var product = unitOfWork.GetRepository<Product>().GetAll(x => x.Id.Equals(id))?.Include(x => x.Categories).ThenInclude(pc => pc.Category)?.FirstOrDefault();
            if (product is null)
            {
                return new ApiResponse<ProductResponse>("Not Found");
            }
            var productResponse = mapper.Map<ProductResponse>(product);
            return new ApiResponse<ProductResponse>(productResponse);
        }

        // Ürün Siler.
        public override ApiResponse Delete(int id)
        {
            var productToDelete = unitOfWork.GetRepository<Product>().GetById(id);
            if (productToDelete == null)
            {
                return new ApiResponse("Product not found");
            }

            // İlişkili kategorileri kaldır
            var productCategoriesToRemove = unitOfWork.GetRepository<ProductCategoryMap>().GetAll(pc => pc.ProductId == id);
            foreach (var productCategoryMap in productCategoriesToRemove)
            {
                unitOfWork.GetRepository<ProductCategoryMap>().Delete(productCategoryMap);
            }

            // Ürünü sil
            unitOfWork.GetRepository<Product>().Delete(productToDelete);

            if (unitOfWork.SaveChanges() > 0)
            {
                return new ApiResponse();
            }
            return new ApiResponse("Internal Server Error");
        }

        // Stok günceller.
        public ApiResponse StockUpdate(int id, int stock)
        {
            var product = unitOfWork.GetRepository<Product>().Get(x => x.Id.Equals(id));
            if (product is null)
            {
                return new ApiResponse("Product Not Found.");
            }
            product.Stock = stock;
            unitOfWork.GetRepository<Product>().Update(product);
            if (unitOfWork.SaveChanges() > 0)
            {
                return new ApiResponse();
            }
            return new ApiResponse("Internal Server Error");
        }

    }
}
