using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace YooMoney.Core {
    public class ApiClient : IDisposable {
        public HttpClient Client { get; } = new ();
        public string ApiUri;
        public string Token;

        public void Dispose() {
            Client.Dispose();
        }

        public ApiClient(string apiUri, string token) {
            ApiUri = apiUri;
            Token = token;
        }

        public async Task<HttpResponseMessage> PostAsync(string path, Dictionary<string, string> parameters = null) {
            var fullUri = ApiUri.EndsWith("/") ? ApiUri + path : ApiUri + "/" + path;
            var r = new HttpRequestMessage(HttpMethod.Post, fullUri);
            r.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            if (parameters != null) {
                r.Content = new FormUrlEncodedContent(parameters);
            } else {
                r.Headers.TryAddWithoutValidation("Content-Type", "application/x-www-form-urlencoded");
                r.Headers.TryAddWithoutValidation("Content-Length", "0");
            }
            return await Client.SendAsync(r);
        }
    }
}
