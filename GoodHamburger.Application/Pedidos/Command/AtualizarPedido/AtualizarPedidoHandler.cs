using GoodHamburger.Application.Common.Interfaces;
using GoodHamburger.Application.Pedidos.DTO;
using GoodHamburger.Domain.Entities;
using GoodHamburger.Domain.Enums;
using GoodHamburger.Domain.Exceptions;
using GoodHamburger.Domain.Repositories;
using MediatR;
using Microsoft.IdentityModel.Tokens.Experimental;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodHamburger.Application.Pedidos.Command.AtualizarPedido
{
    public class AtualizarPedidoHandler : IRequestHandler<AtualizarPedidoCommand, PedidoResponse>
    {
        private readonly IPedidoRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public AtualizarPedidoHandler(IPedidoRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<PedidoResponse> Handle(AtualizarPedidoCommand command, CancellationToken cancellationToken)
        {
            var pedido = await _repository.GetByIdTrackedAsync(command.Id, cancellationToken)
                ?? throw new NaoEncontradoException($"Pedido {command.Id} não encontrado.");

            pedido.LimparItens();
            foreach (var item in command.Itens)
            {
                var itemPedido = new ItemPedido((TipoItem)item.Tipo, item.Nome, item.Preco);
                pedido.AdicionarItem(itemPedido);
            }

            pedido.ValidarPedido();

            await _repository.UpdateAsync(pedido, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

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
