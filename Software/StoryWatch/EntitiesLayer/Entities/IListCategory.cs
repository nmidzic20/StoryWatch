using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntitiesLayer.Entities
{
    public interface IListCategory
    {
        int Id { get; set; }
        string Title { get; set; }
        string Color { get; set; }
    }
}
