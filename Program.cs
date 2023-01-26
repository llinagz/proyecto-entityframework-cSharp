using fundamentosEF;
using fundamentosEF.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

//Para que no salten errores con los DateTime a la hora de hacer Migrations en PostgreSQL
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddDbContext<TareasContext>(p => p.UseInMemoryDatabase("TareasDB"));
//builder.Services.AddNpgsql<TareasContext>(builder.Configuration.GetConnectionString("TareasDB"));

var connectionString = builder.Configuration.GetConnectionString("TareasDB");
builder.Services.AddDbContext<TareasContext>(p => p.UseNpgsql(connectionString));

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

//Con este metodo creamos la BBDD
app.MapGet("/dbconexion", async ([FromServices] TareasContext dbContext) => {

    dbContext.Database.EnsureCreated(); //Nos asegura que la BBDD este creada y lyuego nos devuelve si esta creada o no
    return Results.Ok("Base de datos en memoria: " + dbContext.Database.IsInMemory());

});

//Nuevo endpoint para poder OBTENER los datos
app.MapGet("/api/tareas", async ([FromServices] TareasContext dbContext) => {
    
    //Esto representa la coleccion de datos que existe en la BBDD, se traeria todo sin filtrar
    return Results.Ok(dbContext.Tareas.Include(p=> p.Categoria));

});

//Nuevo endpoint para poder INGRESAR datos
//Accederemos al objeto Tarea, con FromBody indicamos que desde el cuerpo del Request nos va a llegar el objeto Tarea
app.MapPost("/api/tareas", async ([FromServices] TareasContext dbContext, [FromBody] Tarea tarea) => {
    
    //Accederemos al objeto Tarea y establecemos unos parametros por defecto
    tarea.TareaId = Guid.NewGuid();
    tarea.FechaCreacion = DateTime.Now;
    await dbContext.AddAsync(tarea);
    //await dbContext.Tareas.AddAsync(tarea);

    //Invocar el metodo SaveChanges
    await dbContext.SaveChangesAsync();

    return Results.Ok();

});

//ACTUALIZAR datos
//Con FromRoute obtenemos el atributo Guid desde la URL
app.MapPut("/api/tareas/{id}", async ([FromServices] TareasContext dbContext, [FromBody] Tarea tarea, [FromRoute] Guid id) => {
    
    var tareaActual = dbContext.Tareas.Find(id);

    if(tareaActual != null){
        tareaActual.CategoriaId = tarea.CategoriaId;
        tareaActual.Titulo = tarea.Titulo;
        tareaActual.PrioridadTarea = tarea.PrioridadTarea;
        tareaActual.Descripcion = tarea.Descripcion;

        await dbContext.SaveChangesAsync();

        return Results.Ok();
    }

    return Results.NotFound();

});

//ELIMINAR datos
app.MapDelete("/api/tareas/{id}", async ([FromServices] TareasContext dbContext, [FromRoute] Guid id) => {
    var tareaActual = dbContext.Tareas.Find(id);

    if(tareaActual == null)
        return Results.NotFound("Tarea no encontrada.");
        
    dbContext.Remove(tareaActual);
    await dbContext.SaveChangesAsync();

    return Results.Ok("¡Eliminada correctamente!");
});


app.Run();