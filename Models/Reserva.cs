﻿using System;
using System.Collections.Generic;

namespace Glamping_Addventures.Models;

public partial class Reserva
{
    public int IdReserva { get; set; }

    public string? NroDocumentoCliente { get; set; }

    public DateTime? FechaReserva { get; set; }

    public DateOnly? FechaInicio { get; set; }

    public DateOnly? FechaFinalizacion { get; set; }

    public double? SubTotal { get; set; }

    public double? Descuento { get; set; }

    public double? Iva { get; set; }

    public double? MontoTotal { get; set; }

    public int? MetodoPago { get; set; }

    public int? IdEstadoReserva { get; set; }

    public virtual ICollection<Abono> Abonos { get; set; } = new List<Abono>();

    public virtual ICollection<DetalleReservaPaquete> DetalleReservaPaquetes { get; set; } = new List<DetalleReservaPaquete>();

    public virtual ICollection<DetalleReservaServicio> DetalleReservaServicios { get; set; } = new List<DetalleReservaServicio>();

    public virtual EstadosReserva? IdEstadoReservaNavigation { get; set; }

    public virtual MetodoPago? MetodoPagoNavigation { get; set; }

    public virtual Cliente? NroDocumentoClienteNavigation { get; set; }
}
