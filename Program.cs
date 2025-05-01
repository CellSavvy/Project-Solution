using DEPI_Graduation_Project.Data;
using ECommerce.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// إضافة DbContext الخاص بـ ApplicationDbContext مع اتصال SQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// إعدادات Identity الخاصة بـ ApplicationUser و IdentityRole
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// إضافة خدمات Controllers مع Views
builder.Services.AddControllersWithViews();

var app = builder.Build();

// التعامل مع بيئة التطوير
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // استخدام الملفات الثابتة (CSS, JavaScript, images)
app.UseRouting();

// إعدادات الـ Authentication و Authorization
app.UseAuthentication();
app.UseAuthorization();

// مسار الـ "Cart" للمستخدم
app.MapControllerRoute(
    name: "user",
    pattern: "Cart/{action=Index}/{id?}",
    defaults: new { controller = "Cart", action = "Index" }
);

// مسار الـ "Dashboard" للـ Admin في المنطقة (Area)
app.MapControllerRoute(
    name: "admin",
    pattern: "Dashboard/{area:exists}/{controller=Admin}/{action=Index}/{id?}",
    defaults: new { area = "Dashboard", controller = "Admin", action = "Index" }
);

// المسار الافتراضي
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

// إضافة مسار خاص بـ Area
app.MapControllerRoute(
    name: "areaRoute",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
);

// إضافة المسارات لـ Login و Register
app.MapControllerRoute(
    name: "login",
    pattern: "Account/Login",
    defaults: new { controller = "Account", action = "Login" }
);

app.MapControllerRoute(
    name: "register",
    pattern: "Account/Register",
    defaults: new { controller = "Account", action = "Register" }
);

app.Run();