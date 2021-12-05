using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Car_Renting.Controllers.Tests
{
    /// <summary>
    /// <para>Generates Action URLs in the format: controller/action#urlfragment?key1=value1&amp;key2=value2</para>
    /// <para>All other methods are not supported.</para>
    /// </summary>
    public class MockUrlHelper : IUrlHelper
    {
        public ActionContext ActionContext => throw new NotSupportedException();
       
        public string Action(UrlActionContext uac)
        {
            return $"{uac.Controller}/{uac.Action}#{uac.Fragment}?" + string.Join("&", new RouteValueDictionary(uac.Values).Select(p => p.Key + "=" + p.Value));
        }

        public string Content(string contentPath)
        {
            throw new NotSupportedException();
        }

        public bool IsLocalUrl(string url)
        {
            throw new NotSupportedException();
        }

        public string Link(string routeName, object values)
        {
            throw new NotSupportedException();
        }

        public string RouteUrl(UrlRouteContext routeContext)
        {
            throw new NotSupportedException();
        }
    }
}
