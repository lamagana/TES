using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TES
{
    public partial class StudentHomepage : System.Web.UI.Page
    {
        // Class level variables to hold the UserId and ProjectId so that they are accessible
        // throughout this class for TimeSheet table queries. They are currently hard coded, but
        // their dynamic assignments are commented out in lines below.
        int userId;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Cookies.PrimaryKey.Value == null || Cookies.PrimaryKey.Value == "")
            {
                Response.Redirect("Login.html", true);
            }

            string errorMessage = string.Empty;
            int.TryParse(Cookies.PrimaryKey.Value, out userId);

            DatabaseAccess.SelectUserFirstNameLastName_SQL(userId, out errorMessage, out string firstName, out string lastName);

            UserName.InnerText = $"Welcome {firstName} {lastName}";

            LoadUsersProjects(userId);
            // This is 
            //BindDataSource(userId, projectId);
        }

        public void LogoutButton_Click(object sender, EventArgs e)
        {
            Cookies.PrimaryKey.Value = string.Empty;
            Response.Redirect("Login.html", true);
        }

        private void LoadUsersProjects(int userId)
        {
            bool result = DatabaseAccess.SelectUsersProjects_SQL(userId, out string errorMessage, out List<Project> projectList);

            if (result)
            {

                foreach (Project project in projectList)
                {
                    //HyperLinks
                    TableRow hyperlinkRow = new TableRow();
                    TableCell hyperlinkCell = new TableCell();

                    Button link = new Button();
                    link.ID = project.ProjectId.ToString();
                    link.Text = project.ProjectId.ToString() + " - " + project.Description;
                    link.Click += Hyperlink_Click;

                    hyperlinkCell.Controls.Add(link);
                    hyperlinkRow.Cells.Add(hyperlinkCell);
                    ProjectHyperlinks.Rows.Add(hyperlinkRow);

                    if (ProjectListDropDown.Items.Count <= projectList.Count)
                    {
                        ListItem item = new ListItem();
                        item.Text = project.ProjectId + " - " + project.Description;
                        item.Value = project.ProjectId.ToString();
                        ProjectListDropDown.Items.Add(item);
                    }
                }

            }
        }

        private void Hyperlink_Click(object sender, EventArgs e)
        {
            var button = sender as Button;
            Cookies.ProjectId.Value = button.ID;
            Response.Redirect("ProjectHomepage.aspx");
        }

        /// <summary>
        /// When the user clicks the punch in button, the current time time stamp will be entered into the db and update
        /// the grid view.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void PunchIn_Click(object sender, EventArgs e)
        {
            if (SelectedProject() > 0)
            {
                TimeAlert.InnerText = "";

                // Get current date time.
                DateTime startTime = DateTime.Now;

                bool isBlank = DatabaseAccess.CheckIfTimesAreBlank(userId, SelectedProject(), out string errorMessage);

                if (!isBlank)
                {
                    bool isClockedIn = DatabaseAccess.CheckIfAlreadyClockedIn(userId, SelectedProject(), out string failMessage);

                    if (isClockedIn)
                    {


                        // Send the 
                        bool result = DatabaseAccess.InsertTime_SQL(userId, SelectedProject(), startTime, out string wrongMessage);

                        if (result)
                        {
                            GridView1.DataBind();
                        }
                        else
                        {
                            TimeAlert.InnerText = wrongMessage;
                        }
                    }
                    else
                    {
                        TimeAlert.InnerText = failMessage;
                    }
                }
                else
                {
                    bool result = DatabaseAccess.InsertTimeForBlankRow(userId, SelectedProject(), startTime, out string problemMessage);
                    if (result)
                    {
                        GridView1.DataBind();
                    }
                    else
                    {
                        TimeAlert.InnerText = problemMessage;
                    }
                }
            }
            BindDataSource(userId, SelectedProject());
        }

        public void PunchOut_Click(object sender, EventArgs e)
        {
            if (SelectedProject() > 0)
            {

                TimeAlert.InnerText = "";

                bool isClockedOut = DatabaseAccess.CheckIfAlreadyClockedOut(userId, SelectedProject(), out string errorMessage);

                if (isClockedOut)
                {
                    DateTime stopTime = DateTime.Now;

                    bool result = DatabaseAccess.UpdateStopTime_SQL(userId, SelectedProject(), stopTime, out string wrongMessage);

                    if (result)
                    {
                        GridView1.DataBind();
                    }
                    else
                    {
                        TimeAlert.InnerText = wrongMessage;
                    }
                }
                else
                {
                    TimeAlert.InnerText = errorMessage;
                }
            }
            BindDataSource(userId, SelectedProject());
        }

        /// <summary>
        /// When the user clicks add row, a new blank row will be inserted into the db and displayed
        /// in the GridView.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAddRow_Click(object sender, EventArgs e)
        {
            if (SelectedProject() > 0)
            {
                // Send the UserId and P
                bool result = DatabaseAccess.InsertNewRow_SQL(userId, SelectedProject(), out string errorMessage);

                if (result)
                {
                    GridView1.DataBind();
                }
            }
            BindDataSource(userId, SelectedProject());
        }

        protected void txtStartTime_TextChanged(object sender, EventArgs e)
        {
            BindDataSource(userId, SelectedProject());
            // Get the clicked in text box
            TextBox selectedBox = sender as TextBox;

            // Get the row the text box is in
            GridViewRow row = selectedBox.NamingContainer as GridViewRow;

            // Set the column for the TimeSheetId as visible to get its data.
            GridView1.Columns[0].Visible = true;

            // Re-bind the data so that the TimeSheetId shows up in the newly visible column.
            GridView1.DataBind();

            // Get the index of the row.
            int index = row.RowIndex;

            // Based on the index of the row, get the cell that the TimeSheetId would be in for that row.
            int timeId = int.Parse(GridView1.Rows[index].Cells[0].Text);

            // Get the exact box the time was changed in.
            TextBox txtStart = row.FindControl("StartTime") as TextBox;

            // Get the time string from the text box.
            string startTime = txtStart.Text;

            // Cast the string as a DateTime 
            DateTime newStart = Convert.ToDateTime(startTime);

            // Send the updated time to the db for an update.
            bool result = DatabaseAccess.ManualAddStartTime_SQL(timeId, newStart, out string errorMessage);

            // Make the column for TimeSheetId invisible again.
            GridView1.Columns[0].Visible = false;

            if (result)
            {
                GridView1.DataBind();
            }
        }

        protected void txtStopTime_TextChanged(object sender, EventArgs e)
        {
            BindDataSource(userId, SelectedProject());
            // Get the clicked in text box
            TextBox selectedBox = sender as TextBox;

            // Get the row the text box is in
            GridViewRow row = selectedBox.NamingContainer as GridViewRow;

            // Set the column for the TimeSheetId as visible to get its data.
            GridView1.Columns[0].Visible = true;

            // Re-bind the data so that the TimeSheetId shows up in the newly visible column.
            GridView1.DataBind();

            // Get the index of the row.
            int index = row.RowIndex;

            // Based on the index of the row, get the cell that the TimeSheetId would be in for that row.
            int timeId = int.Parse(GridView1.Rows[index].Cells[0].Text);

            // Get the exact box the time was changed in.
            TextBox txtStop = row.FindControl("StopTime") as TextBox;

            // Get the time string from the text box.
            string stopTime = txtStop.Text;

            // Cast the string as a DateTime            
            DateTime newStop = Convert.ToDateTime(stopTime);

            // Send the updated time to the db for an update.
            bool result = DatabaseAccess.ManualAddStopTime_SQL(timeId, newStop, out string errorMessage);

            // Make the  column for the TimeSheetId invisible again.
            GridView1.Columns[0].Visible = false;

            if (result)
            {
                GridView1.DataBind();
            }
        }

        private void BindDataSource(int userId, int projectId)
        {
            // This is the string that I am setting as the Select statement for the SqlDataSource. This is how the TimeSheet table
            // data fills the GridView.
            SqlDataSource1.SelectCommand = $"SELECT * FROM [TimeSheet] WHERE [UserId] = '{userId}' AND [ProjectId] = '{projectId}'";
        }

        protected void ProjectListDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindDataSource(userId, SelectedProject());
        }

        public int SelectedProject()
        {
            ListItem item = ProjectListDropDown.Items[ProjectListDropDown.SelectedIndex];
            int.TryParse(item.Value, out int selectedProjectId);
            return selectedProjectId;
        }
    }
}