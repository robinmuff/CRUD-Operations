using System;
using System.Collections.Generic;
using System.IO;

namespace CRUD_Operations
{
    public class CRUD<O> where O : new()
    {
        private string _filename;
        private List<O> _list;

        public CRUD(string piFilename)
        {
            _filename = piFilename;
            readList();
        }
        public List<O> list
        {
            get
            {
                return _list;
            }
            set
            {
                _list = value;
                safeList();
            }
        }

        /* -- File Operations -- */
        private void safeList()
        {
            File.WriteAllText(_filename, Newtonsoft.Json.JsonConvert.SerializeObject(list));
        }
        private void readList()
        {
            if (File.Exists(_filename))
            {
                list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<O>>(File.ReadAllText(_filename));
            }
            else
            {
                list = new List<O>();
            }
        }
    }
}
