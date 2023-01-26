using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace fundamentosEF.Models; //identificamos la seccion

public class Categoria{
    //[Key]
    public Guid CategoriaId {get;set;}
    //[Required] //Esta propiedad será requerida al momento que nosotros insertemos un nuevo registro en la BBDD
    //[MaxLength(150)] //Temas de seguridad, limitamos el numero de caracteres.
    public string Nombre {get;set;}
    public string Descripcion {get;set;}
    public int Peso {get;set;} //el esfuerzo que tendrá la tarea que realizaremos
    [JsonIgnore]
    public virtual ICollection<Tarea> Tareas {get;set;}
}

