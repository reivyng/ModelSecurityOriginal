namespace Entity.Dto
{
    public class UserDto : BaseDto
    {
        public string email { get; set; }
        public string password { get; set; }
        public int role_id { get; set; }
        public int person_id { get; set; }
    }
}
