namespace Entity.Dto
{
        public class CategoriaDto : BaseDto
        {
            public int id_categoria { get; set; }
            public string? nombre { get; set; }
            public string? descripcion { get; set; }
        }
}
