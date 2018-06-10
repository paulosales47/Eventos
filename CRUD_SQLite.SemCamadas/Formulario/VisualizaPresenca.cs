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
    public partial class VisualizaPresenca : Form
    {
        string connectionString = @"Data Source=C:\APP\DADOS\REGISTROS.db";
        
        //CONSTRUTOR
        public VisualizaPresenca()
        {
            InitializeComponent();
            this.CenterToScreen();
        }

        //LOAD
        private void VisualizaPresenca_Load(object sender, EventArgs e)
        {
            CarregaListaEvento();
        }
        
        //BUSCA LISTA DE EVENTOS
        private void CarregaListaEvento()
        {
            cbListaEvento.DisplayMember = "NOME";
            cbListaEvento.ValueMember = "ID_EVENTO";
            cbListaEvento.DataSource = BuscaListaEventos<SQLiteConnection, SQLiteDataAdapter>
                (
                    "SELECT "
                        + "ID_EVENTO"
                        + ",NOME"
                   + " FROM "
                       + "TB_EVENTO");
        }

        public DataTable BuscaListaEventos<S, T>(string query) where S : IDbConnection, new()
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

        //LISTA PRESENÇA - ID_LISTA_PRESENCA
        private void CarregaListaPresencaId(string idEvento)
        {
            dataGridView1.DataSource = BuscaListaPresencaId<SQLiteConnection, SQLiteDataAdapter>
            (
                "SELECT "
                    + "EVENTO.NOME AS 'NOME DO EVENTO'"
                    + ",EVENTO.LOCAL AS 'LOCAL DO EVENTO'"
                    + ",LISTA_PRESENCA.NOME AS 'NOME DA LISTA DE PRESENÇA'"
                    + ",PESSOA.NOME AS 'NOME DA PESSOA'"
                    + ",PESSOA.EMAIL AS 'EMAIL DA PESSOA'"
                    + ",PESSOA.BAIRRO AS 'BAIRRO DA PESSOA' "
                    + ",PESSOA.CPF "
                    + ",PESSOA.RG "
               + "FROM "
                   + "TB_EVENTO AS EVENTO "
               + "INNER JOIN "
                   + "TB_LISTA_PRESENCA AS LISTA_PRESENCA "
                   + "ON EVENTO.ID_EVENTO = LISTA_PRESENCA.ID_EVENTO "
               + "INNER JOIN "
                   + "TB_PESSOA_LISTA_PRESENCA AS PESSOA_LISTA "
                   + "ON LISTA_PRESENCA.ID_LISTA_PRESENCA = PESSOA_LISTA.ID_LISTA_PRESENCA "
               + "INNER JOIN "
                   + "TB_PESSOA AS PESSOA "
                   + "ON PESSOA_LISTA.ID_PESSOA = PESSOA.ID_PESSOA "
               + "WHERE EVENTO.ID_EVENTO = " + idEvento + " "
               + "ORDER BY "
                   + "LISTA_PRESENCA.NOME "
                   + ",PESSOA.NOME "
            );

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

        //EVENTO DE SELEÇÃO DA LISTA DE EVENTOS
        private void cbListaEvento_SelectedIndexChanged(object sender, EventArgs e)
        {
            var idEvento = cbListaEvento.SelectedValue.ToString();
            CarregaListaPresencaId(idEvento);
        }

        //BOTÃO EXPORTAR
        private void btnExportar_Click(object sender, EventArgs e)
        {
            copyAlltoClipboard();
            Microsoft.Office.Interop.Excel.Application xlexcel;
            Microsoft.Office.Interop.Excel.Workbook xlWorkBook;
            Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet;
            object misValue = System.Reflection.Missing.Value;
            xlexcel = new Microsoft.Office.Interop.Excel.Application();
            xlexcel.Visible = true;
            xlWorkBook = xlexcel.Workbooks.Add(misValue);
            xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
            Microsoft.Office.Interop.Excel.Range CR = (Microsoft.Office.Interop.Excel.Range)xlWorkSheet.Cells[1, 1];
            CR.Select();
            xlWorkSheet.PasteSpecial(CR, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, true);
        }

        //MÉTODO DE EXPORTAÇÃO
        private void copyAlltoClipboard()
        {
            dataGridView1.SelectAll();
            DataObject dataObj = dataGridView1.GetClipboardContent();
            if (dataObj != null)
                Clipboard.SetDataObject(dataObj);
        }

        private void btnSair_Click(object sender, EventArgs e)
        {
            this.Close();
        }

       
    }
}
