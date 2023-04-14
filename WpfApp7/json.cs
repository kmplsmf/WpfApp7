using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
namespace WpfApp7
{
    public static class JSON
    {
        public static void serialize(string path, object obj)
        {
            string json = JsonConvert.SerializeObject(obj);
            File.WriteAllText(path, json);
        }
        public static T deserialize<T>(string path)
        {
            T obj = JsonConvert.DeserializeObject<T>(File.ReadAllText(path));
            return obj;
        }
    }
}
