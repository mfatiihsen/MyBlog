using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using myBlog.Models;

namespace myBlog.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly BlogContext _context;

    // Consructur
    public HomeController(ILogger<HomeController> logger, BlogContext context)
    {
        _logger = logger;
        _context = context;
    }

    // Ana Sayfanın açılması
    public IActionResult Index()
    {
        var list = _context.Blog.Take(4).OrderByDescending(x => x.CreateTime).ToList();
        foreach (var blog in list)
        {
            blog.Author = _context.Author.Find(blog.AuthorId);
        }
        return View(list);
    }

    public IActionResult Privacy()
    {
        return View();
    }


    // bloglara tıklandığnda yeni sayfanın açılması
    public IActionResult Post(int Id)
    {
        var blog = _context.Blog.Find(Id);
        blog.Author = _context.Author.Find(blog.AuthorId);
        blog.ImagePath = "/img/" + blog.ImagePath;
        return View(blog);
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
