using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using MySql.Data.MySqlClient;
using System.IO;

namespace Battle_Vortex_Form
{
    public partial class premioAlt : Form
    {
        string caminhoNoServidor1, caminhoNoServidor2, caminhoNoServidor3;
        string nomeArquivo1, nomeArquivo2, nomeArquivo3;

       

        public premioAlt(string id)
        {
            InitializeComponent();
            // Carregar dados do prêmio
            using (MySqlConnection conexao = new MySqlConnection("SERVER=localhost;DATABASE=eventosbv;UID=root;PASSWORD="))
            {
                conexao.Open();
                MySqlCommand consulta = new MySqlCommand("SELECT * FROM premios WHERE id = @id", conexao);
                consulta.Parameters.AddWithValue("@id", id);

                using (MySqlDataReader resultado = consulta.ExecuteReader())
                {
                    if (resultado.HasRows)
                    {
                        while (resultado.Read())
                        {
                            textBox4.Text = resultado["descricao"].ToString();
                            textBox1.Text = resultado["premio_principal"].ToString();
                            textBox2.Text = resultado["premio_secundario"].ToString();
                            textBox3.Text = resultado["premio_terciario"].ToString();
                            textBox5.Text = resultado["id"].ToString();

                            // Carregar imagens
                            string caminhoImagemPrincipal = resultado["logo_premio_principal"].ToString();
                            if (!string.IsNullOrEmpty(caminhoImagemPrincipal) && File.Exists(caminhoImagemPrincipal))
                            {
                                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                                pictureBox1.Image = Image.FromFile(caminhoImagemPrincipal);
                                caminhoNoServidor1 = caminhoImagemPrincipal;
                            }

                            string caminhoImagemSecundario = resultado["logo_premio_secundario"].ToString();
                            if (!string.IsNullOrEmpty(caminhoImagemSecundario) && File.Exists(caminhoImagemSecundario))
                            {
                                pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
                                pictureBox2.Image = Image.FromFile(caminhoImagemSecundario);
                                caminhoNoServidor2 = caminhoImagemSecundario;
                            }

                            string caminhoImagemTerciario = resultado["logo_premio_terciario"].ToString();
                            if (!string.IsNullOrEmpty(caminhoImagemTerciario) && File.Exists(caminhoImagemTerciario))
                            {
                                pictureBox3.SizeMode = PictureBoxSizeMode.StretchImage;
                                pictureBox3.Image = Image.FromFile(caminhoImagemTerciario);
                                caminhoNoServidor3 = caminhoImagemTerciario;
                            }

                            // Preencher comboboxes
                            comboBox1.SelectedValue = resultado["torneio_id"];
                            comboBox2.SelectedItem = resultado["tipo_origem"];
                            comboBox3.SelectedValue = resultado["patrocinador_id"];
                        }
                    }
                    else
                    {
                        MessageBox.Show("Nenhum prêmio encontrado.");
                    }
                }
            }

            // Preencher ComboBox de torneios e patrocinadores
            PreencherComboBoxTorneios();
            PreencherComboBoxTipoOrigem();
            PreencherComboBoxPatrocinadores();
        }

        private void PreencherComboBoxTorneios()
        {
            MySqlConnection conexao = new MySqlConnection("SERVER=localhost;DATABASE=eventosbv;UID=root;PASSWORD=");
            conexao.Open();

            string query1 = "SELECT id, nome FROM torneios";
            MySqlCommand comandos1 = new MySqlCommand(query1, conexao);
            MySqlDataAdapter da1 = new MySqlDataAdapter(comandos1);
            DataTable dt1 = new DataTable();
            da1.Fill(dt1);

            comboBox1.DataSource = dt1;
            comboBox1.DisplayMember = "nome";
            comboBox1.ValueMember = "id";

            conexao.Close();
        }

        private void PreencherComboBoxTipoOrigem()
        {
            comboBox2.Items.Clear();
            comboBox2.Items.Add("Evento");
            comboBox2.Items.Add("Patrocinador");
            comboBox2.SelectedIndex = 0;
        }

