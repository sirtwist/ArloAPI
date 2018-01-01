using System;
using System.Collections.Generic;
using System.Text;

namespace Arlo
{
    public static class Constants
    {
        public const string API_URL = "https://arlo.netgear.com/hmsweb";

        public const string DEVICE_SUPPORT_ENDPOINT = API_URL + "/devicesupport/v2";
        public const string SUBSCRIBE_ENDPOINT = API_URL + "/client/subscribe";
        public const string UNSUBSCRIBE_ENDPOINT = API_URL + "/client/unsubscribe";
        public const string BILLING_ENDPOINT = API_URL + "/users/serviceLevel/v2";
        public const string DEVICES_ENDPOINT = API_URL + "/users/devices";
        public const string FRIENDS_ENDPOINT = API_URL + "/users/friends";
        public const string LIBRARY_ENDPOINT = API_URL + "/users/library";
        public const string LOGIN_ENDPOINT = API_URL + "/login/v2";
        public const string LOGOUT_ENDPOINT = API_URL + "/logout";
        public const string NOTIFY_ENDPOINT = API_URL + "/users/devices/notify/{0}";
        public const string PROFILE_ENDPOINT = API_URL + "/users/profile";
        public const string RESET_ENDPOINT = LIBRARY_ENDPOINT + "/reset";
        public const string RESET_CAM_ENDPOINT = RESET_ENDPOINT + "/?uniqueId={0}";
        public const string STREAM_ENDPOINT = API_URL + "/users/devices/startStream";
        public const string SNAPSHOTS_ENDPOINT = API_URL + "/users/devices/fullFrameSnapshot";

        public const int PRELOAD_DAYS = 30;
    }

    public class FixedModes
    {
        public bool schedule { get; set; }
    }

    public class MessageBody
    {
        public string action { get; set; }
        public string from { get; set; }
        public Dictionary<string,string> properties { get; set; }
        public bool publishResponse { get; set; }
        public string resource { get; set; }
        public string to { get; set; }
    }

    public class ExtendedBody: MessageBody
    {
        public string transId { get; set; }
    }

}
