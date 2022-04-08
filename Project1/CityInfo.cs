/*
 * Author: Group 26 (Dahye Min, Saja Alhadeethi, Nikhi Balachandran)
 * Date: Feburary, 2021
 * Description: CityInfo class holds information about the city
 */

namespace Project1
{
    public class CityInfo
    {
        public int CityId { get; set; }
        public string CityName { get; set; }
        public string CityAscii { get; set; }
        public long Population { get; set; }
        public string Province { get; set; }
        public double Latitude { get; set; }
        public double Logitude { get; set; }
        public bool Capital { get; set; }

        public string GetProvince()
        {
            return Province;
        }

        public long GetPopulation()
        {
            return Population;
        }

        public string GetLocation()
        {
            return Latitude + ", " + Logitude;
        }


    }
}
public class CityInfoWrapper
{
    public string city { get; set; }
    public string city_ascii { get; set; }
    public string lat { get; set; }
    public string lng { get; set; }
    public string admin_name { get; set; }
    public string population { get; set; }
    public string id { get; set; }
}