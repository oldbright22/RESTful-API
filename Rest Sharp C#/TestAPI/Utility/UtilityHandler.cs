using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TestAPI.Utility
{
    public class UtilityHandler
    {
        [return: MaybeNull]
        public static List<T> GetContentList<T>(RestResponse response)
        {
            if (response == null || response.Content == null)
            {
                return default;
            }

            var content = response.Content;
            
            return JsonConvert.DeserializeObject<List<T>>(content);
        }

        [return: MaybeNull]
        public static T GetContent<T>(RestResponse response)
        {
            if (response == null || response.Content == null)
            {
                return default;
            }

            var content = response.Content;
            
            return JsonConvert.DeserializeObject<T>(content);
        }

        [return: MaybeNull]
        public static T ParseJson<T>(String file)
        {
            return JsonConvert.DeserializeObject<T>(File.ReadAllText(file));
        }

        public static string GetFilePath(string name)
        {
            string path = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory));
            path = string.Format(path + "TestData\\{0}", name);
            return path;
        }


        public static string GetFileData(string fileLocation)
        {
            string data = "";
            FileInfo info = new FileInfo(fileLocation);
            using (FileStream stream = info.Open(FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    data = reader.ReadToEnd();
                }
            }
            return data;
        }

        protected struct ObjectResponseResult<T>
        {
            public ObjectResponseResult(T responseObject, string responseText)
            {
                this.Object = responseObject;
                this.Text = responseText;
            }

            public T Object { get; }

            public string Text { get; }
        }
    }
}
