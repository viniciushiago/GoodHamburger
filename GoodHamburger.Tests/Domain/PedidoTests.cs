using FluentAssertions;
using GoodHamburger.Domain.Entities;
using GoodHamburger.Domain.Enums;
using GoodHamburger.Domain.Exceptions;
using GoodHamburger.Tests.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodHamburger.Tests.Domain
{
    public class PedidoTests
    {

        [Fact]
        public void Pedido_ComSanduicheBatataRefrigerante_DeveAplicar20PorCentoDesconto()
        {
            var pedido = PedidoBuilder.BuildPedidoCompleto();

            pedido.Desconto.Should().Be(pedido.Subtotal * 0.20m);
            pedido.Total.Should().Be(pedido.Subtotal * 0.80m);
        }

        [Fact]
        public void Pedido_ComSanduicheERefrigerante_DeveAplicar15PorCentoDesconto()
        {
            var pedido = new Pedido();
            pedido.AdicionarItem(PedidoBuilder.BuildItem(TipoItem.Sanduiche));
            pedido.AdicionarItem(PedidoBuilder.BuildItem(TipoItem.Refrigerante));

            pedido.Desconto.Should().Be(pedido.Subtotal * 0.15m);
            pedido.Total.Should().Be(pedido.Subtotal * 0.85m);
        }

        [Fact]
        public void Pedido_ComSanduicheEBatata_DeveAplicar10PorCentoDesconto()
        {
            var pedido = new Pedido();
            pedido.AdicionarItem(PedidoBuilder.BuildItem(TipoItem.Sanduiche));
            pedido.AdicionarItem(PedidoBuilder.BuildItem(TipoItem.Batata));

            pedido.Desconto.Should().Be(pedido.Subtotal * 0.10m);
            pedido.Total.Should().Be(pedido.Subtotal * 0.90m);
        }

        [Fact]
        public void Pedido_ComApenasOSanduiche_NaoDeveAplicarDesconto()
        {
            var pedido = PedidoBuilder.BuildPedidoComSanduiche();

            pedido.Desconto.Should().Be(0);
            pedido.Total.Should().Be(pedido.Subtotal);
        }

        [Fact]
        public void Pedido_DeveCalcularSubtotalCorretamente()
        {
            var pedido = new Pedido();
            var item1 = PedidoBuilder.BuildItem(TipoItem.Sanduiche);
            var item2 = PedidoBuilder.BuildItem(TipoItem.Batata);

            pedido.AdicionarItem(item1);
            pedido.AdicionarItem(item2);

            pedido.Subtotal.Should().Be(item1.Preco + item2.Preco);
        }


        [Fact]
        public void AdicionarItem_ComTipoDuplicado_DeveLancarNegocioException()
        {
            var pedido = new Pedido();
            pedido.AdicionarItem(PedidoBuilder.BuildItem(TipoItem.Sanduiche));

            var acao = () => pedido.AdicionarItem(PedidoBuilder.BuildItem(TipoItem.Sanduiche));

            acao.Should().Throw<NegocioException>()
                .WithMessage("Já existe um item do tipo Sanduiche no pedido");
        }

        [Fact]
        public void ValidarPedido_SemSanduiche_DeveLancarNegocioException()
        {
            var pedido = new Pedido();
            pedido.AdicionarItem(PedidoBuilder.BuildItem(TipoItem.Batata));

            var acao = () => pedido.ValidarPedido();

            acao.Should().Throw<NegocioException>()
                .WithMessage("Pedido deve conter um sanduíche");
        }

        [Fact]
        public void ValidarPedido_ComSanduiche_NaoDeveLancarExcecao()
        {
            var pedido = PedidoBuilder.BuildPedidoComSanduiche();

            var acao = () => pedido.ValidarPedido();

            acao.Should().NotThrow();
        }

        [Fact]
        public void LimparItens_DeveRemoverTodosOsItens()
        {
            var pedido = PedidoBuilder.BuildPedidoCompleto();

            pedido.LimparItens();

            pedido.Itens.Should().BeEmpty();
            pedido.Subtotal.Should().Be(0);
            pedido.Desconto.Should().Be(0);
            pedido.Total.Should().Be(0);
        }
    }
}
