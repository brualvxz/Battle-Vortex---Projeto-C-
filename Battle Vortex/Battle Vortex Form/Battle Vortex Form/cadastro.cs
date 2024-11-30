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
    public partial class cadastro : Form
    {
        public cadastro()
        {
            InitializeComponent();

            textBox3.UseSystemPasswordChar = true;
            textBox4.UseSystemPasswordChar = true;

            textBox3.PasswordChar = '*';

            textBox4.PasswordChar = '*';

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
    
            string nomeUsuario = textBox1.Text;
            string email = textBox2.Text;
            string senha = textBox3.Text;
            string confirmarSenha = textBox4.Text;

            
            if (senha != confirmarSenha)
            {
                MessageBox.Show("As senhas não coincidem. Tente novamente.", "Erro de Senha", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            
            if (!validarEmail(email))
            {
                MessageBox.Show("O e-mail fornecido não é válido. Tente novamente.", "Erro de E-mail", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            
            string connectionString = "SERVER=127.0.0.1; DATABASE=eventosbv; UID=root; PASSWORD=;";

            using (MySqlConnection conexao = new MySqlConnection(connectionString))
            {
                try
                {
                    conexao.Open();

                    
                    string query = "SELECT COUNT(*) FROM usuarios WHERE nome = @nomeUsuario OR email = @email";
                    using (MySqlCommand comando = new MySqlCommand(query, conexao))
                    {
                        comando.Parameters.AddWithValue("@nomeUsuario", nomeUsuario);
                        comando.Parameters.AddWithValue("@email", email);

                        int count = Convert.ToInt32(comando.ExecuteScalar());

                        if (count > 0)
                        {
                            // Caso já exista o nome de usuário ou e-mail
                            MessageBox.Show("Nome de usuário ou e-mail já existe. Tente outro.", "Erro de Cadastro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    // Caso não exista, realizar o cadastro
                    string insertQuery = "INSERT INTO usuarios (nome, email, senha) VALUES (@nomeUsuario, @email, @senha)";
                    using (MySqlCommand comandoInsert = new MySqlCommand(insertQuery, conexao))
                    {
                        comandoInsert.Parameters.AddWithValue("@nomeUsuario", nomeUsuario);
                        comandoInsert.Parameters.AddWithValue("@email", email);
                        comandoInsert.Parameters.AddWithValue("@senha", senha);  // Senha não deve ser armazenada em texto simples, idealmente seria uma versão criptografada

                        int result = comandoInsert.ExecuteNonQuery();

                        if (result > 0)
                        {
                            MessageBox.Show("Cadastro realizado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            
                            textBox1.Clear();
                            textBox2.Clear();
                            textBox3.Clear();
                            textBox4.Clear();
                        }
                        else
                        {
                            MessageBox.Show("Erro ao realizar o cadastro. Tente novamente.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erro ao conectar ao banco de dados: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Função para validar o e-mail
        private bool validarEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
            Login Login = new Login();
            Login.Show();
            this.Hide();
        }

        private void cadastro_Load(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
