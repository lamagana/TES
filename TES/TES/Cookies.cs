using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TES
{
    public static class Cookies
    {
        public static HttpCookie PrimaryKey = new HttpCookie("primaryKey");

        public static HttpCookie ProjectId = new HttpCookie("projectId");
    }
}