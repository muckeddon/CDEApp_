using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace CDEApp.ViewModels
{
    public class AddDocumentViewModel
    {
        #region Properties

        [Required]
        [Display(Name = "Название документа")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Загрузить файл")]
        public IFormFile Document { get; set; }

        [Required]
        public string Comment { get; set; }

        [Required]
        public int projectId { get; set; } //Document will insert in project by this Id
        #endregion
    }
}
