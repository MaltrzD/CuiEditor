using Assets._Scripts.CUI.Interface;
using Assets._Scripts.Ext;
using System.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Assets._Scripts.CUI.Components
{
    public class CuiRawImageComponent : MonoBehaviour, ICuiComponent
    {
        public float FadeIn { get; private set; }
        public RawImage Image => _image;

        [SerializeField]
        private RawImage _image;

        public string Material { get; private set; }
        public string Sprite { get; private set; }
        public string Png { get; private set; }
        public int ItemId { get; private set; }
        public bool ImageLibrary { get; private set; }

        private void OnValidate()
        {
            if (!_image) _image = gameObject.GetOrAddComponent<RawImage>();
        }

        public void Draw()
        {
            EditorGUILayout.LabelField("Image", EditorStyles.centeredGreyMiniLabel);
            Material = EditorGUILayout.TextField("Material", Material);
            Sprite = EditorGUILayout.TextField("Sprite", Sprite);
            ItemId = EditorGUILayout.IntField("ItemId", ItemId);

            ImageLibrary = EditorGUILayout.Toggle("UseImageLibrary", ImageLibrary);
            if(ImageLibrary) Png = EditorGUILayout.TextField("Png", Png);
            _image.color = EditorGUILayout.ColorField("Color", _image.color);
        }
        public string ToCui()
        {
            string png = "";
            if (ImageLibrary) png = OtherExt.Escape("\n\t\t\t\t\t\tPng = GetImage(\"\"),", Png);
            else png = OtherExt.Escape("\n\t\t\t\t\t\tPng = \"\",", Png);

            return
                "\t\t\tnew CuiImageComponent()" +
                "\n\t\t\t\t\t{" +
                $"{OtherExt.Escape("\n\t\t\t\t\t\tMaterial = \"\",", Material)}" +
                $"{OtherExt.Escape("\n\t\t\t\t\t\tSprite = \"\",", Sprite)}" +
                $"{OtherExt.Escape("\n\t\t\t\t\t\tItemId = \"\",", ItemId)}" +
                png +
                $"\n\t\t\t\t\t\tColor = \"{_image.color.ToRust()}\"," +
                "\n\t\t\t\t\t},";
        }
        public MonoBehaviour DoDestroy()
        {
            DestroyImmediate(_image);

            return this;
        }
        public void Initialize()
        {
            OnValidate();
        }
    }
}