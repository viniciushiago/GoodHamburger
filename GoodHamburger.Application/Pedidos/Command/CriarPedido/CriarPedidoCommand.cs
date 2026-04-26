using GoodHamburger.Application.Pedidos.DTO;
using MediatR;

namespace GoodHamburger.Application.Pedidos.Command.CriarPedido
{
    public class CriarPedidoCommand : IRequest<PedidoResponse>
    {
        public List<ItemPedidoRequest> Itens { get; set; }

        public CriarPedidoCommand(List<ItemPedidoRequest> itens)
        {
            Itens = itens;
        }
    }
}
