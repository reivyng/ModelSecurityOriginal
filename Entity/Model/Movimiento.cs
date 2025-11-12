using System;
using System.Collections.Generic;

namespace Entity.Model
{
    public class Movimiento : BaseModel
    {
        public string tipo { get; set; }
        public DateTime fecha { get; set; }
        public string descripcion { get; set; }
        public int id_bodega { get; set; }
        public int id_usuario { get; set; }
        public Bodega Bodega { get; set; }
        public User User { get; set; } // Relación con el usuario que realiza el movimiento
        public ICollection<DetalleMovimiento> DetalleMovimientos { get; set; }
    }
}
