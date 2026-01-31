using System;
using UnityEngine;

namespace MVsToolkit.Utilities
{
    public static class MVsEnum
    {
        /// <summary>
        /// Renvoie la valeur suivante de l'enum.
        /// </summary>
        /// <typeparam name="T">Type de l'enum.</typeparam>
        /// <param name="src">Valeur actuelle.</param>
        /// <param name="loopValue">
        /// Si true : revient au début lorsque la valeur dépasse la dernière.
        /// Si false : s'arrête à la dernière valeur.
        /// </param>
        /// <returns>La valeur suivante de l'enum.</returns>
        /// <example>
        /// Etat e = Etat.Idle;
        /// e = e.Next();          // -> Etat.Walk
        /// e = e.Next();          // -> Etat.Run
        /// e = e.Next();          // -> Etat.Jump
        /// e = e.Next();          // -> Etat.Idle (loop)
        ///
        /// e = e.Next(loopValue:false); // reste sur Jump si déjà au max
        /// </example>
        public static T Next<T>(this T src, bool loopValue = true) where T : Enum
        {
            var values = (T[])Enum.GetValues(typeof(T));
            int index = Array.IndexOf(values, src) + 1;

            if (loopValue && index >= values.Length)
                index = 0;

            return values[index];
        }

        /// <summary>
        /// Renvoie la valeur précédente de l'enum.
        /// </summary>
        /// <typeparam name="T">Type de l'enum.</typeparam>
        /// <param name="src">Valeur actuelle.</param>
        /// <param name="loopValue">
        /// Si true : revient à la fin lorsque la valeur passe sous la première.
        /// Si false : s'arrête à la première valeur.
        /// </param>
        /// <returns>La valeur précédente de l'enum.</returns>
        /// <example>
        /// Etat e = Etat.Idle;
        /// e = e.Previous();      // -> Etat.Jump (loop)
        ///
        /// e = Etat.Run;
        /// e = e.Previous();      // -> Etat.Walk
        ///
        /// e = e.Previous(loopValue:false); // reste sur Idle si déjà au min
        /// </example>
        public static T Previous<T>(this T src, bool loopValue = true) where T : Enum
        {
            var values = (T[])Enum.GetValues(typeof(T));
            int index = Array.IndexOf(values, src) - 1;

            if (loopValue && index < 0)
                index = values.Length - 1;

            return values[index];
        }
    }
}