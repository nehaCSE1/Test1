using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShanFamily
{
    interface IFamilyTreeGeneration
    {
         List<Node> GenerateFamilyTree();
         Dictionary<string, List<Node>> GenerateMotherChildDictionary(List<Node> FamilyList);
    }
    class FamilyTreeGeneration : IFamilyTreeGeneration
    {
        public static List<Node> FamilyList = new List<Node>();
        public static Dictionary<string, List<Node>> motherChildRelationDictionary;
        void AddNodeToFamily(Node temp1)
        {
            Node temp2 = null;
            GenderType gen;
            FamilyList.Add(temp1);
            if (temp1.SpouseName != null)
            {
                if (temp1.Gender.ToString() == "Male")
                    gen = GenderType.Female;
                else
                    gen = GenderType.Male;
                temp2 = new Node(temp1.SpouseName, gen, null, temp1.Name);
                FamilyList.Add(temp2);
            }
        }
        public  List<Node> GenerateFamilyTree()
        {

            AddNodeToFamily(new Node("Shan", GenderType.Male, null, "Anga"));

            AddNodeToFamily(new Node("Chit", GenderType.Male, "Anga", "Amba"));
            AddNodeToFamily(new Node("Ish", GenderType.Male, "Anga", null));
            AddNodeToFamily(new Node("Vich", GenderType.Male, "Anga", "Lika"));
            AddNodeToFamily(new Node("Arash", GenderType.Male, "Anga", "Chitra"));
            AddNodeToFamily(new Node("Satya", GenderType.Female, "Anga", "Vyan"));

            AddNodeToFamily(new Node("Dritha", GenderType.Female, "Amba", "Jaya"));
            AddNodeToFamily(new Node("Tritha", GenderType.Female, "Amba", null));
            AddNodeToFamily(new Node("Vritha", GenderType.Male, "Amba", null));
            AddNodeToFamily(new Node("Vila", GenderType.Female, "Lika", null));
            AddNodeToFamily(new Node("Chika", GenderType.Female, "Lika", null));
            AddNodeToFamily(new Node("Jnki", GenderType.Female, "Chitra", "Arit"));
            AddNodeToFamily(new Node("Ahit", GenderType.Male, "Chitra", null));
            AddNodeToFamily(new Node("  Asva", GenderType.Male, "Satya", "Satvy"));
            AddNodeToFamily(new Node("Vyas", GenderType.Male, "Satya", "Krpi"));
            AddNodeToFamily(new Node("Atya", GenderType.Female, "Satya", null));

            AddNodeToFamily(new Node("Yodhan", GenderType.Male, "Dritha", null));
            AddNodeToFamily(new Node("Laki", GenderType.Male, "Jnki", null));
            AddNodeToFamily(new Node("Lavnya", GenderType.Female, "Jnki", null));
            AddNodeToFamily(new Node("Vasa", GenderType.Male, "Satvy", null));
            AddNodeToFamily(new Node("Kriya", GenderType.Male, "Krpi", null));
            AddNodeToFamily(new Node("Krithi", GenderType.Female, "Krpi", null));

        //    motherChildRelationDictionary = FamilyList.Where(x1 => x1.MothersName != null).GroupBy(k => k.MothersName, v => v).ToDictionary(g => g.Key, g => g.ToList());

            return FamilyList;
        }
        public Dictionary<string, List<Node>> GenerateMotherChildDictionary( List<Node> familyMemberList) {

            motherChildRelationDictionary = FamilyList.Where(x1 => x1.MothersName != null).GroupBy(k => k.MothersName, v => v).ToDictionary(g => g.Key, g => g.ToList());
            return motherChildRelationDictionary;
        
        }
    }
}
