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
    public partial class FormPessoa : Form
    {
        PessoaEntidade _entidade;
        int? _idListaPresenca = null;
        string operacao = "";
        string connectionString = @"Data Source=C:\APP\DADOS\REGISTROS.db";

        public FormPessoa(PessoaEntidade entidade, int? idListaPresenca)
        {
            InitializeComponent();
            _entidade = entidade;
            _idListaPresenca = idListaPresenca;
            this.CenterToScreen();
        }

        private void FormPessoa_Load(object sender, EventArgs e)
        {
            if (_entidade == null)
            {
                txtNome.Focus();
                operacao = "incluir";
            }
            else
            {
                operacao = "alterar";
                txtId.Text = _entidade.IdPessoa.ToString();
                txtNome.Text = _entidade.Nome;
                txtBairro.Text = _entidade.Bairro;
                txtEntidade.Text = _entidade.Entidade;
                maskedTextBox1.Text = _entidade.Cpf;
                txtRG.Text = _entidade.Rg;
                txtEmail.Text = _entidade.Email;
            }
        }

        private Boolean ValidaDados()
        {
            bool retorno = true;

            if (txtNome.Text == string.Empty)
                retorno = false;

            if (txtBairro.Text == string.Empty)
                retorno = false;

            return retorno;
        }

        public int IncluirDados(PessoaEntidade entidade)
        {
            int resultado = -1;
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(conn))
                {
                    cmd.CommandText = "INSERT INTO TB_PESSOA(NOME, BAIRRO, ENTIDADE, CPF, RG, EMAIL) VALUES (@nome,@bairro,@entidade,@cpf,@rg,@email)";
                    cmd.Prepare();
                    cmd.Parameters.AddWithValue("@nome", entidade.Nome);
                    cmd.Parameters.AddWithValue("@bairro", entidade.Bairro);
                    cmd.Parameters.AddWithValue("@entidade", entidade.Entidade);
                    cmd.Parameters.AddWithValue("@cpf", entidade.Cpf);
                    cmd.Parameters.AddWithValue("@rg", entidade.Rg);
                    cmd.Parameters.AddWithValue("@email", entidade.Email);
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

            if (_idListaPresenca != null)
                IncluirPessoaLista();

            return resultado;
        }

        private void IncluirPessoaLista()
        {
            int maxIdUsuarioCadastrado = BuscaUltimoCadastro();

            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(conn))
                {
                    cmd.CommandText = "INSERT INTO TB_PESSOA_LISTA_PRESENCA(ID_PESSOA, ID_LISTA_PRESENCA) VALUES (@id_pessoa, @id_lista_presenca)";
                    cmd.Prepare();
                    cmd.Parameters.AddWithValue("@id_pessoa", maxIdUsuarioCadastrado);
                    cmd.Parameters.AddWithValue("@id_lista_presenca", _idListaPresenca);
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

        private int BuscaUltimoCadastro()
        {
            string sql = "SELECT MAX(ID_PESSOA) FROM TB_PESSOA AS 'MAX'";

            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                using (SQLiteDataAdapter da = new SQLiteDataAdapter(sql, conn))
                {
                    try
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        var maxId = dt.Rows[0][0];

                        return Convert.ToInt32(maxId);
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

        public int AtualizaDados(PessoaEntidade entidade)
        {
            int resultado = -1;
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(conn))
                {
                    cmd.CommandText = "UPDATE TB_PESSOA SET NOME=@nome, BAIRRO=@bairro, ENTIDADE=@entidade, CPF=@cpf, RG=@rg, EMAIL=@email WHERE ID_PESSOA = @id";
                    cmd.Prepare();
                    cmd.Parameters.AddWithValue("@Id", entidade.IdPessoa);
                    cmd.Parameters.AddWithValue("@nome", entidade.Nome);
                    cmd.Parameters.AddWithValue("@bairro", entidade.Bairro);
                    cmd.Parameters.AddWithValue("@entidade", entidade.Entidade);
                    cmd.Parameters.AddWithValue("@cpf", entidade.Cpf);
                    cmd.Parameters.AddWithValue("@rg", entidade.Rg);
                    cmd.Parameters.AddWithValue("@email", entidade.Email);
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

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            if (ValidaDados())
            {
                try
                {
                    var entidade = new PessoaEntidade
                    {
                        IdPessoa = int.Parse(txtId.Text),
                        Nome = txtNome.Text,
                        Bairro = txtBairro.Text,
                        Entidade= txtEntidade.Text,
                        Cpf = maskedTextBox1.Text,
                        Rg = txtRG.Text,
                        Email = txtEmail.Text,

                    };

                    if (operacao == "incluir")
                    {
                        if (IncluirDados(entidade) > 0)
                        {
                            MessageBox.Show("Dados incluídos com sucesso.");
                            Close();
                        }
                        else
                        {
                            MessageBox.Show("Os dados não foram incluídos.");
                        }
                    }
                    else
                    {
                        if (AtualizaDados(entidade) > 0)
                        {
                            MessageBox.Show("Dados atualizados com sucesso.");
                            Close();
                        }
                        else
                        {
                            MessageBox.Show("Os dados não foram atualizados.");
                            Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro : " + ex.Message);
                    Close();
                }
            }
            else
            {
                MessageBox.Show("Preencha o formulário corretamente");
            }
        }

        private void btnSair_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

