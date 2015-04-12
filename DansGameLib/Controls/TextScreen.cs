using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DansGameCore;

namespace DansGameLib.Controls
{
    public class TextScreen : IScreen
    {
        public string StandardGameFrameHeader
        {
            get
            {
                return String.Format("{0}{1}{2}{1}{0}", Environment.NewLine, new String('*', 17), DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"));
            }
        }

        public string StandardOptionHeader
        {
            get
            {
                return String.Format("{0}\tOptions:{1}{0}", Environment.NewLine, new String('-', 25));
            }
        }

        public TextScreen()
        {
            GameManager.TurnOffConsoleLogging();
            this.GameFrameHeader = StandardGameFrameHeader;
            this.OptionHeader = StandardOptionHeader;
        }

        public string GameFrameHeader { get; set; }

        public string OptionHeader { get; set; }

        public int GetDecision(IBaseGameFrame frame)
        {
            var sb = new StringBuilder(this.GameFrameHeader);
            sb.Append(frame.Message);
            sb.Append(this.OptionHeader);
            foreach(var option in frame.Choices.OrderBy(x => x.Key))
                sb.AppendFormat("{0} - {1}{2}{1}", option.Key, Environment.NewLine, option.Value.Message);

            sb.Append("Enter the number of the choice you want to make:");

            Console.Write(sb.ToString());

            request_input:

            var inp = Console.ReadLine();

            int choice;

            if (inp.ToLowerInvariant() == "exit")
                return GameLoop.ExitOptionCode;

            if (inp.ToLowerInvariant() == "new game")
                return GameLoop.NewGameOptionCode;

            if(!Int32.TryParse(inp, out choice))
            {
                Console.WriteLine("The input must be numeric. Try again..");
                goto request_input;
            }

            if(!frame.Choices.ContainsKey(choice))
            {
                Console.WriteLine("The input does not match any option. Try again...");
                goto request_input;
            }

            return choice;

        }
    }
}
