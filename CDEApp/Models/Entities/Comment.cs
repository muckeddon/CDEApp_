using System;

namespace CDEApp.Models
{
    public class Comment
    {
        #region Properties
        public int Id { get; set; }
        public string Text { get; set; } //Text of comment
        public int DocumentId { get; set; } //Id of document which one comment has a relation
        public User User { get; set; } //User who made comment
        public DateTime Date { get; set; } //Date and time when comment was made
        public Document Document { get; set; } //Document(File)
        #endregion
    }
}
