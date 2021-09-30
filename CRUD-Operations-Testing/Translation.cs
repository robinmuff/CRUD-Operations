using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD_Operations_Testing
{
    public class Translation
    {
        public Translation()
        {
            this.key = "";
            this.comment = "";
            this.texts = new List<TranslationValue>();
        }
        public Translation(string piKey, string piComment = "")
        {
            this.key = piKey;
            this.comment = piComment;
            this.texts = new List<TranslationValue>();
        }

        public string key;
        public string comment;
        public List<TranslationValue> texts;
    }
}
