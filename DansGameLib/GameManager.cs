using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DansGameCore;
using DansGameCore.Serialization;
using System.Runtime.InteropServices;

namespace DansGameLib
{
    /// <summary>
    /// This class manages the game! It configures the objects you want to use in your game
    /// </summary>
    public sealed class GameManager
    {
        private static log4net.ILog logger = log4net.LogManager.GetLogger(typeof(GameManager));

        private static volatile GameManager instance = null;
        private static volatile object instance_locker = new object();

        /// <summary>
        /// Gets the underlying singleton GameManager instance
        /// </summary>
        public static GameManager Manager
        {
            get
            {
                if (GameManager.instance == null)
                {
                    lock(GameManager.instance_locker)
                    {
                        if (GameManager.instance == null)
                            GameManager.instance = new GameManager();
                    }
                }
                return GameManager.instance;
            }
        }

        private Dictionary<Type, TypeLookupItem> type_lookup = new Dictionary<Type, TypeLookupItem>();

        private GameManager()
        {
            if(!log4net.LogManager.GetRepository().Configured)
            {
                using (var reader = new System.IO.MemoryStream(Encoding.UTF8.GetBytes(Properties.Resources.test_logging_config)))
                    log4net.Config.XmlConfigurator.Configure(reader);

                logger.InfoFormat("{0}{0}{1}{0}BEGINS LOG ON GAME MANAGER CREATION{0}{1}{0}{0}", Environment.NewLine, new String('*', 40));
            }

            if (Properties.Settings.Default.UseDefaultTypeRegistry)
            {
                using (var mem_stream = new System.IO.MemoryStream(Encoding.UTF8.GetBytes(Properties.Resources.TypeRegistry)))
                {
                    var register = TypeRegister.Load(mem_stream);
                    this.RegisterTypeRegister(register, true);
                }
                logger.Info("Initialized TypeRegister with default template");
            }

            switch(Properties.Settings.Default.ScreenToUse)
            {
                case "TextScreen":
                    logger.Info("Starting Text Console as IScreen");
                    this.RegisterTypeEntry<IScreen, Controls.TextScreen>(new Controls.TextScreen(), true, true);
                    break;
                case "GraphicalScreen":
                    logger.Info("Starting GraphicalScreen as IScreen");
                    this.RegisterTypeEntry<IScreen, Controls.GraphicalScreen>(new Controls.GraphicalScreen(), true, true);
                    break;
                default:
                    logger.Warn("NO SCREEN SELECTED - using dummy screen");
                    this.RegisterTypeEntry<IScreen, DummyScreen>(new DummyScreen(), true, true);
                    break;
            }

            logger.Info("GameManager instance created");
        }

        /// <summary>
        /// Registers a <see cref="Serialization.TypeLookupItem"/> with the game manager, meaning that the manager can now create instances of the specified type on request
        /// </summary>
        /// <param name="item">The item to add</param>
        /// <param name="overwite">Whether or not to overwrite any existing key with the same interface type - true will allow an over write</param>
        internal void RegisterTypeEntry(TypeLookupItem item, bool overwite = false)
        {
            if(!item.IsAssigned)
                throw new ArgumentException("The lookup item must be assigned");


            if (this.type_lookup.ContainsKey(item.InterfaceType))
            {
                if (overwite)
                    this.type_lookup[item.InterfaceType] = item;
                else
                    throw new ArgumentException("The specified interface type is already registered");
            }
            else
            {
                this.type_lookup.Add(item.InterfaceType, item);
            }

            logger.InfoFormat("Registered interface type {0} to class type {1}", item.InterfaceTypeName, item.ClassTypeName);
        }

        /// <summary>
        /// Register a Type with the GameManager by passing the interface and class type as parameters.
        /// </summary>
        /// <typeparam name="TInterface">The interface type (the key)</typeparam>
        /// <typeparam name="TClass">The class type (the type of object to instatiate when passed the interface key</typeparam>
        /// <param name="instance">
        /// <para>
        /// A currently existing instance of the class. If is_singleton is set to true, then this will be the only instance returned 
        /// until the Type is unregistered. If not, it will be the first instance returned by the next call to GameManger.Get&lt&gt()
        /// </para>
        /// </param>
        /// <param name="is_singleton">If set to true, the first instance of the class created will be the only one returned.</param>
        /// <param name="overwrite"></param>
        public void RegisterTypeEntry<TInterface, TClass>(TClass instance = null, bool is_singleton = false, bool overwrite = false) 
            where TClass : class, new()
            where TInterface : class
        {
            this.RegisterTypeEntry(new TypeLookupItem(typeof(TInterface), typeof(TClass), instance, is_singleton), overwrite);
        }

        /// <summary>
        /// Tries to find the specified interface in the type registry. 
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <param name="item">If the interface is found, item will be assigned the an instance of that interface, otherwise it will be null /default</param>
        /// <returns>True if found, flase if not</returns>
        public bool TryGet<TInterface>(out TInterface item)
        {
            if (!this.type_lookup.ContainsKey(typeof(TInterface)))
            {
                item = default(TInterface);
                return false;
            }

            item = (TInterface)this.type_lookup[typeof(TInterface)].Instance;

            return true;
        }

