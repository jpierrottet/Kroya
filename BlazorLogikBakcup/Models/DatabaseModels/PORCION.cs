using System;
using System.Collections.Generic;

namespace BlazorProyectoPostres.Models.DatabaseModels;

public partial class PORCION
{
    public int POR_ID { get; set; }

    public string POR_NOMBRE { get; set; }

    public string POR_CANTIDAD { get; set; }

    public decimal? POR_PRECIO { get; set; }

    public int? POR_MENU { get; set; }

    public virtual MENU POR_MENUNavigation { get; set; }
}
