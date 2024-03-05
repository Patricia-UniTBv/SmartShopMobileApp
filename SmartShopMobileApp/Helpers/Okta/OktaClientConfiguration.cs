using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IBrowser = IdentityModel.OidcClient.Browser.IBrowser;

namespace SmartShopMobileApp.Helpers.Okta
{
    public class OktaClientConfiguration
    {
        public string ClientId { get; set; }

        public string RedirectUri { get; set; }

        public string PostLogoutRedirectUri { get; set; }

        public IList<string> Scope { get; set; } = new string[] { "openid", "profile" };

        public string OktaDomain { get; set; }

        public IBrowser Browser { get; set; }

    }
}