        /// <summary>
        /// Returns an instance of the specified interface - the same as GameManager.Get&lt&gt()
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <returns></returns>
        public TInterface GetInstance<TInterface>()
        {
            var type = typeof(TInterface);

            if (!this.type_lookup.ContainsKey(type))
                throw new ArgumentException("The specified interface (" + type.AssemblyQualifiedName + ") was not registered");

            return (TInterface)this.type_lookup[type].Instance;
        }

        /// <summary>
        /// Returns an instance of the specified interface
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Get<T>()
        {
            return GameManager.Manager.GetInstance<T>();
        }

        /// <summary>
        /// Registers a type with the specified properties in the type registry
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <typeparam name="TClass"></typeparam>
        /// <param name="instance"></param>
        /// <param name="is_singleton"></param>
        /// <param name="overwrite"></param>
        public static void RegisterType<TInterface, TClass>(TClass instance = null, bool is_singleton = false, bool overwrite = false) 
            where TClass : class, new()
            where TInterface : class
        {
            GameManager.Manager.RegisterTypeEntry<TInterface, TClass>(instance, is_singleton, overwrite);
        }

        /// <summary>
        /// Creates an entirely new character, overwriting any existing singleton instance currently loaded
        /// </summary>
        /// <param name="make_singleton">specifies whether the class should be a singleton. Usually should be true</param>
        /// <returns></returns>
        public static ICharacter NewCharacter(bool make_singleton = true, bool backup_current_character_file = true)
        {

            logger.InfoFormat("Request to make new character recieved. Singleton? = {0}", make_singleton);

            var inner = new SerializableCharacter();

            GameManager.Manager.RegisterTypeEntry<ISerializableCharacter, SerializableCharacter>(inner, make_singleton, true);

            var c = new Character(inner);

            GameManager.instance.RegisterTypeEntry<ICharacter, Character>(c, make_singleton, true);

            GameManager.SaveCurrentCharacter(backup_current_character_file);

            var ret = GameManager.Get<ICharacter>();

            logger.InfoFormat("New character instance was registered with the GameManager - id = {0}", ret.ID);

            return ret;
        }

        /// <summary>
        /// Saves the current character to file
        /// </summary>
        /// <param name="backup_existing">If an current character file already exists, this flag indicates whether to try to back it up</param>
        public static void SaveCurrentCharacter(bool backup_existing = true)
        {
            ICharacter character;

            if (GameManager.Manager.TryGet<ICharacter>(out character))
            {
                var target = System.IO.Path.ChangeExtension(Properties.Resources.CharacterFileName, SerializableCharacter.CharacterFileExtension);

                if (backup_existing)
                {
                    if (System.IO.File.Exists(target))
                    {
                        try
                        {
                            var c = SerializableCharacter.Load(target);

                            var bck_up_trgt = System.IO.Path.ChangeExtension("Char" + c.ID.ToString(), SerializableCharacter.CharacterFileExtension);

                            if(System.IO.File.Exists(bck_up_trgt))
                                System.IO.File.Delete(bck_up_trgt);

                            System.IO.File.Move(target, bck_up_trgt);

                            logger.InfoFormat("Existing character file backup up to {0}", bck_up_trgt);
                        }
                        catch (Exception e)
                        {

                            logger.Error("Could not backup existing character file.", e);
                        }
                    }
                    else
                    {
                        logger.InfoFormat("No existing file was found @ {0}, so no backup was made.", target);
                    }
                }
                else
                {
                    logger.Info("No backup was requested when saving the current character");

                    if (System.IO.File.Exists(target))
                        logger.InfoFormat("An existing character file was found and will be overwritten @ {0}", target);
                }

                character.Serialize(target);

                logger.InfoFormat("Current character was saved to the default character file @ {0}", target);
            }
            else
                logger.Warn("Could not find an existing character instance to save as the interface was not registered");
        }

        public void RegisterTypeRegister(TypeRegister registery, bool overwrite_existing_interface_keys)
        {
            foreach (var entry in registery.Entries.OrderBy(x => x.CreationIndex))
            {
                if (entry.Value.InterfaceType.Equals(typeof(ICharacter)))
                {
                    var inner = this.GetInstance<ISerializableCharacter>();

                    entry.Value.Instance = new Character(inner);

                }
                else if (entry.Value.InterfaceType.Equals(typeof(ISerializableCharacter)))
                {
                    var target = System.IO.Path.ChangeExtension(Properties.Resources.CharacterFileName, SerializableCharacter.CharacterFileExtension);

                    if (System.IO.File.Exists(target))
                        entry.Value.Instance = SerializableCharacter.Load(target);
                }

                this.RegisterTypeEntry(entry.Value, overwrite_existing_interface_keys);
            }

        }

        /// <summary>
        /// Turns off console logging for the duration of the application
        /// </summary>
        public static void TurnOffConsoleLogging()
        {
            log4net.LogManager.GetRepository().ModifyAppenders<log4net.Appender.ColoredConsoleAppender>(x => x.Close());
        }


        /// <summary>
        /// Quits the game, automatically saving the current character
        /// </summary>
        public static void Quit()
        {
            logger.InfoFormat("GameManager has a recieved a request to quit the game");

            GameManager.SaveCurrentCharacter();
                
            Environment.Exit(0);
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
