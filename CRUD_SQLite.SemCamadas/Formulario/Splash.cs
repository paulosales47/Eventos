using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CRUD_SQLite.SemCamadas.Formulario
{
    public partial class Splash : Form
    {
        public Splash()
        {
            InitializeComponent();
        }
        
        private void Splash_Shown(object sender, EventArgs e)
        {
            timer1 = new Timer();
            timer1.Interval = 3000;
            timer1.Start();
            timer1.Tick += timer1_Tick;
        }
        void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            var home = new Home();
            home.Show();

            this.Hide();
        }
        private void timer2_Tick(object sender, EventArgs e)
        {
            if (progressBar1.Value < 100)
            {
                progressBar1.Value += 1;
            }
        }
    }
}
