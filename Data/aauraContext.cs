using Microsoft.EntityFrameworkCore;

namespace aaura.api.Data;
public class aauraContext : DbContext
{
    public DbSet<Mood> Moods { get; set; }
    public DbSet<DailyData> DailyDatas { get; set; }
    public aauraContext(DbContextOptions options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<DailyData>()
            .HasOne(d => d.Mood)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);
    }
}