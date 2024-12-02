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
    public partial class torneiosAlt : Form
    {

        string caminhoNoServidor;
        string nomeArquivo;

        public torneiosAlt(string id)
        {
            InitializeComponent();

           
            using (MySqlConnection conexao = new MySqlConnection("SERVER=localhost;DATABASE=eventosbv;UID=root;PASSWORD="))
            {
                conexao.Open();

                
                MySqlCommand consulta = new MySqlCommand("SELECT * FROM torneios WHERE id = @id", conexao);
                consulta.Parameters.AddWithValue("@id", id);

                using (MySqlDataReader resultado = consulta.ExecuteReader())
                {
                    if (resultado.HasRows)
                    {
                        while (resultado.Read())
                        {
                            textBox6.Text = resultado["id"].ToString();
                            textBox1.Text = resultado["nome"].ToString();               
                            textBox2.Text = resultado["local"].ToString();             
                            textBox3.Text = resultado["descricao"].ToString();
                            textBox4.Text = resultado["regras"].ToString();              
                            textBox5.Text = resultado["vagas"].ToString();
                           
                            dateTimePicker1.Value = Convert.ToDateTime(resultado["data_inicio"]); 
                            dateTimePicker2.Value = Convert.ToDateTime(resultado["data_fim"]);    

                            // Carregar imagem
                            string caminhoImagem = resultado["logo"].ToString();
                            if (!string.IsNullOrEmpty(caminhoImagem) && File.Exists(caminhoImagem))
                            {
                                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                                pictureBox1.Image = Image.FromFile(caminhoImagem);
                                caminhoNoServidor = caminhoImagem;
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Nenhum registro encontrado.");
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // Conexão com o banco de dados
                using (MySqlConnection conexao = new MySqlConnection("SERVER=localhost;DATABASE=eventosbv;UID=root;PASSWORD="))
                {
                    conexao.Open();

                    // Atualizar dados do torneio
                    MySqlCommand comando = new MySqlCommand(@" UPDATE torneios SET nome = @nome, local = @local, descricao = @descricao, vagas = @vagas, 
                    data_inicio = @dataInicio, data_fim = @dataFim, regras = @regras, logo = @logo  WHERE id = @id", conexao);

                    // Adicionando parâmetros
                    comando.Parameters.AddWithValue("@nome", textBox1.Text);
                    comando.Parameters.AddWithValue("@local", textBox2.Text);
                    comando.Parameters.AddWithValue("@descricao", textBox3.Text);
                    comando.Parameters.AddWithValue("@regras", textBox4.Text);
                    comando.Parameters.AddWithValue("@vagas", textBox5.Text);
                    comando.Parameters.AddWithValue("@dataInicio", dateTimePicker1.Value);
                    comando.Parameters.AddWithValue("@dataFim", dateTimePicker2.Value);
                    comando.Parameters.AddWithValue("@logo", caminhoNoServidor);
                    comando.Parameters.AddWithValue("@id", textBox6.Text);

                    int linhasAfetadas = comando.ExecuteNonQuery();

                    if (linhasAfetadas > 0)
                    {
                        MessageBox.Show("Torneio atualizado com sucesso!");
                    }
                    else
                    {
                        MessageBox.Show("Nenhuma alteração foi realizada.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao atualizar o torneio: {ex.Message}");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Imagens|*.jpg;*.jpeg;*.png";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string caminhoOrigem = openFileDialog.FileName;
                string pastaDestino = @"F:\Battle Vortex\Imagens\fotobanco";
                nomeArquivo = Path.GetFileName(caminhoOrigem);
                caminhoNoServidor = Path.Combine(pastaDestino, nomeArquivo);

                try
                {
                    File.Copy(caminhoOrigem, caminhoNoServidor, true);
                    pictureBox1.Image = Image.FromFile(caminhoOrigem);
                    pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erro ao carregar a imagem: {ex.Message}");
                }
            }
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
