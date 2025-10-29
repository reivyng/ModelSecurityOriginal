namespace Entity.Model
{
    public class FormModule : BaseModel
    {
        public  int form_id { get; set; }
        public  int module_id { get; set; }
        public Form Form { get; set; }
        public Module Module { get; set; }
    }
}
