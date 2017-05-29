﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BookStore.Report {
    public partial class frmReport : Form {
        public frmReport() {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
        }

        private void frmReport_Load(object sender, EventArgs e) {
            // TODO: This line of code loads data into the 'DataSet1.SACH' table. You can move, or remove it, as needed.
            this.SACHTableAdapter.Fill(this.DataSet1.SACH);

            this.reportViewer1.RefreshReport();
        }
    }
}
