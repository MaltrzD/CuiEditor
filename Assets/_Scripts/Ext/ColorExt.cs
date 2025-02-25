using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets._Scripts.Ext
{
    public static class ColorExt
    {
        public static string ToRust(this Color color) => $"{color.r} {color.g} {color.b} {color.a}".Replace(',', '.');
    }
}