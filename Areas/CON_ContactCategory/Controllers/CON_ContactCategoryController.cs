using AddressBookMulti.Areas.CON_ContactCategory.Models;
using AddressBookMulti.DAL;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using AddressBookMulti.BAL;

namespace AddressBookMulti.Areas.CON_ContactCategory.Controllers
{

    [CheckAccess]
    [Area("CON_ContactCategory")]
    [Route("CON_ContactCategory/[controller]/[action]")]

    public class CON_ContactCategoryController : Controller
    {
        string myConnectionString = DALHelper.myConnectionString;

        private IConfiguration Configuration;

        public CON_ContactCategoryController(IConfiguration _configuration)
        {
            Configuration = _configuration;
        }

        #region SelectAll
        public IActionResult Index()
        {
            CON_DAL dalLOC = new CON_DAL();
            DataTable dtContactCategory = dalLOC.PR_CON_ContactCategory_SelectAll();
            return View("CON_ContactCategoryList", dtContactCategory);
        }
        #endregion

        #region Delete
        public IActionResult Delete(int ContactCategoryID)
        {
            CON_DAL dalCON = new CON_DAL();
            DataTable dtContactCategory = dalCON.PR_CON_ContactCategory_DeleteByPK(ContactCategoryID);
            return RedirectToAction("Index");
        }

        #endregion

        #region Add
        public IActionResult Add(int ContactCategoryID)
        {
            if (ContactCategoryID != null)
            {

                CON_DAL dalCON = new CON_DAL();
                DataTable dtContactCategory = dalCON.PR_CON_ContactCategory_SelectByPK(ContactCategoryID);

                CON_ContactCategoryModel modelCON_ContactCategoryModel = new CON_ContactCategoryModel();

                foreach (DataRow dr in dtContactCategory.Rows)
                {
                    modelCON_ContactCategoryModel.UserID = Convert.ToInt32(dr["UserID"]);
                    modelCON_ContactCategoryModel.ContactCategoryID = Convert.ToInt32(dr["ContactCategoryID"]);
                    modelCON_ContactCategoryModel.ContactCategory = Convert.ToString(dr["ContactCategory"]);
                    modelCON_ContactCategoryModel.Creationdate = Convert.ToDateTime(dr["Creationdate"]);
                    modelCON_ContactCategoryModel.ModificationDate = Convert.ToDateTime(dr["ModificationDate"]);
                }
                return View("CON_ContactCategoryAddEdit", modelCON_ContactCategoryModel);

            }
            return View("CON_ContactCategoryAddEdit");
        }

        #endregion

        #region Save
        public IActionResult Save(CON_ContactCategoryModel modelCON_ContactCategory)
        {
            CON_DAL dalLOC = new CON_DAL();

            if (modelCON_ContactCategory.ContactCategoryID == null)
            {
                DataTable dtContactCategory = dalLOC.PR_CON_ContactCategory_Insert(modelCON_ContactCategory);
            }
            else
            {
                DataTable dtContactCategory = dalLOC.PR_CON_ContactCategory_UpdateByPK(modelCON_ContactCategory);
            }

            return RedirectToAction("Index");

        }

        #endregion

        #region Filter
        public IActionResult Filter(string ContactCategory)
        {
            
            DataTable dtContactCategory = new DataTable();
            SqlConnection connCC = new SqlConnection(myConnectionString);
            connCC.Open();
            SqlCommand Objcmd = connCC.CreateCommand();
            Objcmd.CommandType = CommandType.StoredProcedure;
            Objcmd.CommandText = "PR_CON_ContactCategory_SelectByContactCategoryName";
            if (ContactCategory == null)
                Objcmd.Parameters.AddWithValue("@ContactCategory", DBNull.Value);
            else
                Objcmd.Parameters.AddWithValue("@ContactCategory", ContactCategory);

            SqlDataReader SDR = Objcmd.ExecuteReader();
            dtContactCategory.Load(SDR);
            connCC.Close();
            return View("CON_ContactCategoryList", dtContactCategory);
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
