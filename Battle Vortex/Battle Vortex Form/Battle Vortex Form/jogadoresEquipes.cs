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
    public partial class jogadoresEquipes : Form
    {

        string caminhoNoServidor;
        string nomeArquivo;
        public int idEquipe; // Variável que irá armazenar o id da equipe, passada pela outra tela

        public jogadoresEquipes(int idEquipe)
        {
            InitializeComponent();
            this.idEquipe = idEquipe; // Armazena o id da equipe fornecido pela outra tela

            // Estabelece uma conexão com o banco de dados eventosbv
            MySqlConnection conexao = new MySqlConnection("SERVER=127.0.0.1; DATABASE=eventosbv; UID=root; PASSWORD=;");
            conexao.Open();

            // Consulta para selecionar jogadores que não têm uma equipe associada
            string query = "SELECT id, nome FROM jogadores WHERE equipe_id IS NULL";
            MySqlCommand comandos = new MySqlCommand(query, conexao);
            MySqlDataAdapter da = new MySqlDataAdapter(comandos);
            DataTable dt = new DataTable();
            da.Fill(dt);

            // Preenche a ComboBox com os jogadores que não têm equipe
            comboBox1.DataSource = dt;
            comboBox1.DisplayMember = "nome";
            comboBox1.ValueMember = "id";

            conexao.Close();


        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Verifica se um jogador foi selecionado na ComboBox
            if (comboBox1.SelectedIndex == -1)
            {
                MessageBox.Show("Selecione um jogador para adicionar à equipe.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int idJogador = (int)comboBox1.SelectedValue;

            // Conexão com o banco de dados
            MySqlConnection conexao = new MySqlConnection("SERVER=127.0.0.1; DATABASE=eventosbv; UID=root; PASSWORD=;");
            conexao.Open();

            try
            {
                // Verifica se a equipe existe
                string verificarEquipeQuery = "SELECT COUNT(*) FROM equipes WHERE id = @idEquipe";
                MySqlCommand verificarEquipeCmd = new MySqlCommand(verificarEquipeQuery, conexao);
                verificarEquipeCmd.Parameters.AddWithValue("@idEquipe", this.idEquipe);

                int count = Convert.ToInt32(verificarEquipeCmd.ExecuteScalar());

                if (count == 0)
                {
                    MessageBox.Show("A equipe selecionada não existe no banco de dados.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Atualiza o jogador na tabela 'jogadores', associando a equipe a ele
                string atualizarJogadorQuery = "UPDATE jogadores SET equipe_id = @idEquipe WHERE id = @idJogador";
                MySqlCommand cmd = new MySqlCommand(atualizarJogadorQuery, conexao);
                cmd.Parameters.AddWithValue("@idEquipe", this.idEquipe); // Usa o idEquipe passado pela outra tela
                cmd.Parameters.AddWithValue("@idJogador", idJogador);
                cmd.ExecuteNonQuery();

                // Adiciona o jogador na tabela 'equipes_jogadores' para manter a relação
                string inserirEquipeJogadorQuery = "INSERT INTO equipes_jogadores (equipe_id, jogador_id) VALUES (@idEquipe, @idJogador)";
                MySqlCommand cmd2 = new MySqlCommand(inserirEquipeJogadorQuery, conexao);
                cmd2.Parameters.AddWithValue("@idEquipe", this.idEquipe);
                cmd2.Parameters.AddWithValue("@idJogador", idJogador);
                cmd2.ExecuteNonQuery();

                MessageBox.Show("Jogador adicionado à equipe com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao adicionar jogador à equipe: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                conexao.Close();
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void jogadoresEquipes_Load(object sender, EventArgs e)
        {

        }
    }
}
