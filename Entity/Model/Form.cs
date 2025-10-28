namespace Entity.Model
{
    public class Form : Base
    {
        public string name { get; set; }
        public string description { get; set; }
        public string path { get; set; }
        public ICollection<FormModule> FormModule { get; set; }
        public ICollection<RolFormPermission> RolFormPermission { get; set; }
    }
}
