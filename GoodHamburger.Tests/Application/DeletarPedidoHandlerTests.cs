using FluentAssertions;
using GoodHamburger.Application.Common.Interfaces;
using GoodHamburger.Application.Pedidos.Command.DeletarPedido;
using GoodHamburger.Domain.Entities;
using GoodHamburger.Domain.Exceptions;
using GoodHamburger.Domain.Repositories;
using GoodHamburger.Tests.Builders;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodHamburger.Tests.Application
{
    public class DeletarPedidoHandlerTests
    {
        private readonly IPedidoRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly DeletarPedidoHandler _handler;

        public DeletarPedidoHandlerTests()
        {
            _repository = Substitute.For<IPedidoRepository>();
            _unitOfWork = Substitute.For<IUnitOfWork>();
            _handler = new DeletarPedidoHandler(_repository, _unitOfWork);
        }

        [Fact]
        public async Task Handle_PedidoExistente_DeveDeletarESalvar()
        {
            var pedidoExistente = PedidoBuilder.BuildPedidoComSanduiche();
            _repository.GetByIdTrackedAsync(Arg.Any<long>(), Arg.Any<CancellationToken>())
                .Returns(pedidoExistente);

            await _handler.Handle(new DeletarPedidoCommand(1), CancellationToken.None);

            await _repository.Received(1).DeleteAsync(Arg.Any<Pedido>(), Arg.Any<CancellationToken>());
            await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_PedidoNaoEncontrado_DeveLancarNaoEncontradoException()
        {
            _repository.GetByIdTrackedAsync(Arg.Any<long>(), Arg.Any<CancellationToken>())
                .Returns((Pedido?)null);

            var acao = async () => await _handler.Handle(new DeletarPedidoCommand(9999), CancellationToken.None);

            await acao.Should().ThrowAsync<NaoEncontradoException>()
                .WithMessage("Pedido 9999 não encontrado.");
        }

        [Fact]
        public async Task Handle_PedidoNaoEncontrado_NaoDeveChamarDeleteNemSaveChanges()
        {
            _repository.GetByIdTrackedAsync(Arg.Any<long>(), Arg.Any<CancellationToken>())
                .Returns((Pedido?)null);

            var acao = async () => await _handler.Handle(new DeletarPedidoCommand(9999), CancellationToken.None);

            await acao.Should().ThrowAsync<NaoEncontradoException>();
            await _repository.DidNotReceive().DeleteAsync(Arg.Any<Pedido>(), Arg.Any<CancellationToken>());
            await _unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
        }
    }
}
