using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CRUD_SQLite.SemCamadas.Formulario
{
    public partial class Home : Form
    {
        public Home()
        {
            InitializeComponent();
            this.CenterToScreen();
        }

        private void eventosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new Evento().ShowDialog();
        }

        private void listaDePresençaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new ListaPresenca().ShowDialog();
        }

        private void pessoaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new Pessoa().ShowDialog();
        }

        private void presençaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new PresencaEvento().ShowDialog();
        }

        private void visualizarExportarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new VisualizaPresenca().ShowDialog();
        }

        private void btnSair_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Home_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
