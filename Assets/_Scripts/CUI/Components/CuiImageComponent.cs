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

    [SerializeField] private string material;
    [SerializeField] private string sprite;
    [SerializeField] private string png;
    [SerializeField] private int itemId;

    public string Material => material;
    public string Sprite => sprite;
    public string Png => png;
    public int ItemId => itemId;

    private void OnValidate()
    {
        if(!_image) _image = gameObject.GetOrAddComponent<Image>();
    }

    public void Draw()
    {
        EditorGUILayout.LabelField("Image", EditorStyles.centeredGreyMiniLabel);
        material = EditorGUILayout.TextField("Material", material);
        sprite = EditorGUILayout.TextField("Sprite", sprite);
        itemId = EditorGUILayout.IntField("ItemId", itemId);
        png = EditorGUILayout.TextField("Png", png);
        _image.color = EditorGUILayout.ColorField("Color", _image.color);
        FadeIn = EditorGUILayout.FloatField("FadeIn", FadeIn);
    }
    public string ToCui()
    {
        return
            "\t\t\tnew CuiImageComponent()" +
            "\n\t\t\t\t\t{" +
            $"{OtherExt.Escape("\n\t\t\t\t\t\tMaterial = \"\",", material)}" +
            $"{OtherExt.Escape("\n\t\t\t\t\t\tSprite = \"\",", sprite)}" +
            $"{OtherExt.Escape("\n\t\t\t\t\t\tItemId = \"\",", itemId)}" +
            $"{OtherExt.Escape("\n\t\t\t\t\t\tPng = \"\",", png)}" +
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
