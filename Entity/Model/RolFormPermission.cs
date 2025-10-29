namespace Entity.Model
{
    public class RolFormPermission : BaseModel
    {
        public int rol_id { get; set; }
        public int form_id { get; set; }
        public int permission_id { get; set; }
        public Rol Rol { get; set; }
        public Form Form { get; set; }
        public Permission Permission { get; set; }
    }
}
