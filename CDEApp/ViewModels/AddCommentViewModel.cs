using CDEApp.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CDEApp.ViewModels
{
    public class AddCommentViewModel
    {
        #region Properties
        public bool IsProjectClose { get; set; } //Project status
        public int DocumentId { get; set; } //Document ID
        public List<Document> Documents { get; set; } //List of document in project
        public string Admin { get; set; } //Admin of project
        public string CurrentUser { get; set; } //Current user name

        [Required]
        [Display(Name = "Добавить комментарий к документу:")]
        public string CommentText { get; set; } //Text of comment

        #endregion
    }
}
