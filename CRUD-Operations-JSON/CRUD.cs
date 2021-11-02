using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace CRUD_Operations_JSON
{
    public class CRUD<O> : CRUD_Operations.CRUDDefault<O>
    {
        private string filepath;
        public CRUD(string filepath)
        {
            this.filepath = filepath;
            readList();
        }

        public override void readList()
        {
            if (File.Exists(filepath))
            {
                set(JsonConvert.DeserializeObject<List<O>>(File.ReadAllText(filepath)));
            }
        }
        public override void safeList()
        {
            File.WriteAllText(filepath, JsonConvert.SerializeObject(getList()));
        }
    }
}
