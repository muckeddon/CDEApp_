using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace CDEApp.Models
{
    public class User : IdentityUser
    {
        #region Properties
        public List<Project> Projects { get; set; } = new List<Project>(); //List of projects
        #endregion
    }
}
