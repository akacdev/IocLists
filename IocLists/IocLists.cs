using System;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Generic;

namespace IocLists
{
    /// <summary>
    /// The primary class for interacting with the IOC Lists API.
    /// </summary>
    public class IocListsClient
    {
        private static readonly HttpClientHandler HttpHandler = new()
        {
            AutomaticDecompression = DecompressionMethods.All
        };

        private readonly HttpClient Client = new(HttpHandler)
        {
            BaseAddress = Constants.BaseUri,
            DefaultRequestVersion = new(2, 0),
        };

        /// <summary>
        /// Create a new instance of the API client.
        /// </summary>
        /// <param name="key">Your IOC Lists API key. Create one at <a href="https://ioclists.com/r/settings"></a>.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public IocListsClient(string key)
        {
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key), "Key is null or empty.");

            Client.DefaultRequestHeaders.AcceptEncoding.ParseAdd(Constants.AcceptedEncoding);
            Client.DefaultRequestHeaders.UserAgent.ParseAdd(Constants.UserAgent);
            Client.DefaultRequestHeaders.Accept.ParseAdd(Constants.JsonContentType);
            Client.DefaultRequestHeaders.Add("X-API-Key", key);
        }

        /// <summary>
        /// Create a new list of indicators using parameters.
        /// </summary>
        /// <param name="username">Your username.</param>
        /// <param name="listName">The target list's name.</param>
        /// <param name="description">Short (250 characters) descriptor of the list.</param>
        /// <param name="hoursToExpire">Number of hours the indicators in this list remain active. (This is currently informational only)</param>
        /// <param name="feedType">Classification of the types of indicators in the list.</param>
        /// <param name="isPrivate">When <see langword="true"/>, only a restricted set of users are able to view this list.</param>
        /// <param name="isActive">When <see langword="true"/>, your list is considered active.</param>
        /// <param name="isMirror">When <see langword="true"/>, you declare that your list is a mirror of an existing source.</param>
        /// <param name="mirrorOriginalUrl">Optionally, you can set the URL of the source for indicators in this list.</param>
        /// <returns></returns>
        /// <exception cref="IocListsException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public async Task CreateList(string username, string listName, string description, int hoursToExpire = 8766, FeedType feedType = FeedType.Security, bool isPrivate = false, bool isActive = true, bool isMirror = false, string mirrorOriginalUrl = null)
            => await CreateList(new()
            {
                Username = username,
                ListName = listName,
                Description = description,
                HoursToExpire = hoursToExpire,
                FeedType = feedType,
                IsPrivate = isPrivate,
                IsActive = isActive,
                IsMirror = isMirror,
                MirrorOriginalUrl = mirrorOriginalUrl
            });

        /// <summary>
        /// Create a new list of indicators using an object.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        /// <exception cref="IocListsException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public async Task CreateList(CreateListParameters parameters)
        {
            if (parameters is null) throw new ArgumentNullException(nameof(parameters), "Parameters are null or empty.");
            if (parameters.Username is null) throw new ArgumentNullException(nameof(parameters), "Username is null or empty.");
            if (parameters.ListName is null) throw new ArgumentNullException(nameof(parameters), "List name is null or empty.");
            if (parameters.HoursToExpire < 1) throw new ArgumentOutOfRangeException(nameof(parameters), "Hours to expire have to be a positive number.");
            if (parameters.Description is not null && parameters.Description.Length > 250)
                throw new ArgumentOutOfRangeException(nameof(parameters), "Description length is over 250 characters.");

            await Client.Request(HttpMethod.Post, $"lists/{parameters.Username}/", parameters);
        }

        /// <summary>
        /// Get the <c>20</c> most recent indicators in a list.
        /// </summary>
        /// <param name="username">The target list's author username.</param>
        /// <param name="listName">The target list's name.</param>
        /// <returns></returns>
        /// <exception cref="IocListsException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<Entry[]> GetRecent(string username, string listName)
        {
            if (username is null) throw new ArgumentNullException(nameof(username), "Username is null or empty.");
            if (listName is null) throw new ArgumentNullException(nameof(listName), "List name is null or empty.");

            HttpResponseMessage res = await Client.Request(HttpMethod.Get, $"lists/{username}/{listName.UrlEncode()}/");

            return (await res.Deseralize<EntriesContainer>()).Entries;
        }

        /// <summary>
        /// Add a new indicator into your list.
        /// </summary>
        /// <param name="username">Your username.</param>
        /// <param name="listName">Your list's name.</param>
        /// <param name="entry">
        ///     <para>
        ///         The raw entry value, including a comment.<br/>
        ///         See <a href="https://ioclists.com/doc#entries"></a> for documentation on the syntax of entries.
        ///     </para>
        ///     <para>
        ///     <b>Examples:</b><br/>
        ///         <example>
        ///             <c>http://buithiyennhi[.]com:80/smt/loki/fre.php ^8e3951897bf8371e6010e3254b99e86d #lokibot -- C2 for lokibot</c>
        ///         </example><br/>
        ///         <example>
        ///             <c>hxxps://example[.]com -- Example Indicator</c>
        ///         </example>
        ///     </para>
        /// </param>
        /// <returns></returns>
        /// <exception cref="IocListsException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task Add(string username, string listName, string entry)
        {
            if (username is null) throw new ArgumentNullException(nameof(username), "Username is null or empty.");
            if (listName is null) throw new ArgumentNullException(nameof(listName), "List name is null or empty.");
            if (entry is null) throw new ArgumentNullException(nameof(entry), "Entry is null or empty.");

            await Client.Request(HttpMethod.Post, $"lists/{username}/{listName.UrlEncode()}/", new EntryAddParameters()
            {
                Entry = entry
            });
        }

        /// <summary>
        /// Get all unique indicators in a list.
        /// </summary>
        /// <param name="username">The target list's author username.</param>
        /// <param name="listName">The target list name.</param>
        /// <returns></returns>
        /// <exception cref="IocListsException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<string[]> GetUnique(string username, string listName)
        {
            if (username is null) throw new ArgumentNullException(nameof(username), "Username is null or empty.");
            if (listName is null) throw new ArgumentNullException(nameof(listName), "List name is null or empty.");

            HttpResponseMessage res = await Client.Request(HttpMethod.Get, $"lists/{username}/{listName.UrlEncode()}/indicators/unique", Constants.PlainTextContentType);

            List<string> output = new();

            using Stream stream = await res.Content.ReadAsStreamAsync();
            using (StreamReader sr = new(stream))
            {
                string line;
                while ((line = await sr.ReadLineAsync()) is not null)
                {
                    if (string.IsNullOrEmpty(line)) break;

                    output.Add(line);
                }
            }

            return output.ToArray();
        }

        /// <summary>
        /// Search for an indicator across the platform.
        /// <para>
        ///     <b>Accepted values:</b> <c>IPv4, IPv6, FQDN, URLs, MD5, SHA1,</c> and <c>SHA256</c>
        /// </para>
        /// </summary>
        /// <param name="query">The query to search for.</param>
        /// <returns></returns>
        /// <exception cref="IocListsException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<Entry[]> Search(string query)
        {
            if (query is null) throw new ArgumentNullException(nameof(query), "Query is null or empty.");

            HttpResponseMessage res = await Client.Request(HttpMethod.Get, $"indicator/entries?indicator={query.UrlEncode()}");

            return (await res.Deseralize<SearchContainer>()).Entries;
        }
    }
}