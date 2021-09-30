using System;
using System.Collections.Generic;
using System.IO;

namespace CRUD_Operations
{
    public class CRUD<O> where O : new()
    {
        private string _filename;
        private List<O> list;

        public CRUD(string piFilename)
        {
            _filename = piFilename;
        }

        /* -- File Operations -- */
        private void safeList()
        {
            File.WriteAllText(_filename, Newtonsoft.Json.JsonConvert.SerializeObject(list));
        }
        private void readList(bool piForce = false)
        {
            if (list == null || piForce)
            {
                if (!File.Exists(_filename)) { File.WriteAllText(_filename, "[]"); }
                list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<O>>(File.ReadAllText(_filename));
            }
        }

        /* -- Call Operations -- */
        public List<O> getAll()
        {
            readList();
            return list;
        }
        public void setAll(List<O> piList)
        {
            list = piList;
            safeList();
        }

        public void appendElement(O piElement)
        {
            list.Add(piElement);
            safeList();
        }
        public void removeElement(O piElement)
        {
            list.Remove(piElement);
            safeList();
        }
        public void removeElementAtPositon(int piPosition)
        {
            list.RemoveAt(piPosition);
            safeList();
        }
        public void updateElement(string piPropertyName, string piPropertyValue, O piUdated)
        {
            var itemForOverride = new O();

            foreach (O item in list)
            {
                if (item.GetType().GetProperty(piPropertyName).GetValue(item).ToString() == piPropertyValue)
                {
                    itemForOverride = item;
                }
            }

            if (itemForOverride != null) { itemForOverride = piUdated; }

            safeList();
        }
    }
}
