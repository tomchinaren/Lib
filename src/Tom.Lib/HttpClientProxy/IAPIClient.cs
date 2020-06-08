using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tom.Lib.HttpClientProxy
{
    interface IAPIClient
    {
        T Execute<T>(IRequest<T> request) where T : Response;
    }
}
