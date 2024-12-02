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
    public partial class equipesAlt : Form
    {
        string caminhoNoServidor;
        string nomeArquivo;
        public equipesAlt(string id)
        {
            InitializeComponent();
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "dd/MM/yyyy";
            MySqlConnection conexao = new MySqlConnection("SERVER=localhost;DATABASE=eventosbv;UID=root;PASSWORD=");
            conexao.Open();

            
            MySqlCommand consulta = new MySqlCommand();
            consulta.Connection = conexao;
            consulta.CommandText = "SELECT * FROM equipes WHERE id = " + id;

            MySqlDataReader resultado = consulta.ExecuteReader();
            if (resultado.HasRows)
            {
                while (resultado.Read())
                {
                   
                    textBox4.Text = resultado["id"].ToString();  
                    textBox1.Text = resultado["nome"].ToString(); 
                    textBox2.Text = resultado["localidade"].ToString(); 
                    textBox3.Text = resultado["email"].ToString();  
                    dateTimePicker1.Value = Convert.ToDateTime(resultado["data_criacao"]); 

                    
                    string caminhoImagem = resultado["logo"].ToString();
                    caminhoNoServidor = caminhoImagem.Replace("+", @"\"); 
                    pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                    pictureBox1.Image = Image.FromFile(caminhoNoServidor);
                }
            }
            else
            {
                MessageBox.Show("Nenhum registro foi encontrado");
            }

            resultado.Close();

            // Fechar a conexão
            conexao.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // Conexão com o banco de dados eventosbv
                using (MySqlConnection conexao = new MySqlConnection("SERVER=localhost;DATABASE=eventosbv;UID=root;PASSWORD="))
                {
                    conexao.Open();

                    // Comando para atualizar os dados da equipe com base no ID
                    MySqlCommand comando = new MySqlCommand();
                    comando.Connection = conexao;
                    comando.CommandText = @"UPDATE equipes SET nome = @nome, localidade = @localidade, email = @email, data_criacao = @data_criacao, logo = @logo WHERE id = @id";

                    // Adicionando os parâmetros para a consulta
                    comando.Parameters.AddWithValue("@nome", textBox1.Text);
                    comando.Parameters.AddWithValue("@localidade", textBox2.Text);
                    comando.Parameters.AddWithValue("@email", textBox3.Text);
                    comando.Parameters.AddWithValue("@data_criacao", dateTimePicker1.Value.ToString("yyyy-MM-dd"));
                    comando.Parameters.AddWithValue("@logo", caminhoNoServidor);  // Caminho da imagem
                    comando.Parameters.AddWithValue("@id", textBox4.Text);  // ID da equipe

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

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
