using System.Net;
using System.Web.Http;
using Microsoft.Azure.Mobile.Server.Config;

namespace democsbackendService.Controllers
{
    [MobileAppController]
    public class AdditionController : ApiController
    {
        // GET api/Addition
        public ResultViewModel Get(int? first, int? second)
        {
            if (first == null || second == null)
            {
                throw new 
                    HttpResponseException(HttpStatusCode.BadRequest);
            }
            ResultViewModel result =
                new ResultViewModel
                {
                    First = first.GetValueOrDefault(),
                    Second = second.GetValueOrDefault()
                };
            result.Result = result.First + result.Second;
            return result;
        }
    }

    public class ResultViewModel
    {
        public int First { get; set; }
        public int Second { get; set; }
        public int Result { get; set; }
    }    
}
