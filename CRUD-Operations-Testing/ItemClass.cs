using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD_Operations_Testing
{
    public class ItemClass
    {
        public ItemClass()
        {
            this.key = "";
            this.value = "";
        }
        public ItemClass(string key, string value)
        {
            this.key = key;
            this.value = value;
        }

        public string key { get; set; }
        public string value { get; set; }
    }
}
