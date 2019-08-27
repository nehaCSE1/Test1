using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ShanFamily
{
    class Program
    {
        public static List<Node> FamilyList = new List<Node>();
        public static Dictionary<string, List<Node>> motherChildRelationDictionary = new Dictionary<string, List<Node>>();
        
        
        static Node FindNode(string name) {
            return FamilyList.Where(x => x.Name == name).FirstOrDefault<Node>(); ;
        }
        static Node FindGrandMother(string name) {
            Node tempNode = FindNode(name);
            if (tempNode == null) { Console.WriteLine("PERSON_NOT_FOUND"); }
            else {
                tempNode = FindNode(tempNode.MothersName);
                if (tempNode.MothersName == null)
                {
                    tempNode = FindNode(tempNode.SpouseName);
                    if (tempNode == null) {
                        return tempNode;
                    }
                    else
                    tempNode = FindNode(tempNode.MothersName);
                }
                else { 
                tempNode = FindNode(tempNode.MothersName);
                }
            }
            return tempNode;
        }

        static string FindUncleOrAunt(string name, GenderType relativeGender,string relativeSide) {
           string parentName="";
           string resultString = "";
            Node tempNode = FindGrandMother(name);
            List<Node> lst;
            if (tempNode == null) { return ("NONE"); }
            else {

                 switch (relativeSide)
                 {
                case "Paternal":
                    parentName = FindNode(FindNode(name).MothersName).SpouseName;                    
                    break;
                case "Maternal":
                     parentName = FindNode(name).MothersName; 
                     break;

            }
                 motherChildRelationDictionary.TryGetValue(tempNode.Name,out lst);
                 lst = lst.Where(x => x.Name != parentName && x.Gender == relativeGender).ToList<Node>();
                if (lst.Count == 0)
                {
                    resultString="NONE";
                }
                else {
                    resultString = string.Join(" ", lst.Select(x => x.Name));
                }
                return resultString;
            }
        }
      
        static string returnInLawsName(Node node, GenderType gender)
        {
            string resultString = "";
            List<Node> lst = null;
            if (node.MothersName != null)
            { //Wives or Husbands of siblings
                lst = returnSiblingsList(node);
                if (lst != null)
                {
                    lst = lst.Where(x => x.SpouseName != null).DefaultIfEmpty().ToList<Node>();
                    if (lst[0] != null)
                    {
                        lst = lst.Where(x => (FindNode(x.SpouseName).Gender) == gender).DefaultIfEmpty().ToList<Node>();
                    }   
                }
                if (lst[0] == null)
                {
                    resultString = "NONE";
                }
                else
                {
                    resultString = string.Join(" ", lst.Select(x => x.SpouseName));
                }
            }
            else
            { //Spouse’s sisters or brother
                lst = returnSiblingsList(FindNode(node.SpouseName));
                if (lst != null)
                {
                    lst = lst.Where(x => x.Gender == gender).ToList<Node>();                   
                }
               
                if (lst == null)
                {
                    resultString = "NONE";
               }
                else
                {
                    resultString = string.Join(" ", lst.Select(x => x.Name));
                }
            }
            
            return resultString;
        }
        static List<Node> returnSiblingsList(Node tempNode)
        {
            List<Node> lst = null;
            Node motherNode = FindNode(tempNode.MothersName);
            if (motherNode != null)
            {
                motherChildRelationDictionary.TryGetValue(motherNode.Name, out lst);
                var templst = lst.Where(x => x.Name != tempNode.Name);
                if (templst != null) {
                    lst= templst.ToList<Node>();
                }
            }
            return lst;
        }
          
        static Node mapToFemaleRelative(Node node)
        {
            Node tempNode = null;
            if (node.Gender == GenderType.Male)
            {
                if (node.SpouseName != null)
                    tempNode = FindNode(node.SpouseName);               
            }
           
            return tempNode;
        
        }
        static string returnChildName(Node node,GenderType gender) {
            Node tempNode = mapToFemaleRelative(node);
            if (tempNode == null) return "NONE";
           List<Node> lst;
           string resultString = "";
           motherChildRelationDictionary.TryGetValue(tempNode.Name,out lst);
           lst = lst.Where(x=> x.Gender == gender).DefaultIfEmpty().ToList<Node>();
           if (lst[0] == null)
           {
               resultString = "NONE";
           }
           else
           {
               resultString = string.Join(" ", lst.Select(x => x.Name));

           
           }
       return resultString;

        }

        static string returnSiblingName(Node tempNode) { 
        string resultString="NONE";
        List<Node> templst = returnSiblingsList(tempNode);
            if(templst==null)
            {
                resultString = "NONE";
            }
            else
            {
                resultString = string.Join(" ", templst.Select(x => x.Name));


            }
      
                
        return resultString;

        }
        static string HandleRelationship(string name, string relation) {
            Node tempNode=FindNode(name);
            if (tempNode == null)
            { 
                return "PERSON_NOT_FOUND";
            }
            string resultstr="";
           
            
            switch (relation) {
                case "Paternal-Uncle":
                    resultstr = FindUncleOrAunt(name, GenderType.Male, "Paternal");
                    break;
                case "Maternal-Uncle":

                    resultstr = FindUncleOrAunt(name, GenderType.Male, "Maternal");
                    break;
                case "Paternal-Aunt":
                    resultstr = FindUncleOrAunt(name, GenderType.Female, "Paternal");
                    break;
                case "Maternal-Aunt":
                    resultstr = FindUncleOrAunt(name, GenderType.Female, "Maternal");
                    break;
                case "Sister-In-Law":
                    resultstr =returnInLawsName(tempNode,GenderType.Female);
                    break;
                case "Brother-In-Law": 
                      resultstr =returnInLawsName(tempNode,GenderType.Male);
                      break;
                case "Son":
                    resultstr = returnChildName(tempNode, GenderType.Male);
                    break;
                case "Daughter":
                    resultstr = returnChildName(tempNode, GenderType.Female);
                    break;
                    
                case "Siblings":
                    resultstr = returnSiblingName(tempNode);
                    break;
                default: break;
            }
            return resultstr;
        
        
        }
        static void DisplayList() {
            foreach (Node node in FamilyList) {
                Console.WriteLine("-----------------------------------");
                Console.WriteLine("Name: "+node.Name);
                Console.WriteLine("Gender: " + node.Gender);
                Console.WriteLine("Mother's Name: " + node.MothersName);
                Console.WriteLine("Spouse Name: " + node.SpouseName);            
            }
        
        }
        static string AddChild(string mothersName,string childName,string gender) {
            Node motherNode = FindNode(mothersName);
            if (motherNode != null)
            {
                List<Node> lst = null;
                motherChildRelationDictionary.TryGetValue(mothersName, out lst);
                if (lst != null)
                {
                    GenderType gen;
                    Enum.TryParse(gender, false, out gen);
                    Node tempNode = new Node(childName, gen, mothersName, null);
                    FamilyList.Add(tempNode);
                    lst.Add(tempNode);
                    motherChildRelationDictionary["mothersName"] = lst;
                    return "CHILD_ADDITION_SUCCEEDED";
                }
                return "CHILD_ADDITION_FAILED"; 
            }
            else
                return "PERSON_NOT_FOUND";
        }

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
           if (args.Length > 0)
           {
               IEnumerable<string> inputLines = File.ReadAllLines(args[0]);
               foreach (string str in inputLines)
               {
                   CallFunction(str);
               }
           }
           else
           {
               Console.WriteLine("No command line arguments found.");
           } 
            Console.ReadLine();
        }
    }
}
