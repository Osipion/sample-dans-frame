using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DansGameCore;

namespace Frames
{
    [GameFrame("5FC67782-E105-485C-A3D5-E98FAF1F31F8", true, false, true)]
    public sealed class QuitFrame : DisplayableGameFrame
    {
        public static Guid QuitFrameID = Guid.Parse("5FC67782-E105-485C-A3D5-E98FAF1F31F8");

        public override Guid ID
        {
            get
            {
                return QuitFrame.QuitFrameID;
            }
        }

        public QuitFrame()
        {
            base.Choices.Add(1, new DisplayableOption() { Message = "Quitting. Progress will be saved...", NextFrameID = SampleFrame.SampleFrameID, DisplayID = 0 });

            base.Message = "Quiting Game.";

            base.Image = (System.Drawing.Bitmap)Properties.Resources.BlankFrameImage.Clone();
        }

        public override void MakeDecision(int option, ICharacter character_context)
        {
            base.MakeDecision(option, character_context);
        }
    }
}
