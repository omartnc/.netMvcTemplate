using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace EventManagementSystem.Web.Helpers.HttpHelpers
{
    public static class HttpRequestFactory
    {
        public static HttpResponseMessage Get(string requestUri)
        {
            HttpClient httpClient = new HttpClient();
           return  httpClient.GetAsync(requestUri).Result;
        }

        public static  HttpResponseMessage Get(string requestUri, string bearerToken)
        {
            var builder = new HttpRequestBuilder()
                                .AddMethod(HttpMethod.Get)
                                .AddRequestUri(requestUri)
                                .AddBearerToken(bearerToken);
            try
            {
                HttpResponseMessage result =  builder.SendAsync().Result;
                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
           
            
        }

        public static HttpResponseMessage PostWithContent(string requestUri, string content)
        {
            HttpClient httpClient = new HttpClient();
            var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
            var response = httpClient.PostAsync("http://10.6.10.42:81/api/TbbsAllUserInfo/GetAllUserInfo", stringContent).Result;
            return response;
        }

        public static async Task<HttpResponseMessage> Post(string requestUri, HttpContent value)
            => await Post(requestUri, value, "");

        public static async Task<HttpResponseMessage> Post(
            string requestUri, HttpContent value, string bearerToken)
        {
            var builder = new HttpRequestBuilder()
                                .AddMethod(HttpMethod.Post)
                                .AddRequestUri(requestUri)
                                .AddContent(value)
                                .AddBearerToken(bearerToken);

            return await builder.SendAsync();
        }



        public static async Task<HttpResponseMessage> Put(string requestUri, object value)
            => await Put(requestUri, value, "");

        public static async Task<HttpResponseMessage> Put(
            string requestUri, object value, string bearerToken)
        {
            var builder = new HttpRequestBuilder()
                                .AddMethod(HttpMethod.Put)
                                .AddRequestUri(requestUri)
                                .AddContent(new JsonContent(value))
                                .AddBearerToken(bearerToken);

            return await builder.SendAsync();
        }

        //public static async Task<HttpResponseMessage> Patch(string requestUri, object value)
        //    => await Patch(requestUri, value, "");

        //public static async Task<HttpResponseMessage> Patch(
        //    string requestUri, object value, string bearerToken)
        //{
        //    var builder = new HttpRequestBuilder()
        //                        .AddMethod(new HttpMethod("PATCH"))
        //                        .AddRequestUri(requestUri)
        //                        .AddContent(new PatchContent(value))
        //                        .AddBearerToken(bearerToken);

        //    return await builder.SendAsync();
        //}

        public static async Task<HttpResponseMessage> Delete(string requestUri)
            => await Delete(requestUri, "");

        public static async Task<HttpResponseMessage> Delete(
            string requestUri, string bearerToken)
        {
            var builder = new HttpRequestBuilder()
                                .AddMethod(HttpMethod.Delete)
                                .AddRequestUri(requestUri)
                                .AddBearerToken(bearerToken);

            return await builder.SendAsync();
        }

        public static async Task<HttpResponseMessage> PostFile(string requestUri,
            string filePath, string apiParamName)
            => await PostFile(requestUri, filePath, apiParamName, "");

        public static async Task<HttpResponseMessage> PostFile(string requestUri,
            string filePath, string apiParamName, string bearerToken)
        {
            var builder = new HttpRequestBuilder()
                                .AddMethod(HttpMethod.Post)
                                .AddRequestUri(requestUri)
                                .AddContent(new FileContent(filePath, apiParamName))
                                .AddBearerToken(bearerToken);

            return await builder.SendAsync();
        }
    }
}