using System;
using System.Data.SQLite;
using System.Windows.Forms;

namespace CRUD_SQLite.SemCamadas
{
    public partial class IPPLAN : Form
    {
        Aluno _aluno;
        string operacao = "";
        //string connectionString = @"Data Source=G:\ezvid\Desenvolvimento\C#\CRUD_SQLite\BD\REGISTROS.db";
        string connectionString = @"Data Source=C:\APP\ARQUIVOS\REGISTROS.db";

        public IPPLAN(Aluno aluno)
        {
            InitializeComponent();
            _aluno = aluno;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            if (_aluno == null)
            {
                txtNome.Focus();
                operacao = "incluir";
            }
            else
            {
                operacao = "alterar";
                txtId.Text = _aluno.Id.ToString();
                txtNome.Text = _aluno.nome;
                txtEmail.Text = _aluno.email;
                txtIdade.Text = _aluno.idade.ToString();
            }
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {

            if (ValidaDados())
            {
                try
                {
                    Aluno aluno = new Aluno
                    {
                        Id = int.Parse(txtId.Text),
                        nome = txtNome.Text,
                        email = txtEmail.Text,
                        idade = Convert.ToInt32(txtIdade.Text)
                    };
                    
                    if (operacao == "incluir")
                    {
                        if (IncluirDados(aluno) > 0)
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
                        if (AtualizaDados(aluno) > 0)
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
                MessageBox.Show("Dados inválidos.");
            }
        }

        private void btnSair_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private Boolean ValidaDados()
        {
            bool retorno = true;

            if (txtNome.Text == string.Empty)
                retorno = false;

            if (txtEmail.Text == string.Empty)
                retorno = false;

            if (txtIdade.Text == string.Empty)
                retorno = false;

            return retorno;
        }

        public int IncluirDados(Aluno aluno)
        {
            int resultado = -1;
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(conn))
                {
                    cmd.CommandText = "INSERT INTO alunos(nome,email,idade) VALUES (@nome,@email,@idade)";
                    cmd.Prepare();
                    cmd.Parameters.AddWithValue("@nome", aluno.nome);
                    cmd.Parameters.AddWithValue("@email", aluno.email);
                    cmd.Parameters.AddWithValue("@idade", aluno.idade);
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

        public int AtualizaDados(Aluno aluno)
        {
            int resultado = -1;
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(conn))
                {
                    cmd.CommandText = "UPDATE alunos SET nome=@nome, email=@email, idade=@idade WHERE Id = @id";
                    cmd.Prepare();
                    cmd.Parameters.AddWithValue("@Id", aluno.Id);
                    cmd.Parameters.AddWithValue("@nome", aluno.nome);
                    cmd.Parameters.AddWithValue("@email", aluno.email);
                    cmd.Parameters.AddWithValue("@idade", aluno.idade);
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
    }
}
