using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Battle_Vortex_Form
{
    public partial class patrocinadoresPat : Form
    {
        public patrocinadoresPat()
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

                // Consulta para obter o ID do patrocinador vinculado ao usuário logado
                string queryPatrocinadorId = "SELECT id FROM patrocinadores WHERE dono_id = @usuarioId";
                MySqlCommand consultaPatrocinador = new MySqlCommand(queryPatrocinadorId, conexao);
                consultaPatrocinador.Parameters.AddWithValue("@usuarioId", UsuarioLogado.Id);

                object patrocinadorIdResult = consultaPatrocinador.ExecuteScalar();

                if (patrocinadorIdResult == null || patrocinadorIdResult == DBNull.Value)
                {
                    // Se o usuário logado não possui nenhuma marca de patrocinador cadastrada
                    MessageBox.Show("Você não possui nenhuma marca de patrocinador cadastrada.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    // Limpa o DataGridView e impede que ele seja carregado
                    dataGridView2.DataSource = null;
                    dataGridView2.Columns.Clear();
                    return;
                }

                int patrocinadorId = Convert.ToInt32(patrocinadorIdResult);

                // Consulta para obter informações da marca do patrocinador
                string queryPatrocinador = "SELECT id, nome, conquistas, logo FROM patrocinadores WHERE id = @patrocinadorId";
                MySqlCommand consultaDados = new MySqlCommand(queryPatrocinador, conexao);
                consultaDados.Parameters.AddWithValue("@patrocinadorId", patrocinadorId);
                MySqlDataAdapter da = new MySqlDataAdapter(consultaDados);
                DataTable dt = new DataTable();
                da.Fill(dt);

                // Atualiza o DataGridView com os dados do patrocinador
                dataGridView2.DataSource = null;
                dataGridView2.Columns.Clear();
                dataGridView2.DataSource = dt;

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

                // Consulta para pegar o nome das colunas da tabela patrocinadores
                MySqlCommand consultaColunas = new MySqlCommand("SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA = 'eventosbv' AND TABLE_NAME = 'patrocinadores';", conexao);
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

                // Consulta para carregar os dados dos patrocinadores
                string query = "SELECT id, nome, conquistas, logo FROM patrocinadores";
                MySqlCommand consultaDados = new MySqlCommand(query, conexao);
                MySqlDataAdapter da = new MySqlDataAdapter(consultaDados);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dataGridView1.DataSource = null;
                dataGridView1.Columns.Clear();
                dataGridView1.DataSource = dt;
            }

        }




        private void button1_Click(object sender, EventArgs e)
        {
            patrocinadoresCadastrar torneioUser = new patrocinadoresCadastrar();
            torneioUser.ShowDialog();
            this.Close();

           
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
                    string query = $"SELECT id, nome, conquistas, logo FROM patrocinadores WHERE `{campo}` LIKE @valorCampo";
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
            comboBox1.SelectedIndex = -1;
            textBox1.Clear();
            CarregarDados();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            LimparFiltro();
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void patrocinadoresPat_Load(object sender, EventArgs e)
        {

        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Verifica se não é o cabeçalho
            {
                // Verifica se a coluna "Alterar" existe
                if (dataGridView2.Columns.Contains("Alterar") && e.ColumnIndex == dataGridView2.Columns["Alterar"].Index)
                {
                    string id = dataGridView2.Rows[e.RowIndex].Cells["id"].Value.ToString();
                    patrocinadoresAlt(id);
                }
                // Verifica se a coluna "Excluir" existe
                else if (dataGridView2.Columns.Contains("Excluir") && e.ColumnIndex == dataGridView2.Columns["Excluir"].Index)
                {
                    string id = dataGridView2.Rows[e.RowIndex].Cells["id"].Value.ToString();
                    patrocinadoresEx(id);
                }
            }
        }


        private void CarregarLogoPatrocinador(int idPatrocinador)
        {
            // Estabelece a conexão com o banco de dados
            MySqlConnection conexao = new MySqlConnection("SERVER=127.0.0.1; DATABASE=eventosbv; UID=root; PASSWORD=;");
            conexao.Open();

            // Consulta para obter o logo do patrocinador com o ID fornecido
            string query = "SELECT logo FROM patrocinadores WHERE id = @id";
            MySqlCommand comando = new MySqlCommand(query, conexao);
            comando.Parameters.AddWithValue("@id", idPatrocinador);

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
                    MessageBox.Show("O logo do patrocinador não foi encontrado no caminho especificado: " + caminhoLogo);
                }
            }
            else
            {
                MessageBox.Show("Patrocinador não encontrado.");
            }

            reader.Close();
            conexao.Close();
        }

        private void patrocinadoresEx(string id)
        {
            MySqlConnection conexao = new MySqlConnection("SERVER=127.0.0.1; DATABASE=eventosbv; UID=root; PASSWORD=;");
            conexao.Open();

            // Confirmação da exclusão
            DialogResult result = MessageBox.Show("Tem certeza que deseja excluir o patrocinador?", "Confirmação", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                // Executa a exclusão
                string excluir = "DELETE FROM patrocinadores WHERE id = " + id;
                MySqlCommand comandos = new MySqlCommand(excluir, conexao);
                comandos.ExecuteNonQuery();
                MessageBox.Show("Patrocinador excluído com sucesso!");

                // Recarrega os dados
                CarregarDados();
                CarregarDados2();
            }

            conexao.Close();
        }

        private void patrocinadoresAlt(string id)
        {
            // Abre o formulário de alteração com o ID do patrocinador
            patrocinadoresAlt alterarForm = new patrocinadoresAlt(id);
            alterarForm.ShowDialog();

            // Recarrega os dados após o formulário de alteração ser fechado
            CarregarDados();
            CarregarDados2();
        }
        
    

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView2_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Verifica se a célula clicada não está na linha de cabeçalho
            {
                int idPatrocinador = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["id"].Value);
                CarregarLogoPatrocinador(idPatrocinador);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
    
}
