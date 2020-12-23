using System;
using System.IO;
using Newtonsoft.Json;
using RestSharp.Serializers;

namespace IvoreFilm.Helpers
{
    public class NewtonsoftJsonSerializer : ISerializer
    {
        private readonly JsonSerializerSettings Settings;

        public NewtonsoftJsonSerializer()
        {
            Settings = new JsonSerializerSettings();
            Settings.NullValueHandling = NullValueHandling.Ignore;
            Settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        }

        public string ContentType
        {
            get => "application/json";
            set { }
        }

        public string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj, Settings);
        }

        public T Deserialize<T>(string s)
        {
            throw new NotImplementedException();
        }

        public T Deserialize<T>(Stream stream)
        {
            throw new NotImplementedException();
        }
    }
}