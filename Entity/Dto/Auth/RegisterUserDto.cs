namespace Entity.Dto.Auth
{
    public class RegisterUserDto
    {
        public string first_name { get; set; }
        public string first_last_name { get; set; }
        public int phone_number { get; set; }
        public int number_identification { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string confirm_password { get; set; }
    }
}
