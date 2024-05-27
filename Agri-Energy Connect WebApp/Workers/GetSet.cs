namespace Agri_Energy_Connect_WebApp.Workers
{
    public class GetSet
    {
        //Static Variable to store which user is using the application
        static string userEmployee;
        static int userFarmer;
        //----------------------------------------------------------------------------------------------------------------
        //Static Getters and Setters associated user using the application
        public static string UserEmployee { get => userEmployee; set => userEmployee = value; }
        public static int UserFarmer { get => userFarmer; set => userFarmer = value; }

        //----------------------------------------------------------------------------------------------------------------
    }
}
