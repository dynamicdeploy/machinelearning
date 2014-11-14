using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace RegressionAzureMLWP
{
    public class AutoPredictionAzureMLClient
    {
        public static readonly string[] Make = new string[] {
        "alfa-romero", "audi", "bmw", "chevrolet", "dodge", "honda",
                               "isuzu", "jaguar", "mazda", "mercedes-benz", "mercury",
                               "mitsubishi", "nissan", "peugot", "plymouth", "porsche",
                               "renault", "saab", "subaru", "toyota", "volkswagen", "volvo"};

        public static readonly string[] BodyStyle = new string[] { "hardtop", "sedan", "wagon", "hatchback", "convertible", ""};
       
        public const double WHEEL_BASE_MIN = 86;
        public const double WHEEL_BASE_MAX = 121;
        public static readonly string[] NumberOfCylinders = new string[] { "eight", "five", "four", "six", "three", "twelve", "two" };

        public const int ENGINE_SIZE_MIN = 61;
        public const int ENGINE_SIZE_MAX = 326;

        public const int HORSEPOWER_MIN = 48;
        public const int HORSEPOWER_MAX = 288;

        public const int PEAK_RPM_MIN = 4150;
        public const int PEAK_RPM_MAX = 6600;

        public const int HIGHWAY_MPG_MIN = 16;
        public const int HIGHWAY_MPG_MAX = 54;

        public static async Task<string> InvokeRequestResponseService(string make, string bodyStyle, double wheelBase, string numOfCylinders, int engineSize, int horsepower, int peakRpm, int highwayMpg)
        {
            using (var client = new HttpClient())
            {
                ScoreData scoreData = new ScoreData()
                {
                    FeatureVector = new Dictionary<string, string>() 
                    {
                        { "make", make },
                        { "body-style", bodyStyle },
                        { "wheel-base", wheelBase.ToString() },
                        { "num-of-cylinders", numOfCylinders },
                        { "engine-size", engineSize.ToString() },
                        { "horsepower", horsepower.ToString() },
                        { "peak-rpm", peakRpm.ToString() },
                        { "highway-mpg", highwayMpg.ToString() },
                        { "price", "0" },
                    },
                    GlobalParameters = new Dictionary<string, string>()
                    {
                    }
                };

                ScoreRequest scoreRequest = new ScoreRequest()
                {
                    Id = "score00001",
                    Instance = scoreData
                };

                const string apiKey = "7EodnI2lplp2KGahJZzfwCHVE4e9NUNIwiWsOwq20QNtQgYKGENHGppRe7Vq9ruT5FwJCZ6ME0SnmiAoEAjFXg=="; // Replace this with the API key for the web service
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

                client.BaseAddress = new Uri("https://ussouthcentral.services.azureml.net/workspaces/5c51465da0e94cbcaeb05879306f7141/services/d70917b2d61245c7b819c250f6cac619/score");
                HttpResponseMessage response = await client.PostAsJsonAsync("", scoreRequest);
                if (response.IsSuccessStatusCode)
                {
                    string jsonString = await response.Content.ReadAsStringAsync();

                   string[] rs = JsonConvert.DeserializeObject<string[]>(jsonString);

                    if(rs != null && rs.Count() > 0)
                    {
                        return String.Format("{0:0.00}", (Double.Parse(rs[9])));
                    }

                    throw new Exception("Nothing returned.");

                }
                else
                {
                    throw new Exception(string.Format("Failed with status code: {0}", response.StatusCode));
                }
            }
        }
    }

    public class ScoreData
    {
        public Dictionary<string, string> FeatureVector { get; set; }
        public Dictionary<string, string> GlobalParameters { get; set; }
    }

    public class ScoreRequest
    {
        public string Id { get; set; }
        public ScoreData Instance { get; set; }
    }


    public class Response
    {
        public string[] Properties { get; set; }
    }

}





