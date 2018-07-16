using System;
using System.Data.Entity;

namespace VPlusSalesManager.Business.Infrastructure.Contract
{
    internal interface IVPlusSalesManagerContext : IDisposable 
    {
        DbContext VPlusSalesManagerDbContext { get; }
    }
}
