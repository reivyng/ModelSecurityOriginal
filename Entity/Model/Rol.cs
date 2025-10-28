namespace Entity.Model
{
    public class Rol : Base
    {
        public string type_rol { get; set; }
        public string description { get; set; }
        public ICollection<User> User { get; set; }
        public ICollection<RolFormPermission> RolFormPermission { get; set; }
    }
}
