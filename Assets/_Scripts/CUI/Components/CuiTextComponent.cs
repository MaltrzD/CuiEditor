using Assets._Scripts.CUI.Interface;
using Assets._Scripts.Ext;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class CuiTextComponent : MonoBehaviour, ICuiComponent
{
    [SerializeField] private float fadeIn;

    public float FadeIn => fadeIn;
    public Text Text => _text;
    
    private Text _text;
    private void OnValidate()
    {
        if(!_text) _text = GetComponent<Text>();
    }

    public void Draw()
    {
        EditorGUILayout.LabelField("TEXT", EditorStyles.centeredGreyMiniLabel);

        fadeIn = EditorGUILayout.FloatField("fadeIn", fadeIn);
        _text.text = EditorGUILayout.TextField("Text", _text.text);
        _text.color = EditorGUILayout.ColorField("Color", _text.color);
        _text.fontSize = EditorGUILayout.IntField("FontSize", _text.fontSize);
        _text.alignment = (TextAnchor)EditorGUILayout.EnumFlagsField("Alignment", _text.alignment);
    }
    public string ToCui()
    {
        return
        "\t\t\tnew CuiTextComponent()" +
        "\n\t\t\t\t\t{" +
        $"\n\t\t\t\t\t\tText = \"{_text.text}\"," +
        $"\n\t\t\t\t\t\tFontSize = {_text.fontSize}," +
        $"\n\t\t\t\t\t\tFadeIn = {fadeIn.ToString().Replace(',', '.')}f," +
        $"\n\t\t\t\t\t\tColor = \"{_text.color.ToRust()}\"," +
        $"\n\t\t\t\t\t\tAlign = TextAnchor.{_text.alignment}," +
        "\n\t\t\t\t\t},";
    }

    public MonoBehaviour DoDestroy()
    {
        DestroyImmediate(_text);

        return this;
    }
    public void Initialize()
    {
        OnValidate();
    }
}
