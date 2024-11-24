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
    public partial class equipesAdm : Form
    {
        public equipesAdm()
        {
            InitializeComponent();
            CarregarDados();
        }
        private void CarregarDados()
        {
            using (MySqlConnection conexao = new MySqlConnection("SERVER=127.0.0.1; DATABASE=eventosbv; UID=root; PASSWORD=;"))
            {
                conexao.Open();

                // Consulta os nomes das colunas da tabela 'equipes'
                MySqlCommand consultaColunas = new MySqlCommand("SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA = 'eventosbv' AND TABLE_NAME = 'equipes';", conexao);
                comboBox1.Items.Clear();

                using (MySqlDataReader resultadoColunas = consultaColunas.ExecuteReader())
                {
                    if (resultadoColunas.HasRows)
                    {
                        // Adiciona os nomes das colunas ao ComboBox
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

                // Consulta para obter os dados da tabela 'equipes'
                string query = "SELECT id, nome, logo, localidade, email, data_criacao FROM equipes";
                MySqlCommand consultaDados = new MySqlCommand(query, conexao);
                MySqlDataAdapter da = new MySqlDataAdapter(consultaDados);
                DataTable dt = new DataTable();
                da.Fill(dt);

                // Atualiza o DataGridView
                dataGridView1.DataSource = null;
                dataGridView1.Columns.Clear();
                dataGridView1.DataSource = dt;

                // Adiciona os botões "Alterar" e "Excluir"
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
        private void CarregarLogoEquipe(int idEquipe)
        {
            using (MySqlConnection conexao = new MySqlConnection("SERVER=127.0.0.1; DATABASE=eventosbv; UID=root; PASSWORD=;"))
            {
                conexao.Open();

                
                string query = "SELECT logo FROM equipes WHERE id = @id";
                MySqlCommand comando = new MySqlCommand(query, conexao);
                comando.Parameters.AddWithValue("@id", idEquipe);

                MySqlDataReader reader = comando.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();
                    
                    string caminhoLogo = reader["logo"].ToString();

                    
                    if (File.Exists(caminhoLogo))
                    {
                        
                        pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                        pictureBox1.Image = Image.FromFile(caminhoLogo);
                    }
                    else
                    {
                        
                        MessageBox.Show("O logo da equipe não foi encontrado no caminho especificado: " + caminhoLogo);
                    }
                }
                else
                {
                    MessageBox.Show("Equipe não encontrada.");
                }

                reader.Close();
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                // Obtém o ID da equipe clicada
                int idEquipe = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["id"].Value);
                CarregarLogoEquipe(idEquipe);
            }

            if (e.RowIndex >= 0)
            {
                // Se clicar na coluna "Alterar"
                if (e.ColumnIndex == dataGridView1.Columns["Alterar"].Index)
                {
                    string id = dataGridView1.Rows[e.RowIndex].Cells["id"].Value.ToString();
                    AlterarEquipe(id);
                }
                // Se clicar na coluna "Excluir"
                else if (e.ColumnIndex == dataGridView1.Columns["Excluir"].Index)
                {
                    string id = dataGridView1.Rows[e.RowIndex].Cells["id"].Value.ToString();
                    ExcluirEquipe(id);
                }
            }
        }

        private void ExcluirEquipe(string id)
        {
            using (MySqlConnection conexao = new MySqlConnection("SERVER=127.0.0.1; DATABASE=eventosbv; UID=root; PASSWORD=;"))
            {
                conexao.Open();

                // Confirmação da exclusão
                DialogResult result = MessageBox.Show("Tem certeza que deseja excluir a equipe?", "Confirmação", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    string excluir = "DELETE FROM equipes WHERE id = @id";
                    MySqlCommand comando = new MySqlCommand(excluir, conexao);
                    comando.Parameters.AddWithValue("@id", id);
                    comando.ExecuteNonQuery();

                    MessageBox.Show("Equipe excluída com sucesso!");

                    // Recarrega os dados
                    CarregarDados();
                }
            }
        }

        private void AlterarEquipe(string id)
        {
            // Abre o formulário de alteração com o ID da equipe
            equipesAlt alterarForm = new equipesAlt(id);
            alterarForm.ShowDialog();

            // Recarrega os dados após o formulário ser fechado
            CarregarDados();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            equipesCadastrar equipesCadastrar = new equipesCadastrar();
            equipesCadastrar.Show();
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            homeAdm homeadm = new homeAdm();
            homeadm.Show();
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
                    string query = $"SELECT id, nome, logo, localidade, email, data_criacao FROM equipes WHERE `{campo}` LIKE @valorCampo";
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
    }
}
