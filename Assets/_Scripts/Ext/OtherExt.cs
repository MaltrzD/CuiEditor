using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets._Scripts.Ext
{
    public static class OtherExt
    {
        public static string Escape(string value, object arg)
        {
            if (string.IsNullOrEmpty(value)) return "";
            if (string.IsNullOrEmpty(arg?.ToString())) return "";

            if (Int32.TryParse(arg.ToString(), out int result))
            {
                if (result == 0) return "";
            }

            return value.Replace("\"\"", $"\"{arg}\"");
        }
    }
}
