using System.Collections.Generic;
using System.IO;
using System;
using Newtonsoft.Json;

namespace IIS_Manager_Framework.Library
{
    public static class Utilities
    {
        public static T LoadJsonConfigFile<T>()
        {
            try
            {
                using (StreamReader r = new StreamReader("appsetting.json"))
                {
                    string json = r.ReadToEnd();
                    return JsonConvert.DeserializeObject<T>(json);
                }
            }
            catch (Exception)
            {
                return default;
            }
        }
        public static T MapObject<T>(this object item)
        {
            T sr = default(T);
            if (item != null)
            {
                var obj = JsonConvert.SerializeObject(item);
                sr = JsonConvert.DeserializeObject<T>(obj);
            }
            return sr;
        }
        public static List<T> MapObjects<T>(this object item)
        {
            List<T> sr = default(List<T>);
            if (item != null)
            {
                var obj = JsonConvert.SerializeObject(item);
                sr = JsonConvert.DeserializeObject<List<T>>(obj);
            }
            return sr;
        }
    }
}
