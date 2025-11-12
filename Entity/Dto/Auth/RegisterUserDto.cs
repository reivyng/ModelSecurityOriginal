namespace Entity.Dto.Auth
{
    public class RegisterUserDto
    {
        public string first_name { get; set; }
        public string first_last_name { get; set; }
        public long phone_number { get; set; }
        public long number_identification { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string confirm_password { get; set; }
    }
}
