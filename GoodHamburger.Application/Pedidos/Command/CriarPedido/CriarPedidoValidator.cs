using FluentValidation;
using GoodHamburger.Domain.Cardapio;
using GoodHamburger.Domain.Enums;

namespace GoodHamburger.Application.Pedidos.Command.CriarPedido
{
    public class CriarPedidoValidator : AbstractValidator<CriarPedidoCommand>
    {
        public CriarPedidoValidator()
        {

            RuleFor(x => x.Itens)
                .NotEmpty().WithMessage("O pedido deve ter pelo menos 1 item");

            RuleForEach(x => x.Itens).ChildRules(item =>
            {
                item.RuleFor(i => i.Nome)
                    .NotEmpty().WithMessage("Nome é obrigatório");

                item.RuleFor(i => i.Preco)
                    .GreaterThan(0).WithMessage("Preço deve ser maior que zero");

                item.RuleFor(i => i.Tipo)
                    .IsInEnum().WithMessage("Tipo inválido");

                item.RuleFor(i => i)
                    .Must(i =>
                    {
                        var itemCardapio = CardapioItems.Itens
                            .FirstOrDefault(c => c.Nome.Equals(i.Nome, StringComparison.OrdinalIgnoreCase)
                                              && c.Tipo == (TipoItem)i.Tipo);

                        return itemCardapio != null && itemCardapio.Preco == i.Preco;
                    })
                    .WithMessage("Item não encontrado no cardápio ou preço incorreto");
            });
        }


    }
}
