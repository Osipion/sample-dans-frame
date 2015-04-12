using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DansGameCore;

namespace Frames
{
    [GameFrame("C4608A73-774A-4153-A30C-21899F549FD1", true, true)]
    public sealed class FirstFrame : DisplayableGameFrame
    {
        public static Guid FirstFrameID = Guid.Parse("C4608A73-774A-4153-A30C-21899F549FD1");

        public override Guid ID
        {
            get
            {
                return FirstFrame.FirstFrameID;
            }
        }

        public FirstFrame()
        {
            base.Choices.Add(1, new DisplayableOption() { Message = "Start?", NextFrameID = StoryStartFrame.StoryStartFrameID, DisplayID = 0 });
            base.Choices.Add(2, new DisplayableOption() { Message = "Quit...", NextFrameID = QuitFrame.QuitFrameID, DisplayID = 1 });

            base.Message = "Start Of Game!";

            base.Image = (System.Drawing.Bitmap)Properties.Resources.BlankFrameImage.Clone();
        }

        public new void MakeDecision(int option, ICharacter character_context)
        {
            base.MakeDecision(option, character_context);
        }
    }
}
