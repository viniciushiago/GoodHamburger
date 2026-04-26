using GoodHamburger.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodHamburger.Domain.Entities
{
    public class ItemPedido
    {
        public long Id { get; private set; }
        public long PedidoId { get; private set; }
        public TipoItem Tipo { get; private set; }
        public string Nome { get; private set; }
        public decimal Preco { get; private set; }

        private ItemPedido() { }

        public ItemPedido(TipoItem tipo, string nome, decimal preco)
        {
            Tipo = tipo;
            Nome = nome;
            Preco = preco;
        }
    }
}
