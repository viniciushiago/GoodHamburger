using GoodHamburger.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GoodHamburger.Infrastructure.Persistence.Configurations
{
    public class PedidoConfiguration : IEntityTypeConfiguration<Pedido>
    {
        public void Configure(EntityTypeBuilder<Pedido> pedido)
        {
            pedido
                .HasKey(x => x.Id);

            pedido
                .Property(x => x.Subtotal)
                .HasColumnType("decimal(10,2)");

            pedido
                 .Property(p => p.Desconto)
                 .HasColumnType("decimal(10,2)");

            pedido
                .Property(p => p.Total)
                .HasColumnType("decimal(10,2)");

            pedido.HasMany(p => p.Itens)
                .WithOne()
                .HasForeignKey(i => i.PedidoId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
    