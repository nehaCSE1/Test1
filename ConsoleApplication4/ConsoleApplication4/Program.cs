using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ShanFamily
{
    class Program
    {
       
        

     
        static void CallFunction(string str) {
            string[] inputString = str.Split(' ');
            switch (inputString[0]) {

                case "ADD_CHILD":
                    Console.WriteLine(AddChild(inputString[1], inputString[2], inputString[3]));
                    break;
                case "GET_RELATIONSHIP":
                     Console.WriteLine(HandleRelationship(inputString[1],inputString[2]));
                    break;
                default: break;

            }
        }
        static void Main(string[] args)
        {
            IFamilyTreeGeneration familyTreee = new FamilyTreeGeneration();
           FamilyList=familyTreee.GenerateFamilyTree();

           motherChildRelationDictionary = familyTreee.GenerateMotherChildDictionary(FamilyList);
           //if (args.Length > 0)
           //{
               IEnumerable<string> inputLines = File.ReadAllLines("input.txt");//(args[0]);
               foreach (string str in inputLines)
               {
                   CallFunction(str);
               }
           //}
           //else
           //{
           //    Console.WriteLine("No command line arguments found.");
           //} 
            Console.ReadLine();
        }
    }
}
