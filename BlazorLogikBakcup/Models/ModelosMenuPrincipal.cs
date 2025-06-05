using BlazorProyectoPostres.Models.DatabaseModels;
using System.Collections.Generic;

namespace BlazorProyectoPostres.Models
{
    public class MenuPrincipalViewModel
    {
        public  string NombreMenu {get;set;}
        public  string Descripcion {get;set;}
        public  decimal Precio {get;set;}
        public  List<PORCION> Porciones {get;set;}
        public List<byte[]> Imagenes { get; set; } = new List<byte[]>();

    }
}
