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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Battle_Vortex_Form
{
    public partial class classificacaoAlt : Form
    {
        public classificacaoAlt(string id)
        {
            InitializeComponent();

            using (MySqlConnection conexao = new MySqlConnection("SERVER=127.0.0.1; DATABASE=eventosbv; UID=root; PASSWORD=;"))
            {
                try
                {
                    conexao.Open();

                    // Carregar equipes na ComboBox1
                    string queryEquipes = "SELECT id, nome FROM equipes";
                    MySqlCommand comandosEquipes = new MySqlCommand(queryEquipes, conexao);
                    MySqlDataAdapter daEquipes = new MySqlDataAdapter(comandosEquipes);
                    DataTable dtEquipes = new DataTable();
                    daEquipes.Fill(dtEquipes);
                    comboBox1.DataSource = dtEquipes;
                    comboBox1.DisplayMember = "nome";
                    comboBox1.ValueMember = "id";

                    // Carregar torneios na ComboBox2
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
            CarregarDados(id);
        }

        private void CarregarDados(string id)
        {
            using (MySqlConnection conexao = new MySqlConnection("SERVER=localhost;DATABASE=eventosbv;UID=root;PASSWORD="))
            {
                conexao.Open();

                // Consulta para pegar os dados do ranking
                MySqlCommand consulta = new MySqlCommand("SELECT * FROM rankings WHERE id = @id", conexao);
                consulta.Parameters.AddWithValue("@id", id);

                MySqlDataReader resultado = consulta.ExecuteReader();
                if (resultado.HasRows)
                {
                    while (resultado.Read())
                    {
                        textBox1.Text = resultado["id"].ToString(); // ID do ranking
                        textBox2.Text = resultado["posicao"].ToString(); // Posição no ranking

                        // Preencher ComboBox1 (Equipes)
                        comboBox1.SelectedValue = resultado["equipe_id"]; // Equipe vinculada ao ranking

                        // Preencher ComboBox2 (Torneios)
                        comboBox2.SelectedValue = resultado["torneio_id"]; // Torneio vinculado ao ranking
                    }
                }
                else
                {
                    MessageBox.Show("Nenhum registro foi encontrado");
                }

                resultado.Close();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // Conexão com o banco de dados eventosbv
                using (MySqlConnection conexao = new MySqlConnection("SERVER=localhost;DATABASE=eventosbv;UID=root;PASSWORD="))
                {
                    conexao.Open();

                    // Comando para atualizar os dados do ranking
                    MySqlCommand comando = new MySqlCommand();
                    comando.Connection = conexao;
                    comando.CommandText = @"UPDATE rankings 
                                    SET posicao = @posicao, 
                                        equipe_id = @equipe_id, 
                                        torneio_id = @torneio_id 
                                    WHERE id = @id";

                    // Adicionando os parâmetros para a consulta
                    comando.Parameters.AddWithValue("@posicao", textBox2.Text);
                    comando.Parameters.AddWithValue("@equipe_id", comboBox1.SelectedValue); // ID da equipe selecionada
                    comando.Parameters.AddWithValue("@torneio_id", comboBox2.SelectedValue); // ID do torneio selecionado
                    comando.Parameters.AddWithValue("@id", textBox1.Text); // ID do ranking

                    // Executa a atualização
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


        private void classificacaoAlt_Load(object sender, EventArgs e)
        {

        }
    }
}
