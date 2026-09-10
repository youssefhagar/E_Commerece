using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerece.Application.Common.Settings
{
    public class StripSetting
    {
        public const string SectionName = "Strip";
        public string Secretkey { get; set; } = string.Empty;
        public string Publishablekey { get; set; } = string.Empty;
        public string WebHookSecretkey { get; set; } = string.Empty;
        public string Currency { get; set; } = "usd";

    }
}
