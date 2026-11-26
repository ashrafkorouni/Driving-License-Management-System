using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccess
{
    public class clsDriverData
    {
        public static bool GetDriverInfoByDriverID(int DriverID,
            ref int PersonID, ref int CreatedByUserID, ref DateTime CreatedDate)
        {
            bool isFound = false;

            string query = "SELECT * FROM Drivers WHERE DriverID = @DriverID";


            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@DriverID", SqlDbType.Int).Value = DriverID;

                try
                {
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {

                            isFound = true;

                            PersonID = (int)reader["PersonID"];
                            CreatedByUserID = (int)reader["CreatedByUserID"];
                            CreatedDate = (DateTime)reader["CreatedDate"];

                        }
                    }
                }
                catch (Exception ex)
                {
                    isFound = false;
                }
            }

            return isFound;
        }

        public static bool GetDriverInfoByPersonID(int PersonID,
            ref int DriverID, ref int CreatedByUserID, ref DateTime CreatedDate)
        {
            bool isFound = false;

            string query = "SELECT * FROM Drivers WHERE PersonID = @PersonID";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@PersonID", SqlDbType.Int).Value = PersonID;

                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            isFound = true;

                            DriverID = (int)reader["DriverID"];
                            CreatedByUserID = (int)reader["CreatedByUserID"];
                            CreatedDate = (DateTime)reader["CreatedDate"];
                        }
                    }
                }
                catch (Exception ex)
                {
                    isFound = false;
                }
            }
            return isFound;
        }

        public static DataTable GetAllDrivers()
        {
            DataTable dt = new DataTable();

            string query = "SELECT * FROM Drivers_View ORDER BY FullName";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                try
                {
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            dt.Load(reader);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Console.WriteLine("Error: " + ex.Message);
                }
            }
            return dt;
        }

        public static int AddNewDriver(int PersonID, int CreatedByUserID)
        {
            int DriverID = -1;

            string query = @"INSERT INTO Drivers (PersonID, CreatedByUserID, CreatedDate)
                             VALUES (@PersonID, @CreatedByUserID, @CreatedDate);
                             SELECT SCOPE_IDENTITY();";


            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@PersonID", SqlDbType.Int).Value = PersonID;
                command.Parameters.Add("@CreatedByUserID", SqlDbType.Int).Value = CreatedByUserID;
                command.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = DateTime.Now;

                try
                {
                    connection.Open();

                    object result = command.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        DriverID = Convert.ToInt32(result);
                    }
                }
                catch (Exception ex)
                {
                    //Console.WriteLine("Error: " + ex.Message);
                }
            }
            return DriverID;
        }

        public static bool UpdateDriver(int DriverID, int PersonID, int CreatedByUserID)
        {
            int rowsAffected = 0;

            string query = @"UPDATE Drivers
                             SET PersonID = @PersonID,
                                 CreatedByUserID = @CreatedByUserID
                             WHERE DriverID = @DriverID;";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@DriverID", SqlDbType.Int).Value = DriverID;
                command.Parameters.Add("@PersonID", SqlDbType.Int).Value = PersonID;
                command.Parameters.Add("@CreatedByUserID", SqlDbType.Int).Value = CreatedByUserID;

                try
                {
                    connection.Open();

                    rowsAffected = command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    //Console.WriteLine("Error: " + ex.Message);
                }
            }
            return (rowsAffected > 0);
        }
    }
}

