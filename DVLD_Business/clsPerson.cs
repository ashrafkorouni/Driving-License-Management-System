using DVLD_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Business
{
    public class clsPerson
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int PersonID { set; get; }
        public string FirstName { set; get; }
        public string SecondName { set; get; }
        public string ThirdName { set; get; }
        public string LastName { set; get; }
        public string FullName
        {
            get { return FirstName + " " + SecondName + " " + ThirdName + " " + LastName;  }
        }
        public string NationalNo { set; get; }
        public DateTime DateOfBirth { set; get; }
        public byte Gendor { set; get; }
        public string Phone { set; get; }
        public string Email { set; get; }
        public int NationalityCountryID { set; get; }
        public string Address { set; get; }

        private string _ImagePath;

        public string ImagePath
        { 
            get { return _ImagePath; }

            set { _ImagePath = value; }
        }

        public clsCountry CountryInfo;

        public clsPerson()
        {
            PersonID = -1;
            FirstName = "";
            SecondName = "";
            ThirdName = "";
            LastName = "";
            NationalNo = "";
            DateOfBirth = DateTime.Now;
            Gendor = 0;
            Phone = "";
            Email = "";
            NationalityCountryID = -1;
            Address = "";
            ImagePath = "";

            Mode = enMode.AddNew;
        }

        private clsPerson(int PersonID, string FirstName, string SecondName, string ThirdName,
            string LastName, string NationalNo, DateTime DateOfBirth, byte Gendor, string Phone, string Email,
            int NationalityCountryID, string Address, string ImagePath)
        {
            this.PersonID = PersonID;
            this.FirstName = FirstName;
            this.SecondName = SecondName;
            this.ThirdName = ThirdName;
            this.LastName = LastName;
            this.NationalNo = NationalNo;
            this.DateOfBirth = DateOfBirth;
            this.Gendor = Gendor;
            this.Phone = Phone;
            this.Email = Email;
            this.NationalityCountryID = NationalityCountryID;
            this.Address = Address;
            this.ImagePath = ImagePath;
            this.CountryInfo = clsCountry.Find(NationalityCountryID);

            Mode = enMode.Update;
        }

        private bool _AddNewPerson()
        {
            this.PersonID = clsPersonData.AddNewPerson(this.FirstName, this.SecondName, this.ThirdName,
                this.LastName, this.NationalNo, this.DateOfBirth, this.Gendor, this.Phone, this.Email,
                this.NationalityCountryID, this.Address, this.ImagePath);

            return (this.PersonID != -1);
        }

        private bool _UpdatePerson()
        {
            return clsPersonData.UpdatePerson(this.PersonID, this.FirstName, this.SecondName, this.ThirdName,
                this.LastName, this.NationalNo, this.DateOfBirth, this.Gendor, this.Phone, this.Email,
                this.NationalityCountryID, this.Address, this.ImagePath);
        }

        public static clsPerson Find(int PersonID)
        {
            string FirstName = "", SecondName = "", ThirdName = "", LastName = "", NationalNo = "", Email = "", Phone = "", Address = "", ImagePath = "";
            DateTime DateOfBirth = DateTime.Now;
            int NationalityCountryID = -1;
            byte Gendor = 0;

            bool isFound = clsPersonData.GetPersonInfoByID(PersonID, ref FirstName, ref SecondName, ref ThirdName, ref LastName, ref NationalNo,
                                            ref DateOfBirth, ref Gendor, ref Phone, ref Email, ref NationalityCountryID,
                                            ref Address, ref ImagePath);

            if(isFound)
               return new clsPerson(PersonID, FirstName, SecondName, ThirdName, LastName, NationalNo, DateOfBirth,
                                Gendor, Phone, Email, NationalityCountryID, Address, ImagePath);
            else

                return null;
                                
        }

        public static clsPerson Find(string NationalNo)
        {
            int PersonID = -1;
            string FirstName = "", SecondName = "", ThirdName = "", LastName = "", Email = "", Phone = "", Address = "", ImagePath = "";
            DateTime DateOfBirth = DateTime.Now;
            int NationalityCountryID = -1;
            byte Gendor = 0;

            bool isFound = clsPersonData.GetPersonInfoByNationalNo(NationalNo, ref PersonID, ref FirstName, ref SecondName, ref ThirdName, ref LastName,
                                            ref DateOfBirth, ref Gendor, ref Phone, ref Email, ref NationalityCountryID,
                                            ref Address, ref ImagePath);

            if (isFound)
                return new clsPerson(PersonID, FirstName, SecondName, ThirdName, LastName, NationalNo, DateOfBirth,
                                 Gendor, Phone, Email, NationalityCountryID, Address, ImagePath);
            else

                return null;

        }
        
        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if(_AddNewPerson())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case enMode.Update:

                    return _UpdatePerson();
                    
            }

            return false;
        }

        public static DataTable GetAllPeople()
        {
            return clsPersonData.GetAllPeople();
        }


        public static bool DeletePerson(int PersonID)
        {
            return clsPersonData.DeletePesone(PersonID);
        }

        public static bool IsPersonExist(int PersonID)
        {
            return clsPersonData.IsPersonExist(PersonID);
        }

        public static bool IsPersonExist(string NationalNo)
        {
            return clsPersonData.IsPersonExist(NationalNo);
        }
    }
}
