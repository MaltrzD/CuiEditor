using Assets._Scripts.CUI.Interface;
using Assets._Scripts.Ext;
using Assets._Scripts.Utils;
using System.Collections;
using System.Net.NetworkInformation;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;
using static UnityEditor.Progress;

namespace Assets._Scripts.CUI.Components
{
    public class CuiRawImageComponent : MonoBehaviour, ICuiComponent
    {
        public float FadeIn { get; private set; }
        public RawImage Image => _image;

        [SerializeField]
        private RawImage _image;

        [SerializeField] private string material;
        [SerializeField] private string sprite;
        [SerializeField] private int itemId;
        [SerializeField] private Sprite elemenetImage;
        [SerializeField] private bool imageLibrary;


        public string Material => material;
        public string Sprite => sprite;
        public int ItemId => itemId;
        public Sprite ElemenetImage => elemenetImage;
        public bool ImageLibrary => imageLibrary;

        private void OnValidate()
        {
            if (!_image) _image = gameObject.GetOrAddComponent<RawImage>();
        }

        public void Draw()
        {
            EditorGUILayout.LabelField("Image", EditorStyles.centeredGreyMiniLabel);
            material = EditorGUILayout.TextField("Material", material);
            sprite = EditorGUILayout.TextField("Sprite", sprite);
            itemId = EditorGUILayout.IntField("ItemId", itemId);
            _image.color = EditorGUILayout.ColorField("Color", _image.color);

            imageLibrary = EditorGUILayout.Toggle("UseImageLibrary", imageLibrary);
            if (imageLibrary)
            {
                elemenetImage = (Sprite)EditorGUILayout.ObjectField("PngImage", elemenetImage, typeof(Sprite), false);
                _image.texture = elemenetImage?.texture;
            } 
        }
        public string ToCui()
        {
            string png = "";
            if(ImageLibrary && ElemenetImage != null)
            {
                ImageLibraryImage img = Imgur.GetImage(gameObject.name + "_Image", ElemenetImage.texture.EncodeToPNG());
                if (img != null)
                {
                    png = OtherExt.Escape("\n\t\t\t\t\t\tPng = GetImage(\"\"),", img.Name);

                    CuiBuilder.AddImage(img);
                }
                else
                {
                    Debug.LogError("ImageLibraryImage is null!");
                }
            }
            else if (ImageLibrary)
            {
                Debug.LogError("ImageLibrary == true, but ElementImage is null!");
            }

            return
                "\t\t\tnew CuiRawImageComponent()" +
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