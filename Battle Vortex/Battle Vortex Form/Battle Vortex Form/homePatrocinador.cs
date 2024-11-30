using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Battle_Vortex_Form
{
    public partial class homePatrocinador : Form
    {
        public homePatrocinador()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            patrocinadoresPat  patrocinadoresUser = new patrocinadoresPat();
            patrocinadoresUser.Show();
            this.Close();

        }

        private void button8_Click(object sender, EventArgs e)
        {

        }
    }
}
