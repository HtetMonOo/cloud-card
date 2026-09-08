using CloudCart.Api.DTOs;
using CloudCart.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Threading.Tasks;

namespace CloudCart.Api.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IProductService _productService;

        public OrdersController(IOrderService orderService, IProductService productService)
        {
            _orderService = orderService;
            _productService = productService;
        }

        // GET /Orders/Create
        public async Task<IActionResult> Create()
        {
            var products = await _productService.GetAllAsync();
            ViewBag.Products = products.Select(p => new SelectListItem(p.Name, p.Id.ToString())).ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateOrderDto dto)
        {
            if (dto == null || dto.Items == null || !dto.Items.Any())
            {
                ModelState.AddModelError("Items", "At least one order item is required.");
            }

            if (!ModelState.IsValid)
            {
                var products = await _productService.GetAllAsync();
                ViewBag.Products = products.Select(p => new SelectListItem(p.Name, p.Id.ToString())).ToList();
                return View(dto);
            }

            var created = await _orderService.CreateOrderAsync(dto);
            return RedirectToAction(nameof(Details), new { id = created.Id });
        }

        public async Task<IActionResult> Details(int id)
        {
            var order = await _orderService.GetByIdAsync(id);
            if (order == null) return NotFound();
            return View(order);
        }

        // GET /Orders/UserOrders?userId=1
        public async Task<IActionResult> UserOrders(int userId)
        {
            var list = await _orderService.GetOrdersForUserAsync(userId);
            ViewBag.UserId = userId;
            return View(list);
        }
    }
}
