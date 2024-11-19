using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Battle_Vortex_Form
{
    public partial class homeAdm : Form
    {
        public homeAdm()
        {
            InitializeComponent();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            jogadoresAdm jogadoresAdm = new jogadoresAdm();
            jogadoresAdm.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            patrocinadoresAdm pat = new patrocinadoresAdm();
            pat.Show();
            this.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            torneioAdm torneio = new torneioAdm(); 
            torneio.Show();
            this.Close();

        }
    }
}
