using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EF.MySQL.Models;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EF.MySQL.Controllers
{
    public class HomeController : Controller
    {
        // 数据库content
        private Model _context;
        /// <summary>
        /// 构造函数中引入数据上下文
        /// </summary>
        /// <param name="context"></param>
        public HomeController(Model context)
        {
            _context = context;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            using (var context = _context)
            {
                //数据查询
                // 这个BlogCategorys 可以获取到下面的Category实例
                var blogs = context.Blogs
                    .Include(blog => blog.Posts)
                    .Include(c => c.BlogCategorys)
                    .ToList();

                var categorys = context.Categorys.ToList();
                //发送到前台 可以使用 as 转换
                ViewData["categorys"] = categorys;
                return View(blogs);
            }
        }

        [HttpPost]
        public IActionResult Add(Blog blog,List<int> categorys)
        {
            var dcs = new List<BlogCategory>();
            foreach (var item in categorys)
            {
                dcs.Add(new BlogCategory { CategoryId = item });
            }
            blog.BlogCategorys = dcs;
            using (var db = _context)
            {
                db.Blogs.Add(blog);
                db.SaveChanges();
            }
            return RedirectToAction("Index", "Home");
        }

        public IActionResult VueAjax()
        {
            return View();
        }

        [HttpPost]
        public IActionResult VueAjax(Blog blog)
        {
            var bl = Request.Form["blog"].ToList();
            var b = Request.Form["blog"];
            var js = Newtonsoft.Json.JsonConvert.SerializeObject(blog);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult VueForm()
        {
            return View();
        }
        [HttpPost]
        public IActionResult VueForm(Blog blog)
        {
            using (var db = _context)
            {
                db.Blogs.Add(blog);
                db.SaveChanges();
            }
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Category()
        {
            using (var context = _context)
            {
                // 这个BlogCategorys 不可以获取到下面的Blog实例
                var category = context.Categorys
                    .Include(c => c.BlogCategorys)
                    .ToList();

                //之定义查询出 Blog实例
                int i = 0;
                foreach (var item in category)
                {
                    int j = 0;
                    foreach (var b in item.BlogCategorys)
                    {
                        category[i].BlogCategorys[j].Blog = context.Blogs.Single(blog => blog.BlogId == b.BlogId);
                        j++;
                    }
                    i++;
                }
                return View(category);
            }
        }
        [HttpPost]
        public IActionResult Category(Category category)
        {
            using (var db = _context)
            {
                db.Categorys.Add(category);
                db.SaveChanges();
            }
            return RedirectToAction("category", "Home");
        }

    }

}
