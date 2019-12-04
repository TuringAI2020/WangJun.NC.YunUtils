using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WangJun.NC.BaseWebAPI;
using WangJun.NC.YunUtils;

namespace WangJun.NC.BaseWebAPI
{
    [ApiController]
    [Route("[controller]")]
    public class BaseWebAPIController : ControllerBase
    { 

        private readonly ILogger<BaseWebAPIController> _logger;

        public BaseWebAPIController(ILogger<BaseWebAPIController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public virtual RES Get(string param)
        {
            try
            {
                var req = JRequest.Parse(param);
                if (null != req)
                {
                    var res = req.Execute(req);
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
        public virtual RES Post()
        {
            try
            {
                var req = JRequest.Parse(this.Request.Body);
                if (null != req)
                {
                    var res = req.Execute(req);
                    return res;
                }
                return RES.FAIL("未传入有效参数");
            }
            catch (Exception ex)
            {
                return RES.FAIL(ex);
            }
        }

        [HttpPut]
        public virtual RES Put()
        {
            try
            {
                var req = JRequest.Parse(this.Request.Body);
                if (null != req)
                {
                    var res = req.Execute(req);
                    return res;
                }
                return RES.FAIL("未传入有效参数");
            }
            catch (Exception ex)
            {
                return RES.FAIL(ex);
            }
        }

        [HttpDelete]
        public virtual RES Delete()
        {
            try
            {
                var req = JRequest.Parse(this.Request.Body);
                if (null != req)
                {
                    var res = req.Execute(req);
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
