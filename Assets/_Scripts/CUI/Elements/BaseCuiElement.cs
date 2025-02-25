using Assets._Scripts.CUI.Interface;
using Assets._Scripts.Ext;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Assets._Scripts.CUI
{
    [RequireComponent(typeof(RectTransform))]
    public class BaseCuiElement : MonoBehaviour
    {
        protected static StringBuilder sb = new StringBuilder();
        public RectTransform _rectTransform;
        public float FadeOut { get; private set; }
        public bool NeedCursor { get; private set; }

        protected virtual void OnValidate()
        {
            if(_rectTransform == null) _rectTransform = GetComponent<RectTransform>();
        }

        public Vector2 AnchorMin { get => _rectTransform.anchorMin; }
        public Vector2 AnchorMax { get => _rectTransform.anchorMax; }
        public Vector2 OffsetMin { get => _rectTransform.offsetMin; }
        public Vector2 OffsetMax { get => _rectTransform.offsetMax; }

        //
        public virtual string ToCui(string components = "")
        {
            sb.Clear();
            sb.AppendLine($"container.Add(new CuiElement");
            sb.AppendLine("{");


            sb.AppendLine($"\tParent = \"{transform.parent.name}\",");
            sb.AppendLine($"\tName = \"{gameObject.name}\",");

            sb.AppendLine();

            sb.AppendLine("Components =");
            sb.AppendLine("\t\t\t\t{");

            sb.AppendLine("\t\t" + GetRectTransformComponent());
            sb.AppendLine("\t\t" + components);

            if (NeedCursor)
                sb.AppendLine(GetNeedMouseComponent());

            sb.AppendLine("\t\t\t\t}");


            sb.AppendLine("});");

            return sb.ToString();
        }
        public virtual string GetRectTransformComponent()
        {
            return
                "\t\t\tnew CuiRectTransformComponent()" +
                "\n\t\t\t\t\t{" +
                $"\n\t\t\t\t\t\tAnchorMin = \"{AnchorMin.ToCuiFormat()}\"," +
                $"\n\t\t\t\t\t\tAnchorMax = \"{AnchorMax.ToCuiFormat()}\"," +
                $"\n\t\t\t\t\t\tOffsetMin = \"{OffsetMin.ToCuiFormat()}\"," +
                $"\n\t\t\t\t\t\tOffsetMax = \"{OffsetMax.ToCuiFormat()}\"" +
                "\n\t\t\t\t\t},";
        }
        public virtual string GetNeedMouseComponent()
        {
            return "\t\t\t\t\tnew CuiNeedsCursorComponent() { },";
        }

        public virtual void DrawElements()
        {
            gameObject.name = EditorGUILayout.TextField("Name", gameObject.name);
            FadeOut = EditorGUILayout.FloatField("FadeOut", FadeOut);
            NeedCursor = EditorGUILayout.Toggle("NeedCursor", NeedCursor);
        }
        public virtual void Setup() { }
    }
}