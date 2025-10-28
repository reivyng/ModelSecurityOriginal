namespace Entity.Model
{
    public class RolDto : Base
    {
        public string type_rol { get; set; }
        public string description { get; set; }
        public ICollection<UserDto> users { get; set; }
        public ICollection<RolFormPermissionDto> rolFormPermissions { get; set; }
    }
}
