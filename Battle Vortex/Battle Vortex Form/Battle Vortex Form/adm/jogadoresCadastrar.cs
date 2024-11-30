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
    public partial class jogadoresCadastrar : Form
    {

        string caminhoNoServidor;
        string nomeArquivo;

        public jogadoresCadastrar()
        {

            InitializeComponent();
            // Estabelece uma conexão com o banco de dados eventosbv
            MySqlConnection conexao = new MySqlConnection("SERVER=127.0.0.1; DATABASE=eventosbv; UID=root; PASSWORD=;");
            conexao.Open();  // Abre a conexão com o banco de dados

            // Consulta SQL para selecionar o código e o nome das equipes para a primeira ComboBox
            string query1 = "SELECT id, nome FROM equipes";
            MySqlCommand comandos1 = new MySqlCommand(query1, conexao);
            MySqlDataAdapter da1 = new MySqlDataAdapter(comandos1);
            DataTable dt1 = new DataTable();
            da1.Fill(dt1);

            // Preenche a primeira ComboBox
            comboBox1.DataSource = dt1;
            comboBox1.DisplayMember = "nome";
            comboBox1.ValueMember = "id";

            // Consulta SQL para selecionar o código e o nome dos personagens para a segunda ComboBox
            string query2 = "SELECT id, nome FROM personagens";
            MySqlCommand comandos2 = new MySqlCommand(query2, conexao);
            MySqlDataAdapter da2 = new MySqlDataAdapter(comandos2);
            DataTable dt2 = new DataTable();
            da2.Fill(dt2);

            // Verifica se a primeira `ComboBox` possui itens para remover do segundo conjunto, evitando repetição
            foreach (DataRow row in dt1.Rows)
            {
                for (int i = dt2.Rows.Count - 1; i >= 0; i--)
                {
                    if (dt2.Rows[i]["nome"].ToString() == row["nome"].ToString())
                    {
                        dt2.Rows.RemoveAt(i);
                    }
                }
            }

            // Preenche a segunda ComboBox com os valores que restaram após a verificação
            comboBox2.DataSource = dt2;
            comboBox2.DisplayMember = "nome";
            comboBox2.ValueMember = "id";

            // Fecha a conexão com o banco de dados
            conexao.Close();

            // Limpa a seleção das ComboBox
            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;
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
            string campo1 = textBox1.Text;
            string campo2 = textBox2.Text;
            string campo3 = textBox3.Text;

            MySqlConnection conexao = new MySqlConnection();
            conexao.ConnectionString = ("SERVER=127.0.0.1; DATABASE=eventosbv; UID= root ; PASSWORD = ; ");//indica o caminho e dados do banco
            conexao.Open();//abrindo o banco

            string inserir = "INSERT INTO jogadores(`foto`, `nome`, `nickname`, `equipe_id`, `personagemMain_id`, `conquistas`) " +
                 "VALUES(@foto, @nome, @nickname, @equipe_id, @personagemMain_id, @conquistas)"; 

            MySqlCommand comandos = new MySqlCommand(inserir, conexao);
            comandos.Parameters.AddWithValue("@foto", caminhoNoServidor);  // Insere o caminho da imagem
            comandos.Parameters.AddWithValue("@nome", campo1);
            comandos.Parameters.AddWithValue("@nickname", campo2);
            comandos.Parameters.AddWithValue("@equipe_id", comboBox1.SelectedValue);
            comandos.Parameters.AddWithValue("@personagemMain_id", comboBox2.SelectedValue);
            comandos.Parameters.AddWithValue("@conquistas", campo3);
         

            comandos.ExecuteNonQuery(); //executa o comando no banco

            conexao.Close(); //fechando a conexão com o banco de dados

            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            pictureBox1.Image = null;

            // Limpa a seleção das ComboBox
            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;
            MessageBox.Show("Jogador cadastrado com Sucesso!!!");

           
        }

        private void jogadoresCadastrar_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            jogadoresAdm jogadores = new jogadoresAdm();   
            jogadores.Show();
            this.Close();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
