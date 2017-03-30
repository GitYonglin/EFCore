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
    }
    public class Blog
    {
        public int BlogId { get; set; }
        public string Url { get; set; }

        //一对多关系
        public List<Post> Posts { get; set; }
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
}
