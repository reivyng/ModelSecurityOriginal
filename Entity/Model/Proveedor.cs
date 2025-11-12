using System.Collections.Generic;

namespace Entity.Model
{
    public class Proveedor : BaseModel
    {
        public string nombre_empresa { get; set; }
        public string contacto { get; set; }
        public string telefono { get; set; }
        public string correo { get; set; }
        public string direccion { get; set; }
        public ICollection<ProductoProveedor> ProductoProveedores { get; set; }
    }
}
