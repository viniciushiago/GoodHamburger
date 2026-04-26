using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodHamburger.Application.Pedidos.DTO
{
    public class PedidoResponse
    {
        public long Id { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Desconto { get; set; }
        public decimal Total { get; set; }
        public List<ItemPedidoResponse> Itens { get; set; }
    }

    public class ItemPedidoResponse
    {
        public string Tipo { get; set; }
        public string Nome { get; set; }
        public decimal Preco { get; set; }
    }
}
