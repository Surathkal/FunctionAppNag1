
using System;
using System.Xml.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Xml;

namespace FunctionAppNag1

{
    public class SampleService : ICalculateService
    {
        private ICalculatorChannel proxy;


        public async Task<int> AddAsync(int intA, int intB)
        {
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

            ChannelFactory<ICalculatorChannel> factory = new ChannelFactory<ICalculatorChannel>(binding, address);

            this.proxy = factory.CreateChannel();

            return await this.proxy.AddAsync(intA, intB);
        }


        public string HelloGreeting(string s)
        {
            Console.WriteLine("Test Method Executed!");
            return s + " Welcome Aboard";
        }

    }
}