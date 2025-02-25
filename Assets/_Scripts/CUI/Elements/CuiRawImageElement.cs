using Assets._Scripts.CUI.Components;
using Unity.VisualScripting;

namespace Assets._Scripts.CUI.Elements
{
    public class CuiRawImageElement : BaseCuiElement
    {
        public CuiRawImageComponent CuiRawImageComponent { get; private set; }

        protected override void OnValidate()
        {
            base.OnValidate();

            if (!CuiRawImageComponent)
                CuiRawImageComponent = gameObject.GetOrAddComponent<CuiRawImageComponent>();
        }

        public override void DrawElements()
        {
            base.DrawElements();

            CuiRawImageComponent.Draw();
        }

        public override string ToCui(string components = "")
        {
            return base.ToCui(CuiRawImageComponent.ToCui());
        }
    }
}