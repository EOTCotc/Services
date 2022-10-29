namespace DID.Models.Response
{
    public class ComAddrRespon
    {
        public Dictionary<string, string> country_list
        {
            get; set;
        }
        public Dictionary<string, string> province_list
        {
            get; set;
        }
        public Dictionary<string, string> city_list
        {
            get; set;
        }
        public Dictionary<string, string> county_list
        {
            get; set;
        }
    }

    public class Area
    {
        public string code
        {
            get; set;
        }

        public string name
        {
            get; set;
        }
    }
}
