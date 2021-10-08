using System;
using System.Collections.ObjectModel;

namespace CRUD_Operations
{
    public abstract class CRUDDefault<O>
    {
        ObservableCollection<O> list;

        public CRUDDefault()
        {
            list = new ObservableCollection<O>();

            list.CollectionChanged += List_CollectionChanged;
        }

        public ObservableCollection<O> List
        {
            get
            {
                if (list == null || list.Count == 0) { readList(); }
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
        public abstract void safeList();
        public abstract void readList();
    }
}
