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
        public equipesCadastrar()
        {
            InitializeComponent();
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "dd/MM/yyyy";
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

            MySqlConnection conexao = new MySqlConnection("SERVER=127.0.0.1; DATABASE=eventosbv; UID=root; PASSWORD=;");
            conexao.Open();

            try
            {
                
                string inserir = "INSERT INTO `equipes`(`nome`, `localidade`, `email`, `data_criacao`, `logo`) " +
                                 "VALUES(@nome, @localidade, @email, @data_criacao, @logo)";

                MySqlCommand comandos = new MySqlCommand(inserir, conexao);
                comandos.Parameters.AddWithValue("@nome", nome);
                comandos.Parameters.AddWithValue("@localidade", localidade);
                comandos.Parameters.AddWithValue("@email", email);
                comandos.Parameters.AddWithValue("@data_criacao", dataCriacao.ToString("yyyy-MM-dd")); 
                comandos.Parameters.AddWithValue("@logo", caminhoNoServidor); 

                comandos.ExecuteNonQuery();

                
                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = ""; 
                pictureBox1.Image = null;

                MessageBox.Show("Equipe cadastrada com sucesso!");
            }
            catch (MySqlException ex)
            {
                MessageBox.Show($"Erro ao cadastrar equipe: {ex.Message}");
            }
            finally
            {
                conexao.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Arquivos de Imagem|*.jpg;*.jpeg;*.png;*.gif";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string caminhoDaImagem = openFileDialog.FileName;
                string pastaDestino = @"D:\Battle Vortex\Imagens\fotobanco";
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
            equipesAdm equipesAdm = new equipesAdm();
            equipesAdm.Show();
            this.Close();
        }
    }
}
