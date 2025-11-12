namespace Entity.Dto
{
        public class BodegaDto : BaseDto
        {
            public int id_bodega { get; set; }
            public string? nombre { get; set; }
            public string? ubicacion { get; set; }
            public int capacidad_maxima { get; set; }
            public string? encargado { get; set; }
        }
}
