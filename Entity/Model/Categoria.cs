using System.Collections.Generic;

namespace Entity.Model
{
    public class Categoria : BaseModel
    {
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public ICollection<Producto> Productos { get; set; }
    }
}
