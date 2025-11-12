namespace Entity.Dto
{
    public class PersonDto : BaseDto
    {
        public int Id { get; set; }
        public string first_name { get; set; }
        public string first_last_name { get; set; }
        public long phone_number { get; set; }
        public long number_identification { get; set; }
    }
}
