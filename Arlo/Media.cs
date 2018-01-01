using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Arlo
{
    public class ArloMediaLibrary
    {
        private Arlo arlo = null;

        public ArloMediaLibrary(Arlo session)
        {
            arlo = session;
        }

        public async Task<IEnumerable<ArloVideo>> Load(int days, List<string> limitCameraIds = null, int limit = 0)
        {
            DateTime start = DateTime.Today.AddDays(-days);
            DateTime end = DateTime.Today;

            return await Load(start, end, limitCameraIds, limit);
        }

        public async Task<IEnumerable<ArloVideo>> Load(DateTime dateStart, DateTime dateEnd, List<string> limitCameraIds = null, int limit = 0)
        {
            Dictionary<string, string> extra_params = new Dictionary<string, string>();
            extra_params.Add("dateFrom", dateStart.ToString("yyyyMMdd"));
            extra_params.Add("dateTo", dateEnd.ToString("yyyyMMdd"));
            var results = await arlo.Query(Constants.LIBRARY_ENDPOINT, HttpMethod.Post, extra_params);
            var response = JsonConvert.DeserializeObject<ResultResponse>(results);
            if (response.Success)
            {
                var videos = JsonConvert.DeserializeObject<VideoResponse>(results);
                if (limitCameraIds != null && limitCameraIds.Count > 0)
                {
                    return videos.Videos.Where(x => limitCameraIds.Contains(x.DeviceId)).ToList();
                }
                else
                {
                    return videos.Videos;
                }
            } else
            {
                var error = JsonConvert.DeserializeObject<ResultFailure>(results);
                throw new Exception(error.Data.Error);
            }
        }

    }

    public partial class ArloVideo
    {
        [JsonProperty("ownerId")]
        public string OwnerId { get; set; }

        [JsonProperty("uniqueId")]
        public string UniqueId { get; set; }

        [JsonProperty("deviceId")]
        public string DeviceId { get; set; }

        [JsonProperty("createdDate")]
        public string CreatedDate { get; set; }

        [JsonProperty("currentState")]
        public string CurrentState { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("contentType")]
        public string ContentType { get; set; }

        [JsonProperty("reason")]
        public string Reason { get; set; }

        [JsonProperty("createdBy")]
        public string CreatedBy { get; set; }

        [JsonProperty("lastModified")]
        public long LastModified { get; set; }

        [JsonProperty("localCreatedDate")]
        public long LocalCreatedDate { get; set; }

        [JsonProperty("presignedContentUrl")]
        public string PresignedContentUrl { get; set; }

        [JsonProperty("presignedThumbnailUrl")]
        public string PresignedThumbnailUrl { get; set; }

        [JsonProperty("utcCreatedDate")]
        public long UtcCreatedDate { get; set; }

        [JsonProperty("timeZone")]
        public string TimeZone { get; set; }

        [JsonProperty("mediaDuration")]
        public DateTime MediaDuration { get; set; }

        [JsonProperty("mediaDurationSecond")]
        public long MediaDurationSecond { get; set; }

        [JsonProperty("donated")]
        public bool Donated { get; set; }

        public async Task<Stream> GetVideoStream()
        {
            using (var client = new HttpClient())
            {
                return await client.GetStreamAsync(this.PresignedContentUrl);
            }
        }
    }

    public partial class ArloVideo
    {
        public static ArloVideo FromJson(string json) => JsonConvert.DeserializeObject<ArloVideo>(json, Converter.Settings);
    }

}
