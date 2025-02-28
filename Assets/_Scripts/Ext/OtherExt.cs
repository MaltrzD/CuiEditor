using Assets._Scripts.CUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

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

        // далбоеб на связи 
        public static string OverrideRectTransform(this string originalString, BaseCuiElement element, string anchorMin, string anchorMax, string offsetMin, string offsetMax)
        {
            return originalString
                .Replace($"AnchorMin = \"{element.AnchorMin.ToCuiFormat()}\"", $"AnchorMin = {anchorMin}")
                .Replace($"AnchorMax = \"{element.AnchorMax.ToCuiFormat()}\"", $"AnchorMax = {anchorMax}")
                .Replace($"OffsetMin = \"{element.OffsetMin.ToCuiFormat()}\"", $"OffsetMin = {offsetMin}")
                .Replace($"OffsetMax = \"{element.OffsetMax.ToCuiFormat()}\"", $"OffsetMax = {offsetMax}");
        }

        public static string FloatToStringFormat(this float value) => value.ToString().Replace(',', '.') + "f";
    }
}
