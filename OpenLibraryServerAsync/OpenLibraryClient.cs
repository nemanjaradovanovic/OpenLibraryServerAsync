using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace OpenLibraryServer.P2
{
    public sealed class OpenLibraryClient : IDisposable
    {
        private readonly HttpClient _http;

        public OpenLibraryClient()
        {
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;

            var handler = new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            };

            _http = new HttpClient(handler, disposeHandler: true)
            {
                Timeout = TimeSpan.FromSeconds(15)
            };

            _http.DefaultRequestHeaders.Accept.ParseAdd("application/json");
            _http.DefaultRequestHeaders.UserAgent.ParseAdd("OpenLibraryServer-P2/1.0 (+async)");
        }

        public async Task<byte[]> SearchAsync(IDictionary<string, string> query, CancellationToken ct)
        {
            var b = new StringBuilder();
            bool first = true;
            foreach (var kv in query)
            {
                if (!first) b.Append('&'); else first = false;
                b.Append(HttpUtility.UrlEncode(kv.Key));
                b.Append('=');
                b.Append(HttpUtility.UrlEncode(kv.Value));
            }

            var url = "https://openlibrary.org/search.json?" + b.ToString();

            using (var resp = await _http
                .GetAsync(url, HttpCompletionOption.ResponseHeadersRead, ct)
                .ConfigureAwait(false))
            {
                resp.EnsureSuccessStatusCode();
                return await resp.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
            }
        }

        public void Dispose() => _http.Dispose();
    }
}
