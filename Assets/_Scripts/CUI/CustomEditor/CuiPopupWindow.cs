using Assets._Scripts.CUI;
using UnityEditor;

public class CuiAutoPopupWindow : EditorWindow
{
    private static CuiAutoPopupWindow window;
    private BaseCuiElement selectedElement;

    [InitializeOnLoadMethod]
    private static void Init()
    {
        Selection.selectionChanged += OnSelectionChange;
    }

    private static void OnSelectionChange()
    {
        if (window == null)
            window = GetWindow<CuiAutoPopupWindow>("CUI");
        

        if (Selection.activeGameObject != null)
        {
            BaseCuiElement element = Selection.activeGameObject.GetComponent<BaseCuiElement>();
            if (element != null)
            {
                window.selectedElement = element;
                window.Repaint();
                window.Show();
            }
            else
            {
                if (window == null) return;

                window.selectedElement = null;

                window.Repaint();
                window.Show();
            }
        }
        else
        {
            window.selectedElement = null;

            window.Repaint();
            window.Show();
        }
    }

    private void OnGUI()
    {
        if (selectedElement == null)
        {
            EditorGUILayout.LabelField("Ёлемент не выбран!");
            return;
        }

        selectedElement.DrawElements();
    }
}
