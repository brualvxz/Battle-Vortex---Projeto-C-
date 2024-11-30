using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;


namespace Battle_Vortex_Form
{
    public partial class jogadoresAdm : Form
    {
        public jogadoresAdm()
        {
            InitializeComponent(); 
            CarregarDados();
        }

        private void CarregarDados()
        {
            using (MySqlConnection conexao = new MySqlConnection("SERVER=127.0.0.1; DATABASE=eventosbv; UID=root; PASSWORD=;"))
            {
                conexao.Open();

                // Consulta para pegar o nome das colunas da tabela jogadores
                MySqlCommand consultaColunas = new MySqlCommand("SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA = 'eventosbv' AND TABLE_NAME = 'jogadores';", conexao);
                comboBox1.Items.Clear();

                using (MySqlDataReader resultadoColunas = consultaColunas.ExecuteReader())
                {
                    if (resultadoColunas.HasRows)
                    {
                        // Adiciona os nomes de coluna ao ComboBox, mas não adiciona ao DataGridView
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

                // Consulta para carregar os dados dos jogadores
                string query = "SELECT id, nome, nickname, equipe_id, personagemMain_id, conquistas, foto FROM jogadores";
                MySqlCommand consultaDados = new MySqlCommand(query, conexao);
                MySqlDataAdapter da = new MySqlDataAdapter(consultaDados);
                DataTable dt = new DataTable();
                da.Fill(dt);

                // Limpa o DataGridView antes de adicionar os dados
                dataGridView1.DataSource = null;
                dataGridView1.Columns.Clear(); // Limpa as colunas para evitar duplicação
                dataGridView1.DataSource = dt;

                // Adiciona colunas para "Alterar" e "Excluir" se não existirem
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

            if (e.RowIndex >= 0) // Verifica se a célula clicada não está na linha de cabeçalho
            {
                
                int idJogador = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["id"].Value);

                
                CarregarFotoJogador(idJogador);
            }

            if (e.RowIndex >= 0)
            {
                // Se clicar na coluna "Alterar"
                if (e.ColumnIndex == dataGridView1.Columns["Alterar"].Index)
                {
                    string id = dataGridView1.Rows[e.RowIndex].Cells["id"].Value.ToString();
                    jogadoresAlt(id);
                }
                // Se clicar na coluna "Excluir"
                else if (e.ColumnIndex == dataGridView1.Columns["Excluir"].Index)
                {
                    string id = dataGridView1.Rows[e.RowIndex].Cells["id"].Value.ToString();
                    jogadoresEx(id);
                }
            }
        }

        private void CarregarFotoJogador(int idJogador)
        {
            // Estabelece a conexão com o banco de dados
            MySqlConnection conexao = new MySqlConnection("SERVER=127.0.0.1; DATABASE=eventosbv; UID=root; PASSWORD=;");
            conexao.Open();

            // Consulta para obter a foto do jogador com o ID fornecido
            string query = "SELECT foto FROM jogadores WHERE id = @id";
            MySqlCommand comando = new MySqlCommand(query, conexao);
            comando.Parameters.AddWithValue("@id", idJogador);

            MySqlDataReader reader = comando.ExecuteReader();

            if (reader.HasRows)
            {
                reader.Read();
                // Obtém o caminho da foto do banco de dados
                string caminhoFoto = reader["foto"].ToString();

                // Verifica se o arquivo de imagem existe no caminho especificado
                if (File.Exists(caminhoFoto))
                {
                    // Exibe a foto no PictureBox
                    pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                    pictureBox1.Image = Image.FromFile(caminhoFoto);
                }
                else
                {
                    // Exibe uma mensagem de erro caso a imagem não seja encontrada
                    MessageBox.Show("A foto do jogador não foi encontrada no caminho especificado: " + caminhoFoto);
                }
            }
            else
            {
                MessageBox.Show("Jogador não encontrado.");
            }

            reader.Close();
            conexao.Close();
        }



        private void jogadoresEx(string id)
        {
            MySqlConnection conexao = new MySqlConnection("SERVER=127.0.0.1; DATABASE=eventosbv; UID = root; PASSWORD = ;");
            conexao.Open();

            // Confirmação da exclusão
            DialogResult result = MessageBox.Show("Tem certeza que deseja excluir o jogador?", "Confirmação", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                // Executa a exclusão
                string excluir = "DELETE FROM jogadores WHERE id = " + id;
                MySqlCommand comandos = new MySqlCommand(excluir, conexao);
                comandos.ExecuteNonQuery();
                MessageBox.Show("Jogador excluído com sucesso!");

                // Recarrega os dados
                CarregarDados();
            }

            conexao.Close();
        }

        private void jogadoresAlt(string id)
        {
            // Abre o formulário de alteração com o ID do aluno
            jogadoresAlt alterarForm = new jogadoresAlt(id);
            alterarForm.ShowDialog();

            // Recarrega os dados após o formulário de alteração ser fechado
            CarregarDados();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            jogadoresCadastrar jogadoresCadastrar = new jogadoresCadastrar();
            jogadoresCadastrar.Show();
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
            string query = $"SELECT id, nome, nickname, equipe_id, personagemMain_id, conquistas, foto FROM jogadores WHERE `{campo}` LIKE @valorCampo";
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
        

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void jogadoresAdm_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            homeAdm homeadm = new homeAdm();
            homeadm.Show();
            this.Hide();
        }
    }
}
