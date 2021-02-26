using Furion.DynamicApiController;
using Microsoft.AspNetCore.Mvc;

namespace FurionTemplate.Application
{
    [Route("api/[controller]")]
    public class BaseAppService : IDynamicApiController
    {

    }
}
