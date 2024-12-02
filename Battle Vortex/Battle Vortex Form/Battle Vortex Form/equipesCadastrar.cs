using MySql.Data.MySqlClient;
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

namespace Battle_Vortex_Form
{
    public partial class equipesCadastrar : Form
    {
        string caminhoNoServidor;
        string nomeArquivo;
        private int idJogadorLogado;

        public equipesCadastrar()
        {
            InitializeComponent();
            ObterIdJogadorLogado();  // Garantir que o método seja chamado ao iniciar o formulário
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "dd/MM/yyyy";
        }

        // Método corrigido para obter o jogador_id do usuário logado
        private void ObterIdJogadorLogado()
        {
            using (MySqlConnection conexao = new MySqlConnection("SERVER=127.0.0.1; DATABASE=eventosbv; UID=root; PASSWORD=;"))
            {
                conexao.Open();
                string query = "SELECT usuarios.jogador_id FROM usuarios WHERE usuarios.id = @usuarioId";
                MySqlCommand comando = new MySqlCommand(query, conexao);
                comando.Parameters.AddWithValue("@usuarioId", UsuarioLogado.Id);
                object resultado = comando.ExecuteScalar();

                // Se o resultado for null, significa que o jogador não existe ou não foi encontrado
                idJogadorLogado = resultado != null ? Convert.ToInt32(resultado) : 0;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string nome = textBox1.Text;
            string localidade = textBox2.Text;
            string email = textBox3.Text;
            DateTime dataCriacao = dateTimePicker1.Value;

            if (string.IsNullOrWhiteSpace(nome) || string.IsNullOrWhiteSpace(localidade) || string.IsNullOrWhiteSpace(email))
            {
                MessageBox.Show("Por favor, preencha todos os campos obrigatórios.");
                return;
            }

            if (idJogadorLogado == 0)
            {
                MessageBox.Show("Você ainda não possui um perfil de jogador. Redirecionando para a tela de cadastro de jogador...");
                jogadoresUser formJogadores = new jogadoresUser();
                formJogadores.Show();
                this.Close();
                return;
            }

            using (MySqlConnection conexao = new MySqlConnection("SERVER=127.0.0.1; DATABASE=eventosbv; UID=root; PASSWORD=;"))
            {
                conexao.Open();
                MySqlTransaction transaction = conexao.BeginTransaction(); // Inicia a transação para garantir que as duas operações aconteçam juntas

                try
                {
                    // Inserir nova equipe com todas as informações, incluindo email, data de criação e o id do jogador como capitão
                    string inserirEquipe = "INSERT INTO `equipes`(`nome`, `localidade`, `email`, `data_criacao`, `logo`, `capitao_id`) " +
                                           "VALUES(@nome, @localidade, @email, @data_criacao, @logo, @capitao_id)";

                    MySqlCommand comandos = new MySqlCommand(inserirEquipe, conexao, transaction);
                    comandos.Parameters.AddWithValue("@nome", nome);
                    comandos.Parameters.AddWithValue("@localidade", localidade);
                    comandos.Parameters.AddWithValue("@email", email);
                    comandos.Parameters.AddWithValue("@data_criacao", dataCriacao.ToString("yyyy-MM-dd"));
                    comandos.Parameters.AddWithValue("@logo", caminhoNoServidor);
                    comandos.Parameters.AddWithValue("@capitao_id", idJogadorLogado); // Usando idJogadorLogado

                    comandos.ExecuteNonQuery();

                    // Capturar o ID da equipe recém-criada
                    string ultimoIdEquipe = "SELECT LAST_INSERT_ID()";
                    MySqlCommand comandoIdEquipe = new MySqlCommand(ultimoIdEquipe, conexao, transaction);
                    int idEquipeCriada = Convert.ToInt32(comandoIdEquipe.ExecuteScalar());

                    // Atualizar o jogador logado para vincular à equipe criada
                    string atualizarJogador = "UPDATE `jogadores` SET `equipe_id` = @equipe_id WHERE `id` = @idJogadorLogado";
                    MySqlCommand comandoAtualizarJogador = new MySqlCommand(atualizarJogador, conexao, transaction);
                    comandoAtualizarJogador.Parameters.AddWithValue("@equipe_id", idEquipeCriada);
                    comandoAtualizarJogador.Parameters.AddWithValue("@idJogadorLogado", idJogadorLogado);

                    comandoAtualizarJogador.ExecuteNonQuery();

                    // Agora, adicionar o jogador à tabela 'equipes_jogadores'
                    string inserirEquipesJogadores = "INSERT INTO `equipes_jogadores` (`jogador_id`, `equipe_id`) " +
                                                     "VALUES (@jogador_id, @equipe_id)";
                    MySqlCommand comandoInserirEquipesJogadores = new MySqlCommand(inserirEquipesJogadores, conexao, transaction);
                    comandoInserirEquipesJogadores.Parameters.AddWithValue("@jogador_id", idJogadorLogado);
                    comandoInserirEquipesJogadores.Parameters.AddWithValue("@equipe_id", idEquipeCriada);

                    comandoInserirEquipesJogadores.ExecuteNonQuery();

                    // Commit da transação
                    transaction.Commit();

                    // Limpar campos após o cadastro
                    textBox1.Text = "";
                    textBox2.Text = "";
                    textBox3.Text = "";
                    pictureBox1.Image = null;

                    MessageBox.Show("Equipe cadastrada com sucesso e jogador vinculado!");
                }
                catch (MySqlException ex)
                {
                    // Se ocorrer erro, reverter a transação
                    transaction.Rollback();
                    MessageBox.Show($"Erro ao cadastrar equipe: {ex.Message}");
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Arquivos de Imagem|*.jpg;*.jpeg;*.png;*.gif";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string caminhoDaImagem = openFileDialog.FileName;
                string pastaDestino = @"F:\Battle Vortex\Imagens\fotobanco";
                nomeArquivo = Path.GetFileName(caminhoDaImagem);
                caminhoNoServidor = Path.Combine(pastaDestino, nomeArquivo);

                try
                {
                    File.Copy(caminhoDaImagem, caminhoNoServidor, true);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erro ao copiar a imagem: {ex.Message}");
                }

                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                pictureBox1.Image = Image.FromFile(caminhoDaImagem);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
       
            this.Close();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
