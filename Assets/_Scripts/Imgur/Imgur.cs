using Assets._Scripts.App;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

namespace Assets._Scripts.Utils
{
    public static class Imgur
    {
        public class ImgurImageWrapper
        {
            public List<ImgurImage> Images = new List<ImgurImage>();
        }
        private static ImgurImageWrapper _images = new ImgurImageWrapper();

        private static readonly string _dataPath = Path.Combine(Application.persistentDataPath, "ImgurData.json");
        private static readonly string _apiUrl = "https://api.imgur.com/3/image";

        public static ImageLibraryImage GetImage(string name, byte[] imageData)
        {
            var cached = _images.Images.FirstOrDefault(x => x.ShortName == name);
            if (cached != null)
            {
                if (CheckHash(imageData, cached.Hash))
                {
                    Debug.Log($"FromCache: {name}");
                    return new ImageLibraryImage
                    {
                        Name = name,
                        Url = cached.Url
                    };
                }
            }

            WWWForm form = new WWWForm();
            form.AddField("type", "image");

            form.AddBinaryData("image", imageData);
            UnityWebRequest www = UnityWebRequest.Post(_apiUrl, form);
            www.SetRequestHeader("Authorization", $"Client-ID {AppConfig.Instance.ImgurToken}");

            www.SendWebRequest();

            while (!www.isDone) { }

            if (www.result == UnityWebRequest.Result.Success && www.responseCode == 200)
            {
                JObject jsonResponse = JObject.Parse(www.downloadHandler.text);
                string imageLink = jsonResponse["data"]["link"].ToString();
                //
                _images.Images.Add(new ImgurImage
                {
                    Hash = ComputeHash(imageData),
                    ShortName = name,
                    Url = imageLink
                });

                Debug.Log("Add to cache");

                SaveData();

                return new ImageLibraryImage
                {
                    Url = imageLink,
                    Name = name
                };
            }
            else
            {
                Debug.LogError($"Image upload error: {www.responseCode}/{www.error}/{www.downloadHandler.text}");
                return null;
            }
        }


        static Imgur()
        {
            LoadData();
        }
        private static void SaveData()
        {
            string json = JsonConvert.SerializeObject(_images, Formatting.Indented);
            File.WriteAllText(_dataPath, json);
        }
        public static void ClearCache()
        {
            LoadData();
            _images.Images.Clear();
            SaveData();
        }
        private static void LoadData()
        {
            if (File.Exists(_dataPath))
            {
                string json = File.ReadAllText(_dataPath);
                _images = JsonConvert.DeserializeObject<ImgurImageWrapper>(json);

                Debug.Log($"Loaded cached imaes: {_images.Images.Count}");
            }
            else
            {
                _images = new ImgurImageWrapper();
            }
        }

        public static string ComputeHash(byte[] data)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(data);

                StringBuilder hashStringBuilder = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    hashStringBuilder.Append(b.ToString("x2"));
                }
                return hashStringBuilder.ToString();
            }
        }
        public static bool CheckHash(byte[] data, string expectedHash)
        {
            string computedHash = ComputeHash(data);
            return computedHash.Equals(expectedHash, StringComparison.OrdinalIgnoreCase);
        }
    }
    public class ImageLibraryImage
    {
        public string Name { get; set; }
        public string Url { get; set; }
    }

    [Serializable]
    public class ImgurImage
    {
        public string ShortName;
        public string Url;
        public string Hash;
    }
}
