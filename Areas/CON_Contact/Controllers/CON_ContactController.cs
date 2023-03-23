using AddressBookMulti.Areas.LOC_Country.Models;
using AddressBookMulti.DAL;
using AddressBookMulti.BAL;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using AddressBookMulti.Areas.CON_Contact.Models;
using AddressBookMulti.Areas.LOC_City.Models;
using AddressBookMulti.Areas.LOC_State.Models;
using AddressBookMulti.Areas.CON_ContactCategory.Models;

namespace AddressBookMulti.Areas.CON_Contact.Controllers
{
    [CheckAccess]
    [Area("CON_Contact")]
    [Route("CON_Contact/[controller]/[action]")]

    public class CON_ContactController : Controller
    {
        string myConnectionString = DALHelper.myConnectionString;

        private IConfiguration Configuration;
        public CON_ContactController(IConfiguration _configuration)
        {
            Configuration = _configuration;
        }
        #region SelectAll
        public IActionResult Index()
        {
            CON_DAL dalCON = new CON_DAL();
            DataTable dtContact = dalCON.PR_CON_Contact_SelectAll();
            return View("CON_ContactList", dtContact);
        }
        #endregion

        #region Delete
        public IActionResult Delete(int ContactID)
        {
            CON_DAL dalCON = new CON_DAL();
            DataTable dtContact = dalCON.PR_CON_Contact_DeleteByPK(ContactID);
            return RedirectToAction("Index");
        }
        #endregion

