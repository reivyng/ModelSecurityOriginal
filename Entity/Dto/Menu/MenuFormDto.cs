using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Dto.Menu
{
    public class MenuFormDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<FormItemDto> Form { get; set; }
    }
}
