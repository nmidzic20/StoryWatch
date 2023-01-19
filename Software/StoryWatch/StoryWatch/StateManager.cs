using BusinessLayer;
using EntitiesLayer;
using EntitiesLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryWatch
{
    public static class StateManager
    {
        public static MediaCategory CurrentMediaCategory { get; set; }
        public static User LoggedUser { get; set; }
    }
}
