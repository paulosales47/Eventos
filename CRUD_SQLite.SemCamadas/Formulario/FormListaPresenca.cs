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
    public partial class FormListaPresenca : Form
    {
        ListaPresencaEntidade _entidade;
        string operacao = "";
        string connectionString = @"Data Source=C:\APP\DADOS\REGISTROS.db";

        public FormListaPresenca(ListaPresencaEntidade entidade)
        {
            InitializeComponent();
            _entidade = entidade;
            this.CenterToScreen();
        }

        private void FormListaPresenca_Load(object sender, EventArgs e)
        {
            if (_entidade == null)
            {
                txtNome.Focus();
                operacao = "incluir";
            }
            else
            {
                operacao = "alterar";
                txtId.Text = _entidade.IdListaPresenca.ToString();
                txtNome.Text = _entidade.Nome;
                txtBairro.Text = _entidade.Bairro;
                txtEntidade.Text = _entidade.Entidade;
                cbSalaTecnica.Text = _entidade.SalaTecnica.ToString();
                txtObservacao.Text = _entidade.Observacao;
            }

            CarregaEventos();
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

        public int IncluirDados(ListaPresencaEntidade entidade)
        {
            int resultado = -1;
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(conn))
                {
                    cmd.CommandText = "INSERT INTO TB_LISTA_PRESENCA(NOME, BAIRRO, ENTIDADE, SALA_TECNICA, OBSERVACOES, ID_EVENTO) VALUES (@nome,@bairro,@entidade,@sala,@observacoes,@id_evento)";
                    cmd.Prepare();
                    cmd.Parameters.AddWithValue("@nome", entidade.Nome);
                    cmd.Parameters.AddWithValue("@bairro", entidade.Bairro);
                    cmd.Parameters.AddWithValue("@entidade", entidade.Entidade);
                    cmd.Parameters.AddWithValue("@sala", entidade.SalaTecnica);
                    cmd.Parameters.AddWithValue("@observacoes", entidade.Observacao);
                    cmd.Parameters.AddWithValue("@id_evento", entidade.Evento.IdEvento);
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

        public int AtualizaDados(ListaPresencaEntidade entidade)
        {
            int resultado = -1;
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(conn))
                {
                    cmd.CommandText = "UPDATE TB_LISTA_PRESENCA SET NOME=@nome, BAIRRO=@bairro, ENTIDADE=@entidade, SALA_TECNICA=@sala, OBSERVACOES=@observacoes, ID_EVENTO=@id_evento WHERE ID_LISTA_PRESENCA = @id";
                    cmd.Prepare();
                    cmd.Parameters.AddWithValue("@id", entidade.IdListaPresenca);
                    cmd.Parameters.AddWithValue("@nome", entidade.Nome);
                    cmd.Parameters.AddWithValue("@bairro", entidade.Bairro);
                    cmd.Parameters.AddWithValue("@entidade", entidade.Entidade);
                    cmd.Parameters.AddWithValue("@sala", entidade.SalaTecnica);
                    cmd.Parameters.AddWithValue("@observacoes", entidade.Observacao);
                    cmd.Parameters.AddWithValue("@id_evento", entidade.Evento.IdEvento);
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

        private void CarregaEventos()
        {
            cbEvento.DisplayMember = "NOME";
            cbEvento.ValueMember = "ID_EVENTO";
            cbEvento.DataSource = LeDadosEventos<SQLiteConnection, SQLiteDataAdapter>("SELECT * FROM TB_EVENTO");
        }

        public DataTable LeDadosEventos<S, T>(string query) where S : IDbConnection, new()
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

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            if (ValidaDados())
            {
                try
                {
                    var eventoSelecionado = Convert.ToInt32(cbEvento.SelectedValue.ToString());
                    var salaSelecionada = Convert.ToInt32(cbSalaTecnica.SelectedItem.ToString());

                    var entidade = new ListaPresencaEntidade
                    {
                        IdListaPresenca = int.Parse(txtId.Text),
                        Nome = txtNome.Text,
                        Bairro = txtBairro.Text,
                        Entidade = txtEntidade.Text,
                        Evento = new EventoEntidade { IdEvento = eventoSelecionado },
                        SalaTecnica = salaSelecionada,
                        Observacao = txtObservacao.Text,

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
