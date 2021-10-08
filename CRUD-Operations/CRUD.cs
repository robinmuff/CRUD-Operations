using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace CRUD_Operations
{
    public class CRUD<O> where O : new()
    {
        private string filename;
        private ObservableCollection<O> list;

        public CRUD(string filename)
        {
            this.filename = filename;

            readList();

            list.CollectionChanged += List_CollectionChanged;
        }

        public ObservableCollection<O> List
        {
            get
            {
                return list;
            }
            set
            {
                list = value;
                safeList();
            }
        }
        private void List_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            safeList();
        }

        /* -- File Operations -- */
        private void safeList()
        {
            File.WriteAllText(filename, Newtonsoft.Json.JsonConvert.SerializeObject(list));
        }
        private void readList()
        {
            if (File.Exists(filename))
            {
                list = Newtonsoft.Json.JsonConvert.DeserializeObject<ObservableCollection<O>>(File.ReadAllText(filename));
            }
            else
            {
                list = new ObservableCollection<O>();
            }
        }
    }
}
