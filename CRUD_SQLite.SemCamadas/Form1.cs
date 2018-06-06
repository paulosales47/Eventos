using System;
using System.Data;
using System.Data.SQLite;
using System.Windows.Forms;

namespace CRUD_SQLite.SemCamadas
{
    public partial class Form1 : Form
    {
        string connectionString = @"Data Source=C:\APP\DADOS\REGISTROS.db";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CarregaDados();
        }

        private void CarregaDados()
        {
            dgvAlunos.DataSource = LeDados<SQLiteConnection, SQLiteDataAdapter>("SELECT * from Alunos");
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
                        DataSet ds = new DataSet(); //conn é aberto pelo dataadapter
                        da.Fill(ds);
                        return ds.Tables[0];
                    }
                }
            }
        }

        private Aluno GetDadosDoGrid()
        {
            try
            {
                int linha;
                linha = dgvAlunos.CurrentRow.Index;
                Aluno aluno = new Aluno();
                aluno.Id = Convert.ToInt32(dgvAlunos[0, linha].Value);
                aluno.nome = dgvAlunos[1, linha].Value.ToString();
                aluno.email = dgvAlunos[2, linha].Value.ToString();
                aluno.idade = Convert.ToInt32(dgvAlunos[3, linha].Value);
                return aluno;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int DeletaDados(Aluno aluno)
        {
            int resultado = -1;
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(conn))
                {
                    cmd.CommandText = "DELETE FROM Alunos WHERE Id = @Id";
                    cmd.Prepare();
                    cmd.Parameters.AddWithValue("@Id", aluno.Id);
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
            string sql = "SELECT id, nome, email, idade from Alunos WHERE nome LIKE '" + txtCriterio.Text + "%'";
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                using (SQLiteDataAdapter da = new SQLiteDataAdapter(sql, conn))
                {
                    try
                    {
                        DataTable dt = new DataTable(); //conn é aberto pelo dataadapter
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
                Aluno _aluno = null;
                IPPLAN frm2 = new IPPLAN(_aluno);

                frm2.ShowDialog();
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
                IPPLAN frm2 = new IPPLAN(GetDadosDoGrid());
                frm2.ShowDialog();
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

        private void txtCriterio_TextChanged(object sender, EventArgs e)
        {
            dgvAlunos.DataSource = ProcurarDados();
            //ProcurarDados();
        }

        private void btnSair_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
