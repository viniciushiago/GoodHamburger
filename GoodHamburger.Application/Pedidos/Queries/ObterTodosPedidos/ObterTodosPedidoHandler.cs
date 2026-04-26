using GoodHamburger.Application.Pedidos.DTO;
using GoodHamburger.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodHamburger.Application.Pedidos.Queries.ObterTodosPedidos
{
    public class ObterTodosPedidoHandler : IRequestHandler<ObterTodosPedidosQuery, List<PedidoResponse>>
    {
        private readonly IPedidoRepository _repository;

        public ObterTodosPedidoHandler(IPedidoRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<PedidoResponse>> Handle(ObterTodosPedidosQuery query, CancellationToken cancellationToken)
        {
            var pedidos = await _repository.GetAllAsync(cancellationToken);

            return pedidos.Select(pedidos => new PedidoResponse
            {
                Id = pedidos.Id,
                Subtotal = pedidos.Subtotal,
                Desconto = pedidos.Total,
                Total = pedidos.Total,
                Itens = pedidos.Itens.Select(itens => new ItemPedidoResponse
                {
                    Nome = itens.Nome,
                    Preco = itens.Preco,
                    Tipo = itens.Tipo.ToString()
                }).ToList()
            }).ToList();
        }
    }
}
