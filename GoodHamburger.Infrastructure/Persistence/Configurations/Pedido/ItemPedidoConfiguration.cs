using GoodHamburger.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GoodHamburger.Infrastructure.Persistence.Configurations
{
    public class ItemPedidoConfiguration : IEntityTypeConfiguration<ItemPedido>
    {
        public void Configure(EntityTypeBuilder<ItemPedido> itemPedido)
        {
            itemPedido.ToTable("ItensPedido");

            itemPedido.HasKey("Id");

            itemPedido
                .Property(x => x.Nome)
                .HasMaxLength(100)
                .IsRequired();

            itemPedido
                .Property(x => x.Preco)
                .HasColumnType("decimal(10,2)");

            itemPedido
                .Property(x => x.Tipo)
                .IsRequired();


        }
    }
}
