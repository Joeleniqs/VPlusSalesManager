using VPlusSalesManager.Business.DataManager;
using VPlusSalesManager.Business.Infrastructure.Contract;
using System;
using System.Data.Entity;

namespace VPlusSalesManager.Business.Infrastructure
{
    internal  class VPlusSalesManagerContext : IVPlusSalesManagerContext
    {
        public VPlusSalesManagerContext(DbContext context)
		{
		    VPlusSalesManagerDbContext = context ?? throw new ArgumentNullException(nameof(context));
            VPlusSalesManagerDbContext.Configuration.LazyLoadingEnabled = false;
		}

        public VPlusSalesManagerContext()
		{
            VPlusSalesManagerDbContext = new VPlusSalesManagerModel();
            VPlusSalesManagerDbContext.Configuration.LazyLoadingEnabled = false;
		}

		public void Dispose()
		{
            VPlusSalesManagerDbContext.Dispose();
		}
        
        public DbContext VPlusSalesManagerDbContext { get; private set; }
    }
}
