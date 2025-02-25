using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets._Scripts.Ext
{
    public static class VectorExt
    {
        public static string ToCuiFormat(this Vector2 vector) => $"{vector.x:0.#######} {vector.y:0.#######}".Replace(',', '.');
    }
}