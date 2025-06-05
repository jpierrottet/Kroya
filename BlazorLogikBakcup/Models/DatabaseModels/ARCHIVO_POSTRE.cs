using System;
using System.Collections.Generic;

namespace BlazorProyectoPostres.Models.DatabaseModels;

public partial class ARCHIVO_POSTRE
{
    public int ARP_ID { get; set; }

    public long? ARP_ARCHIVO { get; set; }

    public int? ARP_MENU { get; set; }

    public virtual ARCHIVO ARP_ARCHIVONavigation { get; set; }

    public virtual MENU ARP_MENUNavigation { get; set; }
}
