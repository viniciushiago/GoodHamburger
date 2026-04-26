using FluentValidation.TestHelper;
using GoodHamburger.Application.Pedidos.Command.CriarPedido;
using GoodHamburger.Application.Pedidos.DTO;
using GoodHamburger.Domain.Enums;
using GoodHamburger.Tests.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodHamburger.Tests.Validators
{
    public class CriarPedidoValidatorTests
    {
        private readonly CriarPedidoValidator _validator;

        public CriarPedidoValidatorTests()
        {
            _validator = new CriarPedidoValidator();
        }

        [Fact]
        public async Task Validator_PedidoValido_NaoDeveRetornarErros()
        {
            var command = PedidoBuilder.BuildCriarCommand(TipoItem.Sanduiche, TipoItem.Batata);

            var result = await _validator.TestValidateAsync(command);

            result.ShouldNotHaveAnyValidationErrors();
        }
        
        [Fact]
        public async Task Validator_ListaVazia_DeveRetornarErro()
        {
            var command = new CriarPedidoCommand([]);

            var result = await _validator.TestValidateAsync(command);

            result.ShouldHaveValidationErrorFor(x => x.Itens)
                .WithErrorMessage("O pedido deve ter pelo menos 1 item");
        }

        [Fact]
        public async Task Validator_NomeVazio_DeveRetornarErro()
        {
            var command = PedidoBuilder.BuildCriarCommand(TipoItem.Sanduiche);
            command.Itens[0].Nome = "";

            var result = await _validator.TestValidateAsync(command);

            result.ShouldHaveValidationErrorFor("Itens[0].Nome")
                .WithErrorMessage("Nome é obrigatório");
        }

        [Fact]
        public async Task Validator_PrecoZerado_DeveRetornarErro()
        {
            var command = PedidoBuilder.BuildCriarCommand(TipoItem.Sanduiche);
            command.Itens[0].Preco = 0;

            var result = await _validator.TestValidateAsync(command);

            result.ShouldHaveValidationErrorFor("Itens[0].Preco")
                .WithErrorMessage("Preço deve ser maior que zero");
        }

        [Fact]
        public async Task Validator_TipoInvalido_DeveRetornarErro()
        {
            var command = PedidoBuilder.BuildCriarCommand(TipoItem.Sanduiche);
            command.Itens[0].Tipo = (TipoItem)99;

            var result = await _validator.TestValidateAsync(command);

            result.ShouldHaveValidationErrorFor("Itens[0].Tipo")
                .WithErrorMessage("Tipo inválido");
        }

        [Fact]
        public async Task Validator_ItemNaoExisteNoCardapio_DeveRetornarErro()
        {
            var command = PedidoBuilder.BuildCriarCommand(TipoItem.Sanduiche);
            command.Itens[0].Nome = "X-Frango";
            command.Itens[0].Preco = 5.00m;

            var result = await _validator.TestValidateAsync(command);

            result.ShouldHaveValidationErrorFor("Itens[0]")
                .WithErrorMessage("Item não encontrado no cardápio ou preço incorreto");
        }

        [Fact]
        public async Task Validator_PrecoIncorretoNoCardapio_DeveRetornarErro()
        {
            var command = PedidoBuilder.BuildCriarCommand(TipoItem.Sanduiche);
            command.Itens[0].Nome = "X-Burger";
            command.Itens[0].Preco = 999.00m;

            var result = await _validator.TestValidateAsync(command);

            result.ShouldHaveValidationErrorFor("Itens[0]")
                .WithErrorMessage("Item não encontrado no cardápio ou preço incorreto");
        }

        [Fact]
        public async Task Validator_ItemValidoNoCardapio_NaoDeveRetornarErro()
        {
            var command = new CriarPedidoCommand(new List<ItemPedidoRequest>
            {
                new() { Tipo = TipoItem.Sanduiche, Nome = "X-Burger", Preco = 5.00m }
            });

            var result = await _validator.TestValidateAsync(command);

            result.ShouldNotHaveValidationErrorFor("Itens[0]");
        }
    }
}
