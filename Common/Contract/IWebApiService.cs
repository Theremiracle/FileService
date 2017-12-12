using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Contract
{
    public interface IWebApiService
    {
        string WebApiBaseAddress { get; }
    }
}
