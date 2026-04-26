using GoodHamburger.Application.Pedidos.DTO;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodHamburger.Application.Pedidos.Queries.ObterTodosPedidos
{
    public class ObterTodosPedidosQuery : IRequest<List<PedidoResponse>>
    {
    }
}
