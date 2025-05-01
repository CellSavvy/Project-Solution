using ECommerce.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DEPI_Graduation_Project.Areas.Dashboard.Controllers
{
    [Area("Dashboard")]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // عرض قائمة العملاء
        public async Task<IActionResult> Index()
        {
            var users = _userManager.Users; // جلب جميع المستخدمين
            return View(await users.ToListAsync());
        }

        // عرض صفحة إضافة مستخدم جديد
        public IActionResult Create()
        {
            return View();
        }

        // إضافة مستخدم جديد
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ApplicationUser model)
        {
            if (ModelState.IsValid)
            {
                // تعيين الـ Role للمستخدم
                var user = new ApplicationUser
                {
                    UserName = model.UserName,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    // تعيين الـ Role هنا
                    Role = model.Role ?? "Client" // أو تعيين قيمة افتراضية "Client" إذا لم يتم تحديد Role
                };

                var result = await _userManager.CreateAsync(user, "DefaultPassword123!"); // كلمة مرور افتراضية
                if (result.Succeeded)
                {
                    // إضافة الـ Role بعد إنشاء المستخدم
                    await _userManager.AddToRoleAsync(user, model.Role ?? "Client"); // أو تعيين القيمة الافتراضية هنا
                    return RedirectToAction(nameof(Index));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }

        // عرض صفحة تعديل مستخدم
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // تعديل مستخدم
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, ApplicationUser model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user == null)
                {
                    return NotFound();
                }

                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Email = model.Email;

                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    // التحقق من وجود Role في قاعدة البيانات
                    if (!await _roleManager.RoleExistsAsync(model.Role))
                    {
                        ModelState.AddModelError("Role", "Invalid role.");
                        return View(model);
                    }

                    // تحديث الـ Role هنا
                    var currentRoles = await _userManager.GetRolesAsync(user);
                    await _userManager.RemoveFromRolesAsync(user, currentRoles);
                    await _userManager.AddToRoleAsync(user, model.Role);
                    return RedirectToAction(nameof(Index));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }

        // حذف مستخدم
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Index));
            }

            return View();
        }
    }
}