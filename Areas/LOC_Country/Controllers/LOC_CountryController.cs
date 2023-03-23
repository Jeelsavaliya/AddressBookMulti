using AddressBookMulti.DAL;
using AddressBookMulti.Areas.LOC_Country.Models;
using AddressBookMulti.BAL;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace AddressBookMulti.Areas.LOC_Country.Controllers
{
    [CheckAccess]
    [Area("LOC_Country")]
    [Route("LOC_Country/[controller]/[action]")]

    public class LOC_CountryController : Controller
    {
        string myConnectionString = DALHelper.myConnectionString;

        private IConfiguration Configuration;
       
        public LOC_CountryController(IConfiguration _configuration)
        {
            Configuration = _configuration;
        }

        #region SelectAll
        public IActionResult Index()
        {
            LOC_DAL dalLOC = new LOC_DAL();
            DataTable dtCountry = dalLOC.PR_LOC_Country_SelectAll();
            return View("LOC_CountryList", dtCountry);
        }
        #endregion

        #region Delete
        public IActionResult Delete(int CountryID)
        {
            LOC_DAL dalLOC = new LOC_DAL();
            DataTable dtCountry = dalLOC.PR_LOC_Country_DeleteByPK(CountryID);
            return RedirectToAction("Index");
        }

        #endregion

        #region Save
        [HttpPost]
        public IActionResult Save(LOC_CountryModel modelLOC_Country)
        {
            LOC_DAL dalLOC = new LOC_DAL();

            if (modelLOC_Country.CountryID == null)
            {
                DataTable dtCountry = dalLOC.PR_LOC_Country_Insert(modelLOC_Country);
            }
            else
            {
                DataTable dtCountry = dalLOC.PR_LOC_Country_UpdateByPK(modelLOC_Country);
            }

            return RedirectToAction("Index");
        }
        #endregion

        #region Add
        public IActionResult Add(int? CountryID)
        {
            if (CountryID != null)
            {
                LOC_DAL dalLOC = new LOC_DAL();
                DataTable dtCountry = dalLOC.PR_LOC_Country_SelectByPK(CountryID);

                LOC_CountryModel modelLOC_Country = new LOC_CountryModel();
                foreach (DataRow dr in dtCountry.Rows)
                {
                    modelLOC_Country.UserID = Convert.ToInt32(dr["UserID"]);
                    modelLOC_Country.CountryID = Convert.ToInt32(dr["CountryID"]);
                    modelLOC_Country.CountryName = dr["CountryName"].ToString();
                    modelLOC_Country.CountryCode = dr["CountryCode"].ToString();
                    modelLOC_Country.CreationDate = (DateTime)dr["CreationDate"];
                    modelLOC_Country.ModificationDate = (DateTime)dr["ModificationDate"];
                }
                return View("LOC_CountryAddEdit", modelLOC_Country);

            }
            return View("LOC_CountryAddEdit");
        }

        #endregion

        #region Filter
        public IActionResult Filter(string CountryName, string CountryCode)
        {
            DataTable dtCountry = new DataTable();
            SqlConnection connCountry = new SqlConnection(myConnectionString);
            connCountry.Open();
            SqlCommand Objcmd = connCountry.CreateCommand();
            Objcmd.CommandType = CommandType.StoredProcedure;
            Objcmd.CommandText = "PR_LOC_Country_SelectByCountryName";

            if (CountryName == null)
                Objcmd.Parameters.AddWithValue("@CountryName", DBNull.Value);
            else
                Objcmd.Parameters.AddWithValue("@CountryName", CountryName);
            if (CountryCode == null)
                Objcmd.Parameters.AddWithValue("@CountryCode", DBNull.Value);
            else
                Objcmd.Parameters.AddWithValue("@CountryCode", CountryCode);

            SqlDataReader ObjSDR = Objcmd.ExecuteReader();
            dtCountry.Load(ObjSDR);
            connCountry.Close();
            return View("LOC_CountryList", dtCountry);
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