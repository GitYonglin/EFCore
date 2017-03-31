using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
namespace EF.MySQL.Models
{
    /*
     * 数据库迁移命令
     * PM> Add-Migration MyFirstMigration 生成上下文
     * To undo this action, use Remove-Migration.
     * PM> UpDate-database 更新数据库
     * Done.
     */
    public class Model : DbContext
    {
        //引用数据库链接
        public Model(DbContextOptions<Model> options)
            : base(options)
        { }

        // 生成数据库类设置
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Category> Categorys { get; set; }
        /// <summary>
        /// 第三张表设置
        /// </summary>
        /// <param name="ModelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder ModelBuilder)
        {
            ModelBuilder.Entity<BlogCategory>()
                .HasKey(b => new { b.BlogId, b.CategoryId });

            ModelBuilder.Entity<BlogCategory>()
                .HasOne(bc => bc.Blog)
                .WithMany(b => b.BlogCategorys)
                .HasForeignKey(bc => bc.BlogId);

            ModelBuilder.Entity<BlogCategory>()
                .HasOne(bc => bc.Category)
                .WithMany(b => b.BlogCategorys)
                .HasForeignKey(bc => bc.CategoryId);
        }
    }
    public class Blog
    {
        public int BlogId { get; set; }
        public string Url { get; set; }

        //一对多关系
        public List<Post> Posts { get; set; }
        // 多对多第三张表依赖
        public List<BlogCategory> BlogCategorys { get; set; }
    }

    public class Post
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        //一对多关系
        public int BlogId { get; set; }
        public Blog Blog { get; set; }
    }
    /// <summary>
    /// 分类
    /// </summary>
    public class Category
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }


        public List<BlogCategory> BlogCategorys { get; set; }
    }
    /// <summary>
    /// 第三张表
    /// </summary>
    public class BlogCategory
    {
        public int BlogId { get; set; }
        public Blog Blog { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
