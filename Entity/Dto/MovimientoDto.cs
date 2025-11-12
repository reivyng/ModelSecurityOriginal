namespace Entity.Dto
{
        public class MovimientoDto : BaseDto
        {
            public int id_movimiento { get; set; }
            public string? tipo { get; set; }
            public DateTime fecha { get; set; }
            public string? descripcion { get; set; }
            public int id_bodega { get; set; }
            public int id_usuario { get; set; }
        }
}
