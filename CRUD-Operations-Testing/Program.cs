using System;
using CRUD_Operations_JSON;
using Newtonsoft.Json;

namespace CRUD_Operations_Testing
{
    class Program
    {
        static CRUD<ItemClass> itemList = null;
        static void Main(string[] args)
        {
            Console.WriteLine("--- Start testing ---");

            itemList = new CRUD<ItemClass>("test.json");

            printCurrentList("Initialized");

            itemList.Add(new ItemClass("FirstTest", "FirstTestVal"));

            printCurrentList("First added");

            itemList.Add(new ItemClass("SecTest", "SecTestVal"));

            printCurrentList("Second added");

            itemList.RemoveAt(0);

            printCurrentList("First removed");

            itemList.Clear();

            printCurrentList("List cleared");

            itemList.set(new System.Collections.Generic.List<ItemClass>());

            Console.WriteLine("--- End testing ---");
        }

        static void printCurrentList(string messageBefore)
        {
            Console.WriteLine(messageBefore);
            Console.WriteLine(JsonConvert.SerializeObject(itemList.getList()));
            Console.WriteLine();
        }
    }
}
