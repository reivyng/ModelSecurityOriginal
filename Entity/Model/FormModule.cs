namespace Entity.Model
{
    public class FormModuleDto : Base
    {
        public  int form_id { get; set; }
        public  int module_id { get; set; }
        public Form Form { get; set; }
        public ModuleDto Module { get; set; }
    }
}
