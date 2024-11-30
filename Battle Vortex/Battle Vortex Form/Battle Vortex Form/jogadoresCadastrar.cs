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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using MySql.Data.MySqlClient;

namespace Battle_Vortex_Form
{
    public partial class jogadoresCadastrar : Form
    {

        string caminhoNoServidor;
        string nomeArquivo;


        public jogadoresCadastrar()
        {

            InitializeComponent();


            // Estabelece uma conexão com o banco de dados eventosbv
            MySqlConnection conexao = new MySqlConnection("SERVER=127.0.0.1; DATABASE=eventosbv; UID=root; PASSWORD=;");
            conexao.Open();  // Abre a conexão com o banco de dados

            // Consulta SQL para selecionar o código e o nome dos personagens para a segunda ComboBox
            string query2 = "SELECT id, nome FROM personagens";
            MySqlCommand comandos2 = new MySqlCommand(query2, conexao);
            MySqlDataAdapter da2 = new MySqlDataAdapter(comandos2);
            DataTable dt2 = new DataTable();
            da2.Fill(dt2);

            // Preenche a segunda ComboBox com os valores que restaram após a verificação
            comboBox2.DataSource = dt2;
            comboBox2.DisplayMember = "nome";
            comboBox2.ValueMember = "id";

            // Fecha a conexão com o banco de dados
            conexao.Close();

            // Limpa a seleção das ComboBox
        
            comboBox2.SelectedIndex = -1;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Arquivos de Imagem|*.jpg;*.jpeg;*.png;*.gif";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string caminhoDaImagem = openFileDialog.FileName;

                string pastaDestino = @"G:\Battle Vortex\Imagens\fotobanco";
                nomeArquivo = Path.GetFileName(caminhoDaImagem);
                caminhoNoServidor = Path.Combine(pastaDestino, nomeArquivo);

                try
                {
                    File.Copy(caminhoDaImagem, caminhoNoServidor, true); // Adicionei true para permitir sobrescrever se necessário
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erro ao copiar a imagem: {ex.Message}");
                }

                // Define o modo de exibição da PictureBox para ajustar a imagem ao seu tamanho sem distorção
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                pictureBox1.Image = Image.FromFile(caminhoDaImagem);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Conexão com o banco de dados
            MySqlConnection conexao = new MySqlConnection("SERVER=127.0.0.1; DATABASE=eventosbv; UID=root; PASSWORD=;");
            conexao.Open();

            try
            {
                // Verifica se o usuário logado já possui um jogador cadastrado
                string verificaJogadorQuery = "SELECT jogador_id FROM usuarios WHERE id = @usuarioId";
                MySqlCommand verificaComando = new MySqlCommand(verificaJogadorQuery, conexao);
                verificaComando.Parameters.AddWithValue("@usuarioId", UsuarioLogado.Id);

                object resultado = verificaComando.ExecuteScalar();
                if (resultado != null && resultado != DBNull.Value)
                {
                    MessageBox.Show("Você já possui um jogador cadastrado nesta conta.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return; // Interrompe o processo se já houver jogador vinculado
                }

                // Cadastrar novo jogador
                string inserirJogador = "INSERT INTO jogadores (`foto`, `nome`, `nickname`,`personagemMain_id`, `conquistas`) " +
                                        "VALUES (@foto, @nome, @nickname, @personagemMain_id, @conquistas)";
                MySqlCommand comandos = new MySqlCommand(inserirJogador, conexao);
                comandos.Parameters.AddWithValue("@foto", caminhoNoServidor);
                comandos.Parameters.AddWithValue("@nome", textBox1.Text);
                comandos.Parameters.AddWithValue("@nickname", textBox2.Text);
                comandos.Parameters.AddWithValue("@personagemMain_id", comboBox2.SelectedValue);
                comandos.Parameters.AddWithValue("@conquistas", textBox3.Text);

                comandos.ExecuteNonQuery();

                // Obter o ID do jogador recém-cadastrado
                long jogadorId = comandos.LastInsertedId;

                // Atualizar a tabela usuarios para vincular o jogador ao usuário logado
                string atualizarUsuario = "UPDATE usuarios SET jogador_id = @jogadorId WHERE id = @usuarioId";
                MySqlCommand atualizarComando = new MySqlCommand(atualizarUsuario, conexao);
                atualizarComando.Parameters.AddWithValue("@jogadorId", jogadorId);
                atualizarComando.Parameters.AddWithValue("@usuarioId", UsuarioLogado.Id);

                atualizarComando.ExecuteNonQuery();

                MessageBox.Show("Jogador cadastrado e vinculado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Limpar campos após o cadastro
                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
                pictureBox1.Image = null;
               
                comboBox2.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao cadastrar jogador: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                conexao.Close();
            }

        }

        private void jogadoresCadastrar_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

            this.Close();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
