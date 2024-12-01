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
    public partial class torneiosCadastrar : Form
    {
        string caminhoNoServidor;
        string nomeArquivo;
        public torneiosCadastrar()
        {
            InitializeComponent();
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "dd/MM/yyyy HH:mm";
            dateTimePicker1.ShowUpDown = true;

            dateTimePicker2.Format = DateTimePickerFormat.Custom;
            dateTimePicker2.CustomFormat = "dd/MM/yyyy HH:mm";
            dateTimePicker2.ShowUpDown = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string nome = textBox1.Text;
            string local = textBox2.Text;
            string descricao = textBox3.Text;
            string regras = textBox4.Text;
            int vagas;

            if (!int.TryParse(textBox5.Text, out vagas))
            {
                MessageBox.Show("Por favor, insira um número válido para as vagas.");
                return;
            }

            DateTime dataInicio = dateTimePicker1.Value;
            DateTime dataFim = dateTimePicker2.Value;

            MySqlConnection conexao = new MySqlConnection("SERVER=127.0.0.1; DATABASE=eventosbv; UID=root; PASSWORD=;");
            conexao.Open();

            try
            {
                // Insere o torneio no banco de dados
                string inserir = "INSERT INTO `torneios`(`nome`, `data_inicio`, `data_fim`, `local`, `descricao`, `regras`, `vagas`, `logo`, `organizador_id`) " +
                                 "VALUES(@nome, @data_inicio, @data_fim, @local, @descricao, @regras, @vagas, @logo, @organizadorid)";

                MySqlCommand comandos = new MySqlCommand(inserir, conexao);
                comandos.Parameters.AddWithValue("@nome", nome);
                comandos.Parameters.AddWithValue("@data_inicio", dataInicio);
                comandos.Parameters.AddWithValue("@data_fim", dataFim);
                comandos.Parameters.AddWithValue("@local", local);
                comandos.Parameters.AddWithValue("@descricao", descricao);
                comandos.Parameters.AddWithValue("@regras", regras);
                comandos.Parameters.AddWithValue("@vagas", vagas);
                comandos.Parameters.AddWithValue("@logo", caminhoNoServidor);
                comandos.Parameters.AddWithValue("@organizadorid", UsuarioLogado.Id);// Caminho da imagem

                comandos.ExecuteNonQuery();

                // Obter o ID do torneio recém-cadastrado
                long torneioId = comandos.LastInsertedId;

                // Atualizar a tabela usuarios para vincular o torneio como organizador
                string atualizarUsuario = "UPDATE usuarios SET organizador_id = @torneioId WHERE id = @usuarioId";
                MySqlCommand atualizarComando = new MySqlCommand(atualizarUsuario, conexao);
                atualizarComando.Parameters.AddWithValue("@torneioId", torneioId);
                atualizarComando.Parameters.AddWithValue("@usuarioId", UsuarioLogado.Id);

                atualizarComando.ExecuteNonQuery();

                MessageBox.Show("Torneio cadastrado e vinculado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Limpa os campos após o cadastro
                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
                textBox4.Text = "";
                textBox5.Text = "";
                pictureBox1.Image = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao cadastrar torneio: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
         
            this.Close();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }
    }
}

