using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace AddressBookMulti.Areas.User_Master.Models
{
    public class User_MasterModel
    {
        public int UserID { get; set; }

        [Required]
        [DisplayName("User Name")]
        public string UserName { get; set; }

        [Required]
        [DisplayName("User Name")]
        public string Password { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModificationDate { get; set; }
    }
}
