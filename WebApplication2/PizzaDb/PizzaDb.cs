using Microsoft.EntityFrameworkCore;
using MinimalApiEntityFramework.Models;
using MinimalApiEntityFramework.PizzaDb;
namespace MinimalApiEntityFramework.PizzaDb
{
     public class PizzaDb : DbContext
    {
        public PizzaDb(DbContextOptions options) : base(options) { }
        public DbSet<PizzaModel> Pizzas { get; set; }
    }
}
