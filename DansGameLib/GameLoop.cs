using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DansGameCore;

namespace DansGameLib
{
    /// <summary>
    /// Represents the fundemental game loop
    /// </summary>
    public class GameLoop
    {
        private static log4net.ILog logger = log4net.LogManager.GetLogger(typeof(GameLoop));

        private IDictionary<Guid, BaseGameFrame> frames = null;

        /// <summary>
        /// Gets a collection of all live frames in Frames.dll
        /// </summary>
        public IDictionary<Guid, BaseGameFrame> Frames
        {
            get
            {
                if (this.frames == null)
                {
                    logger.Info("Begining to load frames from file");

                    var clock = System.Diagnostics.Stopwatch.StartNew();

                    this.frames = FrameLoader.LoadFrames();

                    clock.Stop();

                    logger.InfoFormat("It took {0}ms to load all live frames from the Frames library", clock.ElapsedMilliseconds);
                }

                return frames;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GameLoop"/> class
        /// </summary>
        public GameLoop()
        {

        }

        /// <summary>
        /// If this value is returned as an option decision from an IScreen object, 
        /// the GameLoop exits the game and attempts a save with backup
        /// </summary>
        public const int ExitOptionCode = Int32.MinValue;

        /// <summary>
        /// If this value is returned as an option decision from an IScreen object, 
        /// a new game is created, and the existing character backed up
        /// </summary>
        public const int NewGameOptionCode = Int32.MinValue + 1;

        /// <summary>
        /// if this value is returned as an option decision from an IScreen object, 
        /// the character is saved, overwriting any existing current character. 
        /// The game frame is not advanced.
        /// </summary>
        public const int SaveOverCharacterOptionCode = Int32.MinValue + 2;

        /// <summary>
        /// if this value is returned as an option decision from an IScreen object, 
        /// the character is saved, backing up any existing current character. 
        /// The game frame is not advanced.
        /// </summary>
        public const int SaveAndBackupCharacterOptionCode = Int32.MinValue + 3;

        /// <summary>
        /// if this value is returned as an option decision from an IScreen object, 
        /// the last frame (Exit Frame) is displayed next - if it exists.
        /// Exit frames are not recoded as decisions for the character. 
        /// If no exit frame exists, the game simply exits
        /// </summary>
        public const int GotoExitFrameOptionCode = Int32.MinValue + 4;

        /// <summary>
        /// Starts the game! Uses whatever screen is currently configured
        /// </summary>
        /// <param name="new_character"></param>
        public void Run(bool new_character = false)
        {
            BaseGameFrame current_frame = null;

            var character = GameManager.Get<ICharacter>();

            if (!character.IsInitialized || new_character || character.Count() < 1)
            {
                logger.Info("No valid character was found or new character parameter was true. Requesting new character be created");
                character = GameManager.NewCharacter();
                current_frame = this.FindFirstFrame();
            }
            else
            {
                var last_decision = character.OrderBy(x => x.DecisionHistoryOrder).Last();
                current_frame = this.Frames[last_decision.ChosenFrame];
            }

            var screen = GameManager.Get<IScreen>();

            while(true)
            {
                var opt = screen.GetDecision(current_frame);

                if(this.IsLastFrame(current_frame))
                {        
                    logger.InfoFormat("Game ended because current frame is set as last frame - {0}", current_frame.ID);  
                    break;
                }

                current_frame.MakeDecision(opt, character);

                if(opt == GameLoop.ExitOptionCode)
                {
                    logger.Info("Screen requested exit");
                    break;
                }

                if (opt == GameLoop.GotoExitFrameOptionCode)
                {
                    logger.Info("Screen requested go to last frame");

                    var f = this.Frames.Values.FirstOrDefault(x => this.IsLastFrame(x));

                    if(f == null)
                    {
                        logger.Info("No last frame found. Exiting");
                        break;
                    }

                    current_frame = f;
                    continue;
                }

                if(opt == GameLoop.NewGameOptionCode)
                {
                    logger.Info("Screen requested new game");
                    character = GameManager.NewCharacter();
                    current_frame = this.FindFirstFrame();
                    continue;
                }

                if (opt == GameLoop.SaveOverCharacterOptionCode)
                {
                    logger.Info("Screen requested save over character. Frame not advanced this loop");
                    GameManager.SaveCurrentCharacter(false);
                    continue;
                }

                if (opt == GameLoop.SaveOverCharacterOptionCode)
                {
                    logger.Info("Screen requested save character with backup. Frame not advanced this loop");
                    GameManager.SaveCurrentCharacter();
                    continue;
                }

                if (!current_frame.Choices.ContainsKey(opt))
                {
                    logger.InfoFormat("Game ended because no option could be found with ID {0}", opt);
                    break;
                }

                var next_id = current_frame.Choices[opt].NextFrameID;

                if (!this.Frames.ContainsKey(next_id))
                {
                    logger.InfoFormat("Game ended because no frame could be found with ID {0}", next_id);
                    break;
                }

                current_frame = this.Frames[current_frame.Choices[opt].NextFrameID];

                character.StoreDecision(current_frame.ID);
            }

            GameManager.Quit();
 
        }

        private BaseGameFrame FindFirstFrame()
        {
            foreach(var frame in this.Frames.Values)
            {
                if (((GameFrameAttribute)frame.GetType().GetCustomAttributes(typeof(GameFrameAttribute), true)[0]).IsStartFrame)
                    return frame;
            }

            throw new InvalidOperationException("No first frame could be found");
        }

        private bool IsLastFrame(BaseGameFrame frame)
        {
            var attrs = frame.GetType().GetCustomAttributes(typeof(GameFrameAttribute), true);

            foreach(var attr in attrs)
            {
                if (((GameFrameAttribute)attr).IsLastFrame)
                    return true;
            }

            return false;
        }
    }
}
