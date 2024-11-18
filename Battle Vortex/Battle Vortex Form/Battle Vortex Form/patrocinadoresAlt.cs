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
    public partial class patrocinadoresAlt : Form
    {
        string caminhoNoServidor;
        string nomeArquivo;

        public patrocinadoresAlt(string id)
        {
            InitializeComponent();
            // Conexão com o banco de dados eventosbv
            MySqlConnection conexao = new MySqlConnection("SERVER=localhost;DATABASE=eventosbv;UID=root;PASSWORD=");
            conexao.Open();

            // Comando para consultar a tabela 'patrocinadores' com base no ID do patrocinador
            MySqlCommand consulta = new MySqlCommand();
            consulta.Connection = conexao;
            consulta.CommandText = "SELECT * FROM patrocinadores WHERE id = " + id;

            MySqlDataReader resultado = consulta.ExecuteReader();
            if (resultado.HasRows)
            {
                while (resultado.Read())
                {
                    // Preenche os TextBox com os dados do patrocinador
                    textBox2.Text = resultado["id"].ToString();                // ID do patrocinador
                    textBox1.Text = resultado["nome"].ToString();              // Nome do patrocinador
                    textBox3.Text = resultado["conquistas"].ToString();        // Conquistas

                    // Carrega a imagem do patrocinador na PictureBox
                    string caminhoImagem = resultado["logo"].ToString();
                    caminhoNoServidor = caminhoImagem.Replace("+", @"\"); // Converte para o formato correto de caminho
                    pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                    pictureBox1.Image = Image.FromFile(caminhoNoServidor); // Exibe a imagem

                    // Preenche a ComboBox com o evento patrocinado
                    comboBox1.SelectedValue = resultado["evento_patrocinado"];  // Evento patrocinado
                }
            }
            else
            {
                MessageBox.Show("Nenhum registro foi encontrado");
            }

            resultado.Close();

            // Carrega os valores para a ComboBox 'evento_patrocinado'
            MySqlCommand consultaEvento = new MySqlCommand();
            consultaEvento.Connection = conexao;
            consultaEvento.CommandText = "SELECT id, nome FROM torneios"; // Ajuste o nome da tabela de torneios

            MySqlDataReader resultadoEvento = consultaEvento.ExecuteReader();
            comboBox1.Items.Clear();  // Limpa a ComboBox antes de adicionar novos itens
            while (resultadoEvento.Read())
            {
                comboBox1.Items.Add(new { Text = resultadoEvento["nome"].ToString(), Value = resultadoEvento["id"] });
            }
            resultadoEvento.Close();

            // Fechar a conexão
            conexao.Close();
        }

        private void patrocinadoresAlt_Load(object sender, EventArgs e)
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

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // Conexão com o banco de dados eventosbv
                using (MySqlConnection conexao = new MySqlConnection("SERVER=localhost;DATABASE=eventosbv;UID=root;PASSWORD="))
                {
                    conexao.Open();

                    // Comando para atualizar os dados do patrocinador com base no ID
                    MySqlCommand comando = new MySqlCommand();
                    comando.Connection = conexao;
                    comando.CommandText = @"UPDATE patrocinadores SET nome = @nome, conquistas = @conquistas, logo = @logo, evento_patrocinado = @evento_patrocinado WHERE id = @id";

                    // Adicionando os parâmetros para a consulta
                    comando.Parameters.AddWithValue("@nome", textBox1.Text);
                    comando.Parameters.AddWithValue("@conquistas", textBox3.Text);
                    comando.Parameters.AddWithValue("@logo", caminhoNoServidor);  // Caminho da imagem
                    comando.Parameters.AddWithValue("@evento_patrocinado", ((dynamic)comboBox1.SelectedItem).Value);
                    comando.Parameters.AddWithValue("@id", textBox2.Text);

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

        private void button3_Click(object sender, EventArgs e)
        {
            patrocinadoresAdm patrocinadoresAdm = new patrocinadoresAdm();
            patrocinadoresAdm.Show();
            this.Close();
        }
    }
}
