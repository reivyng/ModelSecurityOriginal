namespace Entity.Model
{
    public class BaseModel
    {
        public int Id { get; set; }
        public bool active { get; set; }
        public DateTime? delete_at { get; set; }
    }
}
