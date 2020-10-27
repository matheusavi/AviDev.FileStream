using Microsoft.EntityFrameworkCore;

namespace AviDev.FileStream
{
    public class AviDevFileStreamContext : DbContext
    {
        public AviDevFileStreamContext(DbContextOptions<AviDevFileStreamContext> options)
            : base(options)
        {
        }

        public DbSet<File> File { get; set; }
    }
}
