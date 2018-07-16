namespace VPlusSalesManager.Business.Infrastructure.Contract
{
    internal interface IVPlusSalesManagerUoWork
    {
        void SaveChanges();
        VPlusSalesManagerContext Context { get; }
    }
}
