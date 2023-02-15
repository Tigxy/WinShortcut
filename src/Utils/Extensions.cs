using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace win_short_cut.Utils {
    public static class Extensions {
        public static T DeepClone<T>(this T a) {
            using (MemoryStream stream = new MemoryStream()) {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, a);
                stream.Position = 0;
                return (T)formatter.Deserialize(stream);
            }
        }

        public static bool StreamEqual(this Stream a, Stream b) {
            a.Position = 0;
            b.Position = 0;

            if (a.Length != b.Length)
                return false;

            int bufferLength = 1024;
            byte[] bufferA = new byte[bufferLength];
            byte[] bufferB = new byte[bufferLength];

            int bytesToRead;
            for (int i = 0; i < a.Length; i += bufferLength) {
                bytesToRead = (int)System.Math.Min(bufferLength, a.Length - i);
                a.Read(bufferA, 0, bytesToRead);
                b.Read(bufferB, 0, bytesToRead);

                if (!bufferA.SequenceEqual(bufferB))
                    return false;
            }
            return true;
        }

        public static bool DeepEqual<T>(this T a, T b) {
            using (MemoryStream streamA = new MemoryStream()) {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(streamA, a);
                streamA.Position = 0;

                using (MemoryStream streamB = new MemoryStream()) {
                    BinaryFormatter formatterB = new BinaryFormatter();
                    formatterB.Serialize(streamB, b);
                    streamB.Position = 0;

                    return streamA.StreamEqual(streamB);
                }
            }
        }
    }
}
