using UnityEngine;

namespace MVsToolkit.Utils
{
    public static class EnumUtils
    {
        /// <summary>
        /// Takes an enum value and checks if it is equal to any of the provided values.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumValue"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static bool IsOneOf<T>(this T enumValue, params T[] values) where T : System.Enum
        {
            foreach (T value in values)
            {
                if (enumValue.Equals(value))
                {
                    return true;
                }
            }
            return false;
        }
    }
}