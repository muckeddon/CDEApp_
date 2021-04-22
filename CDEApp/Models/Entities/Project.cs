using System.Collections.Generic;

namespace CDEApp.Models
{
    public class Project
    {
        #region Properties
        public int Id { get; set; }
        public bool IsClose { get; set; } //Property for understanding to status of project
        public string Name { get; set; }  //Name of project
        public List<Document> Documents { get; set; } = new List<Document>(); //List of documents(Files)
        public List<User> Users { get; set; } = new List<User>(); //List of users who can use this project
        public string Admin { get; set; } //User who made this project
        #endregion
    }
}
