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
    public partial class Evento : Form
    {
        string connectionString = @"Data Source=C:\APP\DADOS\REGISTROS.db";

        public Evento()
        {
            InitializeComponent();
            this.CenterToScreen();
        }

        private void Evento_Load(object sender, EventArgs e)
        {
            CarregaDados();
        }

        private void CarregaDados()
        {
            dataGridView1.DataSource = LeDados<SQLiteConnection, SQLiteDataAdapter>("SELECT * FROM TB_EVENTO");
        }

        //MÉTODOS DE CRUD
        public DataTable LeDados<S, T>(string query) where S : IDbConnection, new()
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

        private EventoEntidade GetDadosDoGrid()
        {
            try
            {
                int linha;
                linha = dataGridView1.CurrentRow.Index;
                var entidade = new EventoEntidade();
                entidade.IdEvento  = Convert.ToInt32(dataGridView1[0, linha].Value);
                entidade.Nome = dataGridView1[1, linha].Value.ToString();
                entidade.Local = dataGridView1[2, linha].Value.ToString();
                entidade.Data = Convert.ToDateTime(dataGridView1[3, linha].Value);
                return entidade;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int DeletaDados(EventoEntidade entidade)
        {
            int resultado = -1;
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                DeletaDadosRelacionados(BuscaIdListaEvento(entidade.IdEvento.ToString()));

                using (SQLiteCommand cmd = new SQLiteCommand(conn))
                {
                    cmd.CommandText = "DELETE FROM TB_EVENTO WHERE ID_EVENTO = @Id;";
                    cmd.Prepare();
                    cmd.Parameters.AddWithValue("@Id", entidade.IdEvento);
                    try
                    {
                        resultado = cmd.ExecuteNonQuery();
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
            return resultado;
        }

        public void DeletaDadosRelacionados(List<int> ListaPresencaId)
        {
            foreach (var id in ListaPresencaId)
            {
                using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();
                    using (SQLiteCommand cmd = new SQLiteCommand(conn))
                    {
                        cmd.CommandText = "DELETE FROM TB_LISTA_PRESENCA WHERE ID_LISTA_PRESENCA = @Id;"
                                         +"DELETE FROM TB_PESSOA_LISTA_PRESENCA WHERE ID_LISTA_PRESENCA = @Id";
                        cmd.Prepare();
                        cmd.Parameters.AddWithValue("@Id", id);
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
        }

        private List<int> BuscaIdListaEvento(string idEvento)
        {
            string sql = "SELECT ID_LISTA_PRESENCA FROM TB_LISTA_PRESENCA WHERE ID_EVENTO = "+idEvento;

            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                using (SQLiteDataAdapter da = new SQLiteDataAdapter(sql, conn))
                {
                    try
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        var ListaPresencaId = new List<int>();
                        var ListaObject = new List<object>();

                        foreach (DataRow linha in dt.Rows)
                        {
                            ListaObject.Add(linha.ItemArray.FirstOrDefault());
                        }

                        foreach (var item in ListaObject)
                        {
                            ListaPresencaId.Add(Convert.ToInt32(item));
                        }

                        return ListaPresencaId;
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

        private DataTable ProcurarDados()
        {
            string sql = "SELECT ID_EVENTO, NOME, LOCAL, DATA FROM TB_EVENTO WHERE NOME LIKE '" + txtNomeBusca.Text + "%'";
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                using (SQLiteDataAdapter da = new SQLiteDataAdapter(sql, conn))
                {
                    try
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        return dt;
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
        
        //BOTÕES CRUD
        private void btnIncluir_Click(object sender, EventArgs e)
        {
            try
            {
                EventoEntidade _entidade = null;
                var formEvento = new FormEvento(_entidade);

                formEvento.ShowDialog();
                CarregaDados();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro : " + ex.Message);
            }
        }

        private void btnAlterar_Click(object sender, EventArgs e)
        {
            try
            {
                var formEvento = new FormEvento(GetDadosDoGrid());
                formEvento.ShowDialog();
                CarregaDados();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro : " + ex.Message);
            }
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult response = MessageBox.Show("Deseja deletar este registro ?", "Deletar linha",
                      MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (response == DialogResult.Yes)
                {
                    DeletaDados(GetDadosDoGrid());
                    CarregaDados();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro : " + ex.Message);
            }
        }
        
        //BOTÃO SAIR
        private void btnSair_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //CAMPO DE BUSCA
        private void txtNomeBusca_TextChanged(object sender, EventArgs e)
        {
            dataGridView1.DataSource = ProcurarDados();
            ProcurarDados();
        }

    }
}
