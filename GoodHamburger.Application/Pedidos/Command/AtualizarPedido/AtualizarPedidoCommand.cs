using GoodHamburger.Application.Pedidos.DTO;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodHamburger.Application.Pedidos.Command.AtualizarPedido
{
    public class AtualizarPedidoCommand : IRequest<PedidoResponse>
    {
        public long Id { get; set; }
        public List<ItemPedidoRequest> Itens {  get; set; }
    }
}
