using CloudCart.Api.DTOs;
using CloudCart.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CloudCart.Api.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            var list = await _productService.GetAllAsync();
            return View(list);
        }

        public async Task<IActionResult> Details(int id)
        {
            var p = await _productService.GetByIdAsync(id);
            if (p == null) return NotFound();
            return View(p);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateProductDto dto)
        {
            if (!ModelState.IsValid) return View(dto);
            var created = await _productService.CreateAsync(dto);
            return RedirectToAction(nameof(Details), new { id = created.Id });
        }
    }
}
