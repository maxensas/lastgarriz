using System;
using System.IO;
using System.Text;
using System.Windows;
using Lastgarriz.Models.Serializable;

namespace Lastgarriz.Util
{
    internal sealed class DataManager
    {
        private static DataManager instance = null;
        private static readonly object Instancelock = new();

        internal ConfigData Config { get; private set; }

        internal DataManager()
        {

        }

        internal static DataManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (Instancelock)
                    {
                        if (instance == null)
                        {
                            instance = new DataManager();
                        }
                    }
                }
                return instance;
            }
        }

        internal bool InitSettings()
        {
#if DEBUG
            string path = System.IO.Path.GetFullPath("Assets\\Data\\");
#else
            string path = Path.GetFullPath("Assets\\Data\\");
#endif
            bool returnVal = true;
            FileStream fs = null;
            try
            {
                //string config = Load_Config("Config.json");
                string configJson = null, configName = Strings.File.CONFIG;
                if (ExistFile(configName))
                {
                    configJson = Load_Config(configName);
                }
                else
                {
                    configName = Strings.File.DEFAULT_CONFIG;
                    if (ExistFile(configName))
                    {
                        configJson = Load_Config(configName);
                        Save_Config(configJson, "cfg");
                    }
                    else
                    {
                        return false;
                    }
                }

                Config = Json.Deserialize<ConfigData>(configJson);


                /*
                string lang = "Lang\\" + Strings.Culture[Config.Options.Language] + "\\";
                
                System.Globalization.CultureInfo cultureRefresh = System.Globalization.CultureInfo.CreateSpecificCulture(Strings.Culture[Config.Options.Language]);
                Thread.CurrentThread.CurrentUICulture = cultureRefresh;
                TranslationViewModel.Instance.CurrentCulture = cultureRefresh;
                */
            }/*
                catch (Exception ex)
                {

                }*/
            finally
            {
                if (fs != null)
                    fs.Dispose();
            }

            return returnVal;
        }

        internal static string Load_Config(string configfile)
        {
#if DEBUG
            string path = System.IO.Path.GetFullPath("Assets\\Data\\");
#else
            string path = Path.GetFullPath("Assets\\Data\\");
#endif
            FileStream fs = null;
            string config = null;
            try
            {
                fs = new FileStream(path + configfile, FileMode.Open);

                using StreamReader reader = new(fs);
                fs = null;
                config = reader.ReadToEnd();
            }
            catch (Exception ex)
            {
                MessageBox.Show(Application.Current.MainWindow, ex.Message, "Closing application...");
                Application.Current.Shutdown();
                return null;
            }
            finally
            {
                if (fs != null)
                    fs.Dispose();
            }
            return config;
        }

        private static bool ExistFile(string file)
        {
            string path = System.IO.Path.GetFullPath("Assets\\Data\\");
            return File.Exists(path + file);
        }

        internal static bool Save_File(string json, string location)
        {
            /*
            string path = Path.GetFullPath("Data\\");
            string lang = "Lang\\" + Strings.Culture[Config.Options.Language] + "\\";
            string name = string.Empty;
            */

            using StreamWriter writer = new(location, false, Encoding.UTF8);
            try
            {
                writer.Write(json); // Saving new json
            }
            catch (Exception ex)
            {
                MessageBox.Show(Application.Current.MainWindow, ex.Message, "Error: new json file can not be saved");
                return false;
            }
            return true;
        }

        internal bool Save_Config(string configToSave, string type)
        {
#if DEBUG
            string path = Path.GetFullPath("Assets\\Data\\");
#else
            string path = Path.GetFullPath("Assets\\Data\\");
#endif
            if (type == "cfg")
            {
                string name = Strings.File.CONFIG;

                FileStream fs = null;
                try
                {
                    fs = new FileStream(path + name, FileMode.OpenOrCreate);

                    string configBackup = string.Empty;
                    string configNew = string.Empty;
                    using (StreamReader reader = new(fs))
                    {
                        fs = null;
                        configBackup = reader.ReadToEnd();
                    }

                    using StreamWriter writer = new(path + name, false, Encoding.UTF8);
                    try
                    {
                        if (type == "cfg")
                        {
                            ConfigData newConfigData = Json.Deserialize<ConfigData>(configToSave);
                            configNew = Json.Serialize<ConfigData>(newConfigData);
                            writer.Write(configNew); // Saving new config
                            Config = Json.Deserialize<ConfigData>(configToSave);

                        }
                    }
                    catch (Exception ex)
                    {
                        writer.Write(configBackup); // Backup
                        MessageBox.Show(Application.Current.MainWindow, ex.Message, "Error: new file can not be serialized");

                        return false;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(Application.Current.MainWindow, ex.Message, "Error while saving new file");
                    return false;
                }
                finally
                {
                    if (fs != null)
                        fs.Dispose();
                }
                return true;
            }
            else
            {
                return false;
            }

        }
    }
}
