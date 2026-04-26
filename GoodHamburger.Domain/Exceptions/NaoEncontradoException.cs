using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodHamburger.Domain.Exceptions
{
    public class NaoEncontradoException : Exception
    {
        public NaoEncontradoException(string mensagem) : base(mensagem){}
    }
}
