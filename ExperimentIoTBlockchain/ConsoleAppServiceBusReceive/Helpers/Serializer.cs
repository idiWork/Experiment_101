using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace ConsoleAppServiceBusReceive.Helpers
{
    class Serializer
    {
        public static string SerializeToJson<T>(T obj)
        {
            MemoryStream jsonMemoryStream = new MemoryStream();
            DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(obj.GetType());
            jsonSerializer.WriteObject(jsonMemoryStream, obj);
            jsonMemoryStream.Position = 0;
            StreamReader jsonStreamReader = new StreamReader(jsonMemoryStream);
            string json = jsonStreamReader.ReadToEnd();
            jsonMemoryStream.Close();
            return json;
        }

        public static T DeserializeFromJson<T>(string json)
        {
            MemoryStream jsonMemoryStream = new MemoryStream(Encoding.UTF8.GetBytes(json));
            DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(T));
            T obj = (T) jsonSerializer.ReadObject(jsonMemoryStream);
            jsonMemoryStream.Close();
            return obj;
        }
    }
}
