namespace GoodHamburger.Blazor.Models.Pedidos
{
    public class PedidoResponse
    {
        public long Id { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Desconto { get; set; }
        public decimal Total { get; set; }
        public List<ItemPedidoResponse> Itens { get; set; } = new();
    }
}
