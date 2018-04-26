using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TES
{
    public partial class AdminHomepage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string errorMessage = string.Empty;
            int pk = 0;
            int.TryParse(Cookies.PrimaryKey.Value, out pk);

            string firstName = "";
            string lastName = "";
            DatabaseAccess.SelectUserFirstNameLastName_SQL(pk, out errorMessage, out firstName, out lastName);

            UserName.InnerText = $"Welcome {firstName} {lastName}";

            DataTable table = DatabaseAccess.SelectUncompletedRoleRequests_SQL();

            this.LoadTable(table);
        }

        public void LogoutButton_Click(object sender, EventArgs e)
        {
            Cookies.PrimaryKey.Value = string.Empty;
            Response.Redirect("Login.html", true);
        }

        private void LoadTable(DataTable table)
        {
            foreach (DataRow r in table.Rows)
            {
                TableRow Row = new TableRow();
                TableCell FirstName = new TableCell();
                TableCell LastName = new TableCell();
                TableCell Email = new TableCell();
                TableCell RequestedRole = new TableCell();
                TableCell Accept = new TableCell();
                TableCell Reject = new TableCell();

                FirstName.Controls.Add(new LiteralControl(r["FirstName"].ToString()));
                LastName.Controls.Add(new LiteralControl(r["LastName"].ToString()));
                Email.Controls.Add(new LiteralControl(r["Email"].ToString()));
                RequestedRole.Controls.Add(new LiteralControl(r["RequestedRoleId"].ToString()));

                Button acceptBtn = new Button();
                acceptBtn.ID = r["RoleRequestId"].ToString() + "/" + r["RequestedRoleId"];
                acceptBtn.Text = "Approve";
                acceptBtn.Click += BtnClick;
                Accept.Controls.Add(acceptBtn);


                Button rejectBtn = new Button();
                rejectBtn.ID = r["RoleRequestId"].ToString();

                rejectBtn.Text = "Reject";
                rejectBtn.Click += BtnClick;
                Reject.Controls.Add(rejectBtn);

                Row.Cells.Add(FirstName);
                Row.Cells.Add(LastName);
                Row.Cells.Add(Email);
                Row.Cells.Add(RequestedRole);
                Row.Cells.Add(Accept);
                Row.Cells.Add(Reject);

                Table1.Rows.Add(Row);
            }
        }

        public void BtnClick(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            string[] rrid = btn.ID.Split('/');
            if (btn.Text == "Approve")
            {
                DatabaseAccess.AcceptRoleRequest_SQL(Convert.ToInt32(rrid[0]), Convert.ToInt32(rrid[1]));
            }
            else if (btn.Text == "Reject")
            {
                DatabaseAccess.DeclineRoleRequest_SQL(Convert.ToInt32(rrid[0]));
            }
            Response.Redirect(Request.RawUrl);
        }


    }
}