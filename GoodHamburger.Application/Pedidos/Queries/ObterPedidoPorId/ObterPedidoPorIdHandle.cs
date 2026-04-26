using GoodHamburger.Application.Pedidos.DTO;
using GoodHamburger.Domain.Exceptions;
using GoodHamburger.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodHamburger.Application.Pedidos.Queries.ObterPedidoPorId
{
    public class ObterPedidoPorIdHandle : IRequestHandler<ObterPedidoPorIdQuery, PedidoResponse>
    {
        private readonly IPedidoRepository _repository;

        public ObterPedidoPorIdHandle(IPedidoRepository repository)
        {
            _repository = repository;
        }

        public async Task<PedidoResponse> Handle(ObterPedidoPorIdQuery request, CancellationToken cancellationToken)
        {
            var pedido = await _repository.GetByIdAsync(request.Id, cancellationToken)
                ?? throw new NaoEncontradoException($"Pedido {request.Id} não encontrado.");

            return new PedidoResponse
            {
                Id = pedido.Id,
                Subtotal = pedido.Subtotal,
                Desconto = pedido.Desconto,
                Total = pedido.Total,
                Itens = pedido.Itens.Select(item => new ItemPedidoResponse
                {
                    Nome = item.Nome,
                    Preco = item.Preco,
                    Tipo = item.Tipo.ToString()
                }).ToList()
            };
        }
    }
}
