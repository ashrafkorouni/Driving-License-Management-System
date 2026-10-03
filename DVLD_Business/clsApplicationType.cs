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
        enum enMode { AddNew = 0, Update = 1}
        enMode Mode = enMode.AddNew;

        public int ApplicationTypeID { get; set; }
        public string Title {  get; set; }
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

    }
}
