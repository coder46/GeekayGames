﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
namespace GeekayApp
{
    class DataContract1
    {
        [DataContract]
        public class Response
        {
            [DataMember(Name = "copyright")]
            public string Copyright { get; set; }
            [DataMember(Name = "brandLogoUri")]
            public string BrandLogoUri { get; set; }
            [DataMember(Name = "statusCode")]
            public int StatusCode { get; set; }
            [DataMember(Name = "statusDescription")]
            public string StatusDescription { get; set; }
            [DataMember(Name = "authenticationResultCode")]
            public string AuthenticationResultCode { get; set; }
            [DataMember(Name = "errorDetails")]
            public string[] errorDetails { get; set; }
            [DataMember(Name = "traceId")]
            public string TraceId { get; set; }
        }

    }
}
