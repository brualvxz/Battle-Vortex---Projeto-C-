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
    public partial class premiosCadastrar : Form
    {

       

        public premiosCadastrar()
        {
            InitializeComponent();
           
            PreencherComboBoxTorneios();
            PreencherComboBoxTipoOrigem();
            PreencherComboBoxPatrocinadores();
        }

        private void PreencherComboBoxTorneios()
        {
            MySqlConnection conexao = new MySqlConnection("SERVER=127.0.0.1; DATABASE=eventosbv; UID=root; PASSWORD=;");
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
            MySqlConnection conexao = new MySqlConnection("SERVER=127.0.0.1; DATABASE=eventosbv; UID=root; PASSWORD=;");
            conexao.Open();

            string query2 = "SELECT id, nome FROM patrocinadores";
            MySqlCommand comandos2 = new MySqlCommand(query2, conexao);
            MySqlDataAdapter da2 = new MySqlDataAdapter(comandos2);
            DataTable dt2 = new DataTable();
            da2.Fill(dt2);

            
            DataRow row = dt2.NewRow();
            row["id"] = DBNull.Value; 
            row["nome"] = "Nenhum Patrocinador";
            dt2.Rows.InsertAt(row, 0);

           
            comboBox3.DataSource = dt2;
            comboBox3.DisplayMember = "nome"; 
            comboBox3.ValueMember = "id"; 

            conexao.Close();
        }


        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

   

   

    

        private void premiosCadastrar_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            premiosAdm premiosAdm = new premiosAdm();
            premiosAdm.Show();
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string descricao = textBox4.Text;
            string tipo = comboBox2.SelectedItem.ToString(); // Tipo de prêmio (Evento ou Patrocinador)
       

            if (comboBox1.SelectedItem == null)
            {
                MessageBox.Show("Por favor, selecione um torneio.");
                return;
            }

            int torneioId = (int)((DataRowView)comboBox1.SelectedItem)["id"];
            string premioPrincipal = textBox1.Text;
            string premioSecundario = textBox2.Text;
            string premioTerciario = textBox3.Text;

            MySqlConnection conexao = new MySqlConnection("SERVER=127.0.0.1; DATABASE=eventosbv; UID=root; PASSWORD=;");
            conexao.Open();

            string inserir = "INSERT INTO `premios`(`torneio_id`, `descricao`, `premio_principal`, `premio_secundario`, `premio_terciario`, " +
                            "`logo_premio_principal`, `logo_premio_secundario`, `logo_premio_terciario`, `tipo_origem`, `patrocinador_id`) " +
                            "VALUES(@torneio_id, @descricao, @premio_principal, @premio_secundario, @premio_terciario,  @tipo_origem, @patrocinador_id)";


            MySqlCommand comandos = new MySqlCommand(inserir, conexao);

            comandos.Parameters.AddWithValue("@torneio_id", torneioId);
            comandos.Parameters.AddWithValue("@descricao", descricao);
            comandos.Parameters.AddWithValue("@tipo_origem", comboBox2.SelectedItem.ToString()); // Evento ou Patrocinador
            comandos.Parameters.AddWithValue("@patrocinador_id", comboBox3.SelectedValue);
            comandos.Parameters.AddWithValue("@premio_principal", premioPrincipal);
            comandos.Parameters.AddWithValue("@premio_secundario", premioSecundario);
            comandos.Parameters.AddWithValue("@premio_terciario", premioTerciario);
            
            
            comandos.ExecuteNonQuery(); //executa o comando no banco

            conexao.Close(); //fechando a conexão com o banco de dados

            

            // Limpa os campos após o cadastro
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;
            comboBox3.SelectedIndex = 0;
           

            MessageBox.Show("Prêmio cadastrado com sucesso!");
        }
    }
}
