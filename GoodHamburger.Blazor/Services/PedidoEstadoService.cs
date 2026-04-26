using GoodHamburger.Blazor.Models.Pedidos;

namespace GoodHamburger.Blazor.Services
{
    public class PedidoEstadoService
    {
        public long? PedidoEmEdicaoId { get; private set; }
        public List<ItemPedidoResponse> ItensEmEdicao { get; private set; } = new();
        public bool EmEdicao => PedidoEmEdicaoId.HasValue;

        public void IniciarEdicao(PedidoResponse pedido)
        {
            PedidoEmEdicaoId = pedido.Id;
            ItensEmEdicao = pedido.Itens.ToList();
        }

        public void LimparEdicao()
        {
            PedidoEmEdicaoId = null;
            ItensEmEdicao = new();
        }
    }
}