        #region Add
        public IActionResult Add(int? ContactID)
        {

            #region Country Dropdown

            SqlConnection connContact1 = new SqlConnection(myConnectionString);
            DataTable dtContact1 = new DataTable();
            connContact1.Open();
            SqlCommand ObjCMD1 = connContact1.CreateCommand();
            ObjCMD1.CommandType = CommandType.StoredProcedure;
            ObjCMD1.CommandText = "PR_LOC_Country_SelectForDropDown";
            ObjCMD1.Parameters.Add("@UserID", SqlDbType.Int).Value = CV.UserID();
            SqlDataReader ObjSDR1 = ObjCMD1.ExecuteReader();

            dtContact1.Load(ObjSDR1);

            List<LOC_CountryDropDownModel> list = new List<LOC_CountryDropDownModel>();
            foreach (DataRow dr in dtContact1.Rows)
            {
                LOC_CountryDropDownModel vlst = new LOC_CountryDropDownModel();
                vlst.CountryID = Convert.ToInt32(dr["CountryID"]);
                vlst.CountryName = dr["CountryName"].ToString();
                list.Add(vlst);
            }
            ViewBag.CountryList = list;

            #endregion

            #region State Dropdown

            List<LOC_StateDropDownModel> list1 = new List<LOC_StateDropDownModel>();
            ViewBag.StateList = list1;

            #endregion

            #region City Dropdown
            List<LOC_CityDropDownModel> list2 = new List<LOC_CityDropDownModel>();
            ViewBag.CityList = list2;
            #endregion

            #region Contact Category Dropdown

            SqlConnection connContact2 = new SqlConnection(myConnectionString);
            DataTable dtContact2 = new DataTable();
            connContact2.Open();
            SqlCommand ObjCMD2 = connContact2.CreateCommand();
            ObjCMD2.CommandType = CommandType.StoredProcedure;
            ObjCMD2.CommandText = "PR_CON_ContactCategory_SelectForDropDown";
            ObjCMD2.Parameters.Add("@UserID", SqlDbType.Int).Value = CV.UserID();
            SqlDataReader ObjSDR2 = ObjCMD2.ExecuteReader();
            dtContact2.Load(ObjSDR2);

            List<CON_ContactCategoryDropDownModel> list3 = new List<CON_ContactCategoryDropDownModel>();
            foreach (DataRow dr in dtContact2.Rows)
            {
                CON_ContactCategoryDropDownModel vlst1 = new CON_ContactCategoryDropDownModel();
                vlst1.ContactCategoryID = Convert.ToInt32(dr["ContactCategoryID"]);
                vlst1.ContactCategory = dr["ContactCategory"].ToString();
                list3.Add(vlst1);
            }
            ViewBag.ContactCategoryList = list3;
            #endregion

            #region SelectByPK
            if (ContactID != null)
            {
                CON_DAL dalLOC = new CON_DAL();
                DataTable dtContact = dalLOC.PR_CON_Contact_SelectByPK(ContactID);

                CON_ContactModel modelCON_Contact = new CON_ContactModel();

                foreach (DataRow dr in dtContact.Rows)
                {
                    modelCON_Contact.UserID = Convert.ToInt32(dr["UserID"]);
                    modelCON_Contact.ContactID = Convert.ToInt32(dr["ContactID"]);
                    modelCON_Contact.PhotoPath = Convert.ToString(dr["PhotoPath"]);
                    modelCON_Contact.ContactName = Convert.ToString(dr["ContactName"]);
                    modelCON_Contact.Email = Convert.ToString(dr["Email"]);
                    modelCON_Contact.MobileNo = Convert.ToString(dr["MobileNo"]);
                    modelCON_Contact.CountryID = Convert.ToInt32(dr["CountryID"]);
                    modelCON_Contact.StateID = Convert.ToInt32(dr["StateID"]);
                    modelCON_Contact.CityID = Convert.ToInt32(dr["CityID"]);
                    modelCON_Contact.Profession = Convert.ToString(dr["Profession"]);
                    modelCON_Contact.CompanyName = Convert.ToString(dr["CompanyName"]);
                    modelCON_Contact.Designation = Convert.ToString(dr["Designation"]);
                    modelCON_Contact.ContactCategoryID = Convert.ToInt32(dr["ContactCategoryID"]);
                    modelCON_Contact.Creationdate = Convert.ToDateTime(dr["Creationdate"]);
                    modelCON_Contact.ModificationDate = Convert.ToDateTime(dr["ModificationDate"]);

                }

                DataTable dtContactState = new DataTable();
                SqlConnection connContactState = new SqlConnection(myConnectionString);
                connContactState.Open();

                // Prepare Command
                SqlCommand ObjCMDState = connContactState.CreateCommand();
                ObjCMDState.CommandType = CommandType.StoredProcedure;
                ObjCMDState.CommandText = "PR_LOC_State_SelectDropDownByCountryID";
                ObjCMDState.Parameters.AddWithValue("@CountryID", modelCON_Contact.CountryID);
                SqlDataReader sdrState = ObjCMDState.ExecuteReader();
                dtContactState.Load(sdrState);
                //connContactState.Close();


                List<LOC_StateDropDownModel> vListState = new List<LOC_StateDropDownModel>();
                foreach (DataRow drState in dtContactState.Rows)
                {
                    LOC_StateDropDownModel vStateList = new LOC_StateDropDownModel();
                    vStateList.StateID = Convert.ToInt32(drState["StateID"]);
                    vStateList.StateName = drState["StateName"].ToString();
                    vListState.Add(vStateList);
                }
                ViewBag.StateList = vListState;

                DataTable dtContactCity = new DataTable();
                SqlConnection connContactCity = new SqlConnection(myConnectionString);
                connContactCity.Open();

                // Prepare Command
                SqlCommand ObjCMDCity = connContactState.CreateCommand();
                ObjCMDCity.CommandType = CommandType.StoredProcedure;
                ObjCMDCity.CommandText = "PR_LOC_City_SelectDropDownByStateID";
                ObjCMDCity.Parameters.AddWithValue("@StateID", modelCON_Contact.StateID);
                ObjCMDCity.Parameters.AddWithValue("@UserID", modelCON_Contact.UserID);
                SqlDataReader sdrCity = ObjCMDCity.ExecuteReader();
                dtContactCity.Load(sdrCity);
                connContactCity.Close();


                List<LOC_CityDropDownModel> vListCity = new List<LOC_CityDropDownModel>();
                foreach (DataRow drCity in dtContactCity.Rows)
                {
                    LOC_CityDropDownModel vCityList = new LOC_CityDropDownModel();
                    vCityList.CityID = Convert.ToInt32(drCity["CityID"]);
                    vCityList.CityName = drCity["CityName"].ToString();
                    vListCity.Add(vCityList);
                }
                ViewBag.CityList = vListCity;

                return View("CON_ContactAddEdit", modelCON_Contact);

            }
            #endregion
            return View("CON_ContactAddEdit");
        }
        #endregion

