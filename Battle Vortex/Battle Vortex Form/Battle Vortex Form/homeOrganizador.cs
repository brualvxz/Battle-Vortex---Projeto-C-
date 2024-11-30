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
    public partial class homeOrganizador : Form
    {
     
        public homeOrganizador()
        {
            InitializeComponent();
        }


         private void homeOrganizador_Load(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            torneioOrganizador torneioOrganizador = new torneioOrganizador();
            torneioOrganizador.Show();
            this.Close();
        }

       

        private void button9_Click(object sender, EventArgs e)
        {
            perfil perfil = new perfil();
            perfil.Show();
            this.Close();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            classificacaoUser classificacaoUser = new classificacaoUser();
            classificacaoUser.Show();
            this.Close();
        }

        private void button6_Click(object sender, EventArgs e)
        {

        }


        private void button1_Click(object sender, EventArgs e)
        {

            patrocinadoresUser patrocinadoresUser = new patrocinadoresUser();
            patrocinadoresUser.Show();
            this.Close();
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

        private void button2_Click(object sender, EventArgs e)
        {
            premioOrganizador premioOrganizador = new premioOrganizador();
            premioOrganizador.ShowDialog();


        }
    }
}
