using GoodHamburger.Domain.Cardapio;
using GoodHamburger.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace GoodHamburger.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardapioController : ControllerBase
    {
        [HttpGet]
        public IActionResult ObterCardapio()
        {
            var cardapio = new
            {
                sanduiches = CardapioItems.Itens
                    .Where(i => i.Tipo == TipoItem.Sanduiche)
                    .Select(i => new { i.Nome, i.Preco, Tipo = i.Tipo.ToString() }),

                                acompanhamentos = CardapioItems.Itens
                    .Where(i => i.Tipo == TipoItem.Batata)
                    .Select(i => new { i.Nome, i.Preco, Tipo = i.Tipo.ToString() }),

                                bebidas = CardapioItems.Itens
                    .Where(i => i.Tipo == TipoItem.Refrigerante)
                    .Select(i => new { i.Nome, i.Preco, Tipo = i.Tipo.ToString() }),

                regrasDeDesconto = new[]
                {
                    new { combinacao = "Sanduíche + Batata + Refrigerante", desconto = "20%" },
                    new { combinacao = "Sanduíche + Refrigerante", desconto = "15%" },
                    new { combinacao = "Sanduíche + Batata", desconto = "10%" }
                }
            };

            return Ok(cardapio);
        }
    }
}