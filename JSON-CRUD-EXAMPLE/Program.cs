using JSON_CRUD;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace JSON_CRUD_EXAMPLE
{
    internal class Program
    {
        private static CRUD<Item> itemList;
        private static CRUD<Item> cryptList;
        static void Main(string[] args)
        {
            /* Welcome Text */
            Console.WriteLine("WELCOME TO JSON CRUD. This is a project to save a list very easy to a json file and read automatically.\nFeel free to use this in your application, just give the credits to the public repo here.\n\n");

            /* Set used Variables */
            Item itemMyName = new("My Name is: ", "Robin");
            Item itemSourceCode = new("You can see all the code of: ", "JSON-CRUD");
            Item itemDiffrentProject = new("You can see all the code of: ", "DIFFERENT Project");

            /* Set CRUD with file */
            itemList = new CRUD<Item>("items.json");

            /* Set an Change Listener */
            itemList.AddChangeListener(ChangeListener);

            /* Do some actions, find instant changes in JSON */
            itemList.Add(itemMyName);
            itemList.Add(itemSourceCode);
            itemList.Remove(itemMyName);
            itemList.Add(itemMyName);

            /* Do some other functions with output */
            Console.WriteLine("Item Contains (true): " + itemList.Contains(itemSourceCode).ToString());
            Console.WriteLine("Item Contains (false): " + itemList.Contains(itemDiffrentProject).ToString());

            List<Item> items = itemList.Get().Where(item => item.title == "My Name is: ").ToList();
            Console.WriteLine("--> PRINT <--");
            Console.WriteLine(JsonConvert.SerializeObject(items));
            Console.WriteLine("-------------");

            /* Do some more actions, find instant changes in JSON */
            itemList.RemoveAt(0);
            itemList.Clear();
            itemList.Add(itemMyName);
            itemList.Clear();

            /* Crypt List */
            cryptList = new CRUD<Item>("crypItems.json", new CryptAccess("Crypted", 8));

            /* Crypt List Edit */
            cryptList.Add(itemMyName);
            PrintCryptList();
            cryptList.Add(itemSourceCode);
            PrintCryptList();
            cryptList.Clear();
            PrintCryptList();


            Console.Read();
        }

        static void PrintList()
        {
            Console.WriteLine("--> PRINT <--");
            Console.WriteLine(JsonConvert.SerializeObject(itemList.Get()));
            Console.WriteLine("-------------");
        }
        static void PrintCryptList()
        {
            Console.WriteLine("--> PRINT <--");
            Console.WriteLine(JsonConvert.SerializeObject(cryptList.Get()));
            Console.WriteLine("-------------");
        }

        static void ChangeListener(object sender, NotifyCollectionChangedEventArgs e)
        {
            Console.WriteLine("NOTIFIED FOR CHANGE, print following...");

            PrintList();
        }
    }
}
