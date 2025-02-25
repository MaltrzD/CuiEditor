using Unity.VisualScripting;
using UnityEngine.UI;

namespace Assets._Scripts.CUI.Elements
{
    public class CuiTextElement : BaseCuiElement
    {
        public CuiTextComponent CuiTextComponent { get; private set; }

        protected override void OnValidate()
        {
            base.OnValidate();

            if(!CuiTextComponent)
                CuiTextComponent = gameObject.GetOrAddComponent<CuiTextComponent>();
        }

        public override void DrawElements()
        {
            base.DrawElements();

            CuiTextComponent.Draw();
        }
        public override string ToCui(string components = "")
        {
            return base.ToCui(CuiTextComponent.ToCui());
        }
    }
}