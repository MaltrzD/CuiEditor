using Unity.VisualScripting;
using UnityEngine.UI;
using UnityEngine;
using Assets._Scripts.CUI.Components;
using UnityEditor;
using Assets._Scripts.Ext;

namespace Assets._Scripts.CUI.Elements
{
    [RequireComponent(typeof(Button))]
    public class CuiButtonElement : BaseCuiElement
    {
        public CuiTextComponent CuiTextComponent { get; private set; }
        public CuiImageComponent CuiImageComponent { get; private set; }
        public CuiRawImageComponent CuiRawImageComponent { get; private set; }

        public string Close => close;
        public string Command => command;
        public bool Advanced => advanced;

        [SerializeField] private string close;
        [SerializeField] private string command;
        [SerializeField] private bool advanced;

        private bool _lastAdvanced;

        protected override void OnValidate()
        {
            base.OnValidate();

            CuiTextComponent = transform.Find("Text")?.GetOrAddComponent<CuiTextComponent>();

            if (!CuiTextComponent)
            {
                GameObject textGo = new GameObject("Text");
                textGo.transform.parent = transform;

                CuiTextComponent = textGo.GetOrAddComponent<CuiTextComponent>();

                var rect = textGo.GetComponent<RectTransform>();

                rect.anchorMin = new Vector2(0, 0);
                rect.anchorMax = new Vector2(1, 1);

                rect.offsetMin = Vector2.zero;
                rect.offsetMax = Vector2.zero;

                rect.localScale = Vector2.one;
            }

            if (Advanced)
            {
                if (CuiImageComponent)
                    DestroyImmediate(CuiImageComponent.DoDestroy());

                if (!CuiRawImageComponent)
                {
                    CuiRawImageComponent = gameObject.GetOrAddComponent<CuiRawImageComponent>();
                    CuiRawImageComponent.Initialize();
                }
            }
            else
            {
                if (CuiRawImageComponent)
                    DestroyImmediate(CuiRawImageComponent.DoDestroy());

                if (!CuiImageComponent)
                {
                    CuiImageComponent = gameObject.GetOrAddComponent<CuiImageComponent>();
                    CuiImageComponent.Initialize();
                }
            }
        }

        public override void DrawElements()
        {
            base.DrawElements();

            EditorGUILayout.LabelField("Button", EditorStyles.centeredGreyMiniLabel);
            advanced = EditorGUILayout.Toggle("Advanced", advanced);
            close = EditorGUILayout.TextField("Close", close);
            command = EditorGUILayout.TextField("Command", command);



            CuiTextComponent.Draw();

            if(_lastAdvanced != Advanced)
            {
                OnValidate();

                _lastAdvanced = Advanced;
            }

            if (Advanced) CuiRawImageComponent?.Draw();
            else CuiImageComponent?.Draw();
        }
        public override string ToCui(string components = "", string overrideParent = null, string overrideName = null)
        {
            if (Advanced)
            {
                components += base.ToCui(CuiRawImageComponent.ToCui(), overrideParent, overrideName) + GetButton(overrideParent, overrideName);

                return components;
            }
            else
            {
                return GetButton(overrideParent, overrideName) + "\n";
            }
        }
        public string ToCuiFill(string components = "", string overrideParent = null, string overrideName = null)
        {
            if (Advanced)
            {
                components += base.ToCui(CuiRawImageComponent.ToCui(), overrideParent, overrideName) + GetButton(overrideName, overrideParent);

                return components;
            }
            else
            {
                return GetButton(overrideParent, overrideName) + "\n";
            }
        }

        private string GetButton(string overrideParent = null, string overrideName = null)
        {
            Color btnColor = Advanced ? CuiRawImageComponent.Image.color : CuiImageComponent.Image.color;
            float fadeIn = Advanced ? CuiRawImageComponent.FadeIn : CuiImageComponent.FadeIn;
            string material = Advanced ? CuiRawImageComponent.Material : CuiImageComponent.Material;
            string sprite = Advanced ? CuiRawImageComponent.Sprite : CuiImageComponent.Sprite;

            Vector2 aMin = Advanced ? Vector2.zero : AnchorMin;
            Vector2 aMax = Advanced ? Vector2.one : AnchorMax;
            Vector2 oMin = Advanced ? Vector2.zero : OffsetMin;
            Vector2 oMax = Advanced ? Vector2.zero : OffsetMax;

            string parent = Advanced ? transform.name : transform.parent.name;
            string name = Advanced ? "" : $", \"{transform.name}\"";

            if(overrideParent != null) parent = overrideParent;
            if (overrideName != null) name = $", $\"{overrideName}\"";

            string result = $"container.Add(new CuiButton()\n" +
                            $"{{\n" +
                            $"\tButton =\n" +
                            $"\t\t\t\t{{\n" +
                            $"{OtherExt.Escape("\t\t\t\t\tClose = \"\",\n", Close)}" +
                            $"{OtherExt.Escape("\t\t\t\t\tColor = \"\",\n", btnColor.ToRust())}" +
                            $"{OtherExt.Escape("\t\t\t\t\tCommand = \"\",\n", Command)}" +
                            $"{OtherExt.Escape("\t\t\t\t\tFadeIn = \"\"f,\n", fadeIn.ToString().Replace(',', '.'))}" +
                            $"{OtherExt.Escape("\t\t\t\t\tMaterial = \"\",\n", material)}" +
                            $"{OtherExt.Escape("\t\t\t\t\tSprite = \"\",\n", sprite)}" +
                            $"\t\t\t\t}},\n" +

                            $"\tText =\n" +
                            $"\t\t\t\t{{\n" +
                            $"{OtherExt.Escape("\t\t\t\t\tText = \"\",\n", CuiTextComponent.Text.text)}" +
                            $"\t\t\t\t\tAlign = TextAnchor.{CuiTextComponent.Text.alignment},\n" +
                            $"{OtherExt.Escape("\t\t\t\t\tColor = \"\",\n", CuiTextComponent.Text.color.ToRust())}" +
                            $"{OtherExt.Escape("\t\t\t\t\tFadeIn =  \"\"f,\n", CuiTextComponent.FadeIn.ToString().Replace(',', '.'))}" +
                            $"\t\t\t\t\tFontSize = {CuiTextComponent.Text.fontSize},\n" +
                            $"\t\t\t\t}},\n" +

                            $"\tRectTransform =\n" +
                            $"\t\t\t\t{{\n" +
                            $"\t\t\t\t\tAnchorMin = \"{aMin.ToCuiFormat()}\",\n" +
                            $"\t\t\t\t\tAnchorMax = \"{aMax.ToCuiFormat()}\",\n" +
                            $"\t\t\t\t\tOffsetMin = \"{oMin.ToCuiFormat()}\",\n" +
                            $"\t\t\t\t\tOffsetMax = \"{oMax.ToCuiFormat()}\",\n" +
                            $"\t\t\t\t}},\n" +

                            $"}}, $\"{parent}\"{name});";

            return result;//
        }
    }
}
