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
    public partial class perfil : Form
    {
        public perfil()
        {
            InitializeComponent();
            // Verificar se o usuário está logado
            if (UsuarioLogado.Id != 0)
            {
                // Carregar as informações do usuário logado no banco de dados
                using (MySqlConnection conexao = new MySqlConnection("SERVER=localhost;DATABASE=eventosbv;UID=root;PASSWORD="))
                {
                    conexao.Open();

                    // Consulta para preencher os campos do jogador
                    MySqlCommand consulta = new MySqlCommand("SELECT * FROM usuarios WHERE id = @id", conexao);
                    consulta.Parameters.AddWithValue("@id", UsuarioLogado.Id);
                    MySqlDataReader resultado = consulta.ExecuteReader();

                    if (resultado.HasRows)
                    {
                        while (resultado.Read())
                        {
                            textBox1.Text = resultado["nome"].ToString(); // Nome do jogador
                            textBox2.Text = resultado["email"].ToString(); // Email do jogador (assumindo que tem essa coluna na tabela)
                        }
                    }
                    else
                    {
                        MessageBox.Show("Nenhum registro encontrado.");
                    }

                    resultado.Close();
                }
            }
            else
            {
                MessageBox.Show("Usuário não está logado.");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            perfilAlterar perfilAlterar = new perfilAlterar();
            perfilAlterar.ShowDialog();

            InitializeComponent();
        }

        private void perfil_Load(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            UsuarioLogado.Limpar();
            Login login = new Login();
            login.Show();

            this.Close();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            homeUser homeUser = new homeUser();
            homeUser.Show();
        }
    }
}
