using GoodHamburger.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodHamburger.Domain.Cardapio
{
    public class CardapioItems
    {
        public static readonly IReadOnlyList<CardapioItem> Itens = new List<CardapioItem>
        {
            new(TipoItem.Sanduiche, "X-Burger", 5.00m),
            new(TipoItem.Sanduiche, "X-Egg", 4.50m),
            new(TipoItem.Sanduiche, "X-Bacon", 7.00m),
            new(TipoItem.Batata,    "Batata frita", 2.00m),
            new(TipoItem.Refrigerante, "Refrigerante", 2.50m)
        };
    }

    public record CardapioItem(TipoItem Tipo, string Nome, decimal Preco);
}
