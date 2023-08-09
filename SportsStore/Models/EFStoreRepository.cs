using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore.Models {
    public class EFStoreRepository : IStoreRepository {
        private StoreDbContext ctx;
        public EFStoreRepository(StoreDbContext ctx) {
            this.ctx = ctx;
        }
        public IQueryable<Product> Products => ctx.Products;
    }
}
