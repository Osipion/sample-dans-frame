using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DansGameCore;
using System.Reflection;
using Frames;

namespace DansGameLib
{
    /// <summary>
    /// Loads frames from Frames.dll
    /// </summary>
    public static class FrameLoader
    {
        /// <summary>
        /// Gets a dictionary of all the live frames defined in Frames.dll
        /// </summary>
        /// <returns></returns>
        public static IDictionary<Guid, BaseGameFrame> LoadFrames()
        {
            var frame_asm = Assembly.GetAssembly(typeof(Frames.TypeDefinedWithinFramesAssemblyOnly));

            var dict = new Dictionary<Guid, BaseGameFrame>();

            bool found_first_frame = false, found_last_frame = false;

            foreach (var t in frame_asm.DefinedTypes)
            {
                var attrs = t.GetCustomAttributes<GameFrameAttribute>();

                if (attrs.Count() < 1)
                    continue;

                if (!attrs.First().IsLive)
                    continue;

                if (!(t.IsSubclassOf(typeof(BaseGameFrame))))
                    throw new FrameLoadException("All types implementing GameFrameAttribute must be derived from BaseGameFrame");

                if (t.GetConstructor(Type.EmptyTypes) == null)
                    throw new FrameLoadException("All types implementing GameFrameAttribute must have a public default constructor");

                if (attrs.First().IsStartFrame)
                {
                    if (!found_first_frame)
                        found_first_frame = true;
                    else
                        throw new FrameLoadException("There may only be one live start frame in the frames assembly");
                }

                if(attrs.First().IsLastFrame)
                {
                    if (!found_last_frame)
                        found_last_frame = true;
                    else
                        throw new FrameLoadException("There may only be one live last frame in the frames assembly");
                }

                var a = attrs.First();

                dict.Add(a.ID, (BaseGameFrame)Activator.CreateInstance(t.AsType()));

            }

            if (!found_first_frame)
                throw new FrameLoadException("There must be at least one live start frame in the frames assembly");

            return dict;
        }
    }

    public class FrameLoadException : SystemException
    {
        public FrameLoadException(string message)
            : base(message) { }
    }
}
