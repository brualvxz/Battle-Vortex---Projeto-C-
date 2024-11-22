﻿using System;
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
    public partial class premiosAdm : Form
    {
        public premiosAdm()
        {
            InitializeComponent();
            CarregarDados();
        }

        private void CarregarDados()
        {
            using (MySqlConnection conexao = new MySqlConnection("SERVER=127.0.0.1; DATABASE=eventosbv; UID=root; PASSWORD=;"))
            {
                conexao.Open();

                string query = "SELECT id, torneio_id, descricao, premio_principal, logo_premio_principal, premio_secundario, logo_premio_secundario, premio_terciario, logo_premio_terciario, tipo_origem, patrocinador_id FROM premios";
                MySqlCommand consultaDados = new MySqlCommand(query, conexao);
                MySqlDataAdapter da = new MySqlDataAdapter(consultaDados);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dataGridView1.DataSource = null;
                dataGridView1.Columns.Clear();
                dataGridView1.DataSource = dt;

                dataGridView1.Columns["logo_premio_principal"].Visible = false;
                dataGridView1.Columns["logo_premio_secundario"].Visible = false;
                dataGridView1.Columns["logo_premio_terciario"].Visible = false;

                AdicionarBotoesDataGridView();
                PreencherComboBoxComColunas(dt);
            }
        }

        private void AdicionarBotoesDataGridView()
        {
            if (!dataGridView1.Columns.Contains("Alterar"))
            {
                DataGridViewButtonColumn alterarColumn = new DataGridViewButtonColumn
                {
                    Name = "Alterar",
                    HeaderText = "Alterar",
                    Text = "Alterar",
                    UseColumnTextForButtonValue = true
                };
                dataGridView1.Columns.Add(alterarColumn);
            }

            if (!dataGridView1.Columns.Contains("Excluir"))
            {
                DataGridViewButtonColumn excluirColumn = new DataGridViewButtonColumn
                {
                    Name = "Excluir",
                    HeaderText = "Excluir",
                    Text = "Excluir",
                    UseColumnTextForButtonValue = true
                };
                dataGridView1.Columns.Add(excluirColumn);
            }
        }

        private void CarregarImagensPorId(string id)
        {
            using (MySqlConnection conexao = new MySqlConnection("SERVER=127.0.0.1; DATABASE=eventosbv; UID=root; PASSWORD=;"))
            {
                conexao.Open();

                string query = "SELECT logo_premio_principal, logo_premio_secundario, logo_premio_terciario FROM premios WHERE id = @id";
                MySqlCommand cmd = new MySqlCommand(query, conexao);
                cmd.Parameters.AddWithValue("@id", id);

                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    string logoPrincipal = reader["logo_premio_principal"].ToString();
                    string logoSecundario = reader["logo_premio_secundario"].ToString();
                    string logoTerciario = reader["logo_premio_terciario"].ToString();

                    
                    if (File.Exists(logoPrincipal))
                    {
                        pictureBox1.Image = Image.FromFile(logoPrincipal);
                        pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                    }

                    if (File.Exists(logoSecundario))
                    {
                        pictureBox2.Image = Image.FromFile(logoSecundario);
                        pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
                    }

                    if (File.Exists(logoTerciario))
                    {
                        pictureBox3.Image = Image.FromFile(logoTerciario);
                        pictureBox3.SizeMode = PictureBoxSizeMode.StretchImage;
                    }
                }
            }
        }

        private void PreencherComboBoxComColunas(DataTable dt)
        {
           
            comboBox1.Items.Clear();

           
            foreach (DataColumn column in dt.Columns)
            {
                
                if (!column.ColumnName.Contains("logo"))
                {
                    comboBox1.Items.Add(column.ColumnName);
                }
            }

            
            if (comboBox1.Items.Count > 0)
            {
                comboBox1.SelectedIndex = 0;
            }

            comboBox1.SelectedIndex = -1; 
        }

        private void button1_Click(object sender, EventArgs e)
        {
            premiosCadastrar premiosCadastrar = new premiosCadastrar();
            premiosCadastrar.Show();
            this.Close();
            
        }

        private void premiosAdm_Load(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                // Se clicar na coluna "Alterar"
                if (e.ColumnIndex == dataGridView1.Columns["Alterar"].Index)
                {
                    string id = dataGridView1.Rows[e.RowIndex].Cells["id"].Value.ToString();
                    premioAlt(id);
                }
                // Se clicar na coluna "Excluir"
                else if (e.ColumnIndex == dataGridView1.Columns["Excluir"].Index)
                {
                    string id = dataGridView1.Rows[e.RowIndex].Cells["id"].Value.ToString();
                    premioEx(id);
                }
                else // Se clicar em qualquer outra célula, trocar as fotos do prêmio
                {
                    string id = dataGridView1.Rows[e.RowIndex].Cells["id"].Value.ToString();
                    CarregarImagensPorId(id);
                }
            }
        }

        private void premioEx(string id)
        {
            using (MySqlConnection conexao = new MySqlConnection("SERVER=127.0.0.1; DATABASE=eventosbv; UID=root; PASSWORD=;"))
            {
                conexao.Open();

                
                DialogResult result = MessageBox.Show("Tem certeza que deseja excluir o prêmio?", "Confirmação", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    
                    string excluir = "DELETE FROM premios WHERE id = " + id;
                    MySqlCommand comandos = new MySqlCommand(excluir, conexao);
                    comandos.ExecuteNonQuery();
                    MessageBox.Show("Prêmio excluído com sucesso!");

                    
                    CarregarDados();
                }
            }
        }

        private void premioAlt(string id)
        { 
            premioAlt alterarForm = new premioAlt(id);
            alterarForm.ShowDialog();
            
            CarregarDados();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string campo = comboBox1.Text;
            string valorCampo = textBox1.Text;

            if (string.IsNullOrWhiteSpace(campo) || string.IsNullOrWhiteSpace(valorCampo))
            {
                MessageBox.Show("Por favor, selecione um campo e digite um valor para filtrar.");
                return;
            }

            using (MySqlConnection conexao = new MySqlConnection("SERVER=127.0.0.1; DATABASE=eventosbv; UID=root; PASSWORD=;"))
            {
                try
                {
                    conexao.Open();

                    // Consulta de filtragem usando DataAdapter e DataTable para atualizar DataGridView
                    string query = $"SELECT `id`, `torneio_id`, `descricao`, `premio_principal`, `premio_secundario`, `premio_terciario`, `patrocinador_id`, `tipo_origem` FROM premios WHERE `{campo}` LIKE @valorCampo";
                    MySqlCommand consulta = new MySqlCommand(query, conexao);
                    consulta.Parameters.AddWithValue("@valorCampo", "%" + valorCampo + "%");

                    MySqlDataAdapter da = new MySqlDataAdapter(consulta);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dataGridView1.DataSource = dt;

                    if (dt.Rows.Count == 0)
                    {
                        MessageBox.Show("Nenhum registro foi encontrado.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao filtrar os dados: " + ex.Message);
                }
            }
        }

        private void LimparFiltro()
        {
            comboBox1.SelectedIndex = -1;  // Limpar a seleção no comboBox
            textBox1.Clear();  // Limpar o campo de texto
            CarregarDados();  // Recarregar a tabela sem filtro
        }

        private void button4_Click(object sender, EventArgs e)
        {
            LimparFiltro();
        }
    }
    
}