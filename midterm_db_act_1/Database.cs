using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Windows.Forms;
using System.Data;
using System.Reflection;

namespace midterm_db_act_1
{
    public class Database
    {


        private string conn_str;
        private OleDbConnection connection;
        private OleDbDataAdapter adapter;
        private OleDbCommand command;


        public Database(string db_file_path)
        {
            this.conn_str = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + db_file_path;
            this.connection = new OleDbConnection(conn_str);
        }

        public OleDbConnection Connection { get { return connection; } }

        // Database Query functions


        public void search_by_keyword(string keyword, string brand, DataGridView grdResult)
        {
            string query = "SELECT id as ID, model as MODEL, brand as BRAND, year as [YEAR] from [car] WHERE model=? and brand=?";

            if (this.Connection.State != System.Data.ConnectionState.Open)
            {
                this.Connection.Open();
            }

            using (command = new OleDbCommand(query, this.Connection))
            {
                command.Parameters.AddWithValue("?", keyword);
                command.Parameters.AddWithValue("?", brand);

                using (adapter = new OleDbDataAdapter(command))
                {
                    using (DataTable dt = new DataTable())
                    {
                        adapter.Fill(dt);
                        grdResult.DataSource = dt;

                        // Hide the ID column after binding (optional)
                        if (grdResult.Columns.Contains("ID")) // Check if column exists
                        {
                            grdResult.Columns["ID"].Visible = false;
                        }

                        this.Connection.Close();
                    }
                }
            }
            
        }


        public void search_by_keyword_change(string keyword, string brand, DataGridView grdResult)
        {
            string query = "SELECT id as ID, model as MODEL, brand as BRAND, year as [YEAR] from [car] WHERE model like ? and brand=?";

            if (this.Connection.State != System.Data.ConnectionState.Open)
            {
                this.Connection.Open();
            }

            using (command = new OleDbCommand(query, this.Connection))
            {
                command.Parameters.AddWithValue("?", keyword + "%");
                command.Parameters.AddWithValue("?", brand);

                using (adapter = new OleDbDataAdapter(command))
                {
                    using (DataTable dt = new DataTable())
                    {
                        adapter.Fill(dt);
                        grdResult.DataSource = dt;

                        // Hide the ID column after binding (optional)
                        if (grdResult.Columns.Contains("ID")) // Check if column exists
                        {
                            grdResult.Columns["ID"].Visible = false;
                        }

                        this.Connection.Close();
                    }
                }
            }
        }
        public void view_cars(DataGridView grdResult)
        {
            string query = "SELECT id as ID, model as MODEL, brand as BRAND, year as [YEAR] from [car]";

            if (this.Connection.State != System.Data.ConnectionState.Open)
            {
                this.Connection.Open();
            }

            using (adapter = new OleDbDataAdapter(query, this.Connection))
            {
                using (DataTable dt = new DataTable())
                {
                    adapter.Fill(dt);
                    grdResult.DataSource = dt;

                    // Hide the ID column after binding (optional)
                    if (grdResult.Columns.Contains("ID")) // Check if column exists
                    {
                        grdResult.Columns["ID"].Visible = false;
                    }

                    this.Connection.Close();
                }
            }


        }

        public void insert_car(int id, string model, string brand, int year)
        {
            string query = "INSERT INTO [car] VALUES(?, ?, ?, ?)";

            if (this.Connection.State != System.Data.ConnectionState.Open)
            {
                this.Connection.Open();
            }

            using (command = new OleDbCommand(query, this.Connection))
            {
                command.Parameters.AddWithValue("?", id);
                command.Parameters.AddWithValue("?", model);
                command.Parameters.AddWithValue("?", brand);
                command.Parameters.AddWithValue("?", year);


                int res = command.ExecuteNonQuery();

                if (res > 0)
                {
                    MessageBox.Show("Added to the Record", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                } else
                {
                    MessageBox.Show("Failed to Add to the Record", "Success", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                this.Connection.Close();
            }

        }

        public void update_car(int id, string model, string brand, int year)
        {
            string query = "UPDATE [car] SET model=?, brand=?, [year]=? WHERE id=?;";

            if (this.Connection.State != ConnectionState.Open)
            {
                this.Connection.Open();
            }

            using (command = new OleDbCommand(query, this.Connection))
            {
                command.Parameters.AddWithValue("?", model);
                command.Parameters.AddWithValue("?", brand);
                command.Parameters.AddWithValue("?", year);
                command.Parameters.AddWithValue("?", id);


                int res = command.ExecuteNonQuery();

                if (res > 0)
                {
                    MessageBox.Show("Selected Record has been updated Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Failed to update the Selected Record", "Success", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        }
        
        public void delete_car(int id, string brand) 
        {
            string query = "DELETE from [car] WHERE id=? and brand=?";

            if (this.Connection.State != ConnectionState.Open)
            {
                this.Connection.Open();
            }

            using (command = new OleDbCommand(query, this.Connection))
            {
                command.Parameters.AddWithValue("?", id);
                command.Parameters.AddWithValue("?", brand);
                


                int res = command.ExecuteNonQuery();

                if (res > 0)
                {
                    MessageBox.Show("Selected Record has been deleted Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Failed to delete the Selected Record", "Success", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        }


    }
}
 