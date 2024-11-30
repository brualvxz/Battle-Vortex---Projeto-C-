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
            using (MySqlConnection conexao = new MySqlConnection("SERVER=localhost;DATABASE=eventosbv;UID=root;PASSWORD="))
            {
                conexao.Open();

                // Consulta para preencher os campos do jogador
                MySqlCommand consulta = new MySqlCommand("SELECT * FROM jogadores WHERE id = @id", conexao);
                consulta.Parameters.AddWithValue("@id", id);
                MySqlDataReader resultado = consulta.ExecuteReader();

                if (resultado.HasRows)
                {
                    while (resultado.Read())
                    {
                        textBox4.Text = resultado["id"].ToString();
                        textBox1.Text = resultado["nome"].ToString();
                        textBox2.Text = resultado["nickname"].ToString();
                        textBox3.Text = resultado["conquistas"].ToString();

                        string caminhoImagem = resultado["foto"].ToString();
                        caminhoNoServidor = caminhoImagem.Replace("+", @"\");

                        if (File.Exists(caminhoNoServidor))
                        {
                            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                            pictureBox1.Image = Image.FromFile(caminhoNoServidor);
                        }

                        comboBox2.Tag = resultado["personagemMain_id"];
                    }
                }
                else
                {
                    MessageBox.Show("Nenhum registro encontrado.");
                }

                resultado.Close();

                // Preencher a ComboBox com personagens
                MySqlCommand consultaPersonagem = new MySqlCommand("SELECT id, nome FROM personagens", conexao);
                MySqlDataAdapter da = new MySqlDataAdapter(consultaPersonagem);
                DataTable dt = new DataTable();
                da.Fill(dt);

                comboBox2.DataSource = dt;
                comboBox2.DisplayMember = "nome";
                comboBox2.ValueMember = "id";

                if (comboBox2.Tag != null)
                {
                    comboBox2.SelectedValue = comboBox2.Tag;
                }
            }

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

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // Conexão com o banco de dados eventosbv
                using (MySqlConnection conexao = new MySqlConnection("SERVER=localhost;DATABASE=eventosbv;UID=root;PASSWORD="))
                {
                    conexao.Open();

                    MySqlCommand comando = new MySqlCommand(
                        @"UPDATE jogadores SET nome = @nome, nickname = @nickname, conquistas = @conquistas, 
                        foto = @foto, personagemMain_id = @personagemMain_id WHERE id = @id", conexao);

                    comando.Parameters.AddWithValue("@nome", textBox1.Text);
                    comando.Parameters.AddWithValue("@nickname", textBox2.Text);
                    comando.Parameters.AddWithValue("@conquistas", textBox3.Text);
                    comando.Parameters.AddWithValue("@foto", caminhoNoServidor);
                    comando.Parameters.AddWithValue("@personagemMain_id", comboBox2.SelectedValue);
                    comando.Parameters.AddWithValue("@id", textBox4.Text);

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
