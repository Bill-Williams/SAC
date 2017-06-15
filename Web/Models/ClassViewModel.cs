using SAC.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SAC.Web.Models
{
    public class ClassViewModel
    {
        public IEnumerable<Class> Classes { get; set; }

        public IEnumerable<Group> Groups { get; set; }
    }
}