using Driving_License_Management_System.GlobelClasses;
using DVLD_Business;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Driving_License_Management_System.Applications.Local_Driving_License
{
    public partial class frmAddUpdateLocalDrivingLicesnseApplication : Form
    {
        public enum enMode { AddNew = 0, Update = 1 }

        private enMode _Mode;

        private int _LocalDrivingLicenseID = -1;

        private int _SelectedPersonID = -1;

        clsLocalDrivingLicenseApplication _LocalDrivingLicenseApplication;

        public frmAddUpdateLocalDrivingLicesnseApplication()
        {
            InitializeComponent();
            _Mode = enMode.AddNew;
        }

        public frmAddUpdateLocalDrivingLicesnseApplication(int LocalDrivingLicenseID)
        {
            InitializeComponent();

            _LocalDrivingLicenseID = LocalDrivingLicenseID;
            _Mode = enMode.Update;
        }

        private void _FillLicenseClassesInComboBox()
        {
            DataTable dtLicenseClasses = clsLicenseClass.GetAllLicenseClasses();


            foreach (DataRow row in dtLicenseClasses.Rows)
            {
                cbLicenseClass.Items.Add(row["ClassName"]);
            }
        }

        private void _ResetDefaultValue()
        {
            _FillLicenseClassesInComboBox();

            if (_Mode == enMode.AddNew)
            {
                lblTitle.Text = "New Local Driving License Application";
                this.Text = "New Local Driving License Application";
                _LocalDrivingLicenseApplication = new clsLocalDrivingLicenseApplication();
                ctrlPersonCardWithFilter1.FilterFocus();
                tpApplicationInfo.Enabled = false;

                cbLicenseClass.SelectedIndex = 2;
                lblFees.Text = clsApplicationType.Find((int)clsApplication.enApplicationType.NewDrivingLicense).Fees.ToString();
                lblApplicationDate.Text = DateTime.Now.ToShortDateString();
                lblCreatedByUser.Text = clsGlobal.CurrentUser.UserName;
            }
            else
            {
                lblTitle.Text = "Update Local Driving License Application";
                this.Text = "Update Local Driving License Application";

                tpApplicationInfo.Enabled = true;
                btnSave.Enabled = true;
            }
        }

        private void _LoadData()
        {
            ctrlPersonCardWithFilter1.FilterEnabled = false;
            _LocalDrivingLicenseApplication = clsLocalDrivingLicenseApplication.FindByLocalDrivingApplicanseID(_LocalDrivingLicenseID);

            if (_LocalDrivingLicenseApplication == null)
            {
                MessageBox.Show("No Application with ID = " + _LocalDrivingLicenseID, "Application Not Found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.Close();

                return;
            }

            ctrlPersonCardWithFilter1.LoadPersonInfo(_LocalDrivingLicenseApplication.ApplicantPersonID);
            lblLocalDrivingLicebseApplicationID.Text = _LocalDrivingLicenseApplication.LocalDrivingLicenseApplicationID.ToString();
            lblApplicationDate.Text = clsFormat.DateToShort(_LocalDrivingLicenseApplication.ApplicationDate);
            cbLicenseClass.SelectedIndex = cbLicenseClass.FindString(clsLicenseClass.Find(_LocalDrivingLicenseApplication.LicenseClassID).ClassName);
            lblFees.Text = _LocalDrivingLicenseApplication.PaidFees.ToString();
            lblCreatedByUser.Text = clsUser.FindByUserID(_LocalDrivingLicenseApplication.CreatedByUserID).UserName;
        }

        private void DataBackEvent(object sender, int PersonID)
        {
            // Handle the data received
            _SelectedPersonID = PersonID;
            ctrlPersonCardWithFilter1.LoadPersonInfo(PersonID);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmAddUpdateLocalDrivingLicesnseApplication_Load(object sender, EventArgs e)
        {
            _ResetDefaultValue();

            if (_Mode == enMode.Update)
            {
                _LoadData();
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (_Mode == enMode.Update)
            {
                btnSave.Enabled = true;
                tpApplicationInfo.Enabled = true;

                tcApplicationInfo.SelectedTab = tcApplicationInfo.TabPages["tpApplicationInfo"];
                return;
            }

            if (ctrlPersonCardWithFilter1.PersonID != -1)
            {
                btnSave.Enabled = true;
                tpApplicationInfo.Enabled = true;
                tcApplicationInfo.SelectedTab = tcApplicationInfo.TabPages["tpApplicationInfo"];
            }

            else
            {
                MessageBox.Show("Please Select a Person", "Select a Person", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ctrlPersonCardWithFilter1.FilterFocus();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            clsLicenseClass licenseClass = clsLicenseClass.Find(cbLicenseClass.Text);
            if (licenseClass == null) 
                return;

            int licenseClassID = licenseClass.LicenseClassID;
            int personID = ctrlPersonCardWithFilter1.PersonID;

            if (_Mode == enMode.AddNew)
            {
                int activeApplicationID = clsApplication.GetActiveApplicationIDForLicenseClass(
                    personID, clsApplication.enApplicationType.NewDrivingLicense, licenseClassID);

                if (activeApplicationID != -1)
                {
                    MessageBox.Show("Choose another License Class, the selected Person Already have an active application" +
                        " for the selected class with id=" + activeApplicationID, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    cbLicenseClass.Focus();
                    return;
                }

                if (clsLicense.IsLicenseExistByPersonID(personID, licenseClassID))
                {
                    MessageBox.Show("Person already has a license of this class", "Not allowed",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                _LocalDrivingLicenseApplication.ApplicationDate = DateTime.Now;
                _LocalDrivingLicenseApplication.ApplicationStatus = clsApplication.enApplicationStatus.New;
                _LocalDrivingLicenseApplication.CreatedByUserID = clsGlobal.CurrentUser.UserID;
            }

            _LocalDrivingLicenseApplication.ApplicantPersonID = personID;
            _LocalDrivingLicenseApplication.ApplicationTypeID = (int)clsApplication.enApplicationType.NewDrivingLicense;
            _LocalDrivingLicenseApplication.LastStatusDate = DateTime.Now;
            _LocalDrivingLicenseApplication.PaidFees = Convert.ToSingle(lblFees.Text);
            _LocalDrivingLicenseApplication.LicenseClassID = licenseClassID;

            if (_LocalDrivingLicenseApplication.Save())
            {
                lblLocalDrivingLicebseApplicationID.Text =
                    _LocalDrivingLicenseApplication.LocalDrivingLicenseApplicationID.ToString();
                _Mode = enMode.Update;
                lblTitle.Text = "Update Local Driving License Application";
                MessageBox.Show("Data Saved Successfully.", "Saved",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
                MessageBox.Show("Error: Data Is not Saved Successfully.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void frmAddUpdateLocalDrivingLicesnseApplication_Activated(object sender, EventArgs e)
        {
            ctrlPersonCardWithFilter1.FilterFocus();
        }

        private void ctrlPersonCardWithFilter1_OnPersonSelected(int obj)
        {
            _SelectedPersonID = obj;
        }
    }
}

