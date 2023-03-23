using AddressBookMulti.Areas.LOC_State.Models;
using AddressBookMulti.DAL;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using AddressBookMulti.BAL;
using AddressBookMulti.Areas.LOC_Country.Models;

namespace AddressBookMulti.Areas.LOC_State.Controllers
{
    [CheckAccess]
    [Area("LOC_State")] 
    [Route("LOC_State/[controller]/[action]")]

    public class LOC_StateController : Controller
    {
        string myConnectionString = DALHelper.myConnectionString;

        private IConfiguration Configuration;
        public LOC_StateController(IConfiguration _configuration)
        {
            Configuration = _configuration;
        }

        #region SelectAll
        public IActionResult Index()
        {
            LOC_DAL dalLOC = new LOC_DAL();
            DataTable dtState = dalLOC.PR_LOC_State_SelectAll();
            return View("LOC_StateList", dtState);
        }
        #endregion

        #region Delete
        public IActionResult Delete(int StateID)
        { 
            LOC_DAL dalLOC = new LOC_DAL();
            DataTable dtState = dalLOC.PR_LOC_State_DeleteByPK(StateID);
            return RedirectToAction("Index");
        }

        #endregion

        #region Save

        [HttpPost]
        public IActionResult Save(LOC_StateModel modelLOC_State)
        { 
            LOC_DAL dalLOC = new LOC_DAL();

            if (modelLOC_State.StateID == null)
            {
                DataTable dtState = dalLOC.PR_LOC_State_Insert(modelLOC_State);
            }
            else
            {
                DataTable dtState = dalLOC.PR_LOC_State_UpdateByPK(modelLOC_State);
            }

            return RedirectToAction("Index");
        }
        #endregion

        #region Add
        public IActionResult Add(int? StateID)
        {

            #region Country DropDown

            DataTable dtStateCDD = new DataTable();
            SqlConnection connStateCountry = new SqlConnection(myConnectionString);
            connStateCountry.Open();

            // Prepare Command
            SqlCommand ObjCountry = connStateCountry.CreateCommand();
            ObjCountry.CommandType = CommandType.StoredProcedure;
            ObjCountry.CommandText = "PR_LOC_Country_SelectForDropDown";
            ObjCountry.Parameters.Add("@UserID", SqlDbType.Int).Value = CV.UserID();
            SqlDataReader ObjSDR1 = ObjCountry.ExecuteReader();
            dtStateCDD.Load(ObjSDR1);
            //connStateCountry.Close();

            List<LOC_CountryDropDownModel> countrydroplist = new List<LOC_CountryDropDownModel>();
            foreach (DataRow dr in dtStateCDD.Rows)
            {
                LOC_CountryDropDownModel countrylst = new LOC_CountryDropDownModel();
                countrylst.CountryID = Convert.ToInt32(dr["CountryID"]);
                countrylst.CountryName = dr["CountryName"].ToString();
                countrydroplist.Add(countrylst);
            }
            ViewBag.CountryList = countrydroplist;

            #endregion

            #region Select By PK

            if (StateID != null)
            {
                LOC_DAL dalLOC = new LOC_DAL();
                DataTable dtState = dalLOC.PR_LOC_State_SelectByPK(StateID);

                LOC_StateModel modelLOC_State = new LOC_StateModel();
                foreach (DataRow dr in dtState.Rows)
                {
                    modelLOC_State.UserID = (int)dr["UserID"];
                    modelLOC_State.CountryID = (int)dr["CountryID"];
                    modelLOC_State.StateName = dr["StateName"].ToString();
                    modelLOC_State.StateCode = dr["StateCode"].ToString();
                    modelLOC_State.CreationDate = (DateTime)dr["CreationDate"];
                    modelLOC_State.ModificationDate = (DateTime)dr["ModificationDate"];
                }
                return View("LOC_StateAddEdit", modelLOC_State);
            }
            #endregion

            return View("LOC_StateAddEdit");
        }

        #endregion

        #region Filter
        public IActionResult Filter(string CountryName, string StateName, string StateCode)
        {
            DataTable dtState = new DataTable();
            SqlConnection connState = new SqlConnection(myConnectionString);
            connState.Open();
            SqlCommand Objcmd = connState.CreateCommand();
            Objcmd.CommandType = CommandType.StoredProcedure;
            Objcmd.CommandText = "PR_LOC_State_SelectByStateName";
            if (CountryName == null)
                Objcmd.Parameters.AddWithValue("@CountryName", DBNull.Value);
            else
                Objcmd.Parameters.AddWithValue("@CountryName", CountryName);
            if (StateName == null)
                Objcmd.Parameters.AddWithValue("@StateName", DBNull.Value);
            else
                Objcmd.Parameters.AddWithValue("@StateName", StateName);
            if (StateCode == null)
                Objcmd.Parameters.AddWithValue("@StateCode", DBNull.Value);
            else
                Objcmd.Parameters.AddWithValue("@StateCode", StateCode);
            SqlDataReader SDR = Objcmd.ExecuteReader();
            dtState.Load(SDR);
            connState.Close();
            return View("LOC_StateList", dtState);
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
