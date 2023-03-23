using AddressBookMulti.Areas.LOC_City.Models;
using AddressBookMulti.Areas.LOC_Country.Models;
using AddressBookMulti.Areas.LOC_State.Models;
using AddressBookMulti.DAL;
using AddressBookMulti.BAL;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using System;

namespace AddressBookMulti.Areas.LOC_City.Controllers
{
    [CheckAccess]
    [Area("LOC_City")]
    [Route("LOC_City/[controller]/[action]")]

    public class LOC_CityController : Controller
    {
        string myConnectionString = DALHelper.myConnectionString;

        private IConfiguration Configuration;

        public LOC_CityController(IConfiguration _configuration)
        {
            Configuration = _configuration;
        }

        #region SelectAll
        public IActionResult Index()
        {
            LOC_DAL dalLOC = new LOC_DAL();
            DataTable dtCity = dalLOC.PR_LOC_City_SelectAll();
            return View("LOC_CityList", dtCity);
        }
        #endregion

        #region Delete
        public IActionResult Delete(int CityID)
        {
            LOC_DAL dalLOC = new LOC_DAL();
            DataTable dtCity = dalLOC.PR_LOC_City_DeleteByPK(CityID);
            return RedirectToAction("Index");
        }
        #endregion

        #region Add
        public IActionResult Add(int? CityID)
        {
            #region Country DropDown

            SqlConnection connCountry = new SqlConnection(myConnectionString);
            DataTable dtCountry = new DataTable();
            connCountry.Open();
            SqlCommand ObjCountry = connCountry.CreateCommand();
            ObjCountry.CommandType = CommandType.StoredProcedure;
            ObjCountry.CommandText = "PR_LOC_Country_SelectForDropDown";
            ObjCountry.Parameters.Add("@UserID", SqlDbType.Int).Value = CV.UserID();
            SqlDataReader ObjSDR1 = ObjCountry.ExecuteReader();
            dtCountry.Load(ObjSDR1);

            List<LOC_CountryDropDownModel> countrydroplist = new List<LOC_CountryDropDownModel>();
            foreach (DataRow dr in dtCountry.Rows)
            {
                LOC_CountryDropDownModel countrylst = new LOC_CountryDropDownModel();
                countrylst.CountryID = Convert.ToInt32(dr["CountryID"]);
                countrylst.CountryName = dr["CountryName"].ToString();
                countrydroplist.Add(countrylst);
            }
            ViewBag.CountryList = countrydroplist;

            #endregion

            #region State DropDown

            List<LOC_StateDropDownModel> statedroplist = new List<LOC_StateDropDownModel>();
            ViewBag.StateList = statedroplist;

            #endregion

            #region Select By PK

            if (CityID != null)
            {
                LOC_DAL dalLOC = new LOC_DAL();
                DataTable dtCity = dalLOC.PR_LOC_City_SelectByPK(CityID);

                LOC_CityModel modelLOC_City = new LOC_CityModel();
                foreach (DataRow dr in dtCity.Rows)
                {
                    modelLOC_City.UserID = (Convert.ToInt32(dr["UserID"]));
                    modelLOC_City.CountryID=(Convert.ToInt32(dr["CountryID"]));
                    modelLOC_City.CityID = Convert.ToInt32(dr["CityID"]);
                    modelLOC_City.CityName = dr["CityName"].ToString();
                    modelLOC_City.Pincode = Convert.ToInt32(dr["Pincode"]);
                    modelLOC_City.CountryID = Convert.ToInt32(dr["CountryID"]);
                    modelLOC_City.StateID = Convert.ToInt32(dr["StateID"]);
                }

                DataTable dtState = new DataTable();
                SqlConnection connState = new SqlConnection(myConnectionString);
                connState.Open();

                // Prepare Command
                SqlCommand cmdState = connState.CreateCommand();
                cmdState.CommandType = CommandType.StoredProcedure;
                cmdState.CommandText = "PR_LOC_State_SelectDropDownByCountryID";
                cmdState.Parameters.AddWithValue("@CountryID", modelLOC_City.CountryID);
                SqlDataReader sdrState = cmdState.ExecuteReader();
                dtState.Load(sdrState);
                //connState.Close();


                List<LOC_StateDropDownModel> vListState = new List<LOC_StateDropDownModel>();
                foreach (DataRow drState in dtState.Rows)
                {
                    LOC_StateDropDownModel vStateList = new LOC_StateDropDownModel();
                    vStateList.StateID = Convert.ToInt32(drState["StateID"]);
                    vStateList.StateName = drState["StateName"].ToString();
                    vListState.Add(vStateList);
                }
                ViewBag.StateList = vListState;


                return View("LOC_CityAddEdit", modelLOC_City);

            }

            #endregion

            return View("LOC_CityAddEdit");
        }
        #endregion

