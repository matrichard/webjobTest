using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace ImportData.Controllers
{
    public class FileController : ApiController
    {
        public HttpResponseMessage Post()
        {
            HttpResponseMessage result = null;
            var httpRequest = HttpContext.Current.Request;

            // Check if files are available
            if (httpRequest.Files.Count > 0)
            {
                var files = new List<object>();

                // interate the files and save on the server
                foreach (string file in httpRequest.Files)
                {
                    var postedFile = httpRequest.Files[file];
                    //var filePath = HttpContext.Current.Server.MapPath("~/" + postedFile.FileName);
                    //postedFile.SaveAs(filePath);

                    files.Add(new{file, postedFile.FileName});

                    QueueStorage.AddMessage(new { FileName = postedFile.FileName , Tenant = "sfmm"});
                }

                // return result
                result = Request.CreateResponse(HttpStatusCode.Created, files);
            }
            else
            {
                // return BadRequest (no file(s) available)
                result = Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            return result;
        }
    }
}