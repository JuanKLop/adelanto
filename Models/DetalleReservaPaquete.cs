﻿using System;
using System.Collections.Generic;

namespace Glamping_Addventures.Models;

public partial class DetalleReservaPaquete
{
    public int IddetalleReservaPaquetes { get; set; }

    public int? Idreserva { get; set; }

    public int? Cantidad { get; set; }

    public int? Precio { get; set; }

    public bool? Estado { get; set; }

    public int? Idpaquete { get; set; }

    public virtual Paquete? IdpaqueteNavigation { get; set; }

    public virtual Reserva? IdreservaNavigation { get; set; }
}
