using GoodHamburger.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodHamburger.Domain.Repositories
{
    public interface IPedidoRepository
    {
        Task AddAsync(Pedido pedido, CancellationToken cancellationToken);
        Task<Pedido?> GetByIdAsync(long id, CancellationToken cancellationToken);
        Task<Pedido?> GetByIdTrackedAsync(long id, CancellationToken cancellationToken);
        Task<List<Pedido>> GetAllAsync(CancellationToken cancellationToken);
        Task UpdateAsync(Pedido pedido, CancellationToken cancellationToken);
        Task DeleteAsync(Pedido pedido, CancellationToken cancellationToken);
    }
}
