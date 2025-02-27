using Assets._Scripts;
using Assets._Scripts.CUI;
using Assets._Scripts.Ext;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class CuiFillerSimple : BaseCuiElement, ICustomBuild
{
    [SerializeField] private BaseCuiElement originalElement;
    public int CountDrawElements => countDrawElements;
    public int PerPage => perPage;
    public Vector2 Spacing => spacing;

    [SerializeField] private int perPage = 3;
    [SerializeField] private int countDrawElements = 3;
    [SerializeField] private Vector2 spacing = Vector2.zero;


    protected override void OnValidate()
    {
        base.OnValidate();
    }
    public override void DrawElements()
    {
        base.DrawElements();

        EditorGUILayout.LabelField("Fill settings", EditorStyles.centeredGreyMiniLabel);

        spacing = EditorGUILayout.Vector2Field("Spacing", spacing);
        countDrawElements = EditorGUILayout.IntField("Elements count", countDrawElements);
        perPage = EditorGUILayout.IntField("PerPage", perPage);

        EditorGUILayout.LabelField("Element", EditorStyles.centeredGreyMiniLabel);

        originalElement = (BaseCuiElement)EditorGUILayout.ObjectField("Element", originalElement, typeof(BaseCuiElement), true);
        if (originalElement != null)
        {
            if (GUILayout.Button("Redraw"))
            {
                Redraw();
            }

            if (GUILayout.Button("Clear"))
            {
                Redraw(true);
            }
        }
    }

    private void Redraw(bool clear = false)
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Transform child = transform.GetChild(i);
            if (child != originalElement.transform)
            {
                DestroyImmediate(child.gameObject);
            }
        }

        if (clear) return;
        var initRectMin = originalElement.OffsetMin;
        var initRectMax = originalElement.OffsetMax;

        var rectMin = initRectMin;
        var rectMax = initRectMax;


        for (int i = 1; i < CountDrawElements; i++)
        {
            rectMin.x += (initRectMax.x - initRectMin.x) + Spacing.x;
            rectMax.x += (initRectMax.x - initRectMin.x) + Spacing.x;

            if (i % PerPage == 0 && i != 0)
            {
                rectMin.y -= (initRectMax.y - initRectMin.y) + Spacing.y;
                rectMax.y -= (initRectMax.y - initRectMin.y) + Spacing.y;

                rectMin.x = initRectMin.x;
                rectMax.x = initRectMin.x + (initRectMax.x - initRectMin.x);
            }

            var go = Instantiate(originalElement, transform);
            go.transform.localScale = Vector3.one;

            go._rectTransform.offsetMin = rectMin;
            go._rectTransform.offsetMax = rectMax;
        }

        originalElement.transform.SetSiblingIndex(0);
    }
    public string Build()
    {
        StringBuilder builder = new StringBuilder();

        builder.AppendLine($"private void DrawFill_{transform.name}(BasePlayer player)");
        builder.AppendLine("{");
        builder.AppendLine("    CuiElementContainer container = new CuiElementContainer();");

        builder.AppendLine(ToCui(GetImageComponent()));

        builder.AppendLine($"    var rectMin = new Vector2({originalElement.OffsetMin.x}, {originalElement.OffsetMin.y});");
        builder.AppendLine($"    var rectMax = new Vector2({originalElement.OffsetMax.x}, {originalElement.OffsetMax.y});");

        builder.AppendLine($"    for (int i = 0; i < {CountDrawElements}; i++)");
        builder.AppendLine("    {");

        builder.AppendLine($"        if (i % {PerPage} == 0 && i != 0)");
        builder.AppendLine("        {");
        builder.AppendLine($"            rectMin.y -= {originalElement.OffsetMax.y - originalElement.OffsetMin.y} + {Spacing.y};");
        builder.AppendLine($"            rectMax.y -= {originalElement.OffsetMax.y - originalElement.OffsetMin.y} + {Spacing.y};");
        builder.AppendLine($"            rectMin.x = {originalElement.OffsetMin.x};");
        builder.AppendLine($"            rectMax.x = {originalElement.OffsetMin.x} + {originalElement.OffsetMax.x - originalElement.OffsetMin.x};");
        builder.AppendLine("        }");
        builder.AppendLine("");

        builder.AppendLine(originalElement.ToCui().OverrideRectTransform(originalElement,
            $"\"{originalElement.AnchorMin.ToCuiFormat()}\"",
            $"\"{originalElement.AnchorMax.ToCuiFormat()}\"",
            "$\"{rectMin.x:0.#######} {rectMin.y:0.#######}\"",
            "$\"{rectMax.x:0.#######} {rectMax.y:0.#######}\""));

        var underOriginal = new List<BaseCuiElement>();
        Tests.GetElements(originalElement.transform, underOriginal);
        foreach (var item in underOriginal)
        {
            builder.AppendLine(item.ToCui());
        }


        builder.AppendLine($"        rectMin.x += {originalElement.OffsetMax.x - originalElement.OffsetMin.x} + {Spacing.x};");
        builder.AppendLine($"        rectMax.x += {originalElement.OffsetMax.x - originalElement.OffsetMin.x} + {Spacing.x};");
        builder.AppendLine("    }");

        builder.AppendLine("    CuiHelper.AddUi(player, container);");
        builder.AppendLine("}");

        return builder.ToString();
    }

    public string GetImageComponent()
    {
        return
            "\t\t\tnew CuiImageComponent()" +
            "\n\t\t\t\t\t{" +
            $"\n\t\t\t\t\t\tColor = \"0 0 0 0\"," +
            "\n\t\t\t\t\t},";
    }
}
