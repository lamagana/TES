using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TES
{
    public partial class Test1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Cookies.PrimaryKey.Value == null || Cookies.PrimaryKey.Value == "")
            {
                Response.Redirect("Login.html", true);
            }

            string errorMessage = string.Empty;
            int pk = 0;
            int.TryParse(Cookies.PrimaryKey.Value, out pk);
            if (DatabaseAccess.SelectRoleRequest_SQL(pk, out errorMessage))
            {
                content.InnerHtml = "<h2>Submited Request</h2>";
            }

            string firstName = "";
            string lastName = "";
            DatabaseAccess.SelectUserFirstNameLastName_SQL(pk, out errorMessage, out firstName, out lastName);

            UserName.InnerText = $"Welcome {firstName} {lastName}";
        }

        public void Submit_ServerClick(object sender, EventArgs e)
        {
            if (Request.Form["role"] != null)
            {
                string selectedRole = Request.Form["role"].ToString();
                string errorMessage = string.Empty;
                Enum.TryParse<TES.Enumerations.RoleTypes>(selectedRole, out TES.Enumerations.RoleTypes roleType);
                int.TryParse(Cookies.PrimaryKey.Value, out int pk);
                if (!DatabaseAccess.InsertRoleRequest_SQL((int)roleType, pk, out errorMessage))
                {
                    return;
                }
                content.InnerHtml = "<h2>Submited Request</h2>";
            }
        }

        public void LogoutButton_Click(object sender, EventArgs e)
        {
            Cookies.PrimaryKey.Value = string.Empty;
            Response.Redirect("Login.html", true);
        }

        protected void PieButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("Pie.html", true);
        }
    }
}