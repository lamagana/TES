using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TES
{
    public partial class ProjectHomepage : System.Web.UI.Page
    {
        Project project;
        private int ProjectId;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Cookies.PrimaryKey.Value == null || Cookies.PrimaryKey.Value == "")
            {
                Response.Redirect("Login.html", true);
            }

            if (Cookies.ProjectId.Value == null || Cookies.ProjectId.Value == "")
            {
                Response.Redirect("Login.html", true);
            }

            int.TryParse(Cookies.ProjectId.Value, out ProjectId);

            LoadProject();
        }

        private void LoadProject()
        {
            string errorMessage = "";
            project = null;
            bool result = DatabaseAccess.SelectProjectById_SQL(ProjectId, out errorMessage, out project);
            if (result)
            {
                ProjectLabel.InnerText = project.ProjectId.ToString() + " - " + project.Description;
            }
        }

        public void LogoutButton_Click(object sender, EventArgs e)
        {
            Cookies.PrimaryKey.Value = string.Empty;
            Response.Redirect("Login.html", true);
        }
    }
}