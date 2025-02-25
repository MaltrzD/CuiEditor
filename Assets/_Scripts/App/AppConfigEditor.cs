using UnityEditor;
using UnityEngine;
using Assets._Scripts.App;
using Assets._Scripts.Utils;

namespace Assets._Scripts.Editor
{
    public class AppConfigEditor : EditorWindow
    {
        private static AppConfigEditor window;
        private AppConfig config => AppConfig.Instance;

        [MenuItem("CUIEditor/Settings")]
        public static void Open()
        {
            if (window == null) window = GetWindow<AppConfigEditor>();
            window.titleContent = new GUIContent("App Config");
        }
        private void SaveConfig()
        {
            config.Save();
        }

        private void OnGUI()
        {
            GUILayout.Label("Application Config", EditorStyles.boldLabel);
            config.ImgurToken = EditorGUILayout.TextField("Imgur Token:", config.ImgurToken);

            GUILayout.Label("ImageLibrary", EditorStyles.boldLabel);
            config.AddImageLibraryReference = EditorGUILayout.Toggle("ImageLib Reference", config.AddImageLibraryReference);
            config.AddImageLibraryMethods = EditorGUILayout.Toggle("ImageLib Methods", config.AddImageLibraryMethods);
            config.LoadImagesMethodName = EditorGUILayout.TextField("ImageLoad Method:", config.LoadImagesMethodName);
            if (GUILayout.Button("Clear image cache"))
                Imgur.ClearCache();

            if (GUILayout.Button("Save Config"))
            {
                SaveConfig();
            }
        }
    }
}
