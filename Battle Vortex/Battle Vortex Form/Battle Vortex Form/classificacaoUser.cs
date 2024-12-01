using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace Battle_Vortex_Form
{
    public partial class classificacaoUser : Form
    {
        public classificacaoUser()
        {
            InitializeComponent();
            CarregarDados();
        }

        private void CarregarDados()
        {
            using (MySqlConnection conexao = new MySqlConnection("SERVER=127.0.0.1; DATABASE=eventosbv; UID=root; PASSWORD=;"))
            {
                conexao.Open();


                MySqlCommand consultaColunas = new MySqlCommand("SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA = 'eventosbv' AND TABLE_NAME = 'view_rankings_com_nomes';", conexao);
                comboBox1.Items.Clear();

                using (MySqlDataReader resultadoColunas = consultaColunas.ExecuteReader())
                {
                    if (resultadoColunas.HasRows)
                    {

                        while (resultadoColunas.Read())
                        {
                            string columnName = resultadoColunas["COLUMN_NAME"].ToString();
                            comboBox1.Items.Add(columnName);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Nenhuma coluna encontrada.");
                    }
                }


                string query = "SELECT * FROM `view_rankings_com_nomes` WHERE 1";
                MySqlCommand consultaDados = new MySqlCommand(query, conexao);
                MySqlDataAdapter da = new MySqlDataAdapter(consultaDados);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dataGridView1.DataSource = null;
                dataGridView1.Columns.Clear();
                dataGridView1.DataSource = dt;

                if (dataGridView1.Columns.Contains("id"))
                {
                    dataGridView1.Columns["id"].Visible = false;
                }


            }
        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                int idTorneio = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["id"].Value);
                CarregarLogoTorneio(idTorneio);
            }

        }

        private void CarregarLogoTorneio(int idTorneio)
        {
            // Estabelece a conexão com o banco de dados
            using (MySqlConnection conexao = new MySqlConnection("SERVER=127.0.0.1; DATABASE=eventosbv; UID=root; PASSWORD=;"))
            {
                conexao.Open();

                // Consulta para obter o logo do torneio com o ID fornecido
                string query = "SELECT logo FROM torneios WHERE id = @id";
                MySqlCommand comando = new MySqlCommand(query, conexao);
                comando.Parameters.AddWithValue("@id", idTorneio);

                MySqlDataReader reader = comando.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();
                    // Obtém o caminho do logo do banco de dados
                    string caminhoLogo = reader["logo"].ToString();

                    // Verifica se o arquivo de imagem existe no caminho especificado
                    if (File.Exists(caminhoLogo))
                    {
                        // Exibe o logo no PictureBox
                        pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                        pictureBox1.Image = Image.FromFile(caminhoLogo);
                    }
                    else
                    {
                        // Exibe uma mensagem de erro caso a imagem não seja encontrada
                        MessageBox.Show("O logo do torneio não foi encontrado no caminho especificado: " + caminhoLogo);
                    }
                }
                else
                {
                    MessageBox.Show("Torneio não encontrado.");
                }

                reader.Close();
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void classificacaoUser_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            string connectionString = "SERVER=127.0.0.1; DATABASE=eventosbv; UID=root; PASSWORD=;";

            using (MySqlConnection conexao = new MySqlConnection(connectionString))
            {
                try
                {
                    conexao.Open();

                    // Consulta para obter o tipo de usuário com base no ID logado
                    string query = "SELECT tipo FROM usuarios WHERE id = @idUsuario AND status = 'Ativo'";

                    using (MySqlCommand comando = new MySqlCommand(query, conexao))
                    {
                        comando.Parameters.AddWithValue("@idUsuario", UsuarioLogado.Id);

                        using (MySqlDataReader reader = comando.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string tipo = reader.GetString("tipo");

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
                                    MessageBox.Show("Tipo de usuário inválido.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }

                                this.Hide(); // Oculta a tela atual
                            }
                            else
                            {
                                MessageBox.Show("Usuário não encontrado ou inativo.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void LimparFiltro()
        {
            comboBox1.SelectedIndex = -1;
            textBox1.Clear();
            CarregarDados();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            LimparFiltro();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string campo = comboBox1.Text;
            string valorCampo = textBox1.Text;

            if (string.IsNullOrWhiteSpace(campo) || string.IsNullOrWhiteSpace(valorCampo))
            {
                MessageBox.Show("Por favor, selecione um campo e digite um valor para filtrar.");
                return;
            }

            using (MySqlConnection conexao = new MySqlConnection("SERVER=127.0.0.1; DATABASE=eventosbv; UID=root; PASSWORD=;"))
            {
                try
                {
                    conexao.Open();

                    // Consulta de filtragem usando DataAdapter e DataTable para atualizar DataGridView
                    string query = $"SELECT id, Torneio, Equipe, Colocação FROM view_rankings_com_nomes WHERE `{campo}` LIKE @valorCampo ORDER BY id ASC";
                    MySqlCommand consulta = new MySqlCommand(query, conexao);
                    consulta.Parameters.AddWithValue("@valorCampo", "%" + valorCampo + "%");

                    MySqlDataAdapter da = new MySqlDataAdapter(consulta);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dataGridView1.DataSource = dt;

                    if (dt.Rows.Count == 0)
                    {
                        MessageBox.Show("Nenhum registro foi encontrado.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao filtrar os dados: " + ex.Message);
                }
            }

        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellContentClick_2(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
