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

namespace Battle_Vortex_Form
{
    public partial class classificacaoCadastrar : Form
    {
        public classificacaoCadastrar()
        {
            InitializeComponent();
            PreencherComboBoxes();
            dataGridView1.Columns.Add("Torneio", "Torneio");
            dataGridView1.Columns.Add("Equipe", "Equipe");
            dataGridView1.Columns.Add("Posicao", "Posição");

            // Permitir que o usuário adicione linhas ao DataGridView
            dataGridView1.AllowUserToAddRows = true;
        }

        private void PreencherComboBoxes()
        {
            // Estabelece a conexão com o banco de dados
            using (MySqlConnection conexao = new MySqlConnection("SERVER=127.0.0.1; DATABASE=eventosbv; UID=root; PASSWORD=;"))
            {
                try
                {
                    conexao.Open();

                   
                    string queryEquipes = "SELECT id, nome FROM equipes";
                    MySqlCommand comandosEquipes = new MySqlCommand(queryEquipes, conexao);
                    MySqlDataAdapter daEquipes = new MySqlDataAdapter(comandosEquipes);
                    DataTable dtEquipes = new DataTable();
                    daEquipes.Fill(dtEquipes);

                    // Preenche a ComboBox de equipes
                    comboBox1.DataSource = dtEquipes;
                    comboBox1.DisplayMember = "nome";
                    comboBox1.ValueMember = "id";

                    
                    string queryTorneios = "SELECT id, nome FROM torneios WHERE status = 'Concluído'";
                    MySqlCommand comandosTorneios = new MySqlCommand(queryTorneios, conexao);
                    MySqlDataAdapter daTorneios = new MySqlDataAdapter(comandosTorneios);
                    DataTable dtTorneios = new DataTable();
                    daTorneios.Fill(dtTorneios);

                    
                    comboBox2.DataSource = dtTorneios;
                    comboBox2.DisplayMember = "nome";
                    comboBox2.ValueMember = "id";
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show($"Erro ao carregar as ComboBoxes: {ex.Message}");
                }
            }
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Verifica se o DataGridView contém dados
            if (dataGridView1.Rows.Count > 0)
            {
                // Conecta ao banco de dados
                using (MySqlConnection conexao = new MySqlConnection("SERVER=127.0.0.1; DATABASE=eventosbv; UID=root; PASSWORD=;"))
                {
                    try
                    {
                        conexao.Open();

                       
                        MySqlTransaction transaction = conexao.BeginTransaction();
                        MySqlCommand comando = conexao.CreateCommand();
                        comando.Transaction = transaction;

                       
                        foreach (DataGridViewRow row in dataGridView1.Rows)
                        {
                           
                            if (row.IsNewRow) continue;

                            
                            string torneio = row.Cells["Torneio"].Value?.ToString();
                            string equipe = row.Cells["Equipe"].Value?.ToString();
                            int posicao = 0;
                            int.TryParse(row.Cells["Posicao"].Value?.ToString(), out posicao);

                            
                            if (string.IsNullOrWhiteSpace(torneio) || string.IsNullOrWhiteSpace(equipe) || posicao == 0)
                            {
                                MessageBox.Show("Por favor, preencha todos os campos corretamente.");
                                return;
                            }

                           
                            comando.CommandText = "SELECT id FROM torneios WHERE nome = @torneio";
                            comando.Parameters.Clear();
                            comando.Parameters.AddWithValue("@torneio", torneio);
                            object idTorneio = comando.ExecuteScalar();

                            
                            if (idTorneio == null)
                            {
                                MessageBox.Show($"Torneio '{torneio}' não encontrado.");
                                return;
                            }

                           
                            comando.CommandText = "SELECT id FROM equipes WHERE nome = @equipe";
                            comando.Parameters.Clear();
                            comando.Parameters.AddWithValue("@equipe", equipe);
                            object idEquipe = comando.ExecuteScalar();

                            
                            if (idEquipe == null)
                            {
                                MessageBox.Show($"Equipe '{equipe}' não encontrada.");
                                return;
                            }

                            
                            comando.CommandText = "INSERT INTO rankings (torneio_id, equipe_id, posicao) " +
                                                  "VALUES (@torneio_id, @equipe_id, @posicao)";
                            comando.Parameters.Clear();
                            comando.Parameters.AddWithValue("@torneio_id", idTorneio);
                            comando.Parameters.AddWithValue("@equipe_id", idEquipe);
                            comando.Parameters.AddWithValue("@posicao", posicao);

                            comando.ExecuteNonQuery();
                        }

                        
                        transaction.Commit();
                        MessageBox.Show("Classificações inseridas com sucesso!");
                    }
                    catch (MySqlException ex)
                    {
                        MessageBox.Show($"Erro ao inserir classificações: {ex.Message}");
                    }
                    finally
                    {
                        conexao.Close();
                    }
                }
            }
            else
            {
                MessageBox.Show("Não há dados para inserir.");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            classificacaoAdm classificacaoAdm = new classificacaoAdm();
            classificacaoAdm.Show();
            this.Close();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
