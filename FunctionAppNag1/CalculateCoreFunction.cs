using System;
using System.IO;
using System.Threading.Tasks;
using System.ServiceModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using Microsoft.Azure.WebJobs.Host;
using ServiceReference1;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace FunctionAppNag1
{
    public static class CalculateCoreFunction
    {
        [FunctionName("CalculateCoreFunction")]
        //[Consumes("application/xml")]
        //[Produces("application/xml")]

        public static async Task<IActionResult> Run(
        //public static async Task<int> Run(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req, ILogger log)
        {


            /*
             * 
            // original sample code 
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            string responseMessage = string.IsNullOrEmpty(name)
            ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
            : $"Hello, {name}. This HTTP triggered function executed successfully.";

            return new OkObjectResult(responseMessage);
            */

            // grab the two fields from input



            int firstNum = 0;
            int secondNum = 0;

            //req.Headers.


            //System.Collections.Generic.IEnumerable<string> headerValues;

            /*
            IEnumerable<string> headerValues = req.Headers.Values();
                .GetValues("MyCustomID");
            var id = headerValues.FirstOrDefault();


            var userId = string.Empty;

            if (req.Headers.TryGetValues("SOAPAction", out headerValues))
            {
                userId = headerValues.FirstOrDefault();
            }
            */



            //NameValueCollection coll;

            // Load Header collection into NameValueCollection object.
            string action= req.Headers["SOAPAction"];


            // Put the names of all keys into a string array.
            /*
            String[] arr1 = coll.AllKeys;
            for (loop1 = 0; loop1 < arr1.Length; loop1++)
            {
                Response.Write("Key: " + arr1[loop1] + "<br>");
                // Get all values under this key.
                String[] arr2 = coll.GetValues(arr1[loop1]);
                for (loop2 = 0; loop2 < arr2.Length; loop2++)
                {
                    Response.Write("Value " + loop2 + ": " + Server.HtmlEncode(arr2[loop2]) + "<br>");
                }
            }

            */



            // Use StreamReader to convert any encoding to UTF-16 (default C# and sql Server).

            using (var streamReader = new StreamReader(req.Body))
            {
                var xmlDoc = XDocument.Load(streamReader, LoadOptions.None);
                //LoadAsync(streamReader, LoadOptions.None, CancellationToken.None);
                var model = (Calculate)new XmlSerializer(typeof(Calculate)).Deserialize(xmlDoc.CreateReader());

                firstNum = model.intA;
                secondNum = model.intB;

            }



            log.LogInformation("C# HTTP trigger function processed invoking SOAP method.");


            /*
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            XmlReader inputReader = XmlReader.Create(requestBody);
            */

            /*
            inputReader.ReadStartElement("intA");
            firstNum = int.Parse(inputReader.Value);
            inputReader.Read();

            inputReader.ReadStartElement("intB");
            secondNum = int.Parse(inputReader.Value);
            inputReader.Read();

            */

            BasicHttpBinding binding = new BasicHttpBinding
            {
                SendTimeout = TimeSpan.FromSeconds(10000),
                MaxBufferSize = int.MaxValue,
                MaxReceivedMessageSize = int.MaxValue,
                AllowCookies = true,
                ReaderQuotas = XmlDictionaryReaderQuotas.Max
            };

            binding.Security.Mode = BasicHttpSecurityMode.None;

            EndpointAddress address = new EndpointAddress("http://www.dneonline.com/calculator.asmx");



            // Instantiates the binding object.
            // Depending on the security definition, the security mode should be adjusted.
            //var binding = new BasicHttpBinding(BasicHttpSecurityMode.None);

            // If necessary, appropriate client credential type should be set.
            //binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;

            // Instantiates the endpoint address.
            //var endpoint = new EndpointAddress(Config.WcfServiceEndpoint);

            using (var client = new ServiceReference1.CalculatorSoapClient(binding, address))
            {
                // If necessary, username and password should be provided.
                //client.ClientCredentials.UserName.UserName = "username";
                //client.ClientCredentials.UserName.Password = "password";

                int result = 0;
                if(action.Equals("Add"))
                    result = await client.AddAsync(firstNum, secondNum).ConfigureAwait(false);

                if (action.Equals("Divide"))
                    result = await client.DivideAsync(firstNum, secondNum).ConfigureAwait(false);

                if (action.Equals("Multiply"))
                    result = await client.MultiplyAsync(firstNum, secondNum).ConfigureAwait(false);

                if (action.Equals("Subtract"))
                    result = await client.SubtractAsync(firstNum, secondNum).ConfigureAwait(false);


                log.LogInformation($"{result} received.");


                return (ActionResult) new OkObjectResult(result);


                //return HttpResponseMessage(result);
                //return result;

                //return req.CreateResponse(HttpStatusCode.OK, result);
            }
        }
    }
}