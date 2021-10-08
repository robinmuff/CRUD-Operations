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

            itemList.List.Add(new ItemClass("FirstTest", "FirstTestVal"));

            printCurrentList("First added");

            itemList.List.Add(new ItemClass("SecTest", "SecTestVal"));

            printCurrentList("Second added");

            itemList.List.RemoveAt(0);

            printCurrentList("First removed");

            itemList.List.Clear();

            printCurrentList("List cleared");

            Console.WriteLine("--- End testing ---");
        }

        static void printCurrentList(string messageBefore)
        {
            Console.WriteLine(messageBefore);
            Console.WriteLine(JsonConvert.SerializeObject(itemList.List));
            Console.WriteLine();
        }
    }
}
