using GoodHamburger.Application.Common.Interfaces;
using GoodHamburger.Domain.Exceptions;
using GoodHamburger.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodHamburger.Application.Pedidos.Command.DeletarPedido
{
    public class DeletarPedidoHandler : IRequestHandler<DeletarPedidoCommand>
    {
        private readonly IPedidoRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public DeletarPedidoHandler(IPedidoRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(DeletarPedidoCommand command, CancellationToken cancellationToken)
        {
            var pedido = await _repository.GetByIdTrackedAsync(command.Id, cancellationToken)
                ?? throw new NaoEncontradoException($"Pedido {command.Id} não encontrado.");
           
            await _repository.DeleteAsync(pedido, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
