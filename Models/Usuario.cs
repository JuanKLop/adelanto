using System;
using System.Collections.Generic;

namespace Glamping_Addventures.Models;

public partial class Usuario
{
    public int Idusuario { get; set; }

    public string? NombreUsuario { get; set; }

    public string? Email { get; set; }

    public string? TipoDocumento { get; set; }

    public int? NumeroDocumento { get; set; }

    public string? Telefono { get; set; }

    public string? Direccion { get; set; }

    public string? Pais { get; set; }

    public string? Cuidad { get; set; }

    public int? Idrol { get; set; }

    public string? Contrasena { get; set; }

    public string? Apellido { get; set; }

    public bool? Estado { get; set; }

    public virtual Role? IdrolNavigation { get; set; }
}
