using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DansGameCore
{

    public interface IBaseGameFrame
    {
        Guid ID { get; }

        IDictionary<int, Option> Choices { get; }

        string Message { get; }

        void MakeDecision(int option, ICharacter character_context);

    }

    public class Option
    {
        public string Message { get; set; }

        public Guid NextFrameID { get; set; }
    }

    public class DisplayableOption : Option
    {
        public int DisplayID { get; set; }

        public System.Drawing.Bitmap Image { get; set; }
    }


    public abstract class BaseGameFrame : IBaseGameFrame
    {
        private static log4net.ILog logger = log4net.LogManager.GetLogger(typeof(BaseGameFrame));

        public virtual Guid ID
        {
            get
            {
                throw new NotImplementedException("Derived classes must implement thier own id getter");
            }
        }

        public string Message { get; protected set; }

        public IDictionary<int, Option> Choices { get; protected set; }

        protected BaseGameFrame()
        {
            this.Choices = new Dictionary<int, Option>();

            logger.InfoFormat("BaseGameFrame was initialized - {0}", this.GetType());
        }

        public virtual void MakeDecision(int option, ICharacter character_context)
        {
        }
        
    }

    public abstract class DisplayableGameFrame : BaseGameFrame
    {
        public System.Drawing.Bitmap Image { get; protected set; }

        protected DisplayableGameFrame()
        {

        }
    }

    public sealed class GameFrameAttribute : System.Attribute
    {
        public bool IsLive { get; private set; }

        public string IDString { get; private set; }

        public Guid ID { get { return Guid.Parse(this.IDString); } }

        public bool IsStartFrame { get; private set; }

        public bool IsLastFrame { get; private set; }

        public GameFrameAttribute(string Id, bool IsLive = true, bool IsStartFrame = false, bool IsLastFrame = false)
        {
            if (Id == null)
                throw new ArgumentNullException("GameFrame Id cannot be null");

            this.IDString = Id;
            this.IsLive = IsLive;
            this.IsStartFrame = IsStartFrame;
            this.IsLastFrame = IsLastFrame;

        }
    }
}
