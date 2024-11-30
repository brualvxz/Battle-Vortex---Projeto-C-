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
using System.IO;

namespace Battle_Vortex_Form
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void Login_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {
            cadastro cadUser = new cadastro();
            cadUser.Show();
            this.Hide();
        }


        private void button1_Click_1(object sender, EventArgs e)
        {
            string connectionString = "SERVER=127.0.0.1; DATABASE=eventosbv; UID=root; PASSWORD=;";
            string usuarioOuEmail = textBox1.Text; 
            string senha = textBox2.Text; 

            using (MySqlConnection conexao = new MySqlConnection(connectionString))
            {
                try
                {
                    conexao.Open();

                    // Consulta para verificar se o usuário existe no banco e obter o tipo de usuário (Admin ou Usuário)
                    string query = "SELECT tipo FROM usuarios WHERE (nome = @usuarioOuEmail OR email = @usuarioOuEmail) AND senha = @senha";
                    using (MySqlCommand comando = new MySqlCommand(query, conexao))
                    {
                        // Parâmetros para prevenir SQL Injection
                        comando.Parameters.AddWithValue("@usuarioOuEmail", usuarioOuEmail);
                        comando.Parameters.AddWithValue("@senha", senha);

                        // Executar a consulta e obter o tipo de usuário
                        object tipoUsuario = comando.ExecuteScalar();

                        if (tipoUsuario != null)
                        {
                            // Verifica o tipo do usuário no banco de dados
                            string tipo = tipoUsuario.ToString();

                            if (tipo.Equals("Administrador", StringComparison.OrdinalIgnoreCase))
                            {
                                // Se o tipo for "Administrador", abre a tela do administrador
                                homeAdm homeadm = new homeAdm();
                                homeadm.Show();
                                this.Hide(); // Esconde a tela de login
                            }
                            else if (tipo.Equals("Usuário", StringComparison.OrdinalIgnoreCase))
                            {
                                // Se o tipo for "Usuario", abre a tela do usuário
                                homeUser homeuser = new homeUser();
                                homeuser.Show();
                                this.Hide(); // Esconde a tela de login
                            }
                            else
                            {
                                // Tipo desconhecido
                                MessageBox.Show("Tipo de usuário inválido.", "Erro de Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            // Usuário não encontrado ou senha incorreta
                            MessageBox.Show("Nome de usuário, e-mail ou senha incorretos.", "Erro de Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Tratamento de erros ao conectar ao banco de dados
                    MessageBox.Show($"Erro ao conectar ao banco de dados: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
    
}
