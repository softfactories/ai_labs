using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sf.utils.text
{
    public static class ExtFunc
    {
        public static void Say(string msg)
        {
            System.Console.WriteLine(msg);
        }

        public static string Read(string msg)
        {
            Say(msg);
            return System.Console.ReadLine();
        }
    }
}
