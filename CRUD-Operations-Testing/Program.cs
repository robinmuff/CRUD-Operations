using System;
using CRUD_Operations;
using Newtonsoft.Json;

namespace CRUD_Operations_Testing
{
    class Program
    {
        static CRUD<ItemClass> itemList = null;
        static void Main(string[] args)
        {
            Console.WriteLine("--- Start testing ---");

            itemList = new CRUD<ItemClass>("itemlist.json");

            printCurrentList("Initialized");

            itemList.list.Add(new ItemClass("FirstTest", "FirstTestVal"));

            printCurrentList("First added");

            itemList.list.Add(new ItemClass("SecTest", "SecTestVal"));

            printCurrentList("Second added");

            itemList.list.RemoveAll(item => item.key.Contains("First"));

            printCurrentList("First removed with match");

            itemList.list.Clear();

            printCurrentList("List cleared");

            Console.WriteLine("--- End testing ---");
        }

        static void printCurrentList(string messageBefore)
        {
            Console.WriteLine(messageBefore);
            Console.WriteLine(JsonConvert.SerializeObject(itemList.list));
            Console.WriteLine();
        }
    }
}
