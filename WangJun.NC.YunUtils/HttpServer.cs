using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WangJun.NC.YunUtils
{
    public class HttpServer
    {
        protected HttpListener server = new HttpListener();
        protected long count = 0;
        public static HttpServer GetInst(string prefixUrl)
        {
            var inst = new HttpServer();
            inst.server.Prefixes.Add(prefixUrl);
            return inst;
        }

        public  RES  Run(Func<RES, RES> callback)
        {
            try
            {
                if (!this.server.IsListening)
                {
                    this.server.Start();
                }

                while (this.server.IsListening)
                {
                    
                    Console.WriteLine($"等待处理\t{DateTime.Now}");
                    var context = this.server.GetContext();

                    var req = context.Request;
                    if (null != callback)
                    {

                        var reqMethod = context.Request.HttpMethod.ToUpper();
                        var reqOrigin = context.Request.Headers["Origin"];
                        var corHeader = context.Request.Headers["Access-Control-Request-Headers"];
                        var rawUrl = context.Request.RawUrl;
                        var reqLength = (int)req.ContentLength64;


                        context.Response.StatusCode = 200;
                        context.Response.AddHeader("Access-Control-Allow-Origin", reqOrigin);
                        context.Response.AddHeader("Access-Control-Allow-Methods", "GET,POST,OPTIONS,PUT,DELETE");
                        context.Response.AddHeader("Access-Control-Allow-Headers", $"x-requested-with,{corHeader}");
                        context.Response.AddHeader("Content-Type", "application/json");
                        try
                        {
                            if ("OPTIONS" == reqMethod)
                            {
                                var buffer = Encoding.UTF8.GetBytes(JSON.ToJson(RES.OK($"{reqMethod}{DateTime.Now}")));
                                context.Response.OutputStream.Write(buffer, 0, buffer.Length);
                            }
                            else
                            {
                                var buffer = new byte[(int)reqLength];
                                req.InputStream.Read(buffer, 0, buffer.Length);
                                var procRes = callback(RES.OK(buffer));

                                buffer = procRes.DATA as byte[];

                                context.Response.OutputStream.Write(buffer, 0, buffer.Length);
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                        finally
                        {
                            context.Response.Close();
                        }
                        

                        Console.WriteLine($"[已处理][{reqMethod}]\t{++this.count}\t{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff")}\t{rawUrl}");
                    }
                }

                return RES.OK();
            }
            catch (Exception ex)
            { 
                return RES.FAIL(ex);
            } 
        }

        public RES Pause()
        {
            try
            {
                return RES.OK();
            }
            catch (Exception ex)
            {
                return RES.FAIL();
            }
        }

        public RES Stop()
        {
            try
            {
                return RES.OK();
            }
            catch (Exception ex)
            {
                return RES.FAIL();
            }
        }
    }
}
