using FluentAssertions;
using GoodHamburger.Application.Common.Interfaces;
using GoodHamburger.Application.Pedidos.Command.AtualizarPedido;
using GoodHamburger.Domain.Entities;
using GoodHamburger.Domain.Enums;
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
    public class AtualizarPedidoHandlerTests
    {
        private readonly IPedidoRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly AtualizarPedidoHandler _handler;

        public AtualizarPedidoHandlerTests()
        {
            _repository = Substitute.For<IPedidoRepository>();
            _unitOfWork = Substitute.For<IUnitOfWork>();
            _handler = new AtualizarPedidoHandler(_repository, _unitOfWork);
        }

        [Fact]
        public async Task Handle_PedidoExistente_DeveAtualizarERetornarResponse()
        {
            var pedidoExistente = PedidoBuilder.BuildPedidoComSanduiche();
            _repository.GetByIdTrackedAsync(Arg.Any<long>(), Arg.Any<CancellationToken>())
                .Returns(pedidoExistente);

            var command = PedidoBuilder.BuildAtualizarPedidoCommand(1,
                TipoItem.Sanduiche,
                TipoItem.Batata,
                TipoItem.Refrigerante);

            var response = await _handler.Handle(command, CancellationToken.None);

            response.Should().NotBeNull();
            response.Desconto.Should().Be(response.Subtotal * 0.20m);
            response.Total.Should().Be(response.Subtotal * 0.80m);
        }

        [Fact]
        public async Task Handle_PedidoExistente_DeveChamarUpdateAsyncESaveChanges()
        {
            var pedidoExistente = PedidoBuilder.BuildPedidoComSanduiche();
            _repository.GetByIdTrackedAsync(Arg.Any<long>(), Arg.Any<CancellationToken>())
                .Returns(pedidoExistente);

            var command = PedidoBuilder.BuildAtualizarPedidoCommand(1, TipoItem.Sanduiche);

            await _handler.Handle(command, CancellationToken.None);

            await _repository.Received(1).UpdateAsync(Arg.Any<Pedido>(), Arg.Any<CancellationToken>());
            await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_PedidoNaoEncontrado_DeveLancarNaoEncontradoException()
        {
            _repository.GetByIdTrackedAsync(Arg.Any<long>(), Arg.Any<CancellationToken>())
                .Returns((Pedido?)null);

            var command = PedidoBuilder.BuildAtualizarPedidoCommand(9999, TipoItem.Sanduiche);

            var acao = async () => await _handler.Handle(command, CancellationToken.None);

            await acao.Should().ThrowAsync<NaoEncontradoException>()
                .WithMessage("Pedido 9999 não encontrado.");
        }

        [Fact]
        public async Task Handle_ComItensDuplicados_DeveLancarNegocioException()
        {
            var pedidoExistente = PedidoBuilder.BuildPedidoComSanduiche();
            _repository.GetByIdTrackedAsync(Arg.Any<long>(), Arg.Any<CancellationToken>())
                .Returns(pedidoExistente);

            var command = PedidoBuilder.BuildAtualizarPedidoCommand(1,
                TipoItem.Sanduiche,
                TipoItem.Sanduiche);

            var acao = async () => await _handler.Handle(command, CancellationToken.None);

            await acao.Should().ThrowAsync<NegocioException>();
        }

        [Fact]
        public async Task Handle_SemSanduiche_DeveLancarNegocioException()
        {
            var pedidoExistente = PedidoBuilder.BuildPedidoComSanduiche();
            _repository.GetByIdTrackedAsync(Arg.Any<long>(), Arg.Any<CancellationToken>())
                .Returns(pedidoExistente);

            var command = PedidoBuilder.BuildAtualizarPedidoCommand(1, TipoItem.Batata);

            var acao = async () => await _handler.Handle(command, CancellationToken.None);

            await acao.Should().ThrowAsync<NegocioException>()
                .WithMessage("Pedido deve conter um sanduíche");
        }
    }
}
