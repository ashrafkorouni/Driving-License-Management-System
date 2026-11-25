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

namespace Driving_License_Management_System.Tests
{
    public partial class frmTakeTest : Form
    {
        private int _TestAppointmentID = -1;
        private clsTestType.enTestType _TestTypeID;
        
        private clsTest _Test;



        public frmTakeTest(int TestAppointmentID, clsTestType.enTestType TestTypeID)
        {
            InitializeComponent();
            _TestAppointmentID = TestAppointmentID;
            _TestTypeID = TestTypeID;
        }

        private void frmTakeTest_Load(object sender, EventArgs e)
        {
            ctrlScheduledTest1.TestTypeID = _TestTypeID;

            ctrlScheduledTest1.LoadData(_TestAppointmentID);

            if (ctrlScheduledTest1.TestAppointmentID == -1)
            {
                btnSave.Enabled = false;
            }
            else
            {
                btnSave.Enabled = true;
            }

            int TestID = ctrlScheduledTest1.TestID;
            if (TestID != -1)
            {
                _Test = clsTest.Find(TestID);
                if (_Test.TestResult)
                {
                    rbPass.Checked = true;
                }
                else
                {
                    rbFail.Checked = true;
                }
                txtNotes.Text = _Test.Notes;
                btnSave.Enabled = false;
                lblUserMessage.Visible = true;
                rbFail.Enabled = false;
                rbPass.Enabled = false;
            }
            else
            {
                _Test = new clsTest();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to save? After that you cannot change the Pass/Fail results after you save?.",
                    "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No
           )
            {
                return;
            }

            _Test.TestAppointmentID = _TestAppointmentID;
            _Test.TestResult = rbPass.Checked;
            _Test.Notes = txtNotes.Text.Trim();
            _Test.CreatedByUserID = clsGlobal.CurrentUser.UserID;

            if (_Test.Save())
            {
                MessageBox.Show("Data Saved Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnSave.Enabled = false;

            }
            else
                MessageBox.Show("Error: Data Is not Saved Successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
