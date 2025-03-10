﻿using Assets._Scripts.CUI.Interface;
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

        [SerializeField] private float fadeOut;
        [SerializeField] private bool needCursor;

        protected virtual void OnValidate()
        {
            if(_rectTransform == null) _rectTransform = GetComponent<RectTransform>();
        }

        public Vector2 AnchorMin { get => _rectTransform.anchorMin; }
        public Vector2 AnchorMax { get => _rectTransform.anchorMax; }
        public Vector2 OffsetMin { get => _rectTransform.offsetMin; }
        public Vector2 OffsetMax { get => _rectTransform.offsetMax; }

        //
        public virtual string ToCui(string components = "", string overrideParent = null, string overrideName = null)
        {
            sb.Clear();
            sb.AppendLine($"container.Add(new CuiElement");
            sb.AppendLine("{");

            string parent = overrideParent == null ? transform.parent.name : overrideParent;
            string eName = overrideName == null ? gameObject.name : overrideName;

            sb.AppendLine($"\tParent = $\"{parent}\",");
            sb.AppendLine($"\tName = $\"{eName}\",");

            sb.AppendLine();

            sb.AppendLine("Components =");
            sb.AppendLine("\t\t\t\t{");

            sb.AppendLine("\t\t" + GetRectTransformComponent());
            sb.AppendLine("\t\t" + components);

            if (needCursor)
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
            fadeOut = EditorGUILayout.FloatField("FadeOut", fadeOut);
            needCursor = EditorGUILayout.Toggle("NeedCursor", needCursor);

            EditorUtility.SetDirty(this);
        }
        public virtual void Setup() { }
    }
}