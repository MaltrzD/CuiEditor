using System.IO;
using UnityEditor;
using UnityEngine;

namespace Assets._Scripts.App
{
    [InitializeOnLoad]
    public class AppConfig
    {
        private static readonly string _configPath = Path.Combine(Application.persistentDataPath, "Config.json");
        private static AppConfig _instance;

        public string ImgurToken = string.Empty;
        public bool AddImageLibraryReference = true;
        public bool AddImageLibraryMethods = true;
        public string LoadImagesMethodName = "OnServerInitialized";

        public static AppConfig Instance
        {
            get
            {
                if (_instance == null)
                {
                    Load();
                }
                return _instance;
            }
        }

        static AppConfig()
        {
            Load();
        }

        private static void Load()
        {
            if (File.Exists(_configPath))
            {
                try
                {
                    string json = File.ReadAllText(_configPath);
                    _instance = JsonUtility.FromJson<AppConfig>(json);
                }
                catch
                {
                    _instance = new AppConfig();
                }
            }
            else
            {
                _instance = new AppConfig();
                _instance.Save();
            }
        }

        public void Save()
        {
            string json = JsonUtility.ToJson(this, true);
            File.WriteAllText(_configPath, json);
        }
    }
}
