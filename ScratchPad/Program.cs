using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DansGameCore;
using DansGameLib;

namespace ScratchPad
{
    class Program
    {
        static void Main(string[] args)
        {
            var s = new GameLoop();

            s.Run(true);

        }
    }
}
