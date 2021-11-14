using System;
using System.Collections.Specialized;
using JSON_CRUD;
using Newtonsoft.Json;

namespace JSON_CRUD_EXAMPLE
{
    internal class Program
    {
        private static CRUD<Item> itemList;
        static void Main(string[] args)
        {
            Console.WriteLine("WELCOME TO JSON CRUD. This is a project to save a list very easy to a json file and read automatically.\nFeel free to use this in your application, just give the credits to the public repo here.");

            Item itemMyName = new Item("My Name is: ", "Robin");
            Item itemSourceCode = new Item("You can see all the code of: ", "JSON-CRUD");
            Item itemDiffrentProject = new Item("You can see all the code of: ", "DIFFRENT Project");

            itemList = new CRUD<Item>("items.json");

            printList();

            itemList.Add(itemMyName);

            printList();

            itemList.Add(itemSourceCode);

            printList();

            itemList.Remove(itemMyName);

            printList();

            itemList.Add(itemMyName);

            printList();

            Console.WriteLine("Item Contains (true): " + itemList.Contains(itemSourceCode).ToString());
            Console.WriteLine("Item Contains (false): " + itemList.Contains(itemDiffrentProject).ToString());

            itemList.RemoveAt(0);

            printList();

            itemList.Clear();

            itemList.AddChangeListener(eventListener);

            itemList.Add(itemMyName);

            itemList.Clear();
        }

        static void printList()
        {
            Console.WriteLine("--> PRINT <--");
            Console.WriteLine(JsonConvert.SerializeObject(itemList.Get()));
            Console.WriteLine("-------------");
        }

        static void eventListener(object sender, NotifyCollectionChangedEventArgs e)
        {
            Console.WriteLine("NOTIFIED FOR CHANGE, print following...");

            printList();
        }
    }
}
