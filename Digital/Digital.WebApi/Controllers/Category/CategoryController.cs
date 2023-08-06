using Digital.Base.Response;
using Digital.Operation.Services;
using Digital.Operation.Services.Admin;
using Digital.Schema.Dto.Category;
using Digital.Schema.Dto.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Digital.WebApi.Controllers.Category
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        // Kategorileri ve altındaki ürünleri çekmek için kullanılır.
        [HttpGet("GetAll")]
        public ApiResponse<List<CategoryResponse>> GetAll()
        {
            var response = categoryService.GetAll();
            return response;
        }
        // Id değerine göre kategori ve altındaki ürünleri getirir.
        [HttpGet("GetById")]
        public ApiResponse<CategoryResponse> GetById(int id)
        {
            var response = categoryService.GetById(id);
            return response;
        }

        // Kategori oluşturmak için kullanılır.
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ApiResponse AddCategory(CategoryRequest categoryRequest)
        {
            var response = categoryService.Insert(categoryRequest);
            return response;
        }
        // Kategori bilgilerini güncellemek için kullanılır.
        [HttpPut]
        [Authorize(Roles ="Admin")]
        public ApiResponse UpdateCategory(int id, CategoryRequest categoryRequest)
        {
            var response = categoryService.Update(id, categoryRequest);
            return response;
        }
        // Kategori silmek için kullanılır. Altında ürün varsa kategori silinemez.
        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public ApiResponse DeleteCategory(int id)
        {
            var response = categoryService.Delete(id);
            return response;
        }


    }
}
