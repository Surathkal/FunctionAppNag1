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
            
            int firstNum = 0;
            int secondNum = 0;

            // extract SOAP Action from HTTP Header
            string action= req.Headers["SOAPAction"];

            using (var streamReader = new StreamReader(req.Body))
            {
                var xmlDoc = XDocument.Load(streamReader, LoadOptions.None);
                //LoadAsync(streamReader, LoadOptions.None, CancellationToken.None);
                var model = (Calculate)new XmlSerializer(typeof(Calculate)).Deserialize(xmlDoc.CreateReader());

                firstNum = model.intA;
                secondNum = model.intB;
            }

            log.LogInformation("C# HTTP trigger function processed invoking SOAP method.");

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

            using (var client = new ServiceReference1.CalculatorSoapClient(binding, address))
            {

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

            }
        }
    }
}
