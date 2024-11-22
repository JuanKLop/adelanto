using System;
using System.Collections.Generic;

namespace Glamping_Addventures.Models;

public partial class TokenRecuperacion
{
    public int Id { get; set; }

    public string? Token { get; set; }

    public DateTime FechaExpiracion { get; set; }

    public bool Usado { get; set; }
}
