using UnityEngine;

namespace Assets._Scripts.CUI.Interface
{
    public interface ICuiComponent
    {//
        string ToCui();
        void Draw();
        MonoBehaviour DoDestroy();
        void Initialize();
    }
}
