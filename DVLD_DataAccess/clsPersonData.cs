using System;
using System.Data;
using System.Data.SqlClient;

namespace DVLD_DataAccess
{
    public class clsPersonData
    {
        public static DataTable GetAllPeople()
        {
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "SELECT * FROM People";

            SqlCommand command = new SqlCommand(query, connection);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    dt.Load(reader);
                }
                reader.Close();
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return dt;
        }

        public static int AddNewPerson(string FirstName, string SecondName, string ThirdName, string LastName,
                              string NationalNo, DateTime DateOfBirth, byte Gendor, string Phone, string Email,
                              int NationalityCountryID, string Address, string ImagePath)
        {
            int PersonID = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            
            string query = @"INSERT INTO Contacts (FirstName, SecondName, ThirdName, LastName, NationalNo,
                            DateOfBirth, Gendor, Phone, Email, NationalityCountryID, Address, ImagePath)
                             VALUES (@FirstName, @SecondName, @ThirdName,@LastName, @NationalNo,
                             @DateOfBirth, @Gendor, @Phone, @Email, @NationalityCountryID,
                             @Address, @ImagePath);
                             SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@FirstName", FirstName);
            command.Parameters.AddWithValue("@SecondName", SecondName);
            
            if(ThirdName != "")
                command.Parameters.AddWithValue("@ThirdName", ThirdName);
            else
                command.Parameters.AddWithValue("@ThirdName", System.DBNull.Value);

            command.Parameters.AddWithValue("@LastName", LastName);
            command.Parameters.AddWithValue("@NationalNo", NationalNo);
            command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
            command.Parameters.AddWithValue("@Gendor", Gendor);
            command.Parameters.AddWithValue("@Phone", Phone);

            if(Email != "")
                command.Parameters.AddWithValue("@Email", Email);
            else
                command.Parameters.AddWithValue("@Email", System.DBNull.Value);

            command.Parameters.AddWithValue("@NationalityCountryID", NationalityCountryID);
            command.Parameters.AddWithValue("@Address", Address);
            
            if (ImagePath != "")
                command.Parameters.AddWithValue("@ImagePath", ImagePath);
            else
                command.Parameters.AddWithValue("@ImagePath", System.DBNull.Value);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int InsertedID))
                    PersonID = InsertedID;
            }
            catch (Exception ex)
            {
                
            }
            finally
            {
                connection.Close();
            }

            return PersonID;
        }

        public static bool UpdatePerson(int PersonID, string FirstName, string SecondName, string ThirdName, string LastName,
                              string NationalNo, DateTime DateOfBirth, byte Gendor, string Phone, string Email,
                              int NationalityCountryID, string Address, string ImagePath)
        {
            int rowsAffected = 0;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"Update  People  
                            set FirstName = @FirstName,
                                SecondName = @SecondName,
                                ThirdName = @ThirdName,
                                LastName = @LastName, 
                                NationalNo = @NationalNo,
                                DateOfBirth = @DateOfBirth,
                                Gendor = @Gendor,
                                Phone = @Phone, 
                                Email = @Email, 
                                NationalityCountryID = @NationalityCountryID,
                                Address = @Address, 
                                ImagePath =@ImagePath
                              
                                where PersonID = @PersonID";

            SqlCommand command = new SqlCommand(query, connection);


            command.Parameters.AddWithValue("@PersonID", PersonID);
            command.Parameters.AddWithValue("@FirstName", FirstName);
            command.Parameters.AddWithValue("@SecondName", SecondName);

            if (ThirdName != "")
                command.Parameters.AddWithValue("@ThirdName", ThirdName);
            else
                command.Parameters.AddWithValue("@ThirdName", System.DBNull.Value);

            command.Parameters.AddWithValue("@LastName", LastName);
            command.Parameters.AddWithValue("@NationalNo", NationalNo);
            command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
            command.Parameters.AddWithValue("@Gendor", Gendor);
            command.Parameters.AddWithValue("@Phone", Phone);

            if (Email != "")
                command.Parameters.AddWithValue("@Email", Email);
            else
                command.Parameters.AddWithValue("@Email", System.DBNull.Value);

            command.Parameters.AddWithValue("@NationalityCountryID", NationalityCountryID);
            command.Parameters.AddWithValue("@Address", Address);

            if (ImagePath != "")
                command.Parameters.AddWithValue("@ImagePath", ImagePath);
            else
                command.Parameters.AddWithValue("@ImagePath", System.DBNull.Value);

            try
            {
                connection.Open();
                rowsAffected = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
            }
            finally
            {
                connection.Close();
            }
            return (rowsAffected > 0);
        }

    }
}
