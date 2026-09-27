using CloudCart.Api.DTOs;
using CloudCart.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Threading.Tasks;

namespace CloudCart.Api.Controllers
{
    public class DiscountEventsController : Controller
    {
        private readonly IDiscountEventService _discountService;
        private readonly IProductService _productService;

        public DiscountEventsController(IDiscountEventService discountService, IProductService productService)
        {
            _discountService = discountService;
            _productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            var list = await _discountService.GetActiveDiscountsAsync();
            return View(list);
        }

        public async Task<IActionResult> Details(int id)
        {
            var dto = await _discountService.GetByIdAsync(id);
            if (dto == null) return NotFound();
            return View(dto);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var products = await _productService.GetAllAsync();
            ViewBag.Products = products.Select(p => new SelectListItem(p.Name, p.Id.ToString())).ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateDiscountEventDto dto)
        {
            if (!ModelState.IsValid)
            {
                var products = await _productService.GetAllAsync();
                ViewBag.Products = products.Select(p => new SelectListItem(p.Name, p.Id.ToString())).ToList();
                return View(dto);
            }

            var created = await _discountService.CreateDiscountEventAsync(dto);
            return RedirectToAction(nameof(Details), new { id = created.Id });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var existing = await _discountService.GetByIdAsync(id);
            if (existing == null) return NotFound();

            var products = await _productService.GetAllAsync();
            ViewBag.Products = products.Select(p => new SelectListItem(p.Name, p.Id.ToString())).ToList();

            var dto = new UpdateDiscountEventDto
            {
                Name = existing.Name,
                StartDate = existing.StartDate,
                EndDate = existing.EndDate,
                DiscountType = existing.DiscountType,
                DiscountValue = existing.DiscountValue,
                ProductIds = existing.Products.Select(p => p.Id).ToList()
            };

            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateDiscountEventDto dto)
        {
            if (!ModelState.IsValid)
            {
                var products = await _productService.GetAllAsync();
                ViewBag.Products = products.Select(p => new SelectListItem(p.Name, p.Id.ToString())).ToList();
                return View(dto);
            }

            await _discountService.UpdateDiscountEventAsync(id, dto);
            return RedirectToAction(nameof(Details), new { id });
        }
    }
}
