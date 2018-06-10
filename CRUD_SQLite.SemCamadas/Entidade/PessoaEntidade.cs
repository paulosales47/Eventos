using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD_SQLite.SemCamadas.Entidade
{
    public class PessoaEntidade
    {
        private int _idPessoa;

        public int IdPessoa
        {
            get { return _idPessoa; }
            set { _idPessoa = value; }
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

        private string _email;

        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }

        private string _cpf;

        public string Cpf
        {
            get { return _cpf; }
            set { _cpf = value; }
        }

        private string _rg;

        public string Rg
        {
            get { return _rg; }
            set { _rg = value; }
        }

        private string _entidade;

        public string Entidade
        {
            get { return _entidade; }
            set { _entidade = value; }
        }

    }
}
