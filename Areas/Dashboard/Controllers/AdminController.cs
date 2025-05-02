using ECommerce.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

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
            var users = await _userManager.Users.ToListAsync();
            return View(users);
        }

        // عرض صفحة إضافة مستخدم جديد
        public IActionResult Create()
        {
            ViewData["Roles"] = new SelectList(_roleManager.Roles, "Name", "Name");
            return View();
        }

        // إضافة مستخدم جديد
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ApplicationUser model)
        {
            if (ModelState.IsValid)
            {
                // التحقق من وجود الـ Role
                var role = model.Role ?? "Client";
                var roleExists = await _roleManager.RoleExistsAsync(role);
                if (!roleExists)
                {
                    await _roleManager.CreateAsync(new IdentityRole(role));
                }

                var user = new ApplicationUser
                {
                    UserName = model.UserName,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email
                };

                var result = await _userManager.CreateAsync(user, "DefaultPassword123!");
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, role);
                    return RedirectToAction(nameof(Index));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            ViewData["Roles"] = new SelectList(_roleManager.Roles, "Name", "Name", model.Role);
            return View(model);
        }

        // عرض صفحة تعديل مستخدم
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            // جلب الـ Role الحالي للمستخدم
            var userRoles = await _userManager.GetRolesAsync(user);
            var currentRole = userRoles.FirstOrDefault() ?? "Client";

            // تمرير قائمة الأدوار وتحديد الـ Role الحالي
            ViewData["Roles"] = new SelectList(_roleManager.Roles, "Name", "Name", currentRole);
            ViewData["CurrentRole"] = currentRole;

            return View(user);
        }

        // تعديل مستخدم
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, ApplicationUser model, string Role)
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

                user.UserName = model.UserName;
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Email = model.Email;

                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    // تحديث الـ Role
                    var currentRoles = await _userManager.GetRolesAsync(user);
                    await _userManager.RemoveFromRolesAsync(user, currentRoles);
                    if (!string.IsNullOrEmpty(Role) && await _roleManager.RoleExistsAsync(Role))
                    {
                        await _userManager.AddToRoleAsync(user, Role);
                    }
                    else
                    {
                        await _userManager.AddToRoleAsync(user, "Client"); // Default role
                    }
                    return RedirectToAction(nameof(Index));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // إعادة تعيين قائمة الأدوار في حالة الفشل
            ViewData["Roles"] = new SelectList(_roleManager.Roles, "Name", "Name", Role);
            ViewData["CurrentRole"] = Role;
            return View(model);
        }

        // حذف مستخدم
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

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

            return View(user);
        }
    }
}