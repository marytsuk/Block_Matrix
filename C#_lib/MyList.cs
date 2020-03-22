using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csh_lib
{
    public class MyList : List<double>
    {
        public MyList(int x) : base(x)
        {
            for (int i = 0; i < x; i++)
            {
                this.Add(0);
            }
        }
        public MyList() : base()
        {
        }
        public static MyList operator-(MyList x1, MyList x2)
        {
            MyList res = new MyList(x1.Count);
            for (int i = 0; i < x1.Count(); ++i)
            {
                res[i] = x1[i] - x2[i];
            }
            return res;
        }
    }
}
