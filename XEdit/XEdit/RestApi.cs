using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
namespace XEdit
{
    public static class RestApi
    {
        private static readonly HttpClient _client;
        static RestApi()
        {
            _client = new HttpClient()
            {
                Timeout = TimeSpan.FromHours(1),
                MaxResponseContentBufferSize = int.MaxValue
            };
        }
        public static async System.Threading.Tasks.Task<bool> SendFiles(XEdit.ViewModels.FileModel[] models)
        {
            try
            {
                return (await _client.PostAsync("http://192.168.0.102:5000/file/", new StringContent(JsonConvert.SerializeObject(models), Encoding.UTF8, "application/json"))).IsSuccessStatusCode;
            }
            catch(Exception ex)
            {
                _client.CancelPendingRequests();
                return false;
            }
        }
    }
}
