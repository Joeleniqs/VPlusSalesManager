using VPlusSalesManager.Business.Infrastructure.Contract;
using System;
using System.Data.Entity;


namespace VPlusSalesManager.Business.Infrastructure
{
    internal class VPlusSalesManagerUoWork : IVPlusSalesManagerUoWork, IDisposable
    {
        private readonly VPlusSalesManagerContext _dbContext;

        public VPlusSalesManagerUoWork(VPlusSalesManagerContext context)
		{
			_dbContext = context;
		}

		public VPlusSalesManagerUoWork()
		{
            _dbContext = new VPlusSalesManagerContext();
		}

		public void SaveChanges()
		{
            _dbContext.VPlusSalesManagerDbContext.SaveChanges();
		}
       
        public VPlusSalesManagerContext Context => _dbContext;

        #region Implementation of IDisposable
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool disposing)
		{
			if (!disposing) return;
			if (_disposed) return;
			 _dbContext.Dispose();
			 _disposed = true;
		}

		private bool _disposed;

        ~VPlusSalesManagerUoWork()
		{
			 Dispose(false);
		}

		#endregion


	//Class File Generated from codeZAPP 3.0.135 | www.xplugng.com | All Rights Reserved.
        public DbContextTransaction BeginTransaction()
	    {
            return _dbContext.VPlusSalesManagerDbContext.Database.BeginTransaction();
	    }
    }
}
