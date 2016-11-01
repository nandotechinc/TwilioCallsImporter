using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TwilioCallsImporter.Data;
using TwilioCallsImporter.Services;

namespace TwilioCallsImporter
{
    public class Application
    {
        private ICallData _callData;
        private string _twilioApi;
        private string _credentials;
        private string _twilioEndpoint;

        private static HttpClient Client = new HttpClient();
        public Application(IConfiguration config, ICallData callData)
        {
            _callData = callData;
            _twilioApi = config["TwilioApi"];
            _credentials = config["Credentials"];
            _twilioEndpoint = config["TwilioApiEndpoint"];
        }

        public async Task Run()
        {
            Console.Write("Please type start time (YYYY-MM-DD): ");
            var inputStartTime = Console.ReadLine();
            Console.Write("Please type end date (YYYY-MM-DD): ");
            var inputEndTime = Console.ReadLine();

            DateTime StartDate;
            DateTime EndDate;

            DateTime.TryParse(inputStartTime, out StartDate);
            DateTime.TryParse(inputEndTime, out EndDate);

            string start = $"{StartDate.Year}-{StartDate.Month}-{StartDate.Day}";
            string end = $"{EndDate.Year}-{EndDate.Month}-{EndDate.Day}";

            Console.WriteLine("Attempting to communicate with Twilio...");

            string authUri = $"{_twilioApi}&StartTime>={start}"
                + $"&StartTime<={end}&PageSize=1000";

            Console.WriteLine(authUri);

            var credentials = Encoding.ASCII.GetBytes(_credentials);
            Client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Basic", Convert.ToBase64String(credentials));
            var initRequest = Client.GetAsync(authUri).Result;

            // Console.ReadKey();



            if (initRequest.IsSuccessStatusCode)
            {
                var response = new List<Call>();

                string json = await initRequest.Content.ReadAsStringAsync();

                Result results = JsonConvert.DeserializeObject<Result>(json);

                Console.WriteLine("First & Next Page:");
                Console.WriteLine(results.first_page_uri);
                Console.WriteLine(results.next_page_uri);
               // Console.ReadKey();

                foreach (var call in results.calls)
                {
                    call.from = call.from.Replace("sip:", "").Replace("+", "");
                    call.to = call.to.Replace("sip:", "").Replace("+", "");
                    if (call.to.Contains("@"))
                    {
                        call.to = call.to.Substring(0, call.to.IndexOf("@"));
                    }
                    if (call.from.Contains("@"))
                    {
                        call.from = call.from.Substring(0, call.from.IndexOf("@"));
                    }
                    response.Add(new Call
                    {
                        StartTime = DateTime.Parse(call.start_time),
                        EndTime = DateTime.Parse(call.end_time),
                        TrunkId = call.account_sid,
                        CallId = call.sid,
                        SourceNumber = call.from,
                        DestinationNumber = call.to,
                        Duration = int.Parse(call.duration),
                        CallCost = float.Parse(call.price.Replace("-",""))
                    });
                }

                _callData.Add(response);
                int callCount = results.calls.Count();

                while (results.next_page_uri != null)
                {

                    var nextPage = Client.GetAsync($"{_twilioEndpoint}{results.next_page_uri}").Result;
                    var convertJson = await nextPage.Content.ReadAsStringAsync();
                    var serializerSettings = new JsonSerializerSettings { ObjectCreationHandling = ObjectCreationHandling.Replace };
                    results = JsonConvert.DeserializeObject<Result>(convertJson);

                    callCount += results.calls.Count();
                    Console.WriteLine("This & Next Page:");
                    Console.WriteLine(results.page);
                    Console.WriteLine(results.next_page_uri);
                    Console.WriteLine($"Calls pulled: {callCount}");

                    var nextResponse = new List<Call>();
                    foreach (var call in results.calls)
                    {
                        call.from = call.from.Replace("sip:", "").Replace("+", "");
                        call.to = call.to.Replace("sip:", "").Replace("+", "");
                        if (call.to.Contains("@"))
                        {
                            call.to = call.to.Substring(0, call.to.IndexOf("@"));
                        }
                        if (call.from.Contains("@"))
                        {
                            call.from = call.from.Substring(0, call.from.IndexOf("@"));
                        }


                        var callToAdd = new Call()
                        {
                            StartTime = DateTime.Parse(call.start_time),
                            EndTime = DateTime.Parse(call.end_time),
                            TrunkId = call.account_sid,
                            CallId = call.sid,
                            SourceNumber = call.from,
                            DestinationNumber = call.to,
                            Duration = int.Parse(call.duration ?? "0"),
                            CallCost = Math.Abs(float.Parse(call.price ?? "0.0")) 
                        };
                        

                        nextResponse.Add(callToAdd);
                    }

                    _callData.Add(nextResponse);
                }


                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("Please check arguments & API call! Program will shut down now");
                Console.ReadKey();
            }


        }
    }
}
