using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace SudokuSolver.Helper
{
    public static class Extension
    {
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
