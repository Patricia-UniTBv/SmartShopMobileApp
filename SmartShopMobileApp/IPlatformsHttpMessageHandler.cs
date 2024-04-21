using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartShopMobileApp
{
    public interface IPlatformHttpMessageHandler
    {
        HttpMessageHandler GetHttpMessageHandler();
    }
}
