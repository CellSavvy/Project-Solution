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
        var updatedCartItems = await _context.CartItems
            .Where(c => c.UserId == currentUser.Id)
            .Include(c => c.Product)
            .ToListAsync();

        return Json(updatedCartItems);
    }
    [HttpPost]
    public async Task<IActionResult> AddToCart(int productId, int quantity)
    {
        // Get current user
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            TempData["Error"] = "Please log in to add items to cart.";
            return RedirectToAction("Login", "Account");
        }

        // Check if product exists and has enough stock
        var product = _context.Products.Find(productId);
        if (product == null || product.StockQuantity < quantity)
        {
            TempData["Error"] = "Product is out of stock or insufficient quantity.";
            return RedirectToAction("Index", "Product");
        }

        // Check if product is already in cart
        var cartItem = _context.CartItems
            .FirstOrDefault(c => c.ProductId == productId && c.UserId == user.Id);

        if (cartItem != null)
        {
            // Update quantity if item exists
            cartItem.Quantity += quantity;
        }
        else
        {
            // Add new cart item
            cartItem = new CartItem
            {
                UserId = user.Id,
                ProductId = productId,
                Quantity = quantity
            };
            _context.CartItems.Add(cartItem);
        }

        // Save changes
        await _context.SaveChangesAsync();

        TempData["Success"] = "Product added to cart successfully.";
        return RedirectToAction("Cart");
    }

    public IActionResult Cart()
    {
        var userId = _userManager.GetUserId(User);
        var cartItems = _context.CartItems
            .Where(c => c.UserId == userId)
            .ToList();
        return View(cartItems);
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