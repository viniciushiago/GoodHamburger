using GoodHamburger.Application.Pedidos.DTO;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodHamburger.Application.Pedidos.Queries.ObterPedidoPorId
{
    public class ObterPedidoPorIdQuery : IRequest<PedidoResponse>
    {
        public long Id { get; set; }
        public ObterPedidoPorIdQuery(long id) => Id = id;
    }
}