        #region Save
        [HttpPost]
        public IActionResult Save(CON_ContactModel modelCON_Contact)
        {
            if (modelCON_Contact.File != null)
            {
                string FilePath = "wwwroot\\Upload";
                string path = Path.Combine(Directory.GetCurrentDirectory(), FilePath);

                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                string fileNameWithPath = Path.Combine(path, modelCON_Contact.File.FileName);
                modelCON_Contact.PhotoPath = "~" + FilePath.Replace("wwwroot\\", "/") + "/" + modelCON_Contact.File.FileName;

                using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                {
                    modelCON_Contact.File.CopyTo(stream);
                }
            }

            CON_DAL dalCON = new CON_DAL();

            if (modelCON_Contact.ContactID == null)
            {
                DataTable dt = dalCON.PR_CON_Contact_Insert(modelCON_Contact);
            }
            else
            {
                DataTable dt = dalCON.PR_CON_Contact_UpdateByPK(modelCON_Contact);
            }
            return RedirectToAction("Index");
        }
        #endregion

        #region DropDownByCountry

        [HttpPost]
        public IActionResult DropDownByCountry(int CountryID)
        {
            SqlConnection connContact = new SqlConnection(myConnectionString);
            DataTable dtContact = new DataTable();
            connContact.Open();
            SqlCommand ObjCMD = connContact.CreateCommand();
            ObjCMD.CommandType = CommandType.StoredProcedure;
            ObjCMD.CommandText = "PR_LOC_State_SelectDropDownByCountryID";
            ObjCMD.Parameters.AddWithValue("@CountryID", CountryID);
            SqlDataReader ObjSDR = ObjCMD.ExecuteReader();
            dtContact.Load(ObjSDR);

            List<LOC_StateDropDownModel> statelist = new List<LOC_StateDropDownModel>();
            foreach (DataRow dr in dtContact.Rows)
            {
                LOC_StateDropDownModel statelst = new LOC_StateDropDownModel();
                statelst.StateID = Convert.ToInt32(dr["StateID"]);
                statelst.StateName = dr["StateName"].ToString();
                statelist.Add(statelst);
            }
            var vModel = statelist;
            return Json(vModel);
        }
        #endregion
        
        #region DropDownByState

        [HttpPost]
        public IActionResult DropDownByState(int StateID)
        {
            SqlConnection connContact = new SqlConnection(myConnectionString);
            DataTable dtContact = new DataTable();
            connContact.Open();
            SqlCommand ObjCMD = connContact.CreateCommand();
            ObjCMD.CommandType = CommandType.StoredProcedure;
            ObjCMD.CommandText = "PR_LOC_City_SelectDropDownByStateID";
            ObjCMD.Parameters.AddWithValue("@UserID", 1);
            ObjCMD.Parameters.AddWithValue("@StateID", StateID);
            SqlDataReader ObjSDR = ObjCMD.ExecuteReader();
            dtContact.Load(ObjSDR);

            List<LOC_CityDropDownModel> citylist = new List<LOC_CityDropDownModel>();
            foreach (DataRow dr in dtContact.Rows)
            {
                LOC_CityDropDownModel citylst = new LOC_CityDropDownModel();
                citylst.CityID = Convert.ToInt32(dr["CityID"]);
                citylst.CityName = dr["CityName"].ToString();
                citylist.Add(citylst);
            }
            var vModel = citylist;
            return Json(vModel);
        }
        #endregion

        #region Filter
        public IActionResult Filter(string CountryName, string StateName, string CityName)
        {
            SqlConnection connContact = new SqlConnection(myConnectionString);
            connContact.Open();
            DataTable dtContact = new DataTable();
            SqlCommand ObjCMD = connContact.CreateCommand();
            ObjCMD.CommandType = CommandType.StoredProcedure;
            ObjCMD.CommandText = "PR_CON_Contact_SelectByContactName";
            ObjCMD.Parameters.AddWithValue("@UserID", 1);
            if (CountryName == null)
                ObjCMD.Parameters.AddWithValue("@CountryName", DBNull.Value);
            else
                ObjCMD.Parameters.AddWithValue("@CountryName", CountryName);
            if (StateName == null)
                ObjCMD.Parameters.AddWithValue("@StateName", DBNull.Value);
            else
                ObjCMD.Parameters.AddWithValue("@StateName", StateName);
            if (CityName == null)
                ObjCMD.Parameters.AddWithValue("@CityName", DBNull.Value);
            else
                ObjCMD.Parameters.AddWithValue("@CityName", CityName);
           SqlDataReader SDR = ObjCMD.ExecuteReader();
            dtContact.Load(SDR);
            connContact.Close();
            return View("CON_ContactList", dtContact);
        }
        #endregion

        #region GoHome
        public IActionResult GoHome()
        {
            return RedirectToAction("Index");
        }

        #endregion
    }
}
