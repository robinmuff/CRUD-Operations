using System;
using CRUD_Operations;

namespace CRUD_Operations_Testing
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("--- Start testing ---");

            CRUD<Translation> crud = new CRUD<Translation>("this.json");
            var x = crud.getAll();
            crud.appendElement(new Translation("Test", ""));
            crud.setAll(x);
            crud.appendElement(new Translation("Testing", ""));
            crud.removeElement(new Translation("Testing", ""));
            crud.appendElement(new Translation("Testing", ""));
            crud.removeElementAtPositon(1);
        }
    }
}
