namespace Entity.Dto
{
        public class ProductoDto : BaseDto
        {
            public int id_producto { get; set; }
            public string? nombre { get; set; }
            public string? descripcion { get; set; }
            public decimal precio_unitario { get; set; }
            public int stock { get; set; }
            public int id_categoria { get; set; }
            public string? estado { get; set; }
        }
}
