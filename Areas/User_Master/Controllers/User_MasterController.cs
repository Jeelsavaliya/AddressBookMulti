using AddressBookMulti.DAL;
using AddressBookMulti.Areas.User_Master.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using AddressBookMulti.BAL;

namespace AddressBookMulti.Areas.User_Master.Controllers
{
    [Area("User_Master")]
    [Route("User_Master/[controller]/[action]")]

    public class User_MasterController : Controller
    {
        private IConfiguration Configuration;
        public User_MasterController(IConfiguration _configuration)
        {
            Configuration = _configuration;
        }

        public IActionResult Index()
        {
            return View("Login");
        }

        [HttpPost]
        public IActionResult Login(User_MasterModel modelUser_Master)
        {
            string connstr = this.Configuration.GetConnectionString("myConnectionString");
            string error = null;
            if (modelUser_Master.UserName == null)
            {
                error += "User Name is required";
            }
            if (modelUser_Master.Password == null)
            {
                error += "<br/>Password is required";
            }

            if (error != null)
            {
                TempData["Error"] = error;
                return RedirectToAction("Index");
            }
            else
            {
                SEC_DAL dal = new SEC_DAL();
                DataTable dt = dal.PR_User_Master_SelectByUserNamePassword(connstr, modelUser_Master.UserName, modelUser_Master.Password);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        HttpContext.Session.SetString("UserID", dr["UserID"].ToString());
                        HttpContext.Session.SetString("UserName", dr["UserName"].ToString());
                        HttpContext.Session.SetString("Password", dr["Password"].ToString());
                        //HttpContext.Session.SetString("FirstName", dr["FirstName"].ToString());
                        //HttpContext.Session.SetString("LastName", dr["LastName"].ToString());
                        // HttpContext.Session.SetString("PhotoPath", dr["PhotoPath"].ToString());
                        break;
                    }
                }
                else
                {
                    TempData["Error"] = "User Name or Password is invalid!";
                    return RedirectToAction("Index");
                }
                if (HttpContext.Session.GetString("UserName") != null && HttpContext.Session.GetString("Password") != null)
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            return RedirectToAction("Index");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
    }
}
