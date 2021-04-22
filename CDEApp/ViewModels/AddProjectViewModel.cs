using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace CDEApp.ViewModels
{
    public class AddProjectViewModel
    {
        #region Properties

        [Required]
        [Display(Name = "Название проекта")]
        public string Name { get; set; } //Name of project

        [Required]
        [Display(Name = "Название документа")]
        public string DocumentName { get; set; } //Name of document

        [Required]
        [Display(Name = "Загрузить файл")]
        public IFormFile Document { get; set; }

        [Required]
        [Display(Name = "Комментарий")]
        public string Comment { get; set; }
        #endregion
    }
}
