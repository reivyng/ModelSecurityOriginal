namespace Entity.Model
{
    public class PersonDto : Base
    {
        public int Id { get; set; }
        public string first_name { get; set; }
        public string? second_name { get; set; }
        public string first_last_name { get; set; }
        public string? second_last_name { get; set; }
        public int phone_number { get; set; }
        public int number_identification { get; set; }
        public ICollection<UserDto> users { get; set; }
    }
}
