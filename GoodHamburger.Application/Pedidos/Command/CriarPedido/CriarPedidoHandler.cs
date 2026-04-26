using GoodHamburger.Application.Common.Interfaces;
using GoodHamburger.Application.Pedidos.DTO;
using GoodHamburger.Domain.Entities;
using GoodHamburger.Domain.Enums;
using GoodHamburger.Domain.Repositories;
using MediatR;

namespace GoodHamburger.Application.Pedidos.Command.CriarPedido
{
    public class CriarPedidoHandler : IRequestHandler<CriarPedidoCommand, PedidoResponse>
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CriarPedidoHandler(IPedidoRepository pedidoRepository, IUnitOfWork unitOfWork)
        {
            _pedidoRepository = pedidoRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<PedidoResponse> Handle(CriarPedidoCommand request, CancellationToken cancellationToken)
        {
            var pedido = new Pedido();

            foreach (var item in request.Itens)
            {
                var itemPedido = new ItemPedido(
                    (TipoItem)item.Tipo,
                    item.Nome,
                    item.Preco
                );

                pedido.AdicionarItem(itemPedido);
            }

            pedido.ValidarPedido();

            await _pedidoRepository.AddAsync(pedido, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new PedidoResponse
            {
                Id = pedido.Id,
                Subtotal = pedido.Subtotal,
                Total = pedido.Total,
                Desconto = pedido.Desconto,
                Itens = pedido.Itens.Select(x => new ItemPedidoResponse
                {
                    Tipo = x.Tipo.ToString(),
                    Nome = x.Nome,
                    Preco = x.Preco
                }).ToList()
            };
        }
    }
}