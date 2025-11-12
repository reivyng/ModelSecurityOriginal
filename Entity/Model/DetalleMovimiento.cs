namespace Entity.Model
{
    // Tabla pivote entre Movimiento y Producto
    public class DetalleMovimiento : BaseModel
    {
        public int id_movimiento { get; set; }
        public int id_producto { get; set; }
        public int cantidad { get; set; }
        public decimal precio_unitario { get; set; }
        public decimal subtotal { get; set; }
        public Movimiento Movimiento { get; set; }
        public Producto Producto { get; set; }
    }
}
