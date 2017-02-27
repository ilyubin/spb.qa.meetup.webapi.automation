using System;

namespace AT.Github.Automation.Framework
{
    public static class RandomExtension
    {
        public static void Shuffle<T>(this Random rng, T[] array)
        {
            var n = array.Length;
            while (n > 1)
            {
                var k = rng.Next(n--);
                var temp = array[n];
                array[n] = array[k];
                array[k] = temp;
            }
        }
    }
}
