using DVLD_Business;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Driving_License_Management_System.GlobelClasses
{
    internal static class clsGlobal
    {
        public static clsUser CurrentUser;

        public static bool RememberUserNameAndPassword(string UserName, string Password)
        {
            try
            {
                // Get current Project directory Floder.
                string currentDirectory = System.IO.Directory.GetCurrentDirectory();

                // Define the path to the text file where you want to save the data.
                string filePath = currentDirectory + "\\Data.txt";

                // Incase Username is empty, Delete the file.
                if(UserName == "" && File.Exists(filePath))
                {
                    File.Delete(filePath);
                    return true;
                }

                // concatonate username and passwrod with seperator.
                string DataToSave = UserName + "#//#" + Password;

                // Create a StreamWriter to write to the file
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    // Write the data to the file
                    writer.WriteLine(DataToSave);
                    return true;
                }

            }
            catch(Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
                return false;
            }
        }

        public static bool GetStoredCredential(ref string UserName, ref string Password)
        {
            //this will get the stored username and password and will return true if found and false if not found.
            
            try
            {
                string currentDirectory = System.IO.Directory.GetCurrentDirectory();

                string filePath = currentDirectory + "\\Data.txt";

                if(File.Exists(filePath))
                {
                    using (StreamReader reader = new StreamReader(filePath))
                    {
                        string line;
                        while((line = reader.ReadLine()) != null)
                        {
                            string[] result = line.Split(new string[] { "#//#" }, StringSplitOptions.None);

                            UserName = result[0];
                            Password = result[1];
                        }
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
                return false;
            }
        }
    }
}
