using DEPI_Graduation_Project.Data;
using ECommerce.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

public class CartController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public CartController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // GET: Cart/UpdateQty
    [HttpPost]
    public async Task<IActionResult> UpdateQty(int productId, int qty)
    {
        var currentUser = await _userManager.GetUserAsync(User);

        if (currentUser == null)
        {
            return Unauthorized();
        }

        var cartItem = await _context.CartItems
            .FirstOrDefaultAsync(c => c.ProductId == productId && c.UserId == currentUser.Id);

        if (cartItem != null && qty > 0)
        {
            cartItem.Quantity = qty;
            await _context.SaveChangesAsync();
        }

        // إرجاع المعلومات المحدثة كـ JSON
        var updatedCartItems = await _context.CartItems
            .Where(c => c.UserId == currentUser.Id)
            .Include(c => c.Product)
            .ToListAsync();

        return Json(updatedCartItems);
    }

    // GET: Cart/Index
    public IActionResult Index()
    {
        var cartItems = _context.CartItems
            .Include(c => c.Product)
            .Include(c => c.User)
            .ToList();

        return View(cartItems);
    }
}