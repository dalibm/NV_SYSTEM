using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NV_SYSTEM.BROWSER
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Etes-vous sûr de vouloir quitter l'application?", "Confirmation", MessageBoxButtons.YesNo);
           
            if (result == DialogResult.Yes)
            {

                this.Dispose(true);
            }
            else
            {
                //...
            }
        }
    }
}
