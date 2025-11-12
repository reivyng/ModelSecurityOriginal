using System.Collections.Generic;

namespace Entity.Model
{
    public class Bodega : BaseModel
    {
        // expose legacy property name used in tests, map to Id from BaseModel
        public int id_bodega { get { return Id; } set { Id = value; } }

        public string nombre { get; set; }
        public string ubicacion { get; set; }
        public int capacidad_maxima { get; set; }
        public string encargado { get; set; }
        public ICollection<Movimiento> Movimientos { get; set; }
    }
}
