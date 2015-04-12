using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DansGameCore;

namespace DansGameLib
{
    /// <summary>
    /// The interface with the user
    /// </summary>
    public interface IScreen
    {
        /// <summary>
        /// returns a decision about which option to chose in the game frame
        /// </summary>
        /// <returns>The key of the selected option</returns>
        int GetDecision(IBaseGameFrame frame);
    }

    /// <summary>
    /// A dummy screen that always returns the first choice passed
    /// </summary>
    public class DummyScreen : IScreen
    {
        /// <summary>
        /// A dummy impementation of IScreen's int GetDecision(IBaseGameFrame frame)
        /// </summary>
        /// <param name="frame"></param>
        /// <returns></returns>
        public int GetDecision(IBaseGameFrame frame)
        {
            return frame.Choices.First().Key;
        }
    }
}
