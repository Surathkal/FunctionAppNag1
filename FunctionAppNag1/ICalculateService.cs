using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;


namespace FunctionAppNag1
{
    [ServiceContract]
    public interface ICalculateService
    {
        [OperationContract]
        string HelloGreeting(string s);

        [OperationContract]
        Task<int> AddAsync(int intA, int intB);

    }
}