namespace AddressBookMulti.DAL
{
    public class DALHelper
    {
        #region Database Connection String

        public static string myConnectionString = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetConnectionString("myConnectionString");
        #endregion
    }
}
