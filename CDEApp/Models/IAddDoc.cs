using Microsoft.AspNetCore.Http;

namespace CDEApp.Models
{
    public interface IAddDoc
    {
        #region Properties
        public string Name { get; set; } //Name of document
        public IFormFile Document { get; set; } //File information
        public string Comment { get; set; } //Text of comment
        #endregion
    }
}
