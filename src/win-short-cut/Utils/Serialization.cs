using System.IO;
using System.Text;
using System.Text.Json;
using System.Xml;
using System.Xml.Serialization;

namespace win_short_cut.Utils {
    internal class Serialization
    {
        public static void Store<T>(T obj, string path)
        {
            using (var a = System.IO.File.Open(path, System.IO.FileMode.Create))
            {
                var serializer = new XmlSerializer(typeof(T));
                var streamWriter = XmlWriter.Create(a, new()
                {
                    // to generate a pretty output file
                    Encoding = Encoding.UTF8,
                    Indent = true
                });

                serializer.Serialize(streamWriter, obj);
            }
        }

        public static (bool success, T? data) Load<T>(string path)
        {
            if (System.IO.File.Exists(path))
            {
                using (var a = System.IO.File.OpenRead(path))
                {
                    var serializer = new XmlSerializer(typeof(T));
                    var streamReader = XmlReader.Create(a);

                    if (serializer.Deserialize(streamReader) is T data)
                        return (true, data);

                }
            }

            return (false, default);
        }

        public static void StoreJson<T>(T obj, string path)
        {
            using (var a = System.IO.File.Open(path, System.IO.FileMode.Create))
            {
                using (var writer = new StreamWriter(a))
                {
                    string jsonString = JsonSerializer.Serialize<T>(obj, new JsonSerializerOptions() { WriteIndented = true });
                    writer.Write(jsonString);
                }
            }
        }

        public static (bool success, T? data) LoadJson<T>(string path)
        {
            if (System.IO.File.Exists(path))
            {
                using (var a = System.IO.File.OpenRead(path))
                {
                    using (var reader = new StreamReader(a))
                    {
                        if (JsonSerializer.Deserialize<T>(reader.BaseStream) is T data)
                            return (true, data);
                    }
                }
            }

            return (false, default);
        }
    }
}
