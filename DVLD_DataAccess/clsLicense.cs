using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccess
{
    public class clsLicenseData
    {
        public static int GetActiveLicenseIDByPersonID(int PersonID, int LicenseClassID)
        {
            int LicenseID = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"SELECT        Licenses.LicenseID
                            FROM Licenses INNER JOIN
                                                     Drivers ON Licenses.DriverID = Drivers.DriverID
                            WHERE  
                             
                             Licenses.LicenseClass = @LicenseClass 
                              AND Drivers.PersonID = @PersonID
                              And IsActive=1;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@PersonID", PersonID);
            command.Parameters.AddWithValue("@LicenseClass", LicenseClassID);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    LicenseID = insertedID;
                }
            }

            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);

            }

            finally
            {
                connection.Close();
            }


            return LicenseID;
        }

    }
}
