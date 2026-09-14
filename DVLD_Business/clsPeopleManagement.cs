using DVLD_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Business
{
    public class clsPeopleManagement
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int PersonID { set; get; }
        public string FirstName { set; get; }
        public string SecondName { set; get; }
        public string ThirdName { set; get; }
        public string LastName { set; get; }
        public string NationalNo { set; get; }
        public DateTime DateOfBirth { set; get; }
        public byte Gendor { set; get; }
        public string Phone { set; get; }
        public string Email { set; get; }
        public int NationalityCountryID { set; get; }
        public string Address { set; get; }
        public string ImagePath { set; get; }

        public clsPeopleManagement()
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

        private clsPeopleManagement(int PersonID, string FirstName, string SecondName, string ThirdName,
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

        public static DataTable GetPeople()
        {
            return clsPersonData.GetAllPeople();
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

    }
}
