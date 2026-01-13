using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading.Tasks;
using KoenZomers.OneDrive.Api;
using KoenZomers.OneDrive.Api.Entities;
using Newtonsoft.Json;

namespace KoenZomersKeePassOneDriveSync.Providers
{
    /// <summary>
    /// Helper class for making direct Microsoft Graph API calls
    /// Used as a workaround for deprecated OneDrive endpoints
    /// </summary>
    internal static class GraphApiHelper
    {
        // IMPORTANT: Must end with / for relative paths to work correctly with HttpClient
        private const string GraphApiBaseUrl = "https://graph.microsoft.com/v1.0/";

        /// <summary>
        /// Retrieves items the user is following in their OneDrive for Business.
        /// This is used as a fallback if sharedWithMe isn't available.
        /// Note: Users need to "follow" items in OneDrive web interface for them to appear here.
        /// </summary>
        /// <param name="oneDriveApi">Authenticated OneDrive API instance</param>
        /// <returns>Collection of OneDriveItem objects representing followed items</returns>
        public static async Task<OneDriveItemCollection> GetFollowingItems(OneDriveApi oneDriveApi)
        {
            return await GetItemsFromGraph(oneDriveApi, "me/drive/following");
        }

        /// <summary>
        /// Retrieves items shared with the current user using Microsoft Graph.
        /// </summary>
        /// <param name="oneDriveApi">Authenticated OneDrive API instance</param>
        /// <returns>Collection of OneDriveItem objects representing items shared with the user</returns>
        public static async Task<OneDriveItemCollection> GetSharedWithMeItems(OneDriveApi oneDriveApi)
        {
            return await GetItemsFromGraph(oneDriveApi, "me/drive/sharedWithMe");
        }

        /// <summary>
        /// Retrieves items from a specific drive path using Microsoft Graph.
        /// This allows direct access to a SharePoint folder by drive ID and path.
        /// </summary>
        /// <param name="oneDriveApi">Authenticated OneDrive API instance</param>
        /// <param name="driveId">The ID of the drive (e.g., SharePoint document library)</param>
        /// <param name="folderPath">The path within the drive (e.g., "Infrastruktur/07 Keepass")</param>
        /// <returns>Collection of OneDriveItem objects representing the folder contents</returns>
        public static async Task<OneDriveItemCollection> GetDrivePathChildren(
            OneDriveApi oneDriveApi,
            string driveId,
            string folderPath)
        {
            // Build the request path (no leading slash - BaseUrl ends with /)
            string requestPath;
            if (string.IsNullOrEmpty(folderPath))
            {
                // Root of drive
                requestPath = string.Format("drives/{0}/root/children", driveId);
            }
            else
            {
                requestPath = string.Format("drives/{0}/root:/{1}:/children", driveId, folderPath);
            }
            return await GetItemsFromGraph(oneDriveApi, requestPath);
        }

        /// <summary>
        /// Retrieves SharePoint sites the user has access to
        /// </summary>
        /// <param name="oneDriveApi">Authenticated OneDrive API instance</param>
        /// <returns>Array of GraphSite objects representing accessible SharePoint sites</returns>
        public static async Task<GraphSite[]> GetSites(OneDriveApi oneDriveApi)
        {
            var accessToken = oneDriveApi.AccessToken != null ? oneDriveApi.AccessToken.AccessToken : null;

            if (string.IsNullOrEmpty(accessToken))
            {
                throw new InvalidOperationException("No valid access token available. Please authenticate first.");
            }

            using (var httpClient = CreateGraphHttpClient(accessToken))
            {
                var sites = new List<GraphSite>();
                var nextRequest = "sites?search=*";

                while (!string.IsNullOrEmpty(nextRequest))
                {
                    var response = await httpClient.GetAsync(nextRequest);

                    if (!response.IsSuccessStatusCode)
                    {
                        var errorContent = await response.Content.ReadAsStringAsync();
                        throw new HttpRequestException(
                            string.Format("Graph API request failed with status {0}: {1}", response.StatusCode, errorContent));
                    }

                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<GraphSiteCollectionResponse>(jsonResponse);

                    if (result != null && result.Value != null && result.Value.Length > 0)
                    {
                        sites.AddRange(result.Value);
                    }

                    nextRequest = result != null ? result.NextLink : null;
                }

                return sites.ToArray();
            }
        }

