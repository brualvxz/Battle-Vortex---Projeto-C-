using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Battle_Vortex_Form
{
    public partial class premioOrganizador : Form
    {
        public premioOrganizador()
        {
            InitializeComponent();
            CarregarDados();
            CarregarDados2();

        }

        private void CarregarDados()
        {
            using (MySqlConnection conexao = new MySqlConnection("SERVER=127.0.0.1; DATABASE=eventosbv; UID=root; PASSWORD=;"))
            {
                conexao.Open();

                // Consulta para obter o ID do torneio vinculado ao organizador (usuário logado)
                string queryTorneioId = "SELECT organizador_id FROM usuarios WHERE id = @usuarioId";
                MySqlCommand consultaTorneio = new MySqlCommand(queryTorneioId, conexao);
                consultaTorneio.Parameters.AddWithValue("@usuarioId", UsuarioLogado.Id);

                object torneioIdResult = consultaTorneio.ExecuteScalar();

                if (torneioIdResult == null || torneioIdResult == DBNull.Value)
                {
                    // Se o usuário logado não possui nenhum torneio vinculado
                    MessageBox.Show("Você não possui nenhum torneio vinculado.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int torneioId = Convert.ToInt32(torneioIdResult);

                // Consulta para carregar os prêmios do torneio vinculado
                string queryPremios = "SELECT `id`, `torneio_id`, `descricao`, `premio_principal`, `premio_secundario`, `patrocinador_id`, `tipo_origem`, `premio_terciario` FROM premios WHERE torneio_id = @torneioId";
                MySqlCommand consultaPremios = new MySqlCommand(queryPremios, conexao);
                consultaPremios.Parameters.AddWithValue("@torneioId", torneioId);

                MySqlDataAdapter daPremios = new MySqlDataAdapter(consultaPremios);
                DataTable dtPremios = new DataTable();
                daPremios.Fill(dtPremios);

                // Carregar os dados dos prêmios no DataGridView2
                dataGridView2.DataSource = null;
                dataGridView2.Columns.Clear();
                dataGridView2.DataSource = dtPremios;

                // Adiciona colunas de ações para Alterar e Excluir se não existirem
                if (!dataGridView2.Columns.Contains("Alterar"))
                {
                    DataGridViewButtonColumn alterarColumn = new DataGridViewButtonColumn
                    {
                        Name = "Alterar",
                        HeaderText = "Alterar",
                        Text = "Alterar",
                        UseColumnTextForButtonValue = true
                    };
                    dataGridView2.Columns.Add(alterarColumn);
                }

                if (!dataGridView2.Columns.Contains("Excluir"))
                {
                    DataGridViewButtonColumn excluirColumn = new DataGridViewButtonColumn
                    {
                        Name = "Excluir",
                        HeaderText = "Excluir",
                        Text = "Excluir",
                        UseColumnTextForButtonValue = true
                    };
                    dataGridView2.Columns.Add(excluirColumn);
                }
            }
        }

        private void CarregarDados2()
        {
            using (MySqlConnection conexao = new MySqlConnection("SERVER=127.0.0.1; DATABASE=eventosbv; UID=root; PASSWORD=;"))
            {
                conexao.Open();

                string query = "SELECT id, torneio_id, descricao, premio_principal, premio_secundario, premio_terciario, tipo_origem, patrocinador_id FROM premios";
                MySqlCommand consultaDados = new MySqlCommand(query, conexao);
                MySqlDataAdapter da = new MySqlDataAdapter(consultaDados);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dataGridView1.DataSource = null;
                dataGridView1.Columns.Clear();
                dataGridView1.DataSource = dt;




                PreencherComboBoxComColunas(dt);
            }
        }
        private void PreencherComboBoxComColunas(DataTable dt)
        {

            comboBox1.Items.Clear();


            foreach (DataColumn column in dt.Columns)
            {

                if (!column.ColumnName.Contains("logo"))
                {
                    comboBox1.Items.Add(column.ColumnName);
                }
            }


            if (comboBox1.Items.Count > 0)
            {
                comboBox1.SelectedIndex = 0;
            }

            comboBox1.SelectedIndex = -1;
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
        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                int idTorneio = Convert.ToInt32(dataGridView2.Rows[e.RowIndex].Cells["id"].Value);
                CarregarLogoTorneio(idTorneio);
            }

            if (e.RowIndex >= 0)
            {
                // Se clicar na coluna "Alterar"
                if (e.ColumnIndex == dataGridView2.Columns["Alterar"].Index)
                {
                    string id = dataGridView2.Rows[e.RowIndex].Cells["id"].Value.ToString();
                    premioAlt(id);
                }
                // Se clicar na coluna "Excluir"
                else if (e.ColumnIndex == dataGridView2.Columns["Excluir"].Index)
                {
                    string id = dataGridView2.Rows[e.RowIndex].Cells["id"].Value.ToString();
                    premioEx(id);
                }
            }
        }

        private void premioEx(string id)
        {
            using (MySqlConnection conexao = new MySqlConnection("SERVER=127.0.0.1; DATABASE=eventosbv; UID=root; PASSWORD=;"))
            {
                conexao.Open();


                DialogResult result = MessageBox.Show("Tem certeza que deseja excluir o prêmio?", "Confirmação", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {

                    string excluir = "DELETE FROM premios WHERE id = " + id;
                    MySqlCommand comandos = new MySqlCommand(excluir, conexao);
                    comandos.ExecuteNonQuery();
                    MessageBox.Show("Prêmio excluído com sucesso!");


                    CarregarDados();
                }
            }
        }

        private void premioAlt(string id)
        {
            premioAlt alterarForm = new premioAlt(id);
            alterarForm.ShowDialog();

            CarregarDados();
            CarregarDados2();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            premiosCadastrar premiosCadastrar = new premiosCadastrar();
            premiosCadastrar.ShowDialog();

            CarregarDados();
            CarregarDados2();
        }

        private void premioOrganizador_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            homeOrganizador homeUser = new homeOrganizador();
            homeUser.Show();
            this.Close();
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
                    string query = $"SELECT `id`, `torneio_id`, `descricao`, `premio_principal`, `premio_secundario`, `premio_terciario`, `patrocinador_id`, `tipo_origem` FROM premios WHERE `{campo}` LIKE @valorCampo";
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

        private void LimparFiltro()
        {
            comboBox1.SelectedIndex = -1;  // Limpar a seleção no comboBox
            textBox1.Clear();  // Limpar o campo de texto
            CarregarDados();  // Recarregar a tabela sem filtro
        }

        private void button4_Click(object sender, EventArgs e)
        {
            LimparFiltro();
        }


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                int idTorneio = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["id"].Value);
                CarregarLogoTorneio(idTorneio);
            }
        }

        private void dataGridView2_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
