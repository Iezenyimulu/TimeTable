using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTable {
    public class ClassComparer : IComparer<string> {


        public int Compare(string x, string y) {
            x = x.ToLower();
            y = y.ToLower();


            // js2a js1a || ss2a ss2a
            if (x[0] == y[0]) {
                if (x[2] > y[2])
                    return 1;
                else if (x[2] < y[2])
                    return -1;
                //js2a js2b
                else if (x[2] == y[2]) {
                    if (x[3] > y[3])
                        return 1;
                    else if (x[3] < y[3])
                        return -1;
                    //else if (x[3] == y[3])
                    //    return 0;
                }
                //js1a js1a
                return 0;
            }
            // ss2a js1a
            else if (x[0] > y[0])
                return 1;
            //else if (x[0] < y[0])
            //    return -1;
            else return -1;


        }

    }
}
