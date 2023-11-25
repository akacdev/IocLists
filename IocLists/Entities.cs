using System;
using System.Text.Json.Serialization;

namespace IocLists
{
    /// <summary>
    /// Represents a custom user-friendly error from IocLists.
    /// </summary>
    public class IocListsError
    {
        [JsonPropertyName("result")]
        public string Status { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }
    }

    public class CreateListParameters
    {
        /// <summary>
        /// Your username.
        /// </summary>
        [JsonPropertyName("user")]
        public string Username { get; set; }

        /// <summary>
        /// The target lists's name.
        /// </summary>
        [JsonPropertyName("listname")]
        public string ListName { get; set; }

        /// <summary>
        /// Short (250 characters) descriptor of the list.
        /// </summary>
        [JsonPropertyName("description")]
        public string Description { get; set; }

        /// <summary>
        /// Number of hours the indicators in this list remain active. (This is currently informational only)
        /// </summary>
        [JsonPropertyName("expiration")]
        public int HoursToExpire { get; set; } = 8766;

        /// <summary>
        /// Classification of the types of indicators in the list.
        /// </summary>
        [JsonPropertyName("feed_type")]
        public FeedType FeedType { get; set; } = FeedType.Security;

        /// <summary>
        /// <para>
        ///     When <b><see langword="true"/>:</b><br/>
        ///         Viewed only by owner and any explicitedly granted users. Entries and indicators are not included in search results and are not counted in any metrics.
        /// </para>   
        /// <para>
        ///     When <b><see langword="false"/>:</b><br/>
        ///         Viewed by unauthenticated and authenticated uesrs, all indicators contained in entries are included in indicator searches.
        /// </para>
        /// </summary>
        [JsonPropertyName("private")]
        public bool IsPrivate { get; set; } = false;

        /// <summary>
        /// When <see langword="true"/>, your list is considered active.
        /// </summary>
        [JsonPropertyName("active")]
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// When <see langword="true"/>, you declare that your list is a mirror of an existing source.
        /// </summary>
        [JsonPropertyName("mirror")]
        public bool IsMirror { get; set; } = false;

        /// <summary>
        /// Optionally, you can set the URL of the source for indicators in this list.
        /// </summary>
        [JsonPropertyName("mirror_original_url")]
        public string MirrorOriginalUrl { get; set; }
    }

    public enum FeedType
    {
        /// <summary>
        /// Determines that this list contains IPs, domains, URLs, and hashes with a <b>malicious intent</b>. 
        /// </summary>
        Security,
        /// <summary>
        /// Determines that this list contains IPs, domains, URLs, and hashes which align with a specific theme, such as allow lists, advertising sites, or IPs/domains associated with a specific organizations.
        /// </summary>
        Content
    }

    public class EntriesContainer
    {
        [JsonPropertyName("entries")]
        public Entry[] Entries { get; set; }
    }

    /// <summary>
    /// Represents an indicator present in a list.
    /// </summary>
    public class Entry
    {
        [JsonPropertyName("username")]
        public string Username { get; set; }

        [JsonPropertyName("feedname")]
        public string ListName { get; set; }

        [JsonPropertyName("raw")]
        public string Raw { get; set; }

        [JsonConverter(typeof(DoubleUnixConverter))]
        [JsonPropertyName("timestamp")]
        public DateTime AddedAt { get; set; }

        [JsonPropertyName("entry_id")]
        public string UUID { get; set; }
    }

    public class EntryAddParameters
    {
        [JsonPropertyName("entry")]
        public string Entry { get; set; }
    }

    public class SearchContainer
    {
        [JsonPropertyName("search_results")]
        public Entry[] Entries { get; set; } = Array.Empty<Entry>();
    }
}