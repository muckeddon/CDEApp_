using System.Collections.Generic;

namespace CDEApp.Models
{
    public class Document
    {
        #region Properties
        public int Id { get; set; }
        public string Name { get; set; } //Name of document
        public byte[] Subject { get; set; } //Byte array of file for storage in database
        public string Format { get; set; } //Format of file
        public int ProjectId { get; set; }
        public Project Project { get; set; } //Project for relation
        public List<Comment> Comments { get; set; } = new List<Comment>(); // List of all comments about document
        #endregion
    }
}
