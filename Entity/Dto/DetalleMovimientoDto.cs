namespace Entity.Dto
{
        public class DetalleMovimientoDto : BaseDto
        {
            public int id_movimiento { get; set; }
            public int id_producto { get; set; }
            public int cantidad { get; set; }
            public decimal precio_unitario { get; set; }
            public decimal subtotal { get; set; }
        }
}
