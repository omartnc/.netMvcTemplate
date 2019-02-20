using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using Newtonsoft.Json;

namespace EventManagementSystem.Web.Helpers.HttpHelpers
{
    public class JsonContent : StringContent
    {
        public JsonContent(object value)
            : base(JsonConvert.SerializeObject(value), Encoding.UTF8, "application/json")
        {
        }

        public JsonContent(object value, string mediaType)
            : base(JsonConvert.SerializeObject(value), Encoding.UTF8, mediaType)
        {
        }
    }
}