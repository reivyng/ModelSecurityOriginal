namespace Entity.Model
{
    public class RolFormPermissionDto : Base
    {
        public int rol_id { get; set; }
        public int form_id { get; set; }
        public int permission_id { get; set; }
        public RolDto rol { get; set; }
        public Form form { get; set; }
        public PermissionDto permission { get; set; }
    }
}
