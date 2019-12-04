using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WangJun.NC.Base;
using WangJun.NC.YunUtils;

namespace WangJun.NC.Base.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class YunUtilsController : ControllerBase
    {
        private Utils utils = new Utils();
        private readonly ILogger<YunUtilsController> _logger;

        public YunUtilsController(ILogger<YunUtilsController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public RES Get(string param)
        {
            try
            {
                var req = JRequest.Parse(param);
                if (null != req)
                {
                    var res = req.Execute(utils);
                    return res;
                }
                return RES.FAIL("未传入有效参数");
            }
            catch (Exception ex)
            {
                return RES.FAIL(ex);
            }
        }

        [HttpPost]
        public RES Post()
        {
            try
            {
                var req = JRequest.Parse(this.Request.Body);
                if (null != req)
                {
                    var res = req.Execute(utils);
                    return res;
                }
                return RES.FAIL("未传入有效参数");
            }
            catch (Exception ex)
            {
                return RES.FAIL(ex);
            }
        }
         
    }
}
