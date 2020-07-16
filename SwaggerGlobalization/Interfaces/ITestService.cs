using SwaggerGlobalization.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwaggerGlobalization.Interfaces
{
    public interface ITestService
    {
        Task<TestResponse> Update(TestRequest request, int id);
    }
}
