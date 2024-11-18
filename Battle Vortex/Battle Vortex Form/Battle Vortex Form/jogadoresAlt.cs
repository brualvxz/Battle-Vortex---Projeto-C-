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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Battle_Vortex_Form
{
    public partial class jogadoresAlt : Form
    {

        
        string caminhoNoServidor;
        string nomeArquivo;

        public jogadoresAlt(string id)
        {
            InitializeComponent();
            // Conexão com o banco de dados eventosbv
            MySqlConnection conexao = new MySqlConnection("SERVER=localhost;DATABASE=eventosbv;UID=root;PASSWORD=");
            conexao.Open();

            // Comando para consultar a tabela 'jogadores' com base no ID do jogador (supondo que 'valor' seja o ID)
            MySqlCommand consulta = new MySqlCommand();
            consulta.Connection = conexao;
            consulta.CommandText = "SELECT * FROM jogadores WHERE id = " + id;

            MySqlDataReader resultado = consulta.ExecuteReader();
            if (resultado.HasRows)
            {
                while (resultado.Read())
                {
                    // Preenche os TextBox com os dados do jogador
                    textBox4.Text = resultado["id"].ToString();                // ID do jogador
                    textBox1.Text = resultado["nome"].ToString();              // Nome do jogador
                    textBox2.Text = resultado["nickname"].ToString();          // Nickname
                    textBox3.Text = resultado["conquistas"].ToString();        // Conquistas

                    // Carrega a imagem do jogador na PictureBox
                    string caminhoImagem = resultado["foto"].ToString();
                    caminhoNoServidor = caminhoImagem.Replace("+", @"\"); // Converte para o formato correto de caminho
                    pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                    pictureBox1.Image = Image.FromFile(caminhoNoServidor); // Exibe a imagem

                    // Preenche a ComboBox de 'Equipe' e 'Personagem Main'
                    comboBox1.SelectedValue = resultado["equipe_id"];       // Seleciona a equipe do jogador
                    comboBox2.SelectedValue = resultado["personagemMain_id"]; // Seleciona o personagem principal do jogador
                }
            }
            else
            {
                MessageBox.Show("Nenhum registro foi encontrado");
            }

            resultado.Close();

            // Carregar os valores das ComboBox de 'equipes' e 'personagemMain'

            // Preenche a ComboBox de 'equipe'
            MySqlCommand consultaEquipe = new MySqlCommand();
            consultaEquipe.Connection = conexao;
            consultaEquipe.CommandText = "SELECT id, nome FROM equipes"; // Ajuste o nome da tabela se necessário

            MySqlDataReader resultadoEquipe = consultaEquipe.ExecuteReader();
            comboBox1.Items.Clear();  // Limpa a ComboBox antes de adicionar novos itens
            while (resultadoEquipe.Read())
            {
                comboBox1.Items.Add(new { Text = resultadoEquipe["nome"].ToString(), Value = resultadoEquipe["id"] });
            }
            resultadoEquipe.Close();

            // Preenche a ComboBox de 'personagemMain'
            MySqlCommand consultaPersonagem = new MySqlCommand();
            consultaPersonagem.Connection = conexao;
            consultaPersonagem.CommandText = "SELECT id, nome FROM personagens"; // Ajuste o nome da tabela se necessário

            MySqlDataReader resultadoPersonagem = consultaPersonagem.ExecuteReader();
            comboBox2.Items.Clear();  // Limpa a ComboBox antes de adicionar novos itens
            while (resultadoPersonagem.Read())
            {
                comboBox2.Items.Add(new { Text = resultadoPersonagem["nome"].ToString(), Value = resultadoPersonagem["id"] });
            }
            resultadoPersonagem.Close();

            // Fechar a conexão
            conexao.Close();

        }

        private void jogadoresAlt_Load(object sender, EventArgs e)
        {

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
            try
            {
                // Conexão com o banco de dados eventosbv
                using (MySqlConnection conexao = new MySqlConnection("SERVER=localhost;DATABASE=eventosbv;UID=root;PASSWORD="))
                {
                    conexao.Open();

                    // Comando para atualizar os dados do jogador com base no ID
                    MySqlCommand comando = new MySqlCommand();
                    comando.Connection = conexao;
                    comando.CommandText = @"UPDATE jogadores SET nome = @nome, nickname = @nickname, conquistas = @conquistas, foto = @foto, equipe_id = @equipe_id, personagemMain_id = @personagemMain_id WHERE id = @id";

                    // Adicionando os parâmetros para a consulta
                    comando.Parameters.AddWithValue("@nome", textBox1.Text); 
                    comando.Parameters.AddWithValue("@nickname", textBox2.Text); 
                    comando.Parameters.AddWithValue("@conquistas", textBox3.Text);  
                    comando.Parameters.AddWithValue("@foto", caminhoNoServidor);  // Caminho da imagem
                    comando.Parameters.AddWithValue("@equipe_id", ((dynamic)comboBox1.SelectedItem).Value);  
                    comando.Parameters.AddWithValue("@personagemMain_id", ((dynamic)comboBox2.SelectedItem).Value);  
                    comando.Parameters.AddWithValue("@id", textBox4.Text);  

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

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
