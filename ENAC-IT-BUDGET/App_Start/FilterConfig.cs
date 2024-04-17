using System.Web;
using System.Web.Mvc;

namespace ENAC_IT_BUDGET
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
