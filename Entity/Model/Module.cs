namespace Entity.Model
{
    public class Module : Base
    {
        public string name { get; set; }
        public string description { get; set; }
        public ICollection<FormModule> FormModule { get; set; }
    }
}
