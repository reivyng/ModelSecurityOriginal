namespace Entity.Dto
{
        public class ProveedorDto : BaseDto
        {
            public int id_proveedor { get; set; }
            public string? nombre_empresa { get; set; }
            public string? contacto { get; set; }
            public string? telefono { get; set; }
            public string? correo { get; set; }
            public string? direccion { get; set; }
        }
}
