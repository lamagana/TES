using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace TES
{
    public class TesSession : IRequiresSessionState
    {
        public TesSession()
        {
            HttpContext.Current = new HttpContext(new HttpRequest("", "http://localhost", ""), new HttpResponse(new System.IO.StringWriter()));

            System.Web.SessionState.SessionStateUtility.AddHttpSessionStateToContext(HttpContext.Current, new HttpSessionStateContainer("", new SessionStateItemCollection(), new HttpStaticObjectsCollection(), 20000, true, HttpCookieMode.UseCookies, SessionStateMode.Off, false));
        }

        public void SetSessionValue(string key, string value)
        {
            HttpContext.Current.Session[key] = value;
        }

        public string GetSessionValue(string key)
        {
            if (HttpContext.Current.Session[key] != null)
            {
                return (string)HttpContext.Current.Session[key];
            }

            return null;
        }
    }
}