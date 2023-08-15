using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LINQ
{
    public class Animal
    {
        public string? Name { get; set; }
        public string? Color { get; set; }

        public override string ToString()
        {
           return $"{Name,-60} {Color, 15}";
        }
    }
}