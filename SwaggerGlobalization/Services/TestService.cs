using SwaggerGlobalization.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SwaggerGlobalization.Models;

namespace SwaggerGlobalization.Services
{
    public class TestService : ServiceBase, ITestService
    {
        public TestService(IInfrastructureService infrastructure, IHttpContextAccessor httpContextAccessor)
            : base(infrastructure, httpContextAccessor)
        {

        }

        public async Task<TestResponse> Update(TestRequest request, int id)
        {
            await Task.Delay(1);

            return new TestResponse
            {
                Test = new TestDto
                {
                    Rand = Guid.NewGuid()
                },
                RequestStatus = RequestStatus.OK.ToString()
            };

        }
    }
}
