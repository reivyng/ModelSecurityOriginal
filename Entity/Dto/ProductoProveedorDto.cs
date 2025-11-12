namespace Entity.Dto
{
        public class ProductoProveedorDto : BaseDto
        {
            public int id_producto { get; set; }
            public int id_proveedor { get; set; }
            public decimal precio_compra { get; set; }
            public int tiempo_entrega { get; set; }
        }
}