        #region Save
        [HttpPost]
        public IActionResult Save(LOC_CityModel modelLOC_City)
        {
            LOC_DAL dalLOC = new LOC_DAL();

            if (modelLOC_City.CityID == null)
            {
                DataTable dtCity =dalLOC.PR_LOC_City_Insert(modelLOC_City);
            }
            else
            {
                DataTable dtCity = dalLOC.PR_LOC_City_UpdateByPK(modelLOC_City);
            }
            return RedirectToAction("Index");
        }
        #endregion

        #region DropDown By CountryID
        public IActionResult DropDownByState(int CountryID)
        {

            DataTable dtStateDD = new DataTable();
            SqlConnection connStateDD = new SqlConnection(myConnectionString);
            connStateDD.Open();
            SqlCommand Objcmd = connStateDD.CreateCommand();
            Objcmd.CommandType = CommandType.StoredProcedure;
            Objcmd.CommandText = "PR_LOC_State_SelectDropDownByCountryID";
            Objcmd.Parameters.AddWithValue("@CountryID", CountryID);
            SqlDataReader objSDR = Objcmd.ExecuteReader();
            dtStateDD.Load(objSDR);
            connStateDD.Close();

            List<LOC_StateDropDownModel> statelist = new List<LOC_StateDropDownModel>();

            foreach (DataRow dr in dtStateDD.Rows)
            {
                LOC_StateDropDownModel statelst = new LOC_StateDropDownModel();
                statelst.StateID = Convert.ToInt32(dr["StateID"]);
                statelst.StateName = Convert.ToString(dr["StateName"]);
                statelist.Add(statelst);
            }
            ViewBag.StateList = statelist;
            var vModel = statelist;
            return Json(vModel);
        }
        #endregion

        #region Filter
        public IActionResult Filter(string CountryName, string StateName, string CityName)
        {
            DataTable dtCity = new DataTable();
            SqlConnection connCity = new SqlConnection(myConnectionString);
            connCity.Open();
            SqlCommand Objcmd = connCity.CreateCommand();
            Objcmd.CommandType = CommandType.StoredProcedure;
            Objcmd.CommandText = "PR_LOC_City_SelectyByCityName";
            if (CountryName == null)
                Objcmd.Parameters.AddWithValue("@CountryName", DBNull.Value);
            else
                Objcmd.Parameters.AddWithValue("@CountryName", CountryName);
            if (StateName == null)
                Objcmd.Parameters.AddWithValue("@StateName", DBNull.Value);
            else
                Objcmd.Parameters.AddWithValue("@StateName", StateName);
            if (CityName == null)
                Objcmd.Parameters.AddWithValue("@CityName", DBNull.Value);
            else
                Objcmd.Parameters.AddWithValue("@CityName", CityName);
            SqlDataReader SDR = Objcmd.ExecuteReader();
            dtCity.Load(SDR);
            connCity.Close();
            return View("LOC_CityList", dtCity);
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
