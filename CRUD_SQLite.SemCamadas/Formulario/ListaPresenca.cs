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
    public partial class ListaPresenca : Form
    {
        string connectionString = @"Data Source=C:\APP\DADOS\REGISTROS.db";

        public ListaPresenca()
        {
            InitializeComponent();
            this.CenterToScreen();
        }

        private void ListaPresenca_Load(object sender, EventArgs e)
        {
            CarregaDados();
        }

        //MÉTODOS DE CRUD
        private void CarregaDados()
        {
            dataGridView1.DataSource = LeDados<SQLiteConnection, SQLiteDataAdapter>
                (
                    "SELECT "
                        + "EVENTO.NOME AS 'NOME EVENTO'"
                        + ",LISTA_PRESENCA.*"
                   + "FROM "
                       + "TB_LISTA_PRESENCA AS LISTA_PRESENCA "
                   + "INNER JOIN "
                       + "TB_EVENTO AS EVENTO "
                       + "ON LISTA_PRESENCA.ID_EVENTO = EVENTO.ID_EVENTO"
                );

            //OCULTA COLUNA ID_LISTA_PRESENCA
            dataGridView1.Columns[1].Visible = false;

            //OCULTA COLUNA ID_EVENTO
            dataGridView1.Columns[7].Visible = false;
        }

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

        private ListaPresencaEntidade GetDadosDoGrid()
        {
            try
            {
                int linha;
                linha = dataGridView1.CurrentRow.Index;
                var entidade = new ListaPresencaEntidade();
                entidade.IdListaPresenca = Convert.ToInt32(dataGridView1[1, linha].Value);
                entidade.Nome = dataGridView1[2, linha].Value.ToString();
                entidade.Bairro = dataGridView1[3, linha].Value.ToString();
                entidade.Entidade = dataGridView1[4, linha].Value.ToString();
                entidade.SalaTecnica = Convert.ToInt32(dataGridView1[5, linha].Value.ToString());
                entidade.Observacao = dataGridView1[6, linha].Value.ToString();
                entidade.Evento.IdEvento = Convert.ToInt32(dataGridView1[7, linha].Value.ToString());
                return entidade;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int DeletaDados(ListaPresencaEntidade entidade)
        {
            int resultado = -1;
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(conn))
                {
                    cmd.CommandText = "DELETE FROM TB_PESSOA_LISTA_PRESENCA WHERE ID_LISTA_PRESENCA = @Id;"
                                     +"DELETE FROM TB_LISTA_PRESENCA WHERE ID_LISTA_PRESENCA = @Id";
                    cmd.Prepare();
                    cmd.Parameters.AddWithValue("@Id", entidade.IdListaPresenca);
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

        private DataTable ProcurarDados()
        {
            string sql ="SELECT "
                            + "EVENTO.NOME AS 'NOME EVENTO'"
                            + ",LISTA_PRESENCA.*"
                       + "FROM "
                            + "TB_LISTA_PRESENCA AS LISTA_PRESENCA"
                       + "INNER JOIN "
                           + "TB_EVENTO AS EVENTO "
                           + "ON LISTA_PRESENCA.ID_EVENTO = EVENTO.ID_EVENTO"
                       + "WHERE "
                           + "EVENTO.NOME LIKE '%" + txtBusca.Text + "%' "
                           + "OR LISTA_PRESENCA.NOME LIKE '%" + txtBusca.Text + "%'";

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

        private void btnIncluir_Click(object sender, EventArgs e)
        {
            try
            {
                ListaPresencaEntidade _entidade = null;
                var formEvento = new FormListaPresenca(_entidade);

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
                var formEvento = new FormListaPresenca(GetDadosDoGrid());
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

        private void btnSair_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtBusca_TextChanged(object sender, EventArgs e)
        {
            dataGridView1.DataSource = ProcurarDados();
            ProcurarDados();
        }
    }
}
