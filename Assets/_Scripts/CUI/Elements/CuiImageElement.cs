using Unity.VisualScripting;

namespace Assets._Scripts.CUI.Elements
{
    public class CuiImageElement : BaseCuiElement
    {
        public CuiImageComponent CuiImageComponent { get; private set; }

        protected override void OnValidate()
        {
            base.OnValidate();

            if (!CuiImageComponent)
                CuiImageComponent = gameObject.GetOrAddComponent<CuiImageComponent>();
        }

        public override void DrawElements()
        {
            base.DrawElements();

            CuiImageComponent.Draw();
        }

        public override string ToCui(string components = "")
        {
            return base.ToCui(CuiImageComponent.ToCui());
        }
    }
}