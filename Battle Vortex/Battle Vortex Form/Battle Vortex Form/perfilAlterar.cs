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
    public partial class perfilAlterar : Form
    {
        public perfilAlterar()
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
                            textBox3.Text = resultado["senha"].ToString();
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

        private void perfilAlterar_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // Conexão com o banco de dados eventosbv
                using (MySqlConnection conexao = new MySqlConnection("SERVER=localhost;DATABASE=eventosbv;UID=root;PASSWORD="))
                {
                    conexao.Open();

                    MySqlCommand comando = new MySqlCommand(
                        @"UPDATE usuarios 
                    SET nome = @nome, email = @email, senha = @senha 
                    WHERE id = @id", conexao);

                    comando.Parameters.AddWithValue("@nome", textBox1.Text);  // Novo nome
                    comando.Parameters.AddWithValue("@email", textBox2.Text); // Novo email
                    comando.Parameters.AddWithValue("@senha", textBox3.Text); // Nova senha
                    comando.Parameters.AddWithValue("@id", UsuarioLogado.Id); // ID do usuário logado

                    int linhasAfetadas = comando.ExecuteNonQuery();

                    if (linhasAfetadas > 0)
                    {
                        MessageBox.Show("Dados atualizados com sucesso!");
                    }
                    else
                    {
                        MessageBox.Show("Nenhuma alteração foi realizada.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao atualizar os dados: {ex.Message}");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();   
        }
    }
}
