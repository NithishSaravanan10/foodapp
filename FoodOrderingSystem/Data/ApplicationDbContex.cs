using Microsoft.EntityFrameworkCore;

namespace FoodOrderingSystem.Models;

public class ApplicationDbContext: DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext>options):base(options)
    {

    }

    public DbSet<OrderHistory> OrderHistory{get; set;}
}