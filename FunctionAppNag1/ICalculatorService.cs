using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ServiceModel;


namespace FunctionAppNag1
{
    [ServiceContract(ConfigurationName = "FunctionAppNag1.ICalculatorService")]
    public interface ICalculatorService
    {

        [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/Add", ReplyAction = "*")]
        System.Threading.Tasks.Task<int> AddAsync(int intA, int intB);

        [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/Subtract", ReplyAction = "*")]
        System.Threading.Tasks.Task<int> SubtractAsync(int intA, int intB);

        [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/Multiply", ReplyAction = "*")]
        System.Threading.Tasks.Task<int> MultiplyAsync(int intA, int intB);

        [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/Divide", ReplyAction = "*")]
        System.Threading.Tasks.Task<int> DivideAsync(int intA, int intB);
    }

}
