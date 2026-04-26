using GoodHamburger.Domain.Entities;
using GoodHamburger.Domain.Repositories;
using GoodHamburger.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodHamburger.Infrastructure.Repositories
{
    public class PedidoRepository : IPedidoRepository
    {
        private readonly AppDbContext _appDbContext;

        public PedidoRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task AddAsync(Pedido pedido, CancellationToken cancellationToken)
        {
            await _appDbContext.AddAsync(pedido, cancellationToken);
        }

        public async Task<Pedido?> GetByIdAsync(long id, CancellationToken cancellationToken)
        {
            return await _appDbContext.Pedidos
                .AsNoTracking()
                .Include(x => x.Itens)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }
         
        public async Task<List<Pedido>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _appDbContext.Pedidos
                .AsNoTracking()
                .Include(x => x.Itens)
                .ToListAsync(cancellationToken);
        }

        public async Task UpdateAsync(Pedido pedido, CancellationToken cancellationToken)
        {
            _appDbContext.Pedidos.Update(pedido);
        }

        public async Task DeleteAsync(Pedido pedido, CancellationToken cancellationToken)
        {
            _appDbContext.Pedidos.Remove(pedido);
        }

        public async Task<Pedido?> GetByIdTrackedAsync(long id, CancellationToken cancellationToken)
        {
            return await _appDbContext.Pedidos
                .Include(x => x.Itens)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }
    }
}
