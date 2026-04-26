using Bogus;
using GoodHamburger.Application.Pedidos.Command.AtualizarPedido;
using GoodHamburger.Application.Pedidos.Command.CriarPedido;
using GoodHamburger.Application.Pedidos.DTO;
using GoodHamburger.Domain.Cardapio;
using GoodHamburger.Domain.Entities;
using GoodHamburger.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodHamburger.Tests.Builders
{
    public class PedidoBuilder
    {
        private static readonly Faker Faker = new Faker("pt_BR");

        public static ItemPedidoRequest BuildItemRequest(TipoItem tipo)
        {
            var itemCardapio = CardapioItems.Itens
                .Where(i => i.Tipo == tipo)
                .OrderBy(_ => Faker.Random.Int())
                .First();

            return new ItemPedidoRequest
            {
                Tipo = tipo,
                Nome = itemCardapio.Nome,
                Preco = itemCardapio.Preco
            };
        }

        public static ItemPedido BuildItem(TipoItem tipo)
        {
            var itemCardapio = CardapioItems.Itens
                .Where(i => i.Tipo == tipo)
                .OrderBy(_ => Faker.Random.Int())
                .First();

            return new ItemPedido(
                tipo,
                itemCardapio.Nome,
                itemCardapio.Preco
            );

        }

        public static Pedido BuildPedidoComSanduiche()
        {
            var pedido = new Pedido();
            pedido.AdicionarItem(BuildItem(TipoItem.Sanduiche));
            return pedido;
        }

        public static Pedido BuildPedidoCompleto()
        {
            var pedido = new Pedido();
            pedido.AdicionarItem(BuildItem(TipoItem.Sanduiche));
            pedido.AdicionarItem(BuildItem(TipoItem.Batata));
            pedido.AdicionarItem(BuildItem(TipoItem.Refrigerante));
            return pedido;
        }

        public static CriarPedidoCommand BuildCriarCommand(params TipoItem[] tipos)
        {
            var itens = tipos.Select(BuildItemRequest).ToList();
            return new CriarPedidoCommand(itens);
        }

        public static AtualizarPedidoCommand BuildAtualizarPedidoCommand(long id, params TipoItem[] tipos)
        {
            var itens = tipos.Select(BuildItemRequest).ToList();
            return new AtualizarPedidoCommand { Id = id, Itens = itens };
        }
    }
}
