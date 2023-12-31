﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IQ_Test
{
    public partial class frm_leaderbord : Form
    {
        private const string ConnectionString = "Data Source=DESKTOP-0SV48E5;Initial Catalog=IQ_Test;Integrated Security=True";
        public frm_leaderbord()
        {
            InitializeComponent();
            LoadTopPlayers();
        }

        private void LoadTopPlayers()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                string selectQuery = "SELECT TOP 10 Name, Points FROM Leaderboard ORDER BY Points DESC";
                SqlDataAdapter adapter = new SqlDataAdapter(selectQuery, connection);
                DataTable DataTable = new DataTable();
                adapter.Fill(DataTable);

                dataGridView1.DataSource = DataTable;
            }
        }
    }
}
