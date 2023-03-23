using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AddressBookMulti.Areas.CON_ContactCategory.Models
{
    public class CON_ContactCategoryModel
    {
        public int? ContactCategoryID { get; set; }
        public int? UserID { get; set; }
        [Required]
        [DisplayName("ContactCategory")]
        public string ContactCategory { get; set; }
        public DateTime Creationdate { get; set; }
        public DateTime ModificationDate { get; set; }
    }
    public class CON_ContactCategoryDropDownModel
    {
        public int ContactCategoryID { get; set; }
        public string ContactCategory { get; set; }
    }
}
