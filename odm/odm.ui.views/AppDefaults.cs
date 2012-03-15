using System;
using System.IO;
using System.Windows;
using System.Xml.Serialization;
using utils;
using onvif.services;

namespace odm.ui {
    public class AppDefaults {
        public static void InitConfigs(){
            string path = ConfigFolderPath + VisualSettings.name;
            if (!File.Exists(path)) {
                try {
                    var f = File.Create(path);
                    f.Close();
                } catch (Exception err) {
                    dbg.Error(err);
                }
            }
            VisualSettings visconf = null;
            using (var sr = File.OpenText(path)) {
                try {
                    XmlSerializer deserializer = new XmlSerializer(typeof(VisualSettings));
                    visconf = (VisualSettings)deserializer.Deserialize(sr);
                } catch (Exception err) {
                    dbg.Error(err);
                }
            }
            if (visconf != null && visconf.Version != _version) {
                try {
                    Directory.EnumerateFiles(ConfigFolderPath).ForEach(x => {
                            
                        File.Delete(x);
                    });
                } catch (Exception err) {
                    dbg.Error(err);
                }
            }
        }
        static protected string SystemFolder {
            get {
                return "Synesis";
            }
        }
        static protected string AppDataFolder {
            get {
                return "Onvif Device Manager";
            }
        }
        static public string SystemFolderPath {
            get {
                //string path = Environment.GetEnvironmentVariable("USERPROFILE") + @"\" + SystemFolder + @"\";
                string path = AppDomain.CurrentDomain.BaseDirectory + @"\";// +SystemFolder + @"\";
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                return path;
            }
        }
        static public string AppDataPath {
            get {
                string path = SystemFolderPath;// +AppDataFolder + @"\";
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                return path;
            }
        }
        static public string MetadataFolderPath {
            get {
                string path = AppDataPath + @"meta\";
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                return path;
            }
        }
        static public string ConfigFolderPath {
            get {
                string path = AppDataPath + @"config\";
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                return path;
            }
        }

        public static void SetVisualSettings(VisualSettings visualSet) {
            string path = ConfigFolderPath + VisualSettings.name;
            try {
                var fs = File.Create(path);
                fs.Close();
                using (var sr = File.CreateText(path)) {
                    XmlSerializer serializer = new XmlSerializer(typeof(VisualSettings));

                    serializer.Serialize(sr, visualSet);
                }
            } catch (Exception err) {
                dbg.Error(err);
            }
        }
        protected static VisualSettings NewVisualSettings() {
            return new VisualSettings() { 
                ItemSelector_IsPropertiesExpanded = false, 
                Events_IsEnabled = true, 
                EngineerTool_IsEnabled = false, 
                Snapshot_IsEnabled = true, 
                Version = _version, 
                CustomAnalytics_IsEnabled = true, 
                EventsCollect_IsEnabled = false,
                WndSize = new Rect(50, 50, 640, 480),
                WndState = WindowState.Normal,
                ui_video_rendering_fps = 30,
                Base_Subscription_Port = 8085,
				ShowVideoPlaybackStatistics = false,
                Event_Subscription_Type = VisualSettings.EventType.TRY_PULL,
				Transport_Type = TransportProtocol.UDP, 
				UseOnlyCommonFilterView = false,
				DefEventFilter = ""
            };
        }
        public static VisualSettings visualSettings {
            get {
                string path = ConfigFolderPath + VisualSettings.name;
                if (!File.Exists(path)) {
                    try {
                        var f = File.Create(path);
                        f.Close();
                    } catch (Exception err) {
                        dbg.Error(err);
                    }
                }
                try {
                    using (var sr = File.OpenText(path)) {
                        XmlSerializer deserializer = new XmlSerializer(typeof(VisualSettings));
                        VisualSettings visconf;
                        visconf = (VisualSettings)deserializer.Deserialize(sr);
                        return visconf;
                    }
                } catch (Exception err) {
                    dbg.Error(err);
                    var vs = NewVisualSettings();
                    SetVisualSettings(vs);
                    return vs;
                }
            }
        }
        protected static string _version {
            get {
                System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
                Version ver = assembly.GetName().Version;
                return "v" + ver.Major + "." + ver.Minor + "." + ver.Build;
            }
        }
    }
    
    [XmlRootAttribute(ElementName = "VisualSettings", IsNullable = false)]
    public class VisualSettings{
        public VisualSettings() {
            Events_IsEnabled = true;
            Snapshot_IsEnabled = true;
            ItemSelector_IsPropertiesExpanded = false;
            EngineerTool_IsEnabled = false;
            EventsCollect_IsEnabled = false;
            CustomAnalytics_IsEnabled = true;
            wndSize = new Rect(50, 50, 640, 480);
            WndState = WindowState.Normal;
            ui_video_rendering_fps = 30;
            Event_Subscription_Type = EventType.TRY_PULL;
            Base_Subscription_Port = 8085;
			ShowVideoPlaybackStatistics = false;
			Transport_Type = TransportProtocol.UDP;
			UseOnlyCommonFilterView = false;
			//TODO: railway build
			DefEventFilter = "";
        }
		//TODO: railway build
		public string DefEventFilter { get; set; }

        public string Version { get; set; }
		public enum TlsMode { 
			USE_ALWAYS,
			USE_IF_POSSIBLE
		}
        public enum EventType { 
            ONLY_PULL,
            TRY_PULL,
            ONLY_BASE
        }
		public bool UseOnlyCommonFilterView { get; set; }
		public bool ShowVideoPlaybackStatistics { get; set; }
        public int Base_Subscription_Port { get; set; }
        public EventType Event_Subscription_Type { get; set; }
		public TransportProtocol Transport_Type { get; set; }
        public static readonly string name = "VisualSettings.xml";
        public bool EngineerTool_IsEnabled { get; set; }
        public int ui_video_rendering_fps { get; set; }
        public bool ItemSelector_IsPropertiesExpanded { get; set; }
        public bool Events_IsEnabled { get; set; }
        public bool EventsCollect_IsEnabled { get; set; }
        public bool Snapshot_IsEnabled { get; set; }
        public bool CustomAnalytics_IsEnabled { get; set; }
        Rect wndSize;
        public Rect WndSize { 
            get {
                return wndSize;
            }
            set {
                wndSize = value;
                wndSize.X = wndSize.X < 0 ? 0 : wndSize.X;
                wndSize.Y = wndSize.Y < 0 ? 0 : wndSize.Y;
            }
        }
        public WindowState WndState {get;set;}
    }
    [XmlRootAttribute(ElementName = "MetaConfig", IsNullable = false)]
    public class MetaConfig {
        public static readonly string name = "MetaConfig.xml";
        public bool CollectMeta { get; set; }
    }
}
