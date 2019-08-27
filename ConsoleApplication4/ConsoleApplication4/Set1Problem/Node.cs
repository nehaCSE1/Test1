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
        public string Name { get; set; }
        public GenderType Gender { get; set; }
        public string MothersName { get; set; }
        public string SpouseName { get; set; }
        public Node(string name, GenderType gender,string mothersName,string spouseName)
	{
        Name = name;
        Gender = gender;
        MothersName = mothersName;
        SpouseName = spouseName;
	}
    }
 
}
