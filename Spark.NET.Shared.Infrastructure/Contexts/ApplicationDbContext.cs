using Microsoft.EntityFrameworkCore;

namespace Spark.NET.Infrastructure.Contexts;

public class ApplicationDbContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)  
    {  
        // optionsBuilder.UseSqlServer(@"data source=serverName; initial catalog=TestDB;persist security info=True;user id=sa");  
    }      
}