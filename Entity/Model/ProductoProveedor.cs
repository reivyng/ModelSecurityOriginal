namespace Entity.Model
{
    // Tabla pivote entre Producto y Proveedor
    public class ProductoProveedor : BaseModel
    {
        public int id_producto { get; set; }
        public int id_proveedor { get; set; }
        public decimal precio_compra { get; set; }
        public int tiempo_entrega { get; set; }
        public Producto Producto { get; set; }
        public Proveedor Proveedor { get; set; }
    }
}
