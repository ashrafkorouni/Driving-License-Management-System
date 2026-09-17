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

namespace Driving_License_Management_System
{
    public partial class frmManagePeople : Form
    {
        public frmManagePeople()
        {
            InitializeComponent();
        }

        private void _RefershPeopleList()
        {
            dgvPeople.DataSource = clsPerson.GetPeople();
            lblRecords.Text = dgvPeople.RowCount.ToString();
        }


        private void frmManagePeople_Load(object sender, EventArgs e)
        {
            _RefershPeopleList();
            
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAddNewPerson_Click(object sender, EventArgs e)
        {
            frmAddUpdatePerson frm = new frmAddUpdatePerson();
            frm.ShowDialog();
            _RefershPeopleList();
        }
    }
}
