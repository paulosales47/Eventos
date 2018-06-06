using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD_SQLite.SemCamadas.Entidade
{
    public class EventoEntidade
    {
        private int _idEvento;

        public int IdEvento
        {
            get { return _idEvento; }
            set { _idEvento = value; }
        }

        private int _nome;

        public int Nome
        {
            get { return _nome; }
            set { _nome = value; }
        }

        private DateTime _data;     

        public DateTime Data
        {
            get { return _data; }
            set { _data = value; }
        }

        private string _local;

        public string Local
        {
            get { return _local; }
            set { _local = value; }
        }
    }
}
