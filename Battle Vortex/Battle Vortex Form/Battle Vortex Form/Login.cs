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
            textBox2.UseSystemPasswordChar = true;
            textBox2.PasswordChar = '*';
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

                    // Consulta para verificar o usuário e obter suas informações
                    string query = "SELECT id, nome, email, tipo FROM usuarios " +
                                   "WHERE (nome = @usuarioOuEmail OR email = @usuarioOuEmail) AND senha = @senha AND status = 'Ativo'";
                    using (MySqlCommand comando = new MySqlCommand(query, conexao))
                    {
                        comando.Parameters.AddWithValue("@usuarioOuEmail", usuarioOuEmail);
                        comando.Parameters.AddWithValue("@senha", senha);

                        using (MySqlDataReader reader = comando.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Captura as informações do usuário
                                UsuarioLogado.Id = reader.GetInt32("id");
                                UsuarioLogado.Nome = reader.GetString("nome");
                                UsuarioLogado.Email = reader.GetString("email");
                                string tipo = reader.GetString("tipo");

                                MessageBox.Show($"Bem-vindo, {UsuarioLogado.Nome}!");

                                // Redireciona para a tela correspondente ao tipo de usuário
                                if (tipo.Equals("Administrador", StringComparison.OrdinalIgnoreCase))
                                {
                                    homeAdm homeadm = new homeAdm();
                                    homeadm.Show();
                                }
                                else if (tipo.Equals("Usuário", StringComparison.OrdinalIgnoreCase))
                                {
                                    homeUser homeuser = new homeUser();
                                    homeuser.Show();
                                }
                                else if (tipo.Equals("Patrocinador", StringComparison.OrdinalIgnoreCase))
                                {
                                    homePatrocinador homePat = new homePatrocinador();
                                    homePat.Show();
                                }
                                else if (tipo.Equals("Organizador", StringComparison.OrdinalIgnoreCase))
                                {
                                    homeOrganizador homeOrg = new homeOrganizador();
                                    homeOrg.Show();
                                }
                                else
                                {
                                    MessageBox.Show("Tipo de usuário inválido.", "Erro de Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }

                                this.Hide(); // Esconde a tela de login
                            }
                            else
                            {
                                MessageBox.Show("Nome de usuário, e-mail ou senha incorretos.", "Erro de Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erro ao conectar ao banco de dados: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }
    }

    // Classe estática para armazenar os dados do usuário logado
    public static class UsuarioLogado
    {
        public static int Id { get; set; }
        public static string Nome { get; set; }
        public static string Email { get; set; }

        public static void Limpar()
        {
            Id = 0;
            Nome = null;
            Email = null;
        }
    }
}
