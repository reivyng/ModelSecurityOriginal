namespace Entity.Model
{
    public class User : BaseModel
    {
        public string email { get; set; }
        public string password { get; set; }
        public int rol_id { get; set; }
        public int person_id { get; set; }
        public Rol Rol { get; set; }
        public Person Person { get; set; }
    }
}
