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
            // Conexão com o banco de dados
            MySqlConnection conexao = new MySqlConnection("SERVER=127.0.0.1; DATABASE=eventosbv; UID=root; PASSWORD=;");
            conexao.Open();

            try
            {
                // Verifica se o usuário logado já possui um patrocinador vinculado
                string verificaPatrocinadorQuery = "SELECT patrocinador_id FROM usuarios WHERE id = @usuarioId";
                MySqlCommand verificaComando = new MySqlCommand(verificaPatrocinadorQuery, conexao);
                verificaComando.Parameters.AddWithValue("@usuarioId", UsuarioLogado.Id);

                object resultado = verificaComando.ExecuteScalar();
                if (resultado != null && resultado != DBNull.Value)
                {
                    MessageBox.Show("Você já possui um patrocinador cadastrado nesta conta.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return; // Interrompe o processo se já houver patrocinador vinculado
                }

                string inserirPatrocinador = "INSERT INTO patrocinadores (`nome`, `conquistas`, `logo`, `dono_id`) " +
                              "VALUES (@nome, @conquistas, @logo, @donoId)";
                MySqlCommand comandos = new MySqlCommand(inserirPatrocinador, conexao);
                comandos.Parameters.AddWithValue("@nome", textBox1.Text);
                comandos.Parameters.AddWithValue("@conquistas", textBox3.Text);
                comandos.Parameters.AddWithValue("@logo", caminhoNoServidor);
                
                comandos.Parameters.AddWithValue("@donoId", UsuarioLogado.Id); // Vincula o patrocinador ao usuário logado



                comandos.ExecuteNonQuery();

                // Obter o ID do patrocinador recém-cadastrado
                long patrocinadorId = comandos.LastInsertedId;

                // Atualizar a tabela usuarios para vincular o patrocinador ao usuário logado
                string atualizarUsuario = "UPDATE usuarios SET patrocinador_id = @patrocinadorId WHERE id = @usuarioId";
                MySqlCommand atualizarComando = new MySqlCommand(atualizarUsuario, conexao);
                atualizarComando.Parameters.AddWithValue("@patrocinadorId", patrocinadorId);
                atualizarComando.Parameters.AddWithValue("@usuarioId", UsuarioLogado.Id);

                atualizarComando.ExecuteNonQuery();

                MessageBox.Show("Patrocinador cadastrado e vinculado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Limpar campos após o cadastro
                textBox1.Text = "";
                textBox3.Text = "";
                pictureBox1.Image = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao cadastrar patrocinador: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                conexao.Close();
            }
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

                string pastaDestino = @"F:\Battle Vortex\Imagens\fotobanco";
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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
