using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DansGameCore;
using System.Drawing;
using System.Windows.Interop;

namespace Frames
{
    /// <summary>
    /// This class must be included in this assembly for the loader
    /// </summary>
    public class TypeDefinedWithinFramesAssemblyOnly
    {

    }

    [GameFrame("DD311DD1-96A4-4267-9B12-D9CB8F296CD1", true)]
    public sealed class SampleFrame : DisplayableGameFrame
    {
        public static Guid SampleFrameID = Guid.Parse("DD311DD1-96A4-4267-9B12-D9CB8F296CD1");

        public override Guid ID
        {
            get
            {
                return SampleFrame.SampleFrameID;
            }
        }

        public SampleFrame()
        {
            base.Choices.Add(1, new DisplayableOption() { Message = "Action1", NextFrameID = QuitFrame.QuitFrameID, DisplayID = 0 });
            base.Choices.Add(2, new DisplayableOption() { Message = "Action2", NextFrameID = QuitFrame.QuitFrameID, DisplayID = 1 });

            base.Message = "Sample Frame!";
            base.Image = (System.Drawing.Bitmap)Properties.Resources.BlankFrameImage.Clone();
        }

        public override void MakeDecision(int option, ICharacter character_context)
        {
            base.MakeDecision(option, character_context);
        }
    }

    /// <summary>
    /// Contains extension methods for existing classes
    /// </summary>
    public static class Extensions
    {
        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        private static extern bool DeleteObject(IntPtr hObject);

        private static log4net.ILog logger = log4net.LogManager.GetLogger(typeof(Extensions));

        public static void ModifyAppenders<T>(this log4net.Repository.ILoggerRepository repository, Action<T> modify) where T : log4net.Appender.AppenderSkeleton
        {
            var appenders = from appender in log4net.LogManager.GetRepository().GetAppenders()
                            where appender is T
                            select appender as T;

            foreach (var appender in appenders)
            {
                modify(appender);
                appender.ActivateOptions();
            }
        }

        public static System.Windows.Media.Imaging.BitmapSource ToBitmapImage(this System.Drawing.Bitmap bitmap)
        {

            var clock = System.Diagnostics.Stopwatch.StartNew();

            IntPtr hBitmap = bitmap.GetHbitmap();
            System.Windows.Media.Imaging.BitmapSource retval;

            try
            {
                retval = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                             hBitmap,
                             IntPtr.Zero,
                             System.Windows.Int32Rect.Empty,
                             System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());
            }
            finally
            {
                DeleteObject(hBitmap);
            }

            clock.Stop();

            logger.InfoFormat("It took {0} ms to convert a System.Drawing.Bitmap to a System.Windows.Media.Imaging.BitmapSource", clock.ElapsedMilliseconds);

            return retval;
        }
    }

    
}
