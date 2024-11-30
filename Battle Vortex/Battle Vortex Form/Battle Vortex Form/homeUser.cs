using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using MySql.Data.MySqlClient;
using System.IO;

namespace Battle_Vortex_Form
{
    public partial class homeUser : Form
    {
        public homeUser()
        {
            InitializeComponent();

        }

        private void button4_Click(object sender, EventArgs e)
        {
            jogadoresUser jogadoresUser = new jogadoresUser();
            jogadoresUser.Show();
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            equipesUser equipesUser = new equipesUser();
            equipesUser.Show();
            this.Close();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            perfil perfil = new perfil();
            perfil.Show();
            this.Close();
      
        }

        private void button5_Click(object sender, EventArgs e)
        {
            torneioUser torneioUser = new torneioUser();
            torneioUser.Show();
            this.Close();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            classificacaoUser classificacaoUser = new classificacaoUser();
            classificacaoUser.Show();
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            patrocinadoresUser patrocinadoresUser = new patrocinadoresUser();
            patrocinadoresUser.Show();  
            this.Close();
        }
    }
}
