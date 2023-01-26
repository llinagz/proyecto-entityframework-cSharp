using fundamentosEF.Models;
using Microsoft.EntityFrameworkCore;

namespace fundamentosEF;

public class TareasContext: DbContext{
    public DbSet<Categoria> Categorias {get;set;}
    public DbSet<Tarea> Tareas {get;set;}
    public TareasContext(DbContextOptions<TareasContext> options) :base(options) { }

    //FLUENT API
    protected override void OnModelCreating(ModelBuilder modelBuilder){

        //Con esta coleccion podremos agregar datos iniciales a la tabla Categoria
        List<Categoria> categoriasInit = new List<Categoria>();
        categoriasInit.Add(new Categoria() {CategoriaId = Guid.Parse("22b8f1a9-cb79-456c-8f79-b07153d2581d"), Nombre = "Actividades pendientes", Peso = 20});
        categoriasInit.Add(new Categoria() {CategoriaId = Guid.Parse("22b8f1a9-cb79-456c-8f79-b07153d25802"), Nombre = "Actividades personales", Peso = 50});

        //Hacemos las restricciones necesarias en Categoria
        modelBuilder.Entity<Categoria>(categoria =>{
           //Creamos la tabla
           categoria.ToTable("Categoria"); 
           //Creamos la clave
           categoria.HasKey(p => p.CategoriaId);
           categoria.Property(p => p.Nombre).IsRequired().HasMaxLength(150);
           categoria.Property(p => p.Descripcion).IsRequired(false);
           categoria.Property(p => p.Peso);
           //Agregamos la configuracion inicial de categorias
           categoria.HasData(categoriasInit);

        });


        List<Tarea> tareasInit = new List<Tarea>();
        //Debemos tener exactamente el mismo CategoriaId que hicimos en Categorias, así esa tarea quedara asociada a esa categoria
        tareasInit.Add(new Tarea() {TareaId = Guid.Parse("22b8f1a9-cb79-456c-8f79-b07153d25810"), CategoriaId = Guid.Parse("22b8f1a9-cb79-456c-8f79-b07153d2581d"), PrioridadTarea = Prioridad.Media, Titulo = "Pago de servicios publicos", FechaCreacion = DateTime.Now});
        tareasInit.Add(new Tarea() {TareaId = Guid.Parse("22b8f1a9-cb79-456c-8f79-b07153d25811"), CategoriaId = Guid.Parse("22b8f1a9-cb79-456c-8f79-b07153d25802"), PrioridadTarea = Prioridad.Baja, Titulo = "Terminar de ver pelicula en Netflix", FechaCreacion = DateTime.Now});


        modelBuilder.Entity<Tarea>(tarea =>{
            tarea.ToTable("Tarea");
            tarea.HasKey(p => p.TareaId);
            tarea.HasOne(p => p.Categoria).WithMany(p => p.Tareas).HasForeignKey(p => p.CategoriaId);
            tarea.Property(p => p.Titulo).IsRequired().HasMaxLength(200);
            tarea.Property(p => p.Descripcion).IsRequired(false);
            tarea.Property(p => p.PrioridadTarea);
            tarea.Property(p => p.FechaCreacion);
            tarea.Ignore(p => p.Resumen);
            //Agregamos la configuracion inicial de categorias
            tarea.HasData(tareasInit);
        });
    }
}