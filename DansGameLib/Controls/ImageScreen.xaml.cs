using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DansGameCore;

namespace DansGameLib.Controls
{
    /// <summary>
    /// Interaction logic for Screen.xaml
    /// </summary>
    public partial class ImageScreen : Window
    {
        private static log4net.ILog logger = log4net.LogManager.GetLogger(typeof(ImageScreen));

        public event Action<int> OnChoiceMade;

        private void on_choice_made(int choice)
        {
            if (this.OnChoiceMade != null)
                this.OnChoiceMade(choice);
        }

        public ImageScreen()
        {
            InitializeComponent();
            this.Width = 400;
            this.Height = 600;
            logger.Info("New ImageScreen initialized");
        }

        public void DisplayFrame(IBaseGameFrame frame)
        {
            if (frame is DisplayableGameFrame)
            {
                var d_frame = frame as DisplayableGameFrame;

                try
                {
                    this.imgMainImage.Source = d_frame.Image.ToBitmapImage();
                }
                catch (Exception e)
                {
                    logger.Error("Could not set the image source for frame " + d_frame.ID.ToString(), e);
                }
            }
            else
            {
                logger.WarnFormat("The supplied frame {0} was not displayable", frame.ID);
            }

            try
            {
                this.FillOptions(frame.Choices);
            }
            catch (Exception e)
            {
                MessageBox.Show("An exception was thrown loading the options for this frame - \r\n\r\n" + e.ToString());
                throw;
            }

            this.labMessage.Text = frame.Message;
        }

        

        public void FillOptions(IDictionary<int, Option> options)
        {
            this.lbOptions.Items.Clear();

            foreach(var opt in options)
            {
                if(opt.Value.GetType().IsAssignableFrom(typeof(DisplayableOption)))
                {
                    var d_opt = (DisplayableOption)opt.Value;
                    this.lbOptions.Items.Add(new OptionDisplayer(opt.Key, d_opt));
                }
                else
                {
                    logger.Warn("An option was not displayable, so a new DisplayableOption was created");

                    var d_new_opt = new DisplayableOption()
                    {
                        DisplayID = opt.Key,
                        Message = opt.Value.Message,
                        NextFrameID = opt.Value.NextFrameID,
                        Image = null
                    };

                    this.lbOptions.Items.Add(new OptionDisplayer(opt.Key, d_new_opt));
                }
            }
        }

        private void labSelect_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var item = this.lbOptions.SelectedItem as OptionDisplayer;

            if(item == null)
            {
                logger.Info("Selection option was pressed, but no option had been selected");
                return;
            }

            this.on_choice_made(item.Key);
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void labNewGame_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            this.on_choice_made(GameLoop.NewGameOptionCode);
        }

        private void labSave_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            this.on_choice_made(GameLoop.SaveOverCharacterOptionCode);
        }

        private void labQuit_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            this.on_choice_made(GameLoop.GotoExitFrameOptionCode);
        }

    }

    public class OptionDisplayer
    {
        private DisplayableOption inner_option = null;

        public BitmapSource Image { get { return this.inner_option.Image == null ? null : this.inner_option.Image.ToBitmapImage(); } }
        public string Message { get { return this.inner_option.Message; } }
        public int Key { get; private set; }
        public int DisplayId { get { return this.inner_option.DisplayID; } }

        public OptionDisplayer(int key, DisplayableOption option)
        {
            if (option == null)
                throw new ArgumentNullException();

            this.Key = key;

            this.inner_option = option;
        }
    }

    public class GraphicalScreen : IScreen
    {
        private static log4net.ILog logger = log4net.LogManager.GetLogger(typeof(GraphicalScreen));

        private ImageScreen window = null;

        private System.Threading.Thread window_thread = null;

        private int next_choice = 0;

        private System.Threading.ManualResetEvent next_choice_ready = new System.Threading.ManualResetEvent(false);

        private System.Threading.ManualResetEvent window_initialized = new System.Threading.ManualResetEvent(false);

        private void start_window()
        {

            var can_start = this.window_thread == null ? true : !this.window_thread.IsAlive;

            if(this.window == null && can_start)
            {
                this.window_initialized.Reset();

                logger.Info("Starting window on secondary thread");

                window_thread = new System.Threading.Thread(window_work);

                window_thread.SetApartmentState(System.Threading.ApartmentState.STA);

                window_thread.Start();

                logger.Info("Started waiting for window to be initialized");

                this.window_initialized.WaitOne();
            } 
        }

        private void window_work()
        {
            this.window = new ImageScreen();

            logger.Info("Displaying wpf control on secondary thread");

            this.window.OnChoiceMade += (c) =>
                {
                    logger.InfoFormat("Recieved notfication of choice being made - choice was {0}", c);
                    this.next_choice = c;
                    this.next_choice_ready.Set();
                };

            this.window_initialized.Set();

            this.window.ShowDialog();

            this.window = null;
            
        }

        public GraphicalScreen()
        {
            logger.Info("New GraphicalScreen created");
        }

        public int GetDecision(IBaseGameFrame frame)
        {

            if (this.window == null)
                this.start_window();

            this.window.Dispatcher.Invoke(() => this.window.DisplayFrame(frame));

            logger.Info("Started waiting for choice response from window");

            this.next_choice_ready.WaitOne();

            logger.Info("Choice response available. Resetting wait handle");

            this.next_choice_ready.Reset();

            return this.next_choice;
        }
    }
}
