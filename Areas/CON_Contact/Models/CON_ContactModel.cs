using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AddressBookMulti.Areas.CON_Contact.Models
{
    public class CON_ContactModel
    {
        public int? ContactID { get; set; }
        public int? UserID { get; set; }
        public IFormFile File { get; set; }
        [Required]
        [DisplayName("Photo")]
        public string PhotoPath { get; set; }
        [Required]
        [DisplayName("Contact Name")]
        public string ContactName { get; set; }
        public string Email { get; set; }
        [Required]
        [DisplayName("Mobile No")]
        public string MobileNo { get; set; }
        [Required]
        [DisplayName("Country")]
        public int CountryID { get; set; }
        [Required]
        [DisplayName("State")]
        public int StateID { get; set; }
        [Required]
        [DisplayName("City")]
        public int CityID { get; set; }
        public string Profession { get; set; }
        public string CompanyName { get; set; }
        public string Designation { get; set; }
        public int ContactCategoryID { get; set; }
        public DateTime Creationdate { get; set; }
        public DateTime ModificationDate { get; set; }
    }
}
