using Digital.Base.Response;
using Digital.Operation.Services;
using Digital.Schema.Dto.Category;
using Digital.Schema.Dto.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Digital.WebApi.Controllers.Product
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class ProductController : ControllerBase
    {
        private readonly IProductService productService;

        public ProductController(IProductService productService)
        {
            this.productService = productService;
        }
        // Bütün ürünleri çekmek için kullanılır.
        [HttpGet("GetAll")]
        public ApiResponse<List<ProductResponse>> GetAll()
        {
            var response = productService.GetAll();
            return response;
        }
        // isActive alanı true ise aktif olan ürünleri , false ise pasif olan ürünleri getirir.
        [HttpGet("GetAllProduct")]
        public ApiResponse<List<ProductResponse>> GetAllProduct(bool isActive)
        {
            var response = productService.GetAllProduct(isActive);
            return response;
        }
        // Verilen id'ye göre ürünü getirir.
        [HttpGet("GetById")]
        public ApiResponse<ProductResponse> GetById(int id)
        {
            var response = productService.GetById(id);
            return response;
        }
        // Ürün eklemek için kullanılır.
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ApiResponse AddProduct(ProductRequest categoryRequest)
        {
            var response = productService.Insert(categoryRequest);
            return response;
        }
        // Ürün güncelleme işlemi için kullanılır.
        [HttpPut("UpdateProduct")]
        [Authorize(Roles = "Admin")]
        public ApiResponse UpdateProduct(int id, ProductRequest categoryRequest)
        {
            var response = productService.Update(id, categoryRequest);
            return response;
        }
        // Stok güncellemek için kullanılır.
        [HttpPut("StockUpdate")]
        [Authorize(Roles = "Admin")]
        public ApiResponse StockUpdate(int id, int stock)
        {
            var response = productService.StockUpdate(id, stock);
            return response;
        }
        // Ürün silmek için kullanılır.
        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public ApiResponse DeleteProduct(int id)
        {
            var response = productService.Delete(id);
            return response;
        }

    }
}
