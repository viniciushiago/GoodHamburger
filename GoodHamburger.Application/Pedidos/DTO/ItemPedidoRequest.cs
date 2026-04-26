using GoodHamburger.Domain.Enums;

namespace GoodHamburger.Application.Pedidos.DTO
{
    public class ItemPedidoRequest
    {
        public TipoItem Tipo { get; set; }
        public string Nome { get; set; }
        public decimal Preco { get; set; }
    }
}
