using Microsoft.EntityFrameworkCore;

namespace HDataSetApi.Models{

    public class HDataSetContext : DbContext{
        public HDataSetContext(DbContextOptions<HDataSetContext> options)
            :base(options){

            }
        
        public DbSet<Vegetable> Vegetables {get; set;}
        
    }
}