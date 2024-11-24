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
           
            MySqlConnection conexao = new MySqlConnection("SERVER=localhost;DATABASE=eventosbv;UID=root;PASSWORD=");
            conexao.Open();

            
            MySqlCommand consulta = new MySqlCommand();
            consulta.Connection = conexao;
            consulta.CommandText = "SELECT * FROM patrocinadores WHERE id = " + id;

            MySqlDataReader resultado = consulta.ExecuteReader();
            if (resultado.HasRows)
            {
                while (resultado.Read())
                {
                   
                    textBox2.Text = resultado["id"].ToString();               
                    textBox1.Text = resultado["nome"].ToString();              
                    textBox3.Text = resultado["conquistas"].ToString();        

                    
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
                    comando.CommandText = @"UPDATE patrocinadores SET nome = @nome, conquistas = @conquistas, logo = @logo WHERE id = @id";

                    // Adicionando os parâmetros para a consulta
                    comando.Parameters.AddWithValue("@nome", textBox1.Text);
                    comando.Parameters.AddWithValue("@conquistas", textBox3.Text);
                    comando.Parameters.AddWithValue("@logo", caminhoNoServidor);  
                   
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
            this.Close();
        }
    }
}
