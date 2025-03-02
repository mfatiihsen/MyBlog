using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using AdminBlog.Models;

namespace AdminBlog.Controllers;

public class BlogController : Controller
{
    private readonly ILogger<BlogController> _logger;
    private readonly BlogContext _context; // Veri tabanı bağlantısı

    // consructor işlemi
    public BlogController(ILogger<BlogController> logger, BlogContext context)
    {
        _logger = logger;
        _context = context;
    }

    // blogların listeleneceği sayfa
    public IActionResult Index()
    {
        var list = _context.Blog.ToList();
        return View(list);
    }

    // blogların yayınlanacağı sayfa
    public IActionResult Publish(int Id)
    {
        var blog = _context.Blog.Find(Id);
        blog.IsPublish = true;
        _context.Update(blog);
        _context.SaveChanges();
        return RedirectToAction(nameof(Index));
    }

    // Blog Silme İşlemi
    public  IActionResult Delete(int Id)
    {
        Blog blog = _context.Blog.Find(Id);
        _context.Remove(blog);
        _context.SaveChanges();
        return RedirectToAction(nameof(Index));
    }

    // Blogları listeleme işi
    public IActionResult Blog()
    {
        ViewBag.Categories = _context.Category.Select(w =>
               new SelectListItem
               {
                   Text = w.Name,
                   Value = w.Id.ToString(),
               }).ToList();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Save(Blog model)
    {
        if (model != null)
        {
            var file = Request.Form.Files.First();
            //C:\Users\Ademf\Desktop\myBlog\MyBlog\myBlog\wwwroot\img
            string savePath = Path.Combine("C:", "Users", "Ademf", "Desktop", "myBlog", "MyBlog", "myBlog", "wwwroot", "img");
            var fileName = $"{DateTime.Now:MMddHHmmss}.{file.FileName.Split(".").Last()}";
            var fileUrl = Path.Combine(savePath, fileName);
            using (var fileStream = new FileStream(fileUrl, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
            model.ImagePath = fileName;
            model.AuthorId = (int)HttpContext.Session.GetInt32("id");
            await _context.AddAsync(model);
            await _context.SaveChangesAsync();
            return Json(true);

        }

        return Json(false);
    }

    // Blog Ekleme İşlemi
    // [HttpPost]
    // public async Task<IActionResult> Save(Blog model)
    // {
    //     try
    //     {
    //         if (model == null)
    //         {
    //             return Json(new { success = false, message = "Model boş geliyor!" });
    //         }

    //         if (string.IsNullOrEmpty(model.Title) || string.IsNullOrEmpty(model.Content))
    //         {
    //             return Json(new { success = false, message = "Başlık veya içerik boş!" });
    //         }

    //         // Dosya kontrolü
    //         var file = Request.Form.Files.FirstOrDefault();
    //         if (file == null || file.Length == 0)
    //         {
    //             return Json(new { success = false, message = "Resim yüklenmedi!" });
    //         }

    //         // Dosya kaydetme işlemi
    //         //C:\Users\Ademf\Desktop\myBlog\MyBlog\myBlog\wwwroot\img
    //         string savePath = Path.Combine("C:", "Users", "Ademf", "Desktop", "myBlog", "MyBlog", "myBlog","wwwroot","img");
    //         var fileName = $"{DateTime.Now:MMddHHmmss}.{file.FileName.Split('.').Last()}";
    //         var fileUrl = Path.Combine(savePath, fileName);
    //         using (var fileStream = new FileStream(fileUrl, FileMode.Create))
    //         {
    //             await file.CopyToAsync(fileStream);
    //         }
    //         model.ImagePath = fileName;

    //         // Author bilgisini oturumdan al
    //         var authorId = HttpContext.Session.GetInt32("id");
    //         if (authorId == null)
    //         {
    //             return Json(new { success = false, message = "Oturumdan yazar bilgisi alınamadı!" });
    //         }
    //         model.AuthorId = (int)authorId;

    //         // Blogu kaydet
    //         await _context.Blog.AddAsync(model);
    //         await _context.SaveChangesAsync();

    //         return Json(new { success = true, message = "Blog başarıyla kaydedildi!" });
    //     }
    //     catch (Exception ex)
    //     {
    //         // Hata mesajını döndür
    //         return Json(new { success = false, message = "Hata: " + ex.Message });
    //     }
    // }





    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
