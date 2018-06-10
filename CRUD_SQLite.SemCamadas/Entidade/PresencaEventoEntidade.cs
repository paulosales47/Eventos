using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD_SQLite.SemCamadas.Entidade
{
    public class PresencaEventoEntidade
    {
        public PresencaEventoEntidade()
        {
            _pessoaEntidade = new PessoaEntidade();
            _listaPresencaEntidade = new ListaPresencaEntidade();
        }

        private PessoaEntidade _pessoaEntidade;

        public PessoaEntidade Pessoa
        {
            get { return _pessoaEntidade; }
            set { _pessoaEntidade = value; }
        }

        private ListaPresencaEntidade _listaPresencaEntidade;

        public ListaPresencaEntidade ListaPresenca
        {
            get { return _listaPresencaEntidade; }
            set { _listaPresencaEntidade = value; }
        }

        private bool _presenteLista;

        public bool PresenteLista
        {
            get { return _presenteLista; }
            set { _presenteLista = value; }
        }


    }
}
