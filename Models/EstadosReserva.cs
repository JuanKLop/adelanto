using System;
using System.Collections.Generic;

namespace Glamping_Addventures.Models;

public partial class EstadosReserva
{
    public int IdEstadoReserva { get; set; }

    public string? NombreEstadoReserva { get; set; }

    public virtual ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
}
