/*
 * Author: Group 26 (Dahye Min, Saja Alhadeethi, Nikhil Balachandran)
 * Date: Feburary, 2021
 * Description: DataModeler class parses the data files
 */


using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Project1
{

    public class DataModeler
    {
        public delegate void ParseHandler(string fileName);
        private ParseHandler parseHandler;
        public Dictionary<string, CityInfo> cities = new Dictionary<string, CityInfo>();

        /// <summary>
        /// Parse XML file
        /// </summary>
        /// <param name="fileName"></param>
        public void ParseXml(string fileName)
        {
            string xmlData = File.ReadAllText("Data/" + fileName);
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xmlData);

            foreach (XmlNode node in xmlDocument.SelectNodes("/CanadaCities/*"))
            {
                CityInfo cityInfo = new CityInfo();
                foreach (XmlNode childNode in node.ChildNodes)
                {
                    switch (childNode.Name)
                    {
                        case "city":
                            cityInfo.CityName = childNode.InnerText;
                            break;
                        case "city_ascii":
                            cityInfo.CityAscii = childNode.InnerText;
                            break;
                        case "lat":
                            cityInfo.Latitude = double.Parse(childNode.InnerText);
                            break;
                        case "lng":
                            cityInfo.Logitude = double.Parse(childNode.InnerText);
                            break;
                        case "admin_name":
                            cityInfo.Province = childNode.InnerText;
                            break;
                        case "population":
                            cityInfo.Population = long.Parse(childNode.InnerText);
                            break;
                        case "capital":
                            if (childNode.InnerText == "admin")
                                cityInfo.Capital = true;
                            else
                                cityInfo.Capital = false;
                            break;
                    }
                }
                cities[cityInfo.CityName] = cityInfo;
            }
        }

        /// <summary>
        /// parse JSON file
        /// </summary>
        /// <param name="fileName"></param>
        public void ParseJson(string fileName)
        {
            string json = File.ReadAllText("Data/" + fileName);
            List<CityInfoWrapper> cityArr = JsonConvert.DeserializeObject<List<CityInfoWrapper>>(json);
            foreach(var item in cityArr)
            {
                CityInfo cityInfo = new CityInfo();

                if (!string.IsNullOrEmpty(item.city))
                {
                    //if city name already in dictionary then add the province to the key to allow city name duplicate
                    if (cities.ContainsKey(item.city))
                        cityInfo.CityName = $"{item.city } {item.admin_name}";
                    else
                        cityInfo.CityName = item.city;
                    {
                        cityInfo.CityAscii = item.city_ascii;
                        cityInfo.Logitude = double.Parse(item.lng);
                        cityInfo.Latitude = double.Parse(item.lat);
                        cityInfo.Province = item.admin_name;
                        cityInfo.Population = int.Parse(item.population);
                        cities[cityInfo.CityName] = cityInfo;
                    }
                }
           
            }
            System.Console.WriteLine(cityArr.Count);
            System.Console.WriteLine("dic "+ cities.Count);

        }

        /// <summary>
        /// parse CSV file
        /// </summary>
        /// <param name="fileName"></param>
        public void ParseCsv(string fileName)
        {
            StreamReader reader = new StreamReader("Data/" + fileName);
            reader.ReadLine();

            while (reader.Peek() != -1)
            {
                var line = reader.ReadLine();
                var values = line.Split(',');
                foreach (var item in values)
                {
                    CityInfo cityInfo = new CityInfo();
                    cityInfo.CityName = values[0];
                    cityInfo.CityAscii = values[1];
                    cityInfo.Logitude = double.Parse(values[2]);
                    cityInfo.Latitude = double.Parse(values[3]);
                    cityInfo.Province = values[5];
                    cityInfo.Population = long.Parse(values[7]);
                    cities[cityInfo.CityName] = cityInfo;
                }
            }
        }

        /// <summary>
        /// parse file using delegate
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="fileType"></param>
        /// <returns> Dictionary including string as key and cityInfo as value </returns>
        public Dictionary<string, CityInfo> ParseFile(string fileName, string fileType)
        {
            if (fileType == "csv")
                parseHandler = new ParseHandler(ParseCsv);
            else if (fileType == "json")
                parseHandler = new ParseHandler(ParseJson);
            else if (fileType == "xml")
                parseHandler = new ParseHandler(ParseXml);

            parseHandler(fileName);

            return cities;
        }
    }
}
