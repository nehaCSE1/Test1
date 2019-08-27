using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShanFamily
{
    enum GenderType { 
    Male,
    Female
    }
    class Node
    {
        string Name;
        GenderType Gender;
        string MothersName;
        string SpouseName;
        public Node(string name, GenderType gender, string mothersName, string spouseName)
        {
            Name = name;
            Gender = gender;
            MothersName = mothersName;
            SpouseName = spouseName;
        }
    //}
    //class FamilyTree {

        public static List<Node> FamilyList = new List<Node>();
        public static Dictionary<string, List<Node>> motherChildRelationDictionary = new Dictionary<string, List<Node>>();
        
       static Node FindNode(string name)
       {
           return FamilyList.Where(x => x.Name == name).FirstOrDefault<Node>(); ;
       }
       static Node FindGrandMother(string name)
       {
           Node tempNode = FindNode(name);
           if (tempNode == null) { Console.WriteLine("PERSON_NOT_FOUND"); }
           else
           {
               tempNode = FindNode(tempNode.MothersName);
               if (tempNode.MothersName == null)
               {
                   tempNode = FindNode(tempNode.SpouseName);
                   if (tempNode == null)
                   {
                       return tempNode;
                   }
                   else
                       tempNode = FindNode(tempNode.MothersName);
               }
               else
               {
                   tempNode = FindNode(tempNode.MothersName);
               }
           }
           return tempNode;
       }

       static string FindUncleOrAunt(string name, GenderType relativeGender, string relativeSide)
       {
           string parentName = "";
           string resultString = "";
           Node tempNode = FindGrandMother(name);
           List<Node> lst;
           if (tempNode == null) { return ("NONE"); }
           else
           {

               switch (relativeSide)
               {
                   case "Paternal":
                       parentName = FindNode(FindNode(name).MothersName).SpouseName;
                       break;
                   case "Maternal":
                       parentName = FindNode(name).MothersName;
                       break;

               }
               motherChildRelationDictionary.TryGetValue(tempNode.Name, out lst);
               lst = lst.Where(x => x.Name != parentName && x.Gender == relativeGender).ToList<Node>();
               if (lst.Count == 0)
               {
                   resultString = "NONE";
               }
               else
               {
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
               if (templst != null)
               {
                   lst = templst.ToList<Node>();
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
       static string returnChildName(Node node, GenderType gender)
       {
           Node tempNode = mapToFemaleRelative(node);
           if (tempNode == null) return "NONE";
           List<Node> lst;
           string resultString = "";
           motherChildRelationDictionary.TryGetValue(tempNode.Name, out lst);
           lst = lst.Where(x => x.Gender == gender).DefaultIfEmpty().ToList<Node>();
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
       static string returnSiblingName(Node tempNode)
       {
           string resultString = "NONE";
           List<Node> templst = returnSiblingsList(tempNode);
           if (templst.Capacity == 0)//t==null ||templst[0] == null)
           {
               resultString = "NONE";
           }
           else
           {
               resultString = string.Join(" ", templst.Select(x => x.Name));
           }

           return resultString;

       }
    }
 
}
