using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ECommerce.Models;
using DEPI_Graduation_Project.Models;

public class AccountController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    // GET: Login
    [HttpGet]
    public IActionResult Login()
    {
        return View(new LoginViewModel());
    }

    // POST: Login
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
    if (ModelState.IsValid)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user != null)
        {
            var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, model.RememberMe, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                TempData["Alert"] = "success";
                return RedirectToAction("Index", "Home");
            }
        }
        TempData["Alert"] = "failure";
        ModelState.AddModelError(string.Empty, "Invalid email or password.");
        }
        return View(model);
    }

    // GET: Register
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    // POST: Register
    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            // التحقق من إذا كان البريد الإلكتروني موجود بالفعل في النظام
            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError(string.Empty, "This email is already in use.");
                return View(model);
            }

            var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                TempData["Alert"] = "success"; // استخدام TempData لتمرير إشعار إلى الصفحة التالية
                return RedirectToAction("Index", "Home");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
        return View(model);
    }

    // POST: Logout
    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        TempData["Alert"] = "logout";
        return RedirectToAction("Index", "Home");
    }
}
