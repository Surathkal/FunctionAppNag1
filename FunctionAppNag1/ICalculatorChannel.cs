using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ServiceModel;


namespace FunctionAppNag1
{

    /*
    public interface CalculatorSoapChannel : ServiceReference.CalculatorSoap, System.ServiceModel.IClientChannel
    {
    }
    */

    /// <summary>
    /// Calculator channel.
    /// </summary>
    public interface ICalculatorChannel : ICalculatorService, IClientChannel
    {
    }
}
