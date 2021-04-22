using System.ComponentModel.DataAnnotations;

namespace CDEApp.ViewModels
{
    public class AddUserViewModel
    {
        #region Properties
        public int ProjectId { get; set; } //Project ID

        [Required]
        public string Email { get; set; } //Email of added user

        #endregion
    }
}
