using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccess
{
    public class clsTestData
    {
        public static bool GetTestInfoByID(int TestID,
            ref int TestAppointmentID, ref bool TestResult,
            ref string Notes, ref int CreatedByUserID)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "SELECT * FROM Tests WHERE TestID = @TestID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@TestID", TestID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;

                    TestAppointmentID = (int)reader["TestAppointmentID"];
                    TestResult = (bool)reader["TestResult"];
                    if (reader["Notes"] == DBNull.Value)
                        Notes = "";
                    else
                        Notes = (string)reader["Notes"];

                    CreatedByUserID = (int)reader["CreatedByUserID"];
                }
                else
                {
                    isFound = false;
                }
            }
            catch (Exception ex)
            {
                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }

        public static bool GetLastTestByPersonAndTestTypeAndLicenseClass(
            int PersonID, int LicenseClassID, int TestTypeID,
            ref int TestID, ref int TestAppointmentID, ref bool TestResult,
            ref string Notes, ref int CreatedByUserID)
        {
            bool isFound = false;

            string query = @"SELECT TOP 1 Tests.TestID, 
                            Tests.TestAppointmentID, 
                            Tests.TestResult, 
                            Tests.Notes, 
                            Tests.CreatedByUserID
                     FROM LocalDrivingLicenseApplications 
                     INNER JOIN Applications 
                         ON LocalDrivingLicenseApplications.ApplicationID = Applications.ApplicationID
                     INNER JOIN TestAppointments 
                         ON LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = TestAppointments.LocalDrivingLicenseApplicationID
                     INNER JOIN Tests 
                         ON Tests.TestAppointmentID = TestAppointments.TestAppointmentID
                     WHERE Applications.ApplicantPersonID = @PersonID
                       AND LocalDrivingLicenseApplications.LicenseClassID = @LicenseClassID
                       AND TestAppointments.TestTypeID = @TestTypeID
                     ORDER BY Tests.TestID DESC";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@PersonID", SqlDbType.Int).Value = PersonID;
                    command.Parameters.Add("@LicenseClassID", SqlDbType.Int).Value = LicenseClassID;
                    command.Parameters.Add("@TestTypeID", SqlDbType.Int).Value = TestTypeID;

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            isFound = true;
                            TestID = (int)reader["TestID"];
                            TestAppointmentID = (int)reader["TestAppointmentID"];
                            TestResult = (bool)reader["TestResult"];
                            Notes = reader["Notes"] == DBNull.Value ? "" : (string)reader["Notes"];
                            CreatedByUserID = (int)reader["CreatedByUserID"];
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                isFound = false;
            }

            return isFound;
        }

        public static DataTable GetAllTests()
        {
            DataTable dt = new DataTable();

            string query = "SELECT * FROM Tests ORDER BY TestID";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        dt.Load(reader);
                    }
                }
            }
            catch (Exception ex)
            {
                //System.Diagnostics.Debug.WriteLine("Error in GetAllTests: " + ex.Message);
            }

            return dt;
        }

        public static int AddNewTest(int TestAppointmentID, 
            bool TestResult, string Notes, int CreatedByUserID)
        {
            int TestID = -1;

            string query = @"INSERT INTO Tests (TestAppointmentID,
                             TestResult, Notes, CreatedByUserID)
                             VALUES (@TestAppointmentID, @TestResult,
                             @Notes, @CreatedByUserID);

                             UPDATE TestAppointments
                             SET IsLocked = 1
                             WHERE TestAppointmentID = @TestAppointmentID;

                             SELECT SCOPE_IDENTITY();";
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);
                    command.Parameters.AddWithValue("@TestResult", TestResult);

                    if (Notes != "" && Notes != null)
                        command.Parameters.AddWithValue("@Notes", Notes);
                    else
                        command.Parameters.AddWithValue("@Notes", System.DBNull.Value);

                    command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

                    connection.Open();

                    object result = command.ExecuteScalar();

                    if (result != null && int.TryParse(result.ToString(), out int insertedID))
                    {
                        TestID = insertedID;
                    }
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
            }

            return TestID;
        }

        public static bool UpdateTest(int TestID, int TestAppointmentID, bool TestResult,
             string Notes, int CreatedByUserID)
        {
            int rowsAffected = 0;

            string query = @"UPDATE Tests
                            SET TestAppointmentID = @TestAppointmentID, 
                                TestResult = @TestResult,
                                Notes = @Notes, 
                                CreatedByUserID = @CreatedByUserID
                            WHERE TestID = @TestID;";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();

                    command.Parameters.Add("@TestAppointmentID", SqlDbType.Int).Value = TestAppointmentID;
                    command.Parameters.Add("@TestResult", SqlDbType.Bit).Value = TestResult;

                    if (!string.IsNullOrEmpty(Notes))
                        command.Parameters.Add("@Notes", SqlDbType.NVarChar).Value = Notes;
                    else
                        command.Parameters.Add("@Notes", SqlDbType.NVarChar).Value = DBNull.Value;

                    command.Parameters.Add("@CreatedByUserID", SqlDbType.Int).Value = CreatedByUserID;
                    command.Parameters.Add("@TestID", SqlDbType.Int).Value = TestID;

                    rowsAffected = command.ExecuteNonQuery();
                }
            }
            catch (Exception ex) 
            {
                return false;
            }

            return (rowsAffected > 0);
        }

        public static byte GetPassedTestCount(int LocalDrivingLicenseApplicationID)
        {
            byte PassedTestCount = 0;

            string query = @"SELECT PassedTestCount = COUNT(TestTypeID)
                             FROM Tests INNER JOIN
                             TestAppointments ON Tests.TestAppointmentID = TestAppointments.TestAppointmentID
						     WHERE LocalDrivingLicenseApplicationID =@LocalDrivingLicenseApplicationID and TestResult=1";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@LocalDrivingLicenseApplicationID", SqlDbType.Int).Value = LocalDrivingLicenseApplicationID;

                    connection.Open();

                    object result = command.ExecuteScalar();

                    if (result != null && byte.TryParse(result.ToString(), out byte ptCount))
                    {
                        PassedTestCount = ptCount;
                    }

                }
            }
            catch (Exception ex)
            {
            }

            return PassedTestCount;
        }
    }
}
