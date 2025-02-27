using Assets._Scripts.App;
using Assets._Scripts.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets._Scripts.CUI
{
    public class CuiBuilder
    {
        private static List<ImageLibraryImage> storedImages = new List<ImageLibraryImage>();
        public static void AddImage(ImageLibraryImage image)
        {
            if(storedImages.Contains(image)) return;
            if (image == null) return;

            storedImages.Add(image);
        }

        public static string Build(IEnumerable<BaseCuiElement> elements)
        {
            var groupedElements = elements.ToLookup(x => x.GetComponent<ICustomBuild>() != null);

            var builder = new StringBuilder();
            storedImages.Clear();

            foreach (var build in groupedElements[true].Select(e => e.GetComponent<ICustomBuild>()))
            {
                builder.AppendLine(build.Build());
            }

            elements = groupedElements[false];

            //CUI
            builder.AppendLine("[ChatCommand(\"ui\")]");
            builder.AppendLine("private void DrawUI(BasePlayer player)");
            builder.AppendLine("{");
            builder.AppendLine("var container = new CuiElementContainer();\n");

            foreach (var element in elements)
            {
                builder.Append(element.ToCui() + "\n");
            }

            builder.AppendLine("CuiHelper.AddUi(player, container);");

            builder.AppendLine("}");


            //ImageLibrary
            if (storedImages.Count != 0)
            {
                if (AppConfig.Instance.AddImageLibraryReference) builder.AppendLine("[PluginReference] private Plugin ImageLibrary;");
                if (AppConfig.Instance.AddImageLibraryReference)
                {
                    builder.AppendLine("public string GetImage(string shortname, ulong skin = 0) => (string)ImageLibrary.Call(\"GetImage\", shortname, skin);");
                    builder.AppendLine("public bool AddImage(string url, string shortname, ulong skin = 0) => (bool)ImageLibrary?.Call(\"AddImage\", url, shortname, skin);");
                }

                builder.AppendLine($"private void {AppConfig.Instance.LoadImagesMethodName}()");
                builder.AppendLine("{");

                foreach (var img in storedImages)
                {
                    builder.AppendLine($"AddImage(\"{img.Url}\", \"{img.Name}\");");
                }

                builder.AppendLine("}");
            }

            return builder.ToString();
        }
    }
}
