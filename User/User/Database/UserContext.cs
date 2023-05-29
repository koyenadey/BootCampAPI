using Microsoft.EntityFrameworkCore;
using RegisteredUser.Model;

namespace RegisteredUser.Database;

public class UserContext : DbContext
{
   public UserContext(DbContextOptions<UserContext> options) : base(options) { }
   public DbSet<User> Users { get; set; }
}

    

