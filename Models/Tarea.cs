using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace fundamentosEF.Models;

public class Tarea{
    //[Key]
    public Guid TareaId {get;set;}

    //[ForeignKey("CategoriaId")] //Relacion entre una tabla y otra tabla: clave foranea
    public Guid CategoriaId {get;set;}

    //[Required]
    //[MaxLength(200)]
    public string Titulo {get;set;}
    public string Descripcion {get; set;}
    public Prioridad PrioridadTarea {get;set;}
    public DateTime FechaCreacion {get;set;}
    public virtual Categoria Categoria {get;set;}
    [NotMapped] //Esto hará que en el momento que se haga el mapeo de nuestro contexto a la BBDD, omitirá este campo
    public string Resumen {get;set;}
}

public enum Prioridad{
    Baja,
    Media,
    Alta
}