using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using VPlusSalesManager.BusinessObject.Production;
using VPlusSalesManager.BusinessObject.Setting;
using VPlusSalesManager.BusinessObject.Transaction;

namespace VPlusSalesManager.Business.DataManager
{
    internal partial class VPlusSalesManagerModel : DbContext
    {
        public VPlusSalesManagerModel()
            : base("name=VPlusSalesEntities")
        {
            Configuration.ProxyCreationEnabled = false;
            Configuration.AutoDetectChangesEnabled = false;
            ChangeTracker.DetectChanges();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("VPlusSales");
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        public DbSet<CardType> CardTypes { get; set; }
        public DbSet<CardCommission> CardCommissions { get; set; }
        public DbSet<Beneficiary> Beneficiaries { get; set; }
        public DbSet<BeneficiaryAccount> BeneficiaryAccounts { get; set; }
        public DbSet<BeneficiaryAccountTransaction> BeneficiaryAccountTransactions { get; set; }
        public DbSet<BeneficiaryPayment> BeneficiaryPayments { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<CardItem> CardItems { get; set; }
        public DbSet<CardRequisition> CardRequisitions { get; set; }
        public DbSet<CardRequisitionItem> CardRequisitionItems { get; set; }
        public DbSet<CardDelivery> CardDeliveries { get; set; }
        public DbSet<CardIssuance> CardIssuance { get; set; }

    }
}
