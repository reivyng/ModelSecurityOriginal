namespace Entity.Model
{
    public class ModuleDto : Base
    {
        public string name { get; set; }
        public string description { get; set; }
        public ICollection<FormModuleDto> FormModule { get; set; }
    }
}
