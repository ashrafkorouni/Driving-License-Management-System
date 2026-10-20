using DVLD_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Business
{
    public class clsLicenseClass
    {

        public enum enMode { AddNew = 0, Update = 1}
        
        public enMode Mode = enMode.AddNew;

        public int LicenseClassID { set; get; }
        public string ClassName { set; get; }
        public string ClassDescription { set; get; }
        public byte MinimumAllowedAge { set; get; }
        public byte DefaultValidityLength { set; get; }
        public float ClassFees { set; get; }

        public clsLicenseClass()
        {
            this.LicenseClassID = -1;
            this.ClassName = "";
            this.ClassDescription = "";
            this.MinimumAllowedAge = 18;
            this.DefaultValidityLength = 5;
            this.ClassFees = 15;

            Mode = enMode.AddNew;
        }

        private clsLicenseClass(int LicenseClassID, string ClassName, string ClassDescription,
            byte MinimumAllowedAge, byte DefaultValidityLength, float ClassFees)
        {
            this.LicenseClassID = LicenseClassID;
            this.ClassName = ClassName;
            this.ClassDescription = ClassDescription; 
            this.MinimumAllowedAge = MinimumAllowedAge;
            this.DefaultValidityLength = DefaultValidityLength;
            this.ClassFees = ClassFees;

            Mode = enMode.Update;
        }

        public static DataTable GetAllLicenseClasses()
        {
            return clsLicenseClassData.GetAllLicenseClasses();
        }

        public clsLicenseClass Find(int LicenseClassID)
        {
            string ClassName = "", ClassDescription = "";
            byte MinimumAllowedAge = 18, DefaultValidityLength = 5;
            float ClassFees = 15;

            bool isFound = clsLicenseClassData.GetLicenseClassInfoByID(LicenseClassID, ref ClassName
                , ref ClassDescription, ref MinimumAllowedAge, ref DefaultValidityLength, ref ClassFees);

            if(isFound)
            {
                return new clsLicenseClass(LicenseClassID, ClassName, ClassDescription
                    , MinimumAllowedAge, DefaultValidityLength , ClassFees);
            }
            else
            {
                return null;
            }
        }

        public clsLicenseClass Find(string ClassName)
        {
            int LicenseClassID = -1;
            string ClassDescription = "";
            byte MinimumAllowedAge = 18, DefaultValidityLength = 5;
            float ClassFees = 15;


            bool isFound = clsLicenseClassData.GetLicenseClassInfoByClassName(ClassName, ref LicenseClassID,
                ref ClassDescription, ref MinimumAllowedAge, ref DefaultValidityLength, ref ClassFees);

            if (isFound)
            {
                return new clsLicenseClass(LicenseClassID, ClassName, ClassDescription,
                    MinimumAllowedAge, DefaultValidityLength, ClassFees);
            }
            else
            {
                return null;
            }
        }

    }
}
