using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CRUD_Operations
{
    public abstract class CRUDDefault<O>
    {
        private ObservableCollection<O> list;

        public CRUDDefault()
        {
            list = new ObservableCollection<O>();

            list.CollectionChanged += List_CollectionChanged;

            readList();
        }

        private void List_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            safeList();
        }

        /* -- File Operations -- */
        public abstract void safeList();
        public abstract void readList();

        /* -- List Operations -- */
        public List<O> getList()
        {
            List<O> finalList = new List<O>();
            foreach (O item in list) { finalList.Add(item); }
            return finalList;
        }
        public void set(List<O> newList)
        {
            ObservableCollection<O> addList = new ObservableCollection<O>();
            foreach (O item in newList) { addList.Add(item); }
            list = addList;
            safeList();
        }
        public void Add(O item)
        {
            list.Add(item);
            safeList();
        }
        public void Remove(O item)
        {
            list.Remove(item);
            safeList();
        }
        public int Count()
        {
            return list.Count;
        }
        public void Clear()
        {
            int counter = list.Count;
            for (int i = 0; i < counter; i++)
            {
                list.RemoveAt(0);
            }
            safeList();
        }
        public bool Contains(O item)
        {
            return list.Contains(item);
        }
        public override bool Equals(object item)
        {
            return list.Equals(item);
        }
        public IEnumerator<O> GetEnumerator()
        {
            return list.GetEnumerator();
        }
        public override int GetHashCode()
        {
            return list.GetHashCode();
        }
        public new Type GetType()
        {
            return list.GetType();
        }
        public int IndexOf(O item)
        {
            return list.IndexOf(item);
        }
        public void Insert(int index, O item)
        {
            list.Insert(index, item);
            safeList();
        }
        public void Move(int oldIndex, int newIndex)
        {
            list.Move(oldIndex, newIndex);
            safeList();
        }
        public void RemoveAt(int index)
        {
            list.RemoveAt(index);
            safeList();
        }
        public O GetItem(int index)
        {
            return list[index];
        }
        public override string ToString()
        {
            return list.ToString();
        }
    }
}
