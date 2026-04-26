namespace GoodHamburger.Blazor.Models.Cardapio
{
    public class CardapioResponse
    {
        public List<ItemCardapio> Sanduiches { get; set; } = new();
        public List<ItemCardapio> Acompanhamentos { get; set; } = new();
        public List<ItemCardapio> Bebidas { get; set; } = new();
        public List<RegraDesconto> RegrasDeDesconto { get; set; } = new();
    }
}
