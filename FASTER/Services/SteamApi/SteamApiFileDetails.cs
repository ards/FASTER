namespace FASTER.Services.SteamApi
{
    public class SteamApiFileDetails
    {
        public uint result { get; set; }
        public ulong publishedfileid { get; set; }
        public ulong creator { get; set; }
        public uint creator_appid { get; set; }
        public uint consumer_appid { get; set; }
        public string filename { get; set; }
        public ulong file_size { get; set; }
        public string title { get; set; }
        public string file_description { get; set; }
        public ulong time_created { get; set; }
        public ulong time_updated { get; set; }
        
    }
}
