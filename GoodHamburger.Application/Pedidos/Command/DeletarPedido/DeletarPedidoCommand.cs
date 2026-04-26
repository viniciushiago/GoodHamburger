using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodHamburger.Application.Pedidos.Command.DeletarPedido
{
    public class DeletarPedidoCommand : IRequest
    {
        public long Id { get; set; }

        public DeletarPedidoCommand(long id) => Id = id;
    }
}
