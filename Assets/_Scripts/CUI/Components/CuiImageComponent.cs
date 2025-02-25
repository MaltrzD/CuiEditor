using Assets._Scripts.CUI.Interface;
using Assets._Scripts.Ext;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class CuiImageComponent : MonoBehaviour, ICuiComponent
{
    public float FadeIn { get; private set; }

    public Image Image => _image;

    [SerializeField]
    private Image _image;

    public string Material { get; private set; }
    public string Sprite { get; private set; }
    public string Png { get; private set; }
    public int ItemId { get; private set; }

    private void OnValidate()
    {
        if(!_image) _image = gameObject.GetOrAddComponent<Image>();
    }

    public void Draw()
    {
        EditorGUILayout.LabelField("Image", EditorStyles.centeredGreyMiniLabel);
        Material = EditorGUILayout.TextField("Material", Material);
        Sprite = EditorGUILayout.TextField("Sprite", Sprite);
        ItemId = EditorGUILayout.IntField("ItemId", ItemId);
        Png = EditorGUILayout.TextField("Png", Png);
        _image.color = EditorGUILayout.ColorField("Color", _image.color);
        FadeIn = EditorGUILayout.FloatField("FadeIn", FadeIn);
    }
    public string ToCui()
    {
        return
            "\t\t\tnew CuiImageComponent()" +
            "\n\t\t\t\t\t{" +
            $"{OtherExt.Escape("\n\t\t\t\t\t\tMaterial = \"\",", Material)}" +
            $"{OtherExt.Escape("\n\t\t\t\t\t\tSprite = \"\",", Sprite)}" +
            $"{OtherExt.Escape("\n\t\t\t\t\t\tItemId = \"\",", ItemId)}" +
            $"{OtherExt.Escape("\n\t\t\t\t\t\tPng = \"\",", Png)}" +
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
