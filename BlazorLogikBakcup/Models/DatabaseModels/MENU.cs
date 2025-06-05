using System;
using System.Collections.Generic;

namespace BlazorProyectoPostres.Models.DatabaseModels;

public partial class MENU
{
    public int MEN_ID { get; set; }

    public string MEN_NOMBRE { get; set; }

    public string MEN_DESCRIPCION { get; set; }

    public bool MEN_ESTADO { get; set; }

    public decimal? MEN_PRECIO { get; set; }

    public virtual ICollection<ARCHIVO_POSTRE> ARCHIVO_POSTREs { get; set; } = new List<ARCHIVO_POSTRE>();

    public virtual ICollection<PORCION> PORCIONs { get; set; } = new List<PORCION>();
}
