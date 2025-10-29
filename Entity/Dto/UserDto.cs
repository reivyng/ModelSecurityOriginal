namespace Entity.Dto
{
    public class UserDto : BaseDto
    {
        public string email { get; set; }
        public string password { get; set; }
        public int role_id { get; set; }
        public int person_id { get; set; }
        // Información anidada de la persona asociada (mapeada desde User.Person)
        public PersonDto? person { get; set; }
    }
}
