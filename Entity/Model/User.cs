namespace Entity.Model
{
    public class User : BaseModel
    {
        public string email { get; set; }
        public string password { get; set; }
        public int rol_id { get; set; }
        public int person_id { get; set; }
        public string first_name { get; set; }
        public string first_last_name { get; set; }
        public long phone_number { get; set; }
        public long number_identification { get; set; }
        public Rol Rol { get; set; }
        public Person Person { get; set; }
    }
}
