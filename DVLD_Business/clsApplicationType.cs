using DVLD_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Business
{
    public class clsApplicationType
    {
        enum enMode { AddNew = 0, Update = 1 }
        enMode Mode = enMode.AddNew;

        public int ApplicationTypeID { get; set; }
        public string Title { get; set; }
        public float Fees { get; set; }

        public clsApplicationType()
        {
            ApplicationTypeID = -1;
            Title = "";
            Fees = 0;

            Mode = enMode.AddNew;
        }

        private clsApplicationType(int ID, string Title, float Fees)
        {
            this.ApplicationTypeID = ID;
            this.Title = Title;
            this.Fees = Fees;

            Mode = enMode.Update;
        }

        public static DataTable GetAllApplicationTypes()
        {
            return clsApplicationTypeData.GetAllAplicationTypes();
        }

        private bool _AddNewApplicationType()
        {
            this.ApplicationTypeID = clsApplicationTypeData.AddNewApplicationType(this.Title, this.Fees);

            return (this.ApplicationTypeID != -1);
        }

        private bool _UpdateApplicationType()
        {
            return clsApplicationTypeData.UpdateApplicationType(this.ApplicationTypeID, this.Title, this.Fees);
        }
                
        public static clsApplicationType Find(int ApplicationTypeID)
        {
            string Title = ""; float Fees = 0;

            if (clsApplicationTypeData.GetApplicationTypeInfoByID(ApplicationTypeID, ref Title, ref Fees))
                
                return new clsApplicationType(ApplicationTypeID, Title, Fees);
            else
                return null;
        }
    
        public bool Save()
        {
            switch(Mode)
            {
                case enMode.AddNew:
                    if(_AddNewApplicationType())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return _UpdateApplicationType();
            }
            return false;
        }
    
    }
}
