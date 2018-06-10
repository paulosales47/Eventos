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
    public partial class FormEvento : Form
    {
        EventoEntidade _entidade;
        string operacao = "";
        string connectionString = @"Data Source=C:\APP\DADOS\REGISTROS.db";
        
        public FormEvento(EventoEntidade entidade)
        {
            InitializeComponent();
            _entidade = entidade;
            this.CenterToScreen();
        }

        private void FormEvento_Load(object sender, EventArgs e)
        {
            if (_entidade == null)
            {
                txtNome.Focus();
                operacao = "incluir";
            }
            else
            {
                operacao = "alterar";
                txtId.Text = _entidade.IdEvento.ToString();
                txtNome.Text = _entidade.Nome;
                txtLocal.Text = _entidade.Local;
                dataEvento.Value = _entidade.Data;
            }
        }

        private Boolean ValidaDados()
        {
            bool retorno = true;

            if (txtNome.Text == string.Empty)
                retorno = false;

            if (txtLocal.Text == string.Empty)
                retorno = false;

            return retorno;
        }

        public int IncluirDados(EventoEntidade entidade)
        {
            int resultado = -1;
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(conn))
                {
                    cmd.CommandText = "INSERT INTO TB_EVENTO(NOME,LOCAL,DATA) VALUES (@nome,@local,@data)";
                    cmd.Prepare();
                    cmd.Parameters.AddWithValue("@nome", entidade.Nome);
                    cmd.Parameters.AddWithValue("@local", entidade.Local);
                    cmd.Parameters.AddWithValue("@data", entidade.Data);
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

        public int AtualizaDados(EventoEntidade entidade)
        {
            int resultado = -1;
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(conn))
                {
                    cmd.CommandText = "UPDATE TB_EVENTO SET NOME=@nome, LOCAL=@local, DATA=@data WHERE ID_EVENTO = @id";
                    cmd.Prepare();
                    cmd.Parameters.AddWithValue("@Id", entidade.IdEvento);
                    cmd.Parameters.AddWithValue("@nome", entidade.Nome);
                    cmd.Parameters.AddWithValue("@local", entidade.Local);
                    cmd.Parameters.AddWithValue("@data", entidade.Data);
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

        //BOTÕES
        private void btnSalvar_Click(object sender, EventArgs e)
        {
            if (ValidaDados())
            {
                try
                {
                    var entidade = new EventoEntidade
                    {
                        IdEvento = int.Parse(txtId.Text),
                        Nome = txtNome.Text,
                        Local = txtLocal.Text,
                        Data = Convert.ToDateTime(dataEvento.Text)
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
