namespace Entity.Model
{
    public class Person : BaseModel
    {
        public int Id { get; set; }
        public string first_name { get; set; }
        public string? second_name { get; set; }
        public string first_last_name { get; set; }
        public string? second_last_name { get; set; }
        public int phone_number { get; set; }
        public int number_identification { get; set; }
        public User User { get; set; }
    }
}
