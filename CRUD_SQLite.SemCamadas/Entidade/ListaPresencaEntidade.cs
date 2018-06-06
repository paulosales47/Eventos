using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD_SQLite.SemCamadas.Entidade
{
    public class ListaPresencaEntidade
    {
        private int _idListaPresenca;

        public int IdListaPresenca
        {
            get { return _idListaPresenca; }
            set { _idListaPresenca = value; }
        }

        private string _nome;

        public string Nome
        {
            get { return _nome; }
            set { _nome = value; }
        }

        private string _bairro;

        public string Bairro
        {
            get { return _bairro; }
            set { _bairro = value; }
        }

        private string _entidade;

        public string Entidade
        {
            get { return _entidade; }
            set { _entidade = value; }
        }

        private int _salaTecnica;

        public int SalaTecnica
        {
            get { return _salaTecnica; }
            set { _salaTecnica = value; }
        }

        private string _observacao;

        public string Observacao
        {
            get { return _observacao; }
            set { _observacao = value; }
        }
    }
}
