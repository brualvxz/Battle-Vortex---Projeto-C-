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
    public partial class equipesUser : Form
    {

        private int idJogadorLogado;
        public int idEquipe;
        public bool isCapitao = false;
        public int equipeCapitao;
        public equipesUser()
        {
            InitializeComponent();
            ObterIdJogadorLogado();
            CarregarDados();
            CarregarDados2();
        }


        private void ObterIdJogadorLogado()
        {
            using (MySqlConnection conexao = new MySqlConnection("SERVER=127.0.0.1; DATABASE=eventosbv; UID=root; PASSWORD=;"))
            {
                conexao.Open();
                string query = "SELECT jogador_id FROM usuarios WHERE id = @usuarioId";
                MySqlCommand comando = new MySqlCommand(query, conexao);
                comando.Parameters.AddWithValue("@usuarioId", UsuarioLogado.Id);  
                var resultado = comando.ExecuteScalar();

                if (resultado != null)
                {
                    idJogadorLogado = Convert.ToInt32(resultado);
                }
                else
                {
                    MessageBox.Show("Jogador não encontrado para o usuário logado.");
                }
            }
        }
        private void ObterIdEquipeJogador()
        {
            using (MySqlConnection conexao = new MySqlConnection("SERVER=127.0.0.1; DATABASE=eventosbv; UID=root; PASSWORD=;"))
            {
                // Obtém o ID da equipe do jogador logado
                string queryEquipeId = "SELECT equipe_id FROM jogadores WHERE id = @idJogadorLogado";
                MySqlCommand consultaEquipe = new MySqlCommand(queryEquipeId, conexao);
                consultaEquipe.Parameters.AddWithValue("@idJogadorLogado", idJogadorLogado);

                object equipeIdResult = consultaEquipe.ExecuteScalar();

                if (equipeIdResult == null || equipeIdResult == DBNull.Value)
                {
                    // Se o jogador não pertence a nenhuma equipe
                    MessageBox.Show("O jogador não pertence a nenhuma equipe.");

                    // Limpa o DataGridView e impede que ele seja carregado
                    dataGridView2.DataSource = null;
                    dataGridView2.Columns.Clear();
                    return;
                }

                equipeCapitao = Convert.ToInt32(equipeIdResult);
            }
        }

        private void CarregarDados()
        {
            using (MySqlConnection conexao = new MySqlConnection("SERVER=127.0.0.1; DATABASE=eventosbv; UID=root; PASSWORD=;"))
            {
                conexao.Open();

                // Obtém o ID da equipe do jogador logado
                string queryEquipeId = "SELECT equipe_id FROM jogadores WHERE id = @idJogadorLogado";
                MySqlCommand consultaEquipe = new MySqlCommand(queryEquipeId, conexao);
                consultaEquipe.Parameters.AddWithValue("@idJogadorLogado", idJogadorLogado);

                object equipeIdResult = consultaEquipe.ExecuteScalar();

                if (equipeIdResult == null || equipeIdResult == DBNull.Value)
                {
                    // Se o jogador não pertence a nenhuma equipe
                    MessageBox.Show("O jogador não pertence a nenhuma equipe.");

                    // Limpa o DataGridView e impede que ele seja carregado
                    dataGridView2.DataSource = null;
                    dataGridView2.Columns.Clear();
                    return;
                }

                idEquipe = Convert.ToInt32(equipeIdResult);

                // Consulta para obter informações da equipe
                string queryEquipe = "SELECT id, nome, logo, localidade, data_criacao, email FROM equipes WHERE id = @idEquipe";
                MySqlCommand consultaDados = new MySqlCommand(queryEquipe, conexao);
                consultaDados.Parameters.AddWithValue("@idEquipe", idEquipe);
                MySqlDataAdapter da = new MySqlDataAdapter(consultaDados);
                DataTable dt = new DataTable();
                da.Fill(dt);

                // Atualiza o DataGridView com os dados da equipe
                dataGridView2.DataSource = null;
                dataGridView2.Columns.Clear();
                dataGridView2.DataSource = dt;

                // Consulta separada para obter o ID do capitão
                string queryCapitaoId = "SELECT capitao_id FROM equipes WHERE id = @idEquipe";
                MySqlCommand consultaCapitao = new MySqlCommand(queryCapitaoId, conexao);
                consultaCapitao.Parameters.AddWithValue("@idEquipe", idEquipe);

                isCapitao = false; // Reinicia a variável

                object capitaoIdResult = consultaCapitao.ExecuteScalar();
                if (capitaoIdResult != null && capitaoIdResult != DBNull.Value)
                {
                    int capitaoId = Convert.ToInt32(capitaoIdResult);
                    isCapitao = (capitaoId == idJogadorLogado);
                }

                // Verificação e atualização dos botões
                if (isCapitao)
                {
                    button1.Enabled = false;
                    // Adiciona os botões "Alterar" e "Excluir"
                    if (!dataGridView2.Columns.Contains("Alterar"))
                    {
                        DataGridViewButtonColumn alterarColumn = new DataGridViewButtonColumn
                        {
                            Name = "Alterar",
                            HeaderText = "Alterar",
                            Text = "Alterar",
                            UseColumnTextForButtonValue = true
                        };
                        dataGridView2.Columns.Add(alterarColumn);
                    }

                    if (!dataGridView2.Columns.Contains("Excluir"))
                    {
                        DataGridViewButtonColumn excluirColumn = new DataGridViewButtonColumn
                        {
                            Name = "Excluir",
                            HeaderText = "Excluir",
                            Text = "Excluir",
                            UseColumnTextForButtonValue = true
                        };
                        dataGridView2.Columns.Add(excluirColumn);
                    }
                }
                else
                {

                    button1.Enabled = false;
                    button5.Enabled = false;
                    // Adiciona botão "Sair da Equipe" se ainda não existir
                    if (!dataGridView2.Columns.Contains("Sair"))
                    {
                        DataGridViewButtonColumn sairColumn = new DataGridViewButtonColumn
                        {
                            Name = "Sair",
                            HeaderText = "Sair da Equipe",
                            Text = "Sair",
                            UseColumnTextForButtonValue = true
                        };
                        dataGridView2.Columns.Add(sairColumn);
                    }
                }
            }
        }

        private void CarregarDados2()
        {
            using (MySqlConnection conexao = new MySqlConnection("SERVER=127.0.0.1; DATABASE=eventosbv; UID=root; PASSWORD=;"))
            {
                conexao.Open();

                // Obtém o ID da equipe do jogador logado
                string queryEquipeId = "SELECT equipe_id FROM jogadores WHERE id = @idJogadorLogado";
                MySqlCommand consultaEquipe = new MySqlCommand(queryEquipeId, conexao);
                consultaEquipe.Parameters.AddWithValue("@idJogadorLogado", idJogadorLogado);

                object equipeIdResult = consultaEquipe.ExecuteScalar();

                // Verifica se o valor retornado é DBNull
                if (equipeIdResult == DBNull.Value || equipeIdResult == null)
                {
              
                    // Limpa o DataGridView e impede que ele seja carregado
                    dataGridView1.DataSource = null;
                    dataGridView1.Columns.Clear();
                    return;
                }

                // Agora pode fazer a conversão com segurança
                int idEquipe = Convert.ToInt32(equipeIdResult);

                // Consulta para obter os jogadores da equipe usando a tabela 'equipes_jogadores'
                string queryJogadores = @"
                 SELECT j.id, j.nome, j.nickname
                FROM jogadores j
                INNER JOIN equipes_jogadores ej ON j.id = ej.jogador_id
                WHERE ej.equipe_id = @idEquipe";

                MySqlCommand consultaJogadores = new MySqlCommand(queryJogadores, conexao);
                consultaJogadores.Parameters.AddWithValue("@idEquipe", idEquipe);
                MySqlDataAdapter da = new MySqlDataAdapter(consultaJogadores);
                DataTable dt = new DataTable();
                da.Fill(dt);

                // Atualiza o DataGridView com os dados dos jogadores
                dataGridView1.DataSource = null;
                dataGridView1.Columns.Clear();
                dataGridView1.DataSource = dt;

                // Consulta para obter o ID do capitão
                string queryCapitaoId = "SELECT capitao_id FROM equipes WHERE id = @idEquipe";
                MySqlCommand consultaCapitao = new MySqlCommand(queryCapitaoId, conexao);
                consultaCapitao.Parameters.AddWithValue("@idEquipe", idEquipe);

                isCapitao = false; // Reinicia a variável

                object capitaoIdResult = consultaCapitao.ExecuteScalar();
                if (capitaoIdResult != null && capitaoIdResult != DBNull.Value)
                {
                    int capitaoId = Convert.ToInt32(capitaoIdResult);
                    isCapitao = (capitaoId == idJogadorLogado);
                }

                // Verificação e atualização dos botões
                if (isCapitao)
                {
                    if (!dataGridView1.Columns.Contains("Excluir"))
                    {
                        DataGridViewButtonColumn excluirColumn = new DataGridViewButtonColumn
                        {
                            Name = "Excluir",
                            HeaderText = "Remover Jogador",
                            Text = "Excluir",
                            UseColumnTextForButtonValue = true
                        };
                        dataGridView1.Columns.Add(excluirColumn);
                    }
                }
                else
                {
                    // Não faz nada no "else", conforme solicitado
                }
            }
        }


        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex >= 0)
            {
                // Obtém o ID da equipe clicada
                int idEquipe = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["id"].Value);
                CarregarLogoEquipe(idEquipe);
            }

            if (e.RowIndex >= 0) // Garante que estamos clicando em uma linha válida
            {
                // Obtém o ID da equipe clicada
                string idEquipe = dataGridView2.Rows[e.RowIndex].Cells["id"].Value.ToString();

                // Se o jogador for o capitão
                if (isCapitao)
                {
                    if (e.ColumnIndex == dataGridView2.Columns["Alterar"].Index)
                    {
                        // Chama a função para alterar equipe
                        AlterarEquipe(idEquipe);
                    }
                    else if (e.ColumnIndex == dataGridView2.Columns["Excluir"].Index)
                    {
                        // Chama a função para excluir equipe
                        ExcluirEquipe(idEquipe);
                    }
                }
                else
                {
                    // Se for membro, apenas permitir saída da equipe
                    if (e.ColumnIndex == dataGridView2.Columns["Sair"].Index)
                    {
                        // Chama a função para sair da equipe
                        SairDaEquipe(idEquipe);
                    }
                }
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex >= 0)
            {
                // Obtém o ID da equipe clicada
                int idEquipe = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["id"].Value);
                CarregarLogoEquipe(idEquipe);
            }

            if (e.RowIndex >= 0)
            {
               
                if (e.ColumnIndex == dataGridView1.Columns["Excluir"].Index)
                {
                    // Obtém o ID do jogador que foi clicado na linha
                    int idJogador = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["id"].Value);
                    string nomeJogador = dataGridView1.Rows[e.RowIndex].Cells["nome"].Value.ToString();

                   
                    if (isCapitao)
                    {
                        // Se for o capitão, pode remover o jogador
                        DialogResult confirmacao = MessageBox.Show($"Tem certeza que deseja remover o jogador {nomeJogador}?", "Confirmar remoção", MessageBoxButtons.YesNo);
                        if (confirmacao == DialogResult.Yes)
                        {
                            removerJogador(idJogador);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Apenas o capitão pode remover jogadores.");
                    }
                }
            }
        }

        private void removerJogador(int idJogador)
        {
            using (MySqlConnection conexao = new MySqlConnection("SERVER=127.0.0.1; DATABASE=eventosbv; UID=root; PASSWORD=;"))
            {
                conexao.Open();

                // Verifica se o jogador pertence à equipe
                string verificaEquipeQuery = "SELECT equipe_id FROM jogadores WHERE id = @idJogador";
                MySqlCommand verificaEquipeCmd = new MySqlCommand(verificaEquipeQuery, conexao);
                verificaEquipeCmd.Parameters.AddWithValue("@idJogador", idJogador);
                object equipeIdObj = verificaEquipeCmd.ExecuteScalar();

        
                // Verifica se o valor retornado é DBNull
                if (equipeIdObj == DBNull.Value || equipeIdObj == null)
                {

                    MessageBox.Show("Jogador não encontrado na equipe.");
                    return;
                }

                int equipeId = Convert.ToInt32(equipeIdObj);

                // Remove o jogador da tabela 'equipes_jogadores'
                string removeJogadorEquipeQuery = "DELETE FROM equipes_jogadores WHERE jogador_id = @idJogador AND equipe_id = @equipeId";
                MySqlCommand removeJogadorEquipeCmd = new MySqlCommand(removeJogadorEquipeQuery, conexao);
                removeJogadorEquipeCmd.Parameters.AddWithValue("@idJogador", idJogador);
                removeJogadorEquipeCmd.Parameters.AddWithValue("@equipeId", equipeId);

                int rowsAffectedEquipeJogadores = removeJogadorEquipeCmd.ExecuteNonQuery();

                if (rowsAffectedEquipeJogadores == 0)
                {
                    MessageBox.Show("Erro ao remover o jogador da equipe.");
                    return;
                }

                // Atualiza a tabela 'jogadores' para remover a referência à equipe (coluna 'equipe_id')
                string removeEquipeIdJogadorQuery = "UPDATE jogadores SET equipe_id = NULL WHERE id = @idJogador";
                MySqlCommand removeEquipeIdJogadorCmd = new MySqlCommand(removeEquipeIdJogadorQuery, conexao);
                removeEquipeIdJogadorCmd.Parameters.AddWithValue("@idJogador", idJogador);

                int rowsAffectedJogador = removeEquipeIdJogadorCmd.ExecuteNonQuery();

                if (rowsAffectedJogador > 0)
                {
                    MessageBox.Show("Jogador removido com sucesso da equipe.");
                    CarregarDados2(); // Recarrega os dados no DataGridView
                }
                else
                {
                    MessageBox.Show("Erro ao desvincular o jogador da equipe.");
                }
            }
        }

        private void CarregarLogoEquipe(int idEquipe)
        {
            using (MySqlConnection conexao = new MySqlConnection("SERVER=127.0.0.1; DATABASE=eventosbv; UID=root; PASSWORD=;"))
            {
                conexao.Open();


                string query = "SELECT logo FROM equipes WHERE id = @id";
                MySqlCommand comando = new MySqlCommand(query, conexao);
                comando.Parameters.AddWithValue("@id", idEquipe);

                MySqlDataReader reader = comando.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();

                    string caminhoLogo = reader["logo"].ToString();


                    if (File.Exists(caminhoLogo))
                    {

                        pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                        pictureBox1.Image = Image.FromFile(caminhoLogo);
                    }
                    else
                    {

                        MessageBox.Show("O logo da equipe não foi encontrado no caminho especificado: " + caminhoLogo);
                    }
                }
                else
                {
                    MessageBox.Show("Equipe não encontrada.");
                }

                reader.Close();
            }
        }


        private void ExcluirEquipe(string id)
        {
            using (MySqlConnection conexao = new MySqlConnection("SERVER=127.0.0.1; DATABASE=eventosbv; UID=root; PASSWORD=;"))
            {
                conexao.Open();

                // Confirmação da exclusão
                DialogResult result = MessageBox.Show("Tem certeza que deseja excluir a equipe?", "Confirmação", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    string excluir = "DELETE FROM equipes WHERE id = @id";
                    MySqlCommand comando = new MySqlCommand(excluir, conexao);
                    comando.Parameters.AddWithValue("@id", id);
                    comando.ExecuteNonQuery();

                    MessageBox.Show("Equipe excluída com sucesso!");

                    // Recarrega os dados
                    CarregarDados();
                    CarregarDados2();
                }
            }
        }

        private void AlterarEquipe(string id)
        {
            // Abre o formulário de alteração com o ID da equipe
            equipesAlt alterarForm = new equipesAlt(id);
            alterarForm.ShowDialog();

            // Recarrega os dados após o formulário ser fechado
            CarregarDados();
            CarregarDados2();
        }

        private void SairDaEquipe(string idEquipe)
        {
            using (MySqlConnection conexao = new MySqlConnection("SERVER=127.0.0.1; DATABASE=eventosbv; UID=root; PASSWORD=;"))
            {
                conexao.Open();

                // Confirmação da exclusão
                DialogResult result = MessageBox.Show("Tem certeza que deseja sair e excluir seu vínculo com esta equipe?", "Confirmação", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    try
                    {
                        // Remover o jogador da tabela de vínculo (equipes_jogadores)
                        string excluirVinculo = "DELETE FROM equipes_jogadores WHERE equipe_id = @idEquipe AND jogador_id = @idJogador";
                        MySqlCommand comandoExcluirVinculo = new MySqlCommand(excluirVinculo, conexao);
                        comandoExcluirVinculo.Parameters.AddWithValue("@idEquipe", idEquipe);
                        comandoExcluirVinculo.Parameters.AddWithValue("@idJogador", idJogadorLogado);
                        comandoExcluirVinculo.ExecuteNonQuery();

                        // Desvincular a equipe no perfil do jogador
                        string atualizarJogador = "UPDATE jogadores SET equipe_id = NULL WHERE id = @idJogador";
                        MySqlCommand comandoAtualizarJogador = new MySqlCommand(atualizarJogador, conexao);
                        comandoAtualizarJogador.Parameters.AddWithValue("@idJogador", idJogadorLogado);
                        comandoAtualizarJogador.ExecuteNonQuery();

                        MessageBox.Show("Você foi removido da equipe com sucesso!");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Erro ao excluir o vínculo com a equipe: " + ex.Message);
                    }
                    finally
                    {
                        // Recarrega os dados após a exclusão
                        CarregarDados();
                        CarregarDados2();
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (JogadorJaEstaEmEquipe())
            {
                MessageBox.Show("Você já está em uma equipe.");
            }
            else
            {
                equipesCadastrar equipesCadastrar = new equipesCadastrar();
                equipesCadastrar.ShowDialog();
                CarregarDados() ;
            }

        }

        private bool JogadorJaEstaEmEquipe()
        {
            using (MySqlConnection conexao = new MySqlConnection("SERVER=127.0.0.1; DATABASE=eventosbv; UID=root; PASSWORD=;"))
            {
                conexao.Open();
                string query = "SELECT COUNT(*) FROM equipes_jogadores WHERE jogador_id = @idJogador";
                MySqlCommand comando = new MySqlCommand(query, conexao);
                comando.Parameters.AddWithValue("@idJogador", idJogadorLogado);

                int count = Convert.ToInt32(comando.ExecuteScalar());
                return count > 0;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            homeAdm homeadm = new homeAdm();
            homeadm.Show();
            this.Close();
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
                    string query = $"SELECT id, nome, logo, localidade, email, data_criacao FROM equipes WHERE `{campo}` LIKE @valorCampo";
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
            comboBox1.SelectedIndex = -1;
            textBox1.Clear();
            CarregarDados();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            LimparFiltro();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            jogadoresEquipes jogadores = new jogadoresEquipes(idEquipe);
            jogadores.ShowDialog();

            CarregarDados();
            CarregarDados2();
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            string connectionString = "SERVER=127.0.0.1; DATABASE=eventosbv; UID=root; PASSWORD=;";

            using (MySqlConnection conexao = new MySqlConnection(connectionString))
            {
                try
                {
                    conexao.Open();

                    // Consulta para obter o tipo de usuário com base no ID logado
                    string query = "SELECT tipo FROM usuarios WHERE id = @idUsuario AND status = 'Ativo'";

                    using (MySqlCommand comando = new MySqlCommand(query, conexao))
                    {
                        comando.Parameters.AddWithValue("@idUsuario", UsuarioLogado.Id);

                        using (MySqlDataReader reader = comando.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string tipo = reader.GetString("tipo");

                                // Redireciona para a tela correspondente ao tipo de usuário
                                if (tipo.Equals("Administrador", StringComparison.OrdinalIgnoreCase))
                                {
                                    homeAdm homeadm = new homeAdm();
                                    homeadm.Show();
                                }
                                else if (tipo.Equals("Usuário", StringComparison.OrdinalIgnoreCase))
                                {
                                    homeUser homeuser = new homeUser();
                                    homeuser.Show();
                                }
                                else if (tipo.Equals("Patrocinador", StringComparison.OrdinalIgnoreCase))
                                {
                                    homePatrocinador homePat = new homePatrocinador();
                                    homePat.Show();
                                }
                                else if (tipo.Equals("Organizador", StringComparison.OrdinalIgnoreCase))
                                {
                                    homeOrganizador homeOrg = new homeOrganizador();
                                    homeOrg.Show();
                                }
                                else
                                {
                                    MessageBox.Show("Tipo de usuário inválido.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }

                                this.Hide(); // Oculta a tela atual
                            }
                            else
                            {
                                MessageBox.Show("Usuário não encontrado ou inativo.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erro ao conectar ao banco de dados: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void equipesUser_Load(object sender, EventArgs e)
        {

        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView2_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
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
                    string query = $"SELECT id, nome, logo, localidade, email, data_criacao FROM equipes WHERE `{campo}` LIKE @valorCampo";
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

        private void button4_Click_1(object sender, EventArgs e)
        {
            LimparFiltro();
        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {

        }
    }
}
