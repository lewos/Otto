using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Otto.products.Services;

namespace Otto.products.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ProductsService _productsService;

        public ProductsController(ProductsService productsService)
        {
            _productsService = productsService;
        }

        [HttpGet("GetProductsByMUserId/{id}", Name = "GetProductsByMUserId")]
        public async Task<IActionResult> GetProductsByMUserId(string id)
        {
            var result = await _productsService.GetProductsByMUserId(id);
            return result != null ? (IActionResult)Ok(result) : NotFound();
        }

        //[HttpGet("GetProductsInStockByMUserId/{id}", Name = "GetProductsInStockByMUserId")]
        //public async Task<IActionResult> GetProductsInStockByMUserId(string id)
        //{
        //    var result = await _productsService.GetProductsInStockByMUserId(id);
        //    return result != null ? (IActionResult)Ok(result) : NotFound();
        //}
    }
}
