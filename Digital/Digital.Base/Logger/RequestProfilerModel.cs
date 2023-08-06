using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digital.Base.Logger
{
    public class RequestProfilerModel
    {
        public DateTimeOffset RequestTime { get; set; }
        public HttpContext Context { get; set; }
        public string Request { get; set; }
        public string Response { get; set; }
        public DateTimeOffset ResponseTime { get; set; }
    }

}
