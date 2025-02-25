using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets._Scripts.CUI
{
    public class CuiBuilder
    {
        public static string Build(IEnumerable<BaseCuiElement> elements)
        {
            var builder = new StringBuilder();

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

            return builder.ToString();
        }
    }
}
