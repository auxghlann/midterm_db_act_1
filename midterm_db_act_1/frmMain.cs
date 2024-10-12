using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Reflection;

namespace midterm_db_act_1
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        // class attr
        private string db_file_path = "";
        private string brands_file_path = "";
        private Database _db;

        private string selected_model, selected_brand = "";
        private int selected_id, selected_year = 0;

        // Helper functions

        public void InitializeBrandsCBO(string brands_file_path)
        {
            string[] lines = File.ReadAllLines(brands_file_path);

            foreach (string line in lines)
            {
                cboBrand.Items.Add(line);
                cboNewBrand.Items.Add(line);
                cboToDelBrand.Items.Add(line);
            }

        }


        // -- WInForm Functions --


        // ToolStrips
        private void toolStripImportData_Click(object sender, EventArgs e)
        {

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "*.mdb|";

            if (DialogResult.OK == ofd.ShowDialog())
            {
                this.db_file_path = ofd.FileName;
                this._db = new Database(db_file_path);
                MessageBox.Show("Data Successfully Imported", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }


        }


        private void toolStripCarBrands_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "*.txt|";

            if (DialogResult.OK == ofd.ShowDialog())
            {
                this.brands_file_path = ofd.FileName;
                this._db = new Database(db_file_path);
                InitializeBrandsCBO(brands_file_path);
                MessageBox.Show("Car Brands Successfully Imported", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        // DataGridView
        private void grdResult_SelectionChanged(object sender, EventArgs e)
        {
            if (grdResult.SelectedRows.Count > 0)
            {

                var selected_row = grdResult.SelectedRows[0].DataBoundItem as DataRowView;

                if (selected_row != null)
                {

                    this.selected_id = Convert.ToInt32(selected_row["id"]);
                    this.selected_model = selected_row["model"].ToString();
                    this.selected_brand = selected_row["brand"].ToString();
                    this.selected_year = Convert.ToInt32(selected_row["year"]);

                    txtNewID.Text = selected_id.ToString();
                    txtNewModel.Text = selected_model;
                    cboNewBrand.Text = selected_brand;
                    txtNewYear.Text = selected_year.ToString();
                }

            }
        }

        // Buttons

        private void btnView_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(db_file_path))
            {
                MessageBox.Show("No data to view. Import a database first", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _db.view_cars(grdResult);
            
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(db_file_path))
            {
                MessageBox.Show("Missing Database. Import a database first", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            if (string.IsNullOrEmpty(txtNewID.Text) || string.IsNullOrEmpty(txtNewModel.Text) ||
                string.IsNullOrEmpty(txtNewYear.Text) || string.IsNullOrEmpty(cboNewBrand.Text))
            {
                MessageBox.Show("Input fields should not be empty", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _db.insert_car(Convert.ToInt32(txtNewID.Text), txtNewModel.Text, cboNewBrand.Text, Convert.ToInt32(txtNewYear.Text));

            txtNewID.Clear();
            txtNewModel.Clear();
            txtNewYear.Clear();
            cboNewBrand.SelectedIndex = -1;
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            txtKeyword.Clear();
            cboBrand.SelectedIndex = -1;
            grdResult.DataSource = null;

            txtNewID.Clear();
            txtNewModel.Clear();
            cboNewBrand.SelectedIndex = -1;
            txtNewYear.Clear();

            txtToDel_ID.Clear();
            cboToDelBrand.SelectedIndex = -1;

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(db_file_path))
            {
                MessageBox.Show("No data to view. Import a database first", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(txtKeyword.Text) || string.IsNullOrEmpty(cboBrand.Text))
            {
                MessageBox.Show("Keyword and Brand must not be empty", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _db.search_by_keyword(txtKeyword.Text, cboBrand.Text, grdResult);
        }

        private void txtKeyword_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(db_file_path))
            {
                MessageBox.Show("No data to view. Import a database first", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(cboBrand.Text))
            {
                MessageBox.Show("Brand must not be empty", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _db.search_by_keyword_change(txtKeyword.Text, cboBrand.Text, grdResult);
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(db_file_path))
            {
                MessageBox.Show("No data to view. Import a database first", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            if (string.IsNullOrEmpty(txtNewID.Text) || string.IsNullOrEmpty(txtNewModel.Text) ||
                string.IsNullOrEmpty(txtNewYear.Text) || string.IsNullOrEmpty(cboNewBrand.Text))
            {
                MessageBox.Show("Input fields should not be empty", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _db.update_car(Convert.ToInt32(txtNewID.Text), txtNewModel.Text, cboNewBrand.Text, Convert.ToInt32(txtNewYear.Text));

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(db_file_path))
            {
                MessageBox.Show("No data to view. Import a database first", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            if (string.IsNullOrEmpty(txtToDel_ID.Text) || string.IsNullOrEmpty(cboToDelBrand.Text))
            {
                MessageBox.Show("Input fields should not be empty", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            DialogResult dg = MessageBox.Show("Are you sure?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);    


            if (dg == DialogResult.Yes)
            {
                _db.delete_car(Convert.ToInt32(txtToDel_ID.Text), cboToDelBrand.Text);
            }
            
        }
    }
}
