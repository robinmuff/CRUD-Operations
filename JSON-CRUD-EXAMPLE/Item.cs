using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSON_CRUD_EXAMPLE
{
    public class Item
    {
        public Item() { }
        public Item(string title, string value) { this.title = title; this.value = value; }

        public string title { get; set; }
        public string value { get; set; }
    }
}
