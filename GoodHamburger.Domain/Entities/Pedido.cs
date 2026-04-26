using GoodHamburger.Domain.Enums;
using GoodHamburger.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodHamburger.Domain.Entities
{
    public class Pedido
    {
        public long Id { get; private set; }
        public decimal Subtotal { get; private set; }
        public decimal Desconto { get; private set; }
        public decimal Total { get; private set; }
        public DateTime DataCriacao { get; private set; }

        private readonly List<ItemPedido> _itens = new();
        public ICollection<ItemPedido> Itens => _itens;

        public Pedido()
        {
            DataCriacao = DateTime.UtcNow;
        }

        public void AdicionarItem(ItemPedido item)
        {
            ValidarItem(item);
            _itens.Add(item);
            Recalcular();
        }

        private void ValidarItem(ItemPedido item)
        {
            if (_itens.Any(i => i.Tipo == item.Tipo))
                throw new NegocioException($"Já existe um item do tipo {item.Tipo} no pedido");
        }

        public void ValidarPedido()
        {
            if (!_itens.Any(i => i.Tipo == TipoItem.Sanduiche))
                throw new NegocioException("Pedido deve conter um sanduíche");
        }

        private void Recalcular()
        {
            Subtotal = _itens.Sum(i => i.Preco);
            Desconto = CalcularDesconto();
            Total = Subtotal - Desconto;
        }

        private decimal CalcularDesconto()
        {
            var temSanduiche = _itens.Any(i => i.Tipo == TipoItem.Sanduiche);
            var temBatata = _itens.Any(i => i.Tipo == TipoItem.Batata);
            var temRefrigerante = _itens.Any(i => i.Tipo == TipoItem.Refrigerante);

            if (temSanduiche && temBatata && temRefrigerante)
                return Subtotal * 0.20m;

            if (temSanduiche && temRefrigerante)
                return Subtotal * 0.15m;

            if (temSanduiche && temBatata)
                return Subtotal * 0.10m;

            return 0;
        }

        public void LimparItens()
        {
            _itens.Clear();

            Subtotal = 0;
            Desconto = 0;
            Total = 0;
        }
    }
}
