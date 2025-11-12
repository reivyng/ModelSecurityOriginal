using System.Collections.Generic;

namespace Entity.Model
{
    public class Producto : BaseModel
    {
        // expose legacy property name used in tests, map to Id from BaseModel
        public int id_producto { get { return Id; } set { Id = value; } }

        public string nombre { get; set; }
        public string descripcion { get; set; }
        public decimal precio_unitario { get; set; }
        public int stock { get; set; }
        public int id_categoria { get; set; }
        public string estado { get; set; }
        public Categoria Categoria { get; set; }
        public ICollection<ProductoProveedor> ProductoProveedores { get; set; }
        public ICollection<DetalleMovimiento> DetalleMovimientos { get; set; }
    }
}
