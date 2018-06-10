using CRUD_SQLite.SemCamadas.Entidade;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CRUD_SQLite.SemCamadas.Formulario
{
    public partial class PresencaEvento : Form
    {
        string connectionString = @"Data Source=C:\APP\DADOS\REGISTROS.db";

        public PresencaEvento()
        {
            InitializeComponent();
            this.CenterToScreen();
        }

        private void PresencaEvento_Load(object sender, EventArgs e)
        {
            CarregaListaPresenca();
        }

        private PresencaEventoEntidade GetDadosDoGrid()
        {
            try
            {
                int linha;
                linha = dataGridView1.CurrentRow.Index;
                var entidade = new PresencaEventoEntidade();
                entidade.PresenteLista = dataGridView1[0, linha].Value.ToString().Equals("SIM") ? true : false;
                entidade.Pessoa.Nome = dataGridView1[1, linha].Value.ToString();
                entidade.Pessoa.IdPessoa = Convert.ToInt32(dataGridView1[2, linha].Value);
                return entidade;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        //LISTA PRESENÇA - ID_LISTA_PRESENCA
        private void CarregaListaPresencaId(string idListaPresenca)
        {
            dataGridView1.DataSource = BuscaListaPresencaId<SQLiteConnection, SQLiteDataAdapter>
            (
                "SELECT "
                    + "CASE WHEN PESSOA_LISTA.ID_PESSOA IS NULL THEN 'NÃO' ELSE 'SIM' END AS 'PRESENTE NA LISTA'"
                    + ", PESSOA.NOME"
                    + ", PESSOA.ID_PESSOA" 
                    + ", PESSOA.EMAIL"
                    + ", PESSOA.BAIRRO "
                    + ", PESSOA.ENTIDADE "
                    + ", PESSOA.CPF "
                    + ", PESSOA.RG "
               + "FROM "
                   + "TB_PESSOA AS PESSOA "
               + "LEFT JOIN "
                   + "TB_PESSOA_LISTA_PRESENCA AS PESSOA_LISTA "
                   + "ON PESSOA.ID_PESSOA = PESSOA_LISTA.ID_PESSOA "
               + "WHERE ID_LISTA_PRESENCA = " + idListaPresenca + " "
               + "ORDER BY PESSOA.NOME"
            );

            //OCULTA COLUNA ID_PESSOA
            dataGridView1.Columns[2].Visible = false;

        }

        public DataTable BuscaListaPresencaId<S, T>(string query) where S : IDbConnection, new()
                                           where T : IDbDataAdapter, IDisposable, new()
        {
            using (var conn = new S())
            {
                using (var da = new T())
                {
                    using (da.SelectCommand = conn.CreateCommand())
                    {
                        da.SelectCommand.CommandText = query;
                        da.SelectCommand.Connection.ConnectionString = connectionString;
                        DataSet ds = new DataSet();
                        da.Fill(ds);
                        return ds.Tables[0];
                    }
                }
            }
        }

        //LISTA PRESENÇA - ID_LISTA_PRESENÇA / CAMPO_BUSCA
        private void PesquisaListaPresenca(string idListaPresenca, string campoBusca)
        {
            dataGridView1.DataSource = PesquisaBuscaListaPresenca<SQLiteConnection, SQLiteDataAdapter>
            (
                "SELECT "
                    + "CASE WHEN PESSOA_LISTA.ID_PESSOA IS NULL THEN 'NÃO' ELSE 'SIM' END AS PRESENTE_NA_LISTA"
                    + ", PESSOA.NOME"
                    + ", PESSOA.ID_PESSOA "
               + "FROM "
                   + "TB_PESSOA AS PESSOA "
               + "LEFT JOIN "
                   + "TB_PESSOA_LISTA_PRESENCA AS PESSOA_LISTA "
                   + "ON PESSOA.ID_PESSOA = PESSOA_LISTA.ID_PESSOA "
                   + "AND PESSOA_LISTA.ID_LISTA_PRESENCA = " + idListaPresenca + " "
               + "WHERE PESSOA.NOME LIKE '%"+ campoBusca+"%'"
               + " OR PESSOA.EMAIL LIKE '%"+ campoBusca +"%'"
               + " ORDER BY PRESENTE_NA_LISTA, PESSOA.NOME"
            );

        }

        public DataTable PesquisaBuscaListaPresenca<S, T>(string query) where S : IDbConnection, new()
                                           where T : IDbDataAdapter, IDisposable, new()
        {
            using (var conn = new S())
            {
                using (var da = new T())
                {
                    using (da.SelectCommand = conn.CreateCommand())
                    {
                        da.SelectCommand.CommandText = query;
                        da.SelectCommand.Connection.ConnectionString = connectionString;
                        DataSet ds = new DataSet();
                        da.Fill(ds);
                        return ds.Tables[0];
                    }
                }
            }
        }
        
        //BUSCA LISTA DE EVENTOS
        private void CarregaListaPresenca()
        {
            cbListaPresenca.DisplayMember = "NOME";
            cbListaPresenca.ValueMember = "ID_LISTA_PRESENCA";
            cbListaPresenca.DataSource = BuscaListaPresenca<SQLiteConnection, SQLiteDataAdapter>
                (
                    "SELECT "
                        + "ID_LISTA_PRESENCA"
                        + ",NOME"
                   + " FROM "
                       + "TB_LISTA_PRESENCA"
                );
        }

        public DataTable BuscaListaPresenca<S, T>(string query) where S : IDbConnection, new()
                                           where T : IDbDataAdapter, IDisposable, new()
        {
            using (var conn = new S())
            {
                using (var da = new T())
                {
                    using (da.SelectCommand = conn.CreateCommand())
                    {
                        da.SelectCommand.CommandText = query;
                        da.SelectCommand.Connection.ConnectionString = connectionString;
                        DataSet ds = new DataSet();
                        da.Fill(ds);
                        return ds.Tables[0];
                    }
                }
            }
        }

        //EVENTO DE SELEÇÃO DA LISTA DE PRESENÇA
        private void cbListaPresenca_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtBusca.Text = "";
            var listaSelecionada = cbListaPresenca.SelectedValue.ToString();
            CarregaListaPresencaId(listaSelecionada);
        }

        //EVENTO PARA PESQUISA
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            var listaSelecionada = cbListaPresenca.SelectedValue.ToString();
            var busca = txtBusca.Text.ToString();

            if (txtBusca.Text.Length > 0)
                PesquisaListaPresenca(listaSelecionada, busca);
            else
                CarregaListaPresencaId(listaSelecionada);
        }

        //EVENTO DE DUPLO CLIQUE NA TABELA DE PRESENÇA
        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                var entidade = GetDadosDoGrid();
                entidade.ListaPresenca.IdListaPresenca = Convert.ToInt32(cbListaPresenca.SelectedValue.ToString());

                if (entidade.PresenteLista is false)
                {
                    DialogResult response = MessageBox.Show("Deseja incluir esta pessoa na lista ?", "Incluir na lista",
                          MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                    if (response == DialogResult.Yes)
                    {
                        IncluirPessoaLista(entidade);
                    }
                }
                else
                {
                    DialogResult response = MessageBox.Show("Deseja remover esta pessoa da lista ?", "Remover da lista",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                    if (response == DialogResult.Yes)
                    {
                        RemoverPessoaLista(entidade);
                        
                    }
                }

                CarregaListaPresencaId(entidade.ListaPresenca.IdListaPresenca.ToString());
                txtBusca.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro : " + ex.Message);
            }
        }

        //REMOVE PESSOA DA LISTA
        private void RemoverPessoaLista(PresencaEventoEntidade entidade)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(conn))
                {
                    cmd.CommandText = "DELETE FROM TB_PESSOA_LISTA_PRESENCA WHERE ID_LISTA_PRESENCA = @id_lista_presenca AND ID_PESSOA = @id_pessoa";
                    cmd.Prepare();
                    cmd.Parameters.AddWithValue("@id_lista_presenca", entidade.ListaPresenca.IdListaPresenca);
                    cmd.Parameters.AddWithValue("@id_pessoa", entidade.Pessoa.IdPessoa);
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (SQLiteException ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
        }

        //INCLUI PESSOA NA LISTA
        private void IncluirPessoaLista(PresencaEventoEntidade entidade)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(conn))
                {
                    cmd.CommandText = "INSERT INTO TB_PESSOA_LISTA_PRESENCA(ID_PESSOA, ID_LISTA_PRESENCA) VALUES (@id_pessoa, @id_lista_presenca)";
                    cmd.Prepare();
                    cmd.Parameters.AddWithValue("@id_pessoa", entidade.Pessoa.IdPessoa);
                    cmd.Parameters.AddWithValue("@id_lista_presenca", entidade.ListaPresenca.IdListaPresenca);
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (SQLiteException ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
        }

        private void btnIncluir_Click(object sender, EventArgs e)
        {
            int idListaEvento = Convert.ToInt32(cbListaPresenca.SelectedValue.ToString());

            new FormPessoa(null, idListaEvento).ShowDialog();
            CarregaListaPresencaId(idListaEvento.ToString());
            txtBusca.Text = "";
        }

        private void btnSair_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
