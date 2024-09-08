using NewPortalWebAPI.Model.Entity;
using Microsoft.EntityFrameworkCore;
namespace NewPortalWebAPI.Data;
    public class NewsContext : DbContext
    {
    public NewsContext() { }
    public NewsContext(DbContextOptions<NewsContext> options) : base(options) {}
            public DbSet<NewsInfo> NewsInfos => Set<NewsInfo>();
            public DbSet<Category> Categories => Set<Category>();           
        
    }