        /// <summary>
        /// Retrieves drives (document libraries) for a specific SharePoint site
        /// </summary>
        /// <param name="oneDriveApi">Authenticated OneDrive API instance</param>
        /// <param name="siteId">The ID of the SharePoint site</param>
        /// <returns>Array of GraphDrive objects representing the site's document libraries</returns>
        public static async Task<GraphDrive[]> GetSiteDrives(OneDriveApi oneDriveApi, string siteId)
        {
            var accessToken = oneDriveApi.AccessToken != null ? oneDriveApi.AccessToken.AccessToken : null;

            if (string.IsNullOrEmpty(accessToken))
            {
                throw new InvalidOperationException("No valid access token available. Please authenticate first.");
            }

            using (var httpClient = CreateGraphHttpClient(accessToken))
            {
                var drives = new List<GraphDrive>();
                var nextRequest = string.Format("sites/{0}/drives", siteId);

                while (!string.IsNullOrEmpty(nextRequest))
                {
                    var response = await httpClient.GetAsync(nextRequest);

                    if (!response.IsSuccessStatusCode)
                    {
                        var errorContent = await response.Content.ReadAsStringAsync();
                        throw new HttpRequestException(
                            string.Format("Graph API request failed with status {0}: {1}", response.StatusCode, errorContent));
                    }

                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<GraphDriveCollectionResponse>(jsonResponse);

                    if (result != null && result.Value != null && result.Value.Length > 0)
                    {
                        drives.AddRange(result.Value);
                    }

                    nextRequest = result != null ? result.NextLink : null;
                }

                return drives.ToArray();
            }
        }

        private static async Task<OneDriveItemCollection> GetItemsFromGraph(OneDriveApi oneDriveApi, string requestPath)
        {
            // Get the access token from the OneDriveApi instance
            var accessToken = oneDriveApi.AccessToken != null ? oneDriveApi.AccessToken.AccessToken : null;

            if (string.IsNullOrEmpty(accessToken))
            {
                throw new InvalidOperationException("No valid access token available. Please authenticate first.");
            }

            using (var httpClient = CreateGraphHttpClient(accessToken))
            {
                var items = new List<OneDriveItem>();
                var nextRequest = requestPath;

                while (!string.IsNullOrEmpty(nextRequest))
                {
                    var response = await httpClient.GetAsync(nextRequest);

                    if (!response.IsSuccessStatusCode)
                    {
                        var errorContent = await response.Content.ReadAsStringAsync();
                        throw new HttpRequestException(
                            string.Format("Graph API request failed with status {0}: {1}", response.StatusCode, errorContent));
                    }

                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<GraphCollectionResponse>(jsonResponse);

                    if (result != null && result.Value != null && result.Value.Length > 0)
                    {
                        items.AddRange(result.Value);
                    }

                    nextRequest = result != null ? result.NextLink : null;
                }

                return new OneDriveItemCollection
                {
                    Collection = items.Count > 0 ? items.ToArray() : new OneDriveItem[0]
                };
            }
        }

        /// <summary>
        /// Creates an HttpClient configured for Microsoft Graph API calls with proper authentication and proxy support
        /// </summary>
        /// <param name="accessToken">The bearer token for authentication</param>
        /// <returns>Configured HttpClient instance</returns>
        private static HttpClient CreateGraphHttpClient(string accessToken)
        {
            var proxySettings = Utilities.GetProxySettings();
            var proxyCredentials = Utilities.GetProxyCredentials();

            var httpClientHandler = new HttpClientHandler
            {
                UseDefaultCredentials = proxyCredentials == null,
                UseProxy = proxySettings != null,
                Proxy = proxySettings
            };

            if (proxyCredentials != null && httpClientHandler.Proxy != null)
            {
                httpClientHandler.Proxy.Credentials = proxyCredentials;
            }

            var httpClient = new HttpClient(httpClientHandler)
            {
                BaseAddress = new Uri(GraphApiBaseUrl)
            };

            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            // Add user agent similar to other parts of the plugin
            var assemblyVersion = Assembly.GetCallingAssembly().GetName().Version;
            httpClient.DefaultRequestHeaders.Add("User-Agent",
                string.Format("KoenZomers KeePass OneDriveSync v{0}.{1}.{2}.{3}",
                    assemblyVersion.Major, assemblyVersion.Minor, assemblyVersion.Build, assemblyVersion.Revision));

            return httpClient;
        }

        /// <summary>
        /// Response wrapper for Graph API collection responses (using Newtonsoft.Json)
        /// </summary>
        private class GraphCollectionResponse
        {
            [JsonProperty("value")]
            public OneDriveItem[] Value { get; set; }

            [JsonProperty("@odata.nextLink")]
            public string NextLink { get; set; }
        }

        private class GraphSiteCollectionResponse
        {
            [JsonProperty("value")]
            public GraphSite[] Value { get; set; }

            [JsonProperty("@odata.nextLink")]
            public string NextLink { get; set; }
        }

        private class GraphDriveCollectionResponse
        {
            [JsonProperty("value")]
            public GraphDrive[] Value { get; set; }

            [JsonProperty("@odata.nextLink")]
            public string NextLink { get; set; }
        }
    }

    /// <summary>
    /// Represents a SharePoint site from Microsoft Graph API
    /// </summary>
    public class GraphSite
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("displayName")]
        public string DisplayName { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("webUrl")]
        public string WebUrl { get; set; }
    }

    /// <summary>
    /// Represents a drive (document library) from Microsoft Graph API
    /// </summary>
    public class GraphDrive
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("driveType")]
        public string DriveType { get; set; }

        [JsonProperty("webUrl")]
        public string WebUrl { get; set; }
    }
}
