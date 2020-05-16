using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlmostBinary_Runtime
{
    class Var
    {
        public string name;
        public object value;
        public int scope = 1;

        public Var(string n)
        {
            name = n;
            value = null;
        }
    }
}
