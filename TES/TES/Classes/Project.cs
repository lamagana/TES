using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TES
{
    public class Project
    {
        public int ProjectId { get; set; }

        public string Description { get; set; }

        public Project(int projectId, string description)
        {
            ProjectId = projectId;
            Description = description;
        }
    }
}