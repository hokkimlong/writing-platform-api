using Microsoft.EntityFrameworkCore;
using AuthenticationWebApi.Models;
namespace AuthenticationWebApi.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<User> Users => Set<User>();

        public DbSet<AuthenticationWebApi.Models.Article>? Article { get; set; }

        public DbSet<AuthenticationWebApi.Models.Tag>? Tag { get; set; }

        public DbSet<AuthenticationWebApi.Models.Comment>? Comment { get; set; }
    }
}
