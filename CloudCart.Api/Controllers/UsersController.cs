using CloudCart.Api.DTOs;
using CloudCart.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CloudCart.Api.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            var list = await _userService.GetAllAsync();
            return View(list);
        }

        public async Task<IActionResult> Details(int id)
        {
            var u = await _userService.GetByIdAsync(id);
            if (u == null) return NotFound();
            return View(u);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateUserDto dto)
        {
            if (!ModelState.IsValid) return View(dto);
            var created = await _userService.CreateAsync(dto);
            return RedirectToAction(nameof(Details), new { id = created.Id });
        }
    }
}
