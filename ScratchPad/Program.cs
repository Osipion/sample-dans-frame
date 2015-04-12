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

            //var register = DansGameLib.TypeRegister.Load(@"C:\Users\Tom\Documents\Visual Studio 2013\Projects\DansGameCore\DansGameLib\TypeRegistry.xml");

            //var lst = register.Entries.ToList();

            //lst.Add( new TypeRegisterEntry(){ CreationIndex = 2, Value = new DansGameCore.Serialization.TypeLookupItem(typeof(IGameLoop), typeof(GameLoop)) });

            //register.Entries = lst.ToArray();

            //register.Save(@"C:\Users\Tom\Documents\Visual Studio 2013\Projects\DansGameCore\DansGameLib\TypeRegistry.xml");
            var s = GameManager.Get<IGameLoop>();

            s.Run(true);

            //var str = typeof(IGameLoop).AssemblyQualifiedName;
        }
    }
}
