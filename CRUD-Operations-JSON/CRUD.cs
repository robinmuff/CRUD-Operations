using System;
using System.Collections.ObjectModel;
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
        }

        public override void readList()
        {
            if (File.Exists(filepath))
            {
                List = JsonConvert.DeserializeObject<ObservableCollection<O>>(File.ReadAllText(filepath));
            }
        }
        public override void safeList()
        {
            File.WriteAllText(filepath, JsonConvert.SerializeObject(List));
        }
    }
}
