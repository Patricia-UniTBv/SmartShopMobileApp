using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartShopMobileApp.Helpers
{
    public interface IManageData
    {
        Task<T> GetDataAndDeserializeIt<T>(string url, string json);
        void SetStrategy(ManageDataStrategy manageDataStrategy);
    }
}
