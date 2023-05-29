using Microsoft.EntityFrameworkCore;
using RegisteredUser.Model;

namespace RegisteredUser.Database
{
    public class TaskContext : DbContext
    {
        public TaskContext(DbContextOptions<TaskContext> options):base(options) { }

        public DbSet<tblTask> Tasks { get; set;}
    }
}
