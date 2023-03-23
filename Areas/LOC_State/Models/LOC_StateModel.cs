using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace AddressBookMulti.Areas.LOC_State.Models
{
    #region LOC_StateModel
    public class LOC_StateModel
    {
        public int? StateID { get; set; }
        public int? UserID { get; set; }


        [Required(ErrorMessage = "Please enter State name")]
        [DisplayName("State Name")]
        [StringLength(20, MinimumLength = 3)]
        public string StateName { get; set; }


        [Required(ErrorMessage = "Please enter State Code")]
        [DisplayName("State Code")]
        public string StateCode { get; set; }

        [Required(ErrorMessage = "Please select Country")]
        public int CountryID { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModificationDate { get; set; }

    }
    #endregion

    #region LOC_State_SelectForDropDown
    public class LOC_StateDropDownModel
    {
        public int StateID { get; set; }
        public string StateName { get; set; }
    }
        #endregion
}