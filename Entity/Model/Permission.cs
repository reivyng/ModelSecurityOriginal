namespace Entity.Model
{
    public class Permission : Base
    {
        public string type_permission { get; set; }
        public string description { get; set; }
        public ICollection<RolFormPermission> RolFormPermission { get; set; }
    }
}
