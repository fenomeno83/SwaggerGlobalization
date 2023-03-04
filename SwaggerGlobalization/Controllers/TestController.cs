using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SwaggerGlobalization.Infrastructure.Extensions;
using SwaggerGlobalization.Interfaces;
using SwaggerGlobalization.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;
using Microsoft.AspNetCore.Http;

namespace SwaggerGlobalization.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ApiControllerBase
    {
        private readonly ITestService _testService;

        public TestController(IInfrastructureService infrastructure, ITestService testService)
            : base(infrastructure)
        {
            _testService = testService;
        }


        //for swagger comments use localization resource keys if you want localize by resource file, otherwise write description directly

        /// <summary>
        /// swagger_summary_test_update
        /// </summary>
        ///<remarks>
        /// swagger_remarks_test_update
        ///</remarks>  
        /// <param name="request">swagger_param_test_update_request</param>
        /// <param name="id">swagger_param_test_update_id</param>
        /// <response code="200">swagger_response_200_test_update</response>
        /// <response code="400">swagger_response_400</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(BaseResponse))]
        [HttpPost]
        [Route("Update/{id:int}")]
        public async Task<ActionResult<TestResponse>> Update(TestRequest request, [FromRoute] int id)
        {

            if (!ModelState.IsValid)
            {
                string error = ModelState.GetValidationErrorsFormatted();

                return BadRequest(new BaseResponse
                {
                    Error = new Error()
                    {
                        ErrorCode = (int)HttpStatusCode.BadRequest,
                        ErrorMessage = error
                    },
                    RequestStatus = RequestStatus.KO.ToString()
                });
            }


            //call service
            TestResponse response = await _testService.Update(request, id);

            if (response.RequestStatus == RequestStatus.OK.ToString())
                return Ok(response);
            else
                return BadRequest(response);

        }
    }
}
