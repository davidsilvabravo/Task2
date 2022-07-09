using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using WebServiceAutomation.Model;
using Newtonsoft.Json;
using Task2.Model.JsonModel;
using System.Text;
using Task2.Utils;


namespace Task2.TestCases
{
    [TestClass]
    public class TestingEndpoints
    {
        private string baseUrl = "http://api.countrylayer.com/v2/alpha/";
        private string APIKEY = "fc5401724f7cc8bc52cece0e035b0ecd"; // también: bac541709efb09409e462461af95b5f9

        public void GetCountryInfo(string alpha2Code, int expectedStatus, string expectedCountryName)
        {
            // To create the Http Client
            using (HttpClient httpClient = new HttpClient())
            {
                using (HttpRequestMessage httpRequestMessage = new HttpRequestMessage())
                {
                    string getUrl = baseUrl + alpha2Code + "?access_key=" + APIKEY;
                    httpRequestMessage.RequestUri = new Uri(getUrl);
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    // Create request & execute it
                    Task<HttpResponseMessage> httpResponse = httpClient.SendAsync(httpRequestMessage); // Previous lines where written for preparing parameter for SendAsync

                    // Receive the response
                    using (HttpResponseMessage httpResponseMessage = httpResponse.Result)
                    {
                        // Extract the required information from GET response
                        // Status code
                        HttpStatusCode httpStatusCode = httpResponseMessage.StatusCode;

                        // Response body (data)
                        HttpContent httpContent = httpResponseMessage.Content;
                        Task<string> responseData = httpContent.ReadAsStringAsync();
                        string responseBody = responseData.Result;
                        Console.WriteLine("ResponseBody (data): \n" + responseBody + "\n");

                        //Deserialization of array of Json
                        RestResponse restResponse = new RestResponse((int)httpStatusCode, responseData.Result);
                        JsonResponse jsonResponse = JsonConvert.DeserializeObject<JsonResponse>(restResponse.ResponseData);

                        // Assertions
                        // Validation of status code
                        Assert.AreEqual(expectedStatus, restResponse.StatusCode);
                        Console.WriteLine("Validation of StatusCode OK");

                        if (restResponse.StatusCode == 200)
                        {

                            // Validation of alpha2Code
                            Assert.AreEqual(alpha2Code, jsonResponse.altSpellings[0]);
                            Console.WriteLine("Validation of AlphaCode2 OK");

                            // Validation of Country name
                            Assert.AreEqual(expectedCountryName, jsonResponse.name);
                            Console.WriteLine("Validation of Country name OK");
                        }
                    }
                }
            }
        }


        [TestMethod]
        public void GetCountryInfo_US()
        {
            GetCountryInfo("US", 200, "United States of America");
        }


        [TestMethod]
        public void GetCountryInfo_DE()
        {
            GetCountryInfo("DE", 200, "Germany");
        }


        [TestMethod]
        public void GetCountryInfo_GB()
        {
            GetCountryInfo("GB", 200, "United Kingdom of Great Britain and Northern Ireland");
        }


        [TestMethod]
        public void GetCountryInfo_XX()
        {
            GetCountryInfo("XX", 404, "");
        }


                              
        [TestMethod]
        public void PostNewCountry()
        {
            string alpha2Code = DataGenerator.RandomString(2);
            string alpha3Code = alpha2Code + "X";
            string newCountryName = "Test Country " + alpha3Code;
            string jsonRequest = "{" +
                                      "\"name\": \"" + newCountryName + "\"," +
                                      "\"alpha2_code\": \"" + alpha2Code + "\"," +
                                      "\"alpha3_code\": \"" + alpha3Code + "\"" +
                                 "}";
            // Console.WriteLine("jsonRequest: " + jsonRequest);

            // Post new Country           
            using (HttpClient httpClient = new HttpClient())
            {
                // Create a request & execute it
                string postUrl = baseUrl + alpha2Code + "?access_key=" + APIKEY;
                HttpContent requestContent = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
                Task<HttpResponseMessage> postResponse = httpClient.PostAsync(postUrl, requestContent);

                // Receive the response
                HttpResponseMessage httpResponseMessage = postResponse.Result;

                // Assertion for POST request
                Assert.AreEqual(201, ((int)httpResponseMessage.StatusCode));

                // Validate new Country using GET request
                GetCountryInfo("alpha2Code", 200, newCountryName);
            }
        }
    }
}
