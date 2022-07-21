using Microsoft.EntityFrameworkCore;
using WebAPIAutores.Models;

namespace WebAPIAutores;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        
    }

    public DbSet<Autor> Autores { get; set; }
    
    public DbSet<Libro> Libros { get; set; }

    public DbSet<Comentario> Comentarios { get; set; }

    public DbSet<AutorLibro> AutoresLibros { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("Name=db");
            
        }        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<AutorLibro>().HasKey(al => new { al.AutorId, al.LibroId });
    }
}