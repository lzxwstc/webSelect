using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JIYITECH.WebApi.Configs
{
    public class AppConfig
    {
        public string SQLConnectionStrings { get; set; }
        public string tokenSecret { get; set; }
        /// <summary>
        /// jsencrypt 公钥 🔒
        /// </summary>
        public string PublicKey { get; set; }

        /// <summary>
        /// jsencrypt 私钥 🔑
        /// </summary>
        public string PrivateKey { get; set; }
    }



}
