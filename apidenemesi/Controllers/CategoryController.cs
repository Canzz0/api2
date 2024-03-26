using apidenemesi.Models;
using apidenemesi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace apidenemesi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly CategoryService _categoryService;
        
        public CategoryController(IConfiguration configuration, CategoryService categoryService)
        {
            _configuration = configuration;
            _categoryService = categoryService;
        }
        [HttpGet("api/category")]
        public ActionResult<IEnumerable<Category>> GetAllCategory()
        {
            var categories = _categoryService.GetAllCategories();
            return Ok(categories);
        }

        [HttpPost("api/category/add")]

        public ActionResult<Category> AddCategory(Category category)
        {
            _categoryService.AddCategory(category);
            return Ok("Ekleme Başarılı:"+category);
        }

        [HttpPost("api/category/update")]

        public ActionResult<Category> UpdateCategory(Category category)
        {
            _categoryService.UpdateCategory(category);
            return Ok("Güncelleme Başarılı"+category);
        }

        [HttpPost("api/category/delete")]

        public ActionResult<Category> DeleteCategory(int id)
        {
            _categoryService.DeleteCategory(id);
            return Ok("Silme İşlemi Başarılı");
        }

    }
}
