using Microsoft.EntityFrameworkCore;

public class Rus_Carmen_Lab2Context(DbContextOptions<Rus_Carmen_Lab2Context> options) : DbContext(options)
{
    public DbSet<Rus_Carmen_Lab2.Models.Book> Book { get; set; } = default!;

    public DbSet<Rus_Carmen_Lab2.Models.Publisher> Publisher { get; set; } = default!;

    public DbSet<Rus_Carmen_Lab2.Models.Authors> Authors { get; set; } = default!;

    public DbSet<Rus_Carmen_Lab2.Models.Category> Category { get; set; } = default!;


}
