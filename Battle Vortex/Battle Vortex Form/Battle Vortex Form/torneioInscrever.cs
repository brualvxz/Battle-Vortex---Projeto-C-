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
    public partial class torneioInscrever : Form
    {
        private int idJogadorLogado;
        public int idEquipe;
        public bool isCapitao = false;

        public torneioInscrever()
        {


            InitializeComponent();
            // Estabelece uma conexão com o banco de dados eventosbv
            MySqlConnection conexao = new MySqlConnection("SERVER=127.0.0.1; DATABASE=eventosbv; UID=root; PASSWORD=;");
            conexao.Open();

            // Consulta para selecionar torneios com vagas > 0 e status 'Em andamento'
            string query = "SELECT id, nome FROM torneios WHERE vagas > 0 AND status = 'Em andamento'";
            MySqlCommand comandos = new MySqlCommand(query, conexao);
            MySqlDataAdapter da = new MySqlDataAdapter(comandos);
            DataTable dt = new DataTable();
            da.Fill(dt);

            // Preenche a ComboBox com os torneios válidos
            comboBox1.DataSource = dt;
            comboBox1.DisplayMember = "nome";
            comboBox1.ValueMember = "id";

            conexao.Close();

            ObterIdJogadorLogado();
            CarregarDados();

        }

        private void CarregarDados()
        {
            using (MySqlConnection conexao = new MySqlConnection("SERVER=127.0.0.1; DATABASE=eventosbv; UID=root; PASSWORD=;"))
            {
                conexao.Open();

                // Obtém o ID da equipe do jogador logado
                string queryEquipeId = "SELECT equipe_id FROM jogadores WHERE id = @idJogadorLogado";
                MySqlCommand consultaEquipe = new MySqlCommand(queryEquipeId, conexao);
                consultaEquipe.Parameters.AddWithValue("@idJogadorLogado", idJogadorLogado);

                object equipeIdResult = consultaEquipe.ExecuteScalar();

                if (equipeIdResult == null || equipeIdResult == DBNull.Value)
                {
                    // Se o jogador não pertence a nenhuma equipe
                    MessageBox.Show("O jogador não pertence a nenhuma equipe.");
                    return;
                }

                idEquipe = Convert.ToInt32(equipeIdResult);

                // Consulta separada para obter o ID do capitão
                string queryCapitaoId = "SELECT capitao_id FROM equipes WHERE id = @idEquipe";
                MySqlCommand consultaCapitao = new MySqlCommand(queryCapitaoId, conexao);
                consultaCapitao.Parameters.AddWithValue("@idEquipe", idEquipe);

                isCapitao = false; // Reinicia a variável

                object capitaoIdResult = consultaCapitao.ExecuteScalar();
                if (capitaoIdResult != null && capitaoIdResult != DBNull.Value)
                {
                    int capitaoId = Convert.ToInt32(capitaoIdResult);
                    isCapitao = (capitaoId == idJogadorLogado);
                }

                // Verificação e atualização dos botões
                if (isCapitao)
                {
                    label2.Enabled = false;
                    button1.Enabled = true; // Capitao pode cadastrar a equipe em um torneio
                }
                else
                {
                    label2.Enabled = true;
                    button1.Enabled = false; // Membro não pode cadastrar a equipe em um torneio
                }
            }
        }

        private void ObterIdJogadorLogado()
        {
            using (MySqlConnection conexao = new MySqlConnection("SERVER=127.0.0.1; DATABASE=eventosbv; UID=root; PASSWORD=;"))
            {
                conexao.Open();
                string query = "SELECT jogador_id FROM usuarios WHERE id = @usuarioId";
                MySqlCommand comando = new MySqlCommand(query, conexao);
                comando.Parameters.AddWithValue("@usuarioId", UsuarioLogado.Id);
                var resultado = comando.ExecuteScalar();

                if (resultado != null)
                {
                    idJogadorLogado = Convert.ToInt32(resultado);
                }
                else
                {
                    MessageBox.Show("Jogador não encontrado para o usuário logado.");
                }
            }
        }

        private void torneioInscrever_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Verifica se o jogador selecionou um torneio na ComboBox
            if (comboBox1.SelectedIndex == -1)
            {
                MessageBox.Show("Selecione um torneio para inscrever sua equipe.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int idTorneio = (int)comboBox1.SelectedValue;

            // Verifica se o jogador logado é o capitão da equipe
            if (!isCapitao)
            {
                MessageBox.Show("Apenas o capitão da equipe pode inscrever a equipe em um torneio.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (MySqlConnection conexao = new MySqlConnection("SERVER=127.0.0.1; DATABASE=eventosbv; UID=root; PASSWORD=;"))
                {
                    conexao.Open();

                    // Comando para inscrever a equipe no torneio
                    string queryInscricao = "INSERT INTO equipes_inscritas (torneio_id, equipe_id) VALUES (@torneio_id, @equipe_id)";
                    MySqlCommand comandoInscricao = new MySqlCommand(queryInscricao, conexao);
                    comandoInscricao.Parameters.AddWithValue("@torneio_id", idTorneio);
                    comandoInscricao.Parameters.AddWithValue("@equipe_id", idEquipe);
                    comandoInscricao.ExecuteNonQuery();

                    // Atualiza as vagas do torneio
                    string queryVagas = "UPDATE torneios SET vagas = vagas - 1 WHERE id = @idTorneio";
                    MySqlCommand comandoVagas = new MySqlCommand(queryVagas, conexao);
                    comandoVagas.Parameters.AddWithValue("@idTorneio", idTorneio);
                    comandoVagas.ExecuteNonQuery();

                    MessageBox.Show("Equipe inscrita no torneio com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao inscrever a equipe no torneio: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
    
}
