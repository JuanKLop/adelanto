using System;
using System.Collections.Generic;

namespace Glamping_Addventures.Models;

public partial class Paquete
{
    public int Idpaquete { get; set; }

    public string? NombrePaquete { get; set; }

    public byte[]? ImagenPaquete { get; set; }

    public string? Descripcion { get; set; }

    public int? Idhabitacion { get; set; }

    public int? Idservicio { get; set; }

    public double? Precio { get; set; }

    public bool? Estado { get; set; }

    public virtual ICollection<DetalleReservaPaquete> DetalleReservaPaquetes { get; set; } = new List<DetalleReservaPaquete>();

    public virtual Habitacion? IdhabitacionNavigation { get; set; }

    public virtual Servicio? IdservicioNavigation { get; set; }
}
