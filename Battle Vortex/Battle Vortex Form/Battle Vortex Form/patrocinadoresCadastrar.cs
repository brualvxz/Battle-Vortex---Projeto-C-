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
    public partial class patrocinadoresCadastrar : Form
    {
        string caminhoNoServidor;
        string nomeArquivo;

        public patrocinadoresCadastrar()
        {
            InitializeComponent();
            // Estabelece uma conexão com o banco de dados eventosbv
            MySqlConnection conexao = new MySqlConnection("SERVER=127.0.0.1; DATABASE=eventosbv; UID=root; PASSWORD=;");
            conexao.Open();  // Abre a conexão com o banco de dados

          
         
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string campo1 = textBox1.Text;
            
            string campo3 = textBox3.Text;

            MySqlConnection conexao = new MySqlConnection();
            conexao.ConnectionString = ("SERVER=127.0.0.1; DATABASE=eventosbv; UID= root ; PASSWORD = ; ");
            conexao.Open();

            string inserir = "INSERT INTO `patrocinadores`(`logo`, `nome`, `conquistas`) " +
                 "VALUES(@logo, @nome, @conquistas)";

            MySqlCommand comandos = new MySqlCommand(inserir, conexao);
            comandos.Parameters.AddWithValue("@logo", caminhoNoServidor); 
            comandos.Parameters.AddWithValue("@nome", campo1);
            
            comandos.Parameters.AddWithValue("@conquistas", campo3);


            comandos.ExecuteNonQuery(); 

            conexao.Close(); 

            textBox1.Text = "";
          
            textBox3.Text = "";
            pictureBox1.Image = null;

           
           
        
            MessageBox.Show("Patrocinio cadastrado com Sucesso!!!");
        }

        private void patrocinadoresCadastrar_Load(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
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

        private void button3_Click(object sender, EventArgs e)
        {
            homeAdm homeAdm = new homeAdm();
            homeAdm.Show();
            this.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
