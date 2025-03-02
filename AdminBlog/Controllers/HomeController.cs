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

    public IActionResult Index()
    {
        return View();
    }


    // Giriş Yapma İşlemleri
    public IActionResult Login(string Email, string Password)
    {
        // burada formdan gelen bilgilerin doğruluğunu kontrol ediyoruz.
        var author = _context.Author.FirstOrDefault(w => w.Email == Email && w.Password == Password);
        if (author == null)
        {
            return RedirectToAction(nameof(Index));
        }
        else
        {
            HttpContext.Session.SetInt32("id", author.Id);
            return RedirectToAction(nameof(Category));
        }
    }


    public IActionResult Author()
    {
        List<Author> list = _context.Author.ToList(); // tüm kategorileri listeye çekiyoruz 
        return View(list);
    }


    public async Task<IActionResult> AddAuthor(Author author)
    {
        if (author.Id == 0)
        {
            await _context.AddAsync(author);
        }
        else
        {
            _context.Update(author);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Author));
    }

    public async Task<IActionResult> AuthorDetails(int Id)
    {
        var author = await _context.Author.FindAsync(Id);
        return Json(author);
    }
    public async Task<IActionResult> DeleteAuthor(int? Id)
    {
        Author author = await _context.Author.FindAsync(Id);
        _context.Remove(author);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Author));
    }

    public IActionResult Category()
    {
        List<Category> list = _context.Category.ToList(); // tüm kategorileri listeye çekiyoruz 
        return View(list);
    }

    // Yeni kategori Eklemek İşlemi
    public async Task<IActionResult> AddCategory(Category category)
    {
        // Eğer veri tabanında Id değerine sahip degilse ekleme işlemi  yap ama 0 yoksa güncelleme işlemi yap.
        if (category.Id == 0)
        {
            await _context.AddAsync(category);  // kategoriyi context'e ekle
        }
        else
        {
            _context.Update(category);
        }
        await _context.SaveChangesAsync(); // verileri veri tabanına kaydediyoruz.
        return RedirectToAction(nameof(Category)); // başarılı ise yönlendirelecek sayfa 
    }

    // Kategori Silme İşlemleri
    public async Task<IActionResult> DeleteCategory(int? id)
    {
        Category category = await _context.Category.FindAsync(id); // id'ye sahip kategoriyi buluyoruz
        _context.Remove(category);
        await _context.SaveChangesAsync(); // verileri kaydediyoruz.
        return RedirectToAction(nameof(Category)); // silme işlemi başarıyle gerçekleşirse gidilecek sayfa.
    }


    //Kategori Detaylarını Id numarasına göre getirme  İşlemleri
    public async Task<IActionResult> CategoryDetails(int Id)
    {
        var category = await _context.Category.FindAsync(Id);
        return Json(category);
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
