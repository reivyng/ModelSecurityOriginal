namespace Entity.Model
{
    public class UserDto : Base
    {
        public string email { get; set; }
        public string password { get; set; }
        public int role_id { get; set; }
        public int person_id { get; set; }
        public RolDto rol { get; set; }
        public PersonDto person { get; set; }
    }
}
