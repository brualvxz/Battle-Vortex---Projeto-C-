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
    public partial class torneioAdm : Form
    {
        public torneioAdm()
        {
            InitializeComponent();
            CarregarDados();
        }

        private void CarregarDados()
        {
            using (MySqlConnection conexao = new MySqlConnection("SERVER=127.0.0.1; DATABASE=eventosbv; UID=root; PASSWORD=;"))
            {
                conexao.Open();

                
                MySqlCommand consultaColunas = new MySqlCommand("SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA = 'eventosbv' AND TABLE_NAME = 'torneios';", conexao);
                comboBox1.Items.Clear();

                using (MySqlDataReader resultadoColunas = consultaColunas.ExecuteReader())
                {
                    if (resultadoColunas.HasRows)
                    {
                        // Adiciona os nomes de coluna ao ComboBox
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

              
                string query = "SELECT id, nome, data_inicio, data_fim, local, descricao, regras, vagas, status, logo FROM torneios";
                MySqlCommand consultaDados = new MySqlCommand(query, conexao);
                MySqlDataAdapter da = new MySqlDataAdapter(consultaDados);
                DataTable dt = new DataTable();
                da.Fill(dt);

                
                dataGridView1.DataSource = null;
                dataGridView1.Columns.Clear(); 
                dataGridView1.DataSource = dt;

               
                if (!dataGridView1.Columns.Contains("Alterar"))
                {
                    DataGridViewButtonColumn alterarColumn = new DataGridViewButtonColumn
                    {
                        Name = "Alterar",
                        HeaderText = "Alterar",
                        Text = "Alterar",
                        UseColumnTextForButtonValue = true
                    };
                    dataGridView1.Columns.Add(alterarColumn);
                }

                if (!dataGridView1.Columns.Contains("Excluir"))
                {
                    DataGridViewButtonColumn excluirColumn = new DataGridViewButtonColumn
                    {
                        Name = "Excluir",
                        HeaderText = "Excluir",
                        Text = "Excluir",
                        UseColumnTextForButtonValue = true
                    };
                    dataGridView1.Columns.Add(excluirColumn);
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

            if (e.RowIndex >= 0)
            {
                // Se clicar na coluna "Alterar"
                if (e.ColumnIndex == dataGridView1.Columns["Alterar"].Index)
                {
                    string id = dataGridView1.Rows[e.RowIndex].Cells["id"].Value.ToString();
                    torneiosAlt(id);
                }
                // Se clicar na coluna "Excluir"
                else if (e.ColumnIndex == dataGridView1.Columns["Excluir"].Index)
                {
                    string id = dataGridView1.Rows[e.RowIndex].Cells["id"].Value.ToString();
                    torneiosEx(id);
                }
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


        private void torneiosEx(string id)
        {
            MySqlConnection conexao = new MySqlConnection("SERVER=127.0.0.1; DATABASE=eventosbv; UID=root; PASSWORD=;");
            conexao.Open();

            // Confirmação da exclusão
            DialogResult result = MessageBox.Show("Tem certeza que deseja excluir o torneio?", "Confirmação", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                // Executa a exclusão
                string excluir = "DELETE FROM torneios WHERE id = " + id;
                MySqlCommand comandos = new MySqlCommand(excluir, conexao);
                comandos.ExecuteNonQuery();
                MessageBox.Show("Torneio excluído com sucesso!");

                // Recarrega os dados
                CarregarDados();
            }

            conexao.Close();
        }

        private void torneiosAlt(string id)
        {
            // Abre o formulário de alteração com o ID do torneio
            torneiosAlt alterarForm = new torneiosAlt(id);
            alterarForm.ShowDialog();

            // Recarrega os dados após o formulário de alteração ser fechado
            CarregarDados();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            torneiosCadastrar torneiosCadastrar = new torneiosCadastrar();
            torneiosCadastrar.ShowDialog();

            CarregarDados();
           
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Obtém o campo selecionado no ComboBox e o valor para filtrar
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
                    string query = $"SELECT id, nome, data_inicio, data_fim, local, descricao, regras, vagas, status, logo FROM torneios WHERE `{campo}` LIKE @valorCampo";
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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void torneioAdm_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            homeAdm homeadm = new homeAdm();
            homeadm.Show();
            this.Close();
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
    }
}
