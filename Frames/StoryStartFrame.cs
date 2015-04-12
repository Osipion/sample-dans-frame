using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DansGameCore;

namespace Frames
{
    [GameFrame("6F08A178-107F-4A69-91D1-EABE7F7649DE", true)]
    public sealed class StoryStartFrame : DisplayableGameFrame
    {
        public static Guid StoryStartFrameID = Guid.Parse("6F08A178-107F-4A69-91D1-EABE7F7649DE");

        public override Guid ID
        {
            get
            {
                return StoryStartFrame.StoryStartFrameID;
            }
        }

        public StoryStartFrame()
        {
            base.Choices.Add(1, new DisplayableOption() { Message = "Open Door...", NextFrameID = SampleFrame.SampleFrameID, DisplayID = 0 });
            base.Choices.Add(2, new DisplayableOption() { Message = "Rest a little while longer...", NextFrameID = SampleFrame.SampleFrameID, DisplayID = 1 });

            base.Message = "You awake, dazed, to find yourself in an unfamilliar room. It's dark, but from your bed you can see a lights flashing beyond the door.";
            base.Image = (System.Drawing.Bitmap)Properties.Resources.StartFrameImg.Clone();
        }

        public override void MakeDecision(int option, ICharacter character_context)
        {
            base.MakeDecision(option, character_context);

            if(option == 2)
            {
                character_context.Counters.AddOrUpdate("rests", new Counters.AddOrUpdateValueEvaluatorDelegate((found, val) => ++val));
            }
        }
    }
}
