using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Joss.Helpers
{
    public class Toolbox
    { 
        public static string RemoveLineEndings(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }
            string lineSeparator = ((char)0x2028).ToString();
            string paragraphSeparator = ((char)0x2029).ToString();

            return value.Replace("\r\n", string.Empty)
                        .Replace("\n", string.Empty)
                        .Replace("\r", string.Empty)
                        .Replace(lineSeparator, string.Empty)
                        .Replace(paragraphSeparator, string.Empty);
        }

        public static GameObject GetAncestor(GameObject item)
        {
            if (item.transform.parent == null)
                return item;
            return (GetAncestor(item.transform.parent.gameObject));
        }

        public static float Remap(float input, float fromMin, float fromMax, float toMin, float toMax)
        {
            float fromAbs = input - fromMin;
            float fromMaxAbs = fromMax - fromMin;

            float normal = fromAbs / fromMaxAbs;

            float toMaxAbs = toMax - toMin;
            float toAbs = toMaxAbs * normal;

            float to = toAbs + toMin;

            return to;
        }

        public static float Map(float input, float x1, float x2, float y1, float y2)
        {
            float m = (y2 - y1) / (x2 - x1);
            float c = y1 - m * x1; // point of interest: c is also equal to y2 - m * x2, though float math might lead to slightly different results.

            return m * input + c;
        }

        public static float RoundToCustom(float input, float round)
        {
            return input = input - (input % round);
        }

        public static bool RandomBool()
        {
        if (Random.value >= 0.5)
            return true;

        return false;
        }   

    }
}

