using System;
using System.Collections.Generic;

namespace BlazorProyectoPostres.Models.DatabaseModels;

public partial class ARCHIVO
{
    public long ARC_ID { get; set; }

    public string ARC_NOMBRE { get; set; }

    public byte[] ARC_ARCHIVO { get; set; }

    public string ARC_EXTENSION { get; set; }

    public virtual ICollection<ARCHIVO_POSTRE> ARCHIVO_POSTREs { get; set; } = new List<ARCHIVO_POSTRE>();
}
