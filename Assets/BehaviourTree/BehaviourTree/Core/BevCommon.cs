using System.Collections.Generic;


namespace BevTree
{

    class GuidGen
    {
        public static long GenUniqueGUID()
        {
            byte[] buffer = System.Guid.NewGuid().ToByteArray();
            return System.BitConverter.ToInt64(buffer, 0);
        }
    }

    class RandomGen
    {
        private static System.Random rnd = new System.Random(System.DateTime.Now.Millisecond);

        /// <summary>
        /// get a random integer in [min, max].
        /// </summary>
        /// <param name="min">min value</param>
        /// <param name="max">max value</param>
        /// <returns></returns>
        public static int RandInt(int min, int max)
        {
            return rnd.Next(min, max + 1);
        }

        /// <summary>
        /// get a random float in [0, 1).
        /// </summary>
        /// <returns></returns>
        public static float RandFloat()
        {
            return (rnd.Next(0, int.MaxValue)) / (int.MaxValue + 1.0f);
        }

        /// <summary>
        /// get a random float in (-1, 1).
        /// </summary>
        /// <returns></returns>
        public static float RandClamp()
        {
            return RandFloat() - RandFloat();
        }
    }


}
