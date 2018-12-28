using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace SudokuSolver.Helper
{
    public static class DeepCopyExtension
    {
        /// <summary>
        /// Make a deep copy of self.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <returns>New object - deep copy of self.</returns>
        public static T DeepCopy<T>(this T self)
        {
            using (var stream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, self);
                stream.Position = 0;
                return (T)formatter.Deserialize(stream);
            }
        }
    }
}
