using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DansGameCore
{
    /// <summary>
    /// The interface game frames should implement
    /// </summary>
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

    /// <summary>
    /// The basic game frame class, from which all grame frames should be derived
    /// </summary>
    public abstract class BaseGameFrame : IBaseGameFrame
    {
        private static log4net.ILog logger = log4net.LogManager.GetLogger(typeof(BaseGameFrame));

        /// <summary>
        /// The unique identifier of the frame - must be overridden by derived types
        /// </summary>
        public virtual Guid ID
        {
            get
            {
                throw new NotImplementedException("Derived classes must implement thier own id getter");
            }
        }

        /// <summary>
        /// The message of the frame
        /// </summary>
        public string Message { get; protected set; }


        /// <summary>
        /// A collection of the choice available from the frame
        /// </summary>
        public IDictionary<int, Option> Choices { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseGameFrame"/> class
        /// </summary>
        protected BaseGameFrame()
        {
            this.Choices = new Dictionary<int, Option>();

            logger.InfoFormat("BaseGameFrame was initialized - {0}", this.GetType());
        }

        /// <summary>
        /// When overriden in a derived class, allows the frame to take extra actions on the character when a decision is made.
        /// </summary>
        /// <param name="option"></param>
        /// <param name="character_context"></param>
        public virtual void MakeDecision(int option, ICharacter character_context)
        {
        }
        
    }

    /// <summary>
    /// A game frame with an image to display in graphical screens
    /// </summary>
    public abstract class DisplayableGameFrame : BaseGameFrame
    {
        /// <summary>
        /// Gets the image associated with this frame
        /// </summary>
        public System.Drawing.Bitmap Image { get; protected set; }

        /// <summary>
        /// Initiazlies a new instance of the <see cref="DisplayableGameFrame"/> class
        /// </summary>
        protected DisplayableGameFrame()
        {

        }
    }

    /// <summary>
    /// An attribute which should be applied to all game frames
    /// </summary>
    public sealed class GameFrameAttribute : System.Attribute
    {
        /// <summary>
        /// Gets a flag indicating whether this frame will be loaded at runtime
        /// </summary>
        public bool IsLive { get; private set; }

        /// <summary>
        /// Gets the uniqiue identifier of the frame as a string
        /// </summary>
        public string IDString { get; private set; }

        /// <summary>
        /// The unique Identifier of the frame
        /// </summary>
        public Guid ID { get { return Guid.Parse(this.IDString); } }

        /// <summary>
        /// Indicates that this frame is the start frame. There may only be one live start frame in the frames assembly
        /// </summary>
        public bool IsStartFrame { get; private set; }

        /// <summary>
        /// Indicates that this frame is the last frame. The last frame does not record a decision, and only one live last frame may exist in the frame assembly
        /// </summary>
        public bool IsLastFrame { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GameFrameAttribute"/> class
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="IsLive"></param>
        /// <param name="IsStartFrame"></param>
        /// <param name="IsLastFrame"></param>
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
