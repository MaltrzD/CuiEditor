using Assets._Scripts.CUI;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Assets._Scripts
{
    public class Tests : EditorWindow
    {
        [MenuItem("CUIEditor/Generate")]
        private static void GenerateCui()
        {
            Debug.Log("<color=green>Start generating...</color>");

            var selectedObject = Selection.activeGameObject;
            if (selectedObject == null)
            {
                Debug.LogError("Select root object!");
                return;
            }

            Transform root = selectedObject.transform;

            List<BaseCuiElement> allElements = new List<BaseCuiElement>();
            GetElements(root, allElements);

            Debug.LogWarning(allElements.Count);

            File.WriteAllText($"Assets/Parse/Parse.txt", CuiBuilder.Build(allElements));

            Debug.Log("<color=green>Finish!</color>");
        }

        private static void GetElements(Transform root, List<BaseCuiElement> elements)
        {
            foreach (Transform child in root)
            {
                if (child.TryGetComponent(out BaseCuiElement element))
                {
                    elements.Add(element);

                    GetElements(child, elements);
                }
            }
        }

    }
}