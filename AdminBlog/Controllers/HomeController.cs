using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AdminBlog.Models;

namespace AdminBlog.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly BlogContext _context; // Veri tabanı bağlantısı

    public HomeController(ILogger<HomeController> logger, BlogContext context)
    {
        _logger = logger;
        _context = context;
    }

    // Yeni kategori Eklemek İşlemi
    public async Task<IActionResult> addCategory(Category category)
    {
        await _context.AddAsync(category);  // kategoriyi context'e ekle
        await _context.SaveChangesAsync(); // verileri veri tabanına kaydediyoruz.

        return RedirectToAction(nameof(Category));
    }

    // Kategori Silme İşlemleri
    public async Task<IActionResult> DeleteCategory(int? id)
    {
        Category category = await _context.Category.FindAsync(id); // id'ye sahip kategoriyi buluyoruz
        _context.Remove(category);
        await _context.SaveChangesAsync(); // verileri kaydediyoruz.
        return RedirectToAction(nameof(Category)); // silme işlemi başarıyle gerçekleşirse gidilecek sayfa.
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Category()
    {
        List<Category> list = _context.Category.ToList(); // tüm kategorileri listeye çekiyoruz 
        return View(list);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
