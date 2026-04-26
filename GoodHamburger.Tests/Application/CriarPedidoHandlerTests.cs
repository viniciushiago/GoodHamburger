using FluentAssertions;
using GoodHamburger.Application.Common.Interfaces;
using GoodHamburger.Application.Pedidos.Command.CriarPedido;
using GoodHamburger.Domain.Entities;
using GoodHamburger.Domain.Enums;
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
    public class CriarPedidoHandlerTests
    {
        private readonly IPedidoRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly CriarPedidoHandler _handler;

        public CriarPedidoHandlerTests()
        {
            _repository = Substitute.For<IPedidoRepository>();
            _unitOfWork = Substitute.For<IUnitOfWork>();
            _handler = new CriarPedidoHandler(_repository, _unitOfWork);
        }

        [Fact]
        public async Task Handle_PedidoCompleto_DeveRetornarResponseComDesconto20()
        {
            var command = PedidoBuilder.BuildCriarCommand(
                TipoItem.Sanduiche,
                TipoItem.Batata,
                TipoItem.Refrigerante);

            var response = await _handler.Handle(command, CancellationToken.None);

            response.Desconto.Should().Be(response.Subtotal * 0.20m);
            response.Total.Should().Be(response.Subtotal * 0.80m);
        }

        [Fact]
        public async Task Handle_PedidoComSanduicheERefrigerante_DeveRetornarResponseComDesconto15()
        {
            var command = PedidoBuilder.BuildCriarCommand(
                TipoItem.Sanduiche,
                TipoItem.Refrigerante);

            var response = await _handler.Handle(command, CancellationToken.None);

            response.Desconto.Should().Be(response.Subtotal * 0.15m);
            response.Total.Should().Be(response.Subtotal * 0.85m);
        }

        [Fact]
        public async Task Handle_PedidoComSanduicheEBatata_DeveRetornarResponseComDesconto10()
        {
            var command = PedidoBuilder.BuildCriarCommand(
                TipoItem.Sanduiche,
                TipoItem.Batata);

            var response = await _handler.Handle(command, CancellationToken.None);

            response.Desconto.Should().Be(response.Subtotal * 0.10m);
            response.Total.Should().Be(response.Subtotal * 0.90m);
        }

        [Fact]
        public async Task Handle_PedidoComApenasOSanduiche_DeveRetornarResponseSemDesconto()
        {
            var command = PedidoBuilder.BuildCriarCommand(TipoItem.Sanduiche);

            var response = await _handler.Handle(command, CancellationToken.None);

            response.Desconto.Should().Be(0);
            response.Total.Should().Be(response.Subtotal);
        }

        [Fact]
        public async Task Handle_PedidoValido_DeveChamarAddAsyncESaveChanges()
        {
            var command = PedidoBuilder.BuildCriarCommand(TipoItem.Sanduiche);

            await _handler.Handle(command, CancellationToken.None);

            await _repository.Received(1).AddAsync(Arg.Any<Pedido>(), Arg.Any<CancellationToken>());
            await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_ComItensDuplicados_DeveLancarNegocioException()
        {
            var command = PedidoBuilder.BuildCriarCommand(
                TipoItem.Sanduiche,
                TipoItem.Sanduiche);

            var acao = async () => await _handler.Handle(command, CancellationToken.None);

            await acao.Should().ThrowAsync<GoodHamburger.Domain.Exceptions.NegocioException>();
        }

        [Fact]
        public async Task Handle_SemSanduiche_DeveLancarNegocioException()
        {
            var command = PedidoBuilder.BuildCriarCommand(TipoItem.Batata);

            var acao = async () => await _handler.Handle(command, CancellationToken.None);

            await acao.Should().ThrowAsync<GoodHamburger.Domain.Exceptions.NegocioException>()
                .WithMessage("Pedido deve conter um sanduíche");
        }
    }
}
