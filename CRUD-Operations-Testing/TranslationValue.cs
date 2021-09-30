using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD_Operations_Testing
{
    public class TranslationValue
    {
        public TranslationValue(string piLanguage, string piText)
        {
            this.language = piLanguage;
            this.text = piText;
        }

        public string language { get; set; }
        public string text { get; set; }
    }
}
