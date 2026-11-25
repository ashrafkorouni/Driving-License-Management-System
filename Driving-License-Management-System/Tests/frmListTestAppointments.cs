using Driving_License_Management_System.Properties;
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
    public partial class frmListTestAppointments : Form
    {
        DataTable _dtLicenseTestAppointments;

        private int _LocalDrivingLicenseApplicationID;

        private clsTestType.enTestType _TestTypeID = clsTestType.enTestType.VisionTest;



        public frmListTestAppointments(int LocalDrivingLicenseApplicatioID, clsTestType.enTestType TestTypeID)
        {
            InitializeComponent();

            _LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicatioID;
            _TestTypeID = TestTypeID;
        }

        private void _LoadTestTypeImageAndTitle()
        {
            switch (_TestTypeID)
            {
                case clsTestType.enTestType.VisionTest:
                    {
                        lblTitle.Text = "Vision Test Appointments";
                        this.Text = lblTitle.Text;
                        pbTestTypeImage.Image = Resources.Vision_512;
                        break;
                    }
                case clsTestType.enTestType.WrittenTest:
                    {
                        lblTitle.Text = "Written Test Appointments";
                        this.Text = lblTitle.Text;
                        pbTestTypeImage.Image = Resources.Written_Test_512;
                        break;
                    }
                case clsTestType.enTestType.StreetTest:
                    {
                        lblTitle.Text = "Street Test Appointments";
                        this.Text = lblTitle.Text;
                        pbTestTypeImage.Image = Resources.driving_test_512;
                        break;
                    }
            }
        }

        private void frmListTestAppointments_Load(object sender, EventArgs e)
        {
            _LoadTestTypeImageAndTitle();

            ctrlDrivingLicenseApplicationInfo1.LoadApplicationInfoByLocalDrivingAppID(_LocalDrivingLicenseApplicationID);

            _dtLicenseTestAppointments = clsTestAppointment.GetApplicationTestAppointmentsPerTestType(_LocalDrivingLicenseApplicationID, _TestTypeID);

            dgvLicenseTestAppointments.DataSource = _dtLicenseTestAppointments;

            lblRecordsCount.Text = dgvLicenseTestAppointments.Rows.Count.ToString();

            if (dgvLicenseTestAppointments.Rows.Count > 0)
            {
                dgvLicenseTestAppointments.Columns[0].HeaderText = "Appointment ID";
                dgvLicenseTestAppointments.Columns[0].Width = 150;

                dgvLicenseTestAppointments.Columns[1].HeaderText = "Appointment Date";
                dgvLicenseTestAppointments.Columns[1].Width = 200;

                dgvLicenseTestAppointments.Columns[2].HeaderText = "Paid Fees";
                dgvLicenseTestAppointments.Columns[2].Width = 150;

                dgvLicenseTestAppointments.Columns[3].HeaderText = "Is Locked";
                dgvLicenseTestAppointments.Columns[3].Width = 100;
            }

        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int TestAppointmentID = (int)dgvLicenseTestAppointments.CurrentRow.Cells[0].Value;

            frmScheduleTest frm = new frmScheduleTest(_LocalDrivingLicenseApplicationID, _TestTypeID, TestAppointmentID);   

            frm.ShowDialog();

            frmListTestAppointments_Load(null, null);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAddNewAppointment_Click(object sender, EventArgs e)
        {
            clsLocalDrivingLicenseApplication LocalDrivingLicenseApplication =
                clsLocalDrivingLicenseApplication.FindByLocalDrivingApplicenseID(_LocalDrivingLicenseApplicationID);

            if (LocalDrivingLicenseApplication.IsThereAnActiveScheduleTest(_TestTypeID))
            {
                MessageBox.Show("Person Already have an active appointment for this test, You cannot add new appointment", "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            clsTest LastTest = LocalDrivingLicenseApplication.GetLastTestPerTestType(_TestTypeID);

            // That's mean there is not Test Appointment for this test type before.
            if (LastTest == null)
            {
                frmScheduleTest frm = new frmScheduleTest(_LocalDrivingLicenseApplicationID, _TestTypeID);
                frm.ShowDialog();
                frmListTestAppointments_Load(null, null);
                return;
            }

            // When the test Passed
            if (LastTest.TestResult == true)
            {
                MessageBox.Show("This person already passed this test before, you can only retake faild test", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            frmScheduleTest frm2 = new frmScheduleTest(_LocalDrivingLicenseApplicationID, _TestTypeID);
            frm2.ShowDialog();
            frmListTestAppointments_Load(null, null);
            


        }

        private void takeTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int TestAppointmentID = (int)dgvLicenseTestAppointments.CurrentRow.Cells[0].Value;
            frmTakeTest frm = new frmTakeTest(TestAppointmentID, _TestTypeID);
            frm.ShowDialog();
            frmListTestAppointments_Load(null, null);
        }
    }
}