        private void PreencherComboBoxPatrocinadores()
        {
            MySqlConnection conexao = new MySqlConnection("SERVER=localhost;DATABASE=eventosbv;UID=root;PASSWORD=");
            conexao.Open();

            string query2 = "SELECT id, nome FROM patrocinadores";
            MySqlCommand comandos2 = new MySqlCommand(query2, conexao);
            MySqlDataAdapter da2 = new MySqlDataAdapter(comandos2);
            DataTable dt2 = new DataTable();
            da2.Fill(dt2);

            // Adicionando a opção "Nenhum Patrocinador" com valor null
            DataRow row = dt2.NewRow();
            row["id"] = DBNull.Value;
            row["nome"] = "Nenhum Patrocinador";
            dt2.Rows.InsertAt(row, 0);

            comboBox3.DataSource = dt2;
            comboBox3.DisplayMember = "nome";
            comboBox3.ValueMember = "id";

            conexao.Close();
        }
        private void CarregarImagem(string caminhoImagem, PictureBox pictureBox, ref string caminhoNoServidor)
        {
            if (!string.IsNullOrEmpty(caminhoImagem) && File.Exists(caminhoImagem))
            {
                pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                pictureBox.Image = Image.FromFile(caminhoImagem);
                caminhoNoServidor = caminhoImagem;
            }
        }

        private void premioAlt_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Imagens|*.jpg;*.jpeg;*.png";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string caminhoOrigem = openFileDialog.FileName;
                string pastaDestino = @"D:\Battle Vortex\Imagens\fotobanco";
                nomeArquivo1 = Path.GetFileName(caminhoOrigem);
                caminhoNoServidor1 = Path.Combine(pastaDestino, nomeArquivo1);

                try
                {
                    File.Copy(caminhoOrigem, caminhoNoServidor1, true);
                    pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                    pictureBox1.Image = Image.FromFile(caminhoOrigem);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erro ao copiar a imagem: {ex.Message}");
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Imagens|*.jpg;*.jpeg;*.png";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string caminhoOrigem = openFileDialog.FileName;
                string pastaDestino = @"D:\Battle Vortex\Imagens\fotobanco";
                nomeArquivo2 = Path.GetFileName(caminhoOrigem);
                caminhoNoServidor2 = Path.Combine(pastaDestino, nomeArquivo2);

                try
                {
                    File.Copy(caminhoOrigem, caminhoNoServidor2, true);
                    pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
                    pictureBox2.Image = Image.FromFile(caminhoOrigem);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erro ao copiar a imagem: {ex.Message}");
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Imagens|*.jpg;*.jpeg;*.png";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string caminhoOrigem = openFileDialog.FileName;
                string pastaDestino = @"D:\Battle Vortex\Imagens\fotobanco";
                nomeArquivo3 = Path.GetFileName(caminhoOrigem);
                caminhoNoServidor3 = Path.Combine(pastaDestino, nomeArquivo3);

                try
                {
                    File.Copy(caminhoOrigem, caminhoNoServidor3, true);
                    pictureBox3.SizeMode = PictureBoxSizeMode.StretchImage;
                    pictureBox3.Image = Image.FromFile(caminhoOrigem);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erro ao copiar a imagem: {ex.Message}");
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

                    // Atualizar dados do prêmio
                    MySqlCommand comando = new MySqlCommand(@"UPDATE premios SET descricao = @descricao, premio_principal = @premioPrincipal,
                        premio_secundario = @premioSecundario, premio_terciario = @premioTerciario, 
                        logo_premio_principal = @logoPremioPrincipal, logo_premio_secundario = @logoPremioSecundario, 
                        logo_premio_terciario = @logoPremioTerciario, tipo_origem = @tipoOrigem, patrocinador_id = @patrocinadorId
                        WHERE id = @id", conexao);

                    // Adicionando parâmetros
                    comando.Parameters.AddWithValue("@descricao", textBox4.Text);
                    comando.Parameters.AddWithValue("@premioPrincipal", textBox1.Text);
                    comando.Parameters.AddWithValue("@premioSecundario", textBox2.Text);
                    comando.Parameters.AddWithValue("@premioTerciario", textBox3.Text);
                    comando.Parameters.AddWithValue("@logoPremioPrincipal", caminhoNoServidor1);
                    comando.Parameters.AddWithValue("@logoPremioSecundario", caminhoNoServidor2);
                    comando.Parameters.AddWithValue("@logoPremioTerciario", caminhoNoServidor3);
                    comando.Parameters.AddWithValue("@tipoOrigem", comboBox2.SelectedItem.ToString());
                    comando.Parameters.AddWithValue("@patrocinadorId", comboBox3.SelectedValue);
                    comando.Parameters.AddWithValue("@id", textBox5.Text);

                    int linhasAfetadas = comando.ExecuteNonQuery();

                    if (linhasAfetadas > 0)
                    {
                        MessageBox.Show("Prêmio atualizado com sucesso!");
                    }
                    else
                    {
                        MessageBox.Show("Nenhuma alteração foi realizada.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao atualizar o prêmio: {ex.Message}");
            }
        }
    }
}
