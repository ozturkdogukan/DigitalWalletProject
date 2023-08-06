using AutoMapper;
using Digital.Base.Response;
using Digital.Data.Model;
using Digital.Data.UnitOfWork;
using Digital.Operation.Base;
using Digital.Schema.Dto.Category;
using Digital.Schema.Dto.Product;
using Digital.Schema.Dto.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digital.Operation.Services
{

    public class CategoryService : BaseService<Category, CategoryRequest, CategoryResponse>, ICategoryService
    {

        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
        // Bütün kategorileri ve altında bulunan ürünleri getirir.
        public override ApiResponse<List<CategoryResponse>> GetAll()
        {
            var categories = unitOfWork.GetRepository<Category>()
                    .GetAll()
                    .Include(x => x.Products) // Category sınıfındaki "Products" ilişkisini dahil ediyoruz
                    .ThenInclude(pc => pc.Product) // ProductCategoryMap sınıfındaki "Product" ilişkisini dahil ediyoruz
                    .ToList();
            var categoryResponses = mapper.Map<List<CategoryResponse>>(categories);
            return new ApiResponse<List<CategoryResponse>>(categoryResponses);
        }
        // Id 'si verilen kategoriyi ve onun altındaki ürünleri getirir.
        public override ApiResponse<CategoryResponse> GetById(int id)
        {
            var category = unitOfWork.GetRepository<Category>()
                    .GetAll(x => x.Id.Equals(id))
                    .Include(x => x.Products)
                    .ThenInclude(pc => pc.Product)
                    .FirstOrDefault();
            if (category is null)
            {
                return new ApiResponse<CategoryResponse>("Not Found");
            }
            var categoryResponse = mapper.Map<CategoryResponse>(category);
            return new ApiResponse<CategoryResponse>(categoryResponse);
        }

    }
}
