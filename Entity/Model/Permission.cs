namespace Entity.Model
{
    public class PermissionDto : Base
    {
        public string type_permission { get; set; }
        public string description { get; set; }
        public ICollection<RolFormPermissionDto> rolFormPermissions { get; set; }
    }
}
