using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;

namespace JSON_CRUD
{
    public class CRUD<O> where O : new()
    {
        private ObservableCollection<O> list;
        private string filename;

        public CRUD(string filename)
        {
            this.list = new ObservableCollection<O>();
            this.filename = filename;

            list.CollectionChanged += List_CollectionChanged;

            readList();
        }
        private void List_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            safeList();
        }

        /* -- Save Operations -- */
        private void readList()
        {
            if (File.Exists(filename))
            {
                Set(JsonConvert.DeserializeObject<List<O>>(File.ReadAllText(filename)));
            }
        }
        private void safeList()
        {
            File.WriteAllText(filename, JsonConvert.SerializeObject(Get()));
        }

        /* -- List Operations -- */
        public void Set(List<O> list) { this.list.Clear(); foreach (O item in list) { this.list.Add(item); } }
        public List<O> Get() { List<O> returnlist = new List<O>(); foreach (O item in list) { returnlist.Add(item); } return returnlist; }
        public void Add(O item) { list.Add(item); }
        public void AddRange(List<O> items) { foreach (O item in items) { list.Add(item); } }
        public void Clear() { list.Clear(); }
        public void CopyTo(O[] array, int index) { list.CopyTo(array, index); }
        public bool Contains(O item) { return list.Contains(item); }
        public bool ContainsMultiple(List<O> items) { foreach (O item in items) { if (!list.Contains(item)) { return false; } } return true; }
        public int Count() { return list.Count; }
        public bool Equals() { return list.Equals(list); }
        public IEnumerator GetEnumerator() { return list.GetEnumerator(); }
        public override int GetHashCode() { return list.GetHashCode(); }
        public int IndexOf(O item) { return list.IndexOf(item); }
        public List<int> IndexOfMultiple(List<O> items) { List<int> indexList = new List<int>(); foreach (O item in items) { indexList.Add(list.IndexOf(item)); } return indexList; }
        public void Insert(int index, O item) { list.Insert(index, item); }
        public void Move(int oldIndex, int newIndex) { list.Move(oldIndex, newIndex); }
        public void Remove(O item) { list.Remove(item); }
        public void RemoveMultiple(List<O> items) { foreach (O item in items) { list.Remove(item); } }
        public void RemoveAt(int index) { list.RemoveAt(index); }
        public O GetO(int index) { return list[index]; }
        public string ItemToString(O item) { return list[list.IndexOf(item)].ToString(); }
        public override string ToString() { return list.ToString(); }
        public ObservableCollection<O> GetCollection() { return list; }

        /* -- NOTIFICATIONS -- */
        public void AddChangeListener(NotifyCollectionChangedEventHandler notifyCollectionChangedEventHandler)
        {
            list.CollectionChanged += notifyCollectionChangedEventHandler;
        }
        public void AddMultipleChangeListener(List<NotifyCollectionChangedEventHandler> notifyCollectionChangedEventHandlers)
        {
            foreach (NotifyCollectionChangedEventHandler notifyCollectionChangedEventHandler in notifyCollectionChangedEventHandlers)
            {
                list.CollectionChanged += notifyCollectionChangedEventHandler;
            }
        }
    }
}
