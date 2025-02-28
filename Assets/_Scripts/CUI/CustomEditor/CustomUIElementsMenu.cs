using Assets._Scripts.CUI;
using Assets._Scripts.CUI.Elements;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public static class CustomUIElementsMenu
{
    private const string MenuPath = "GameObject/UI/CUI/";

    [MenuItem(MenuPath + "Button", false, 10)]
    private static void CreateButton()
    {
        var button = Create<CuiButtonElement>("Button");

        ResetElement(button.gameObject);

        SetParentAndAlign(button.gameObject);

        button.CuiTextComponent.Text.text = "Text";
        button.CuiTextComponent.Text.alignment = TextAnchor.MiddleCenter;
        button.CuiTextComponent.Text.color = new Color32(51, 51, 51, 255);
    }
    [MenuItem(MenuPath + "Text", false, 10)]
    private static void CreateText()
    {
        var button = Create<CuiTextElement>("Text");

        ResetElement(button.gameObject);

        SetParentAndAlign(button.gameObject);

        button.CuiTextComponent.Text.text = "Text";
        button.CuiTextComponent.Text.alignment = TextAnchor.MiddleCenter;
        button.CuiTextComponent.Text.color = Color.black;
    }
    [MenuItem(MenuPath + "Image (Panel)", false, 10)]
    private static void CreatePanel()
    {
        var button = Create<CuiImageElement>("Image (Panel)");

        ResetElement(button.gameObject);

        SetParentAndAlign(button.gameObject);
    }
    [MenuItem(MenuPath + "RawImage", false, 10)]
    private static void CreateRawImage()
    {
        var button = Create<CuiRawImageElement>("RawImage");

        ResetElement(button.gameObject);

        SetParentAndAlign(button.gameObject);
    }
    [MenuItem(MenuPath + "Fill", false, 10)]
    private static void CreateFill()
    {
        var button = Create<CuiFillerSimple>("FILL");

        ResetElement(button.gameObject);

        SetParentAndAlign(button.gameObject);
    }

    private static T Create<T>(string name = "CuiElement") where T : BaseCuiElement
    {
        GameObject go = new GameObject(name);

        return go.AddComponent<T>();
    }
    private static void ResetElement(GameObject go)
    {
        var rect = go.GetComponent<RectTransform>();
        var parent = Selection.activeGameObject;

        if (!rect) return;

        rect.offsetMin = new Vector2(0, 0);
        rect.offsetMax = new Vector2(150, 50);

        if(parent != null) rect.anchoredPosition = Vector2.zero;
    }
    private static void SetParentAndAlign(GameObject obj)
    {
        GameObject parent = Selection.activeGameObject;
        if (parent != null)
        {
            Undo.RegisterCreatedObjectUndo(obj, "Create " + obj.name);
            obj.transform.SetParent(parent.transform, false);
        }
        else
        {
            Debug.LogWarning("no selected onject!");
        }
        Selection.activeGameObject = obj;
    }
}
