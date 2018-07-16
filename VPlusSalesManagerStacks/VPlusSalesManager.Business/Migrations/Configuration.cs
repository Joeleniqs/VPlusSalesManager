namespace VPlusSalesManager.Business.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using VPlusSalesManager.Business.DataManager;

    internal sealed partial class Configuration : DbMigrationsConfiguration<VPlusSalesManagerModel>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(VPlusSalesManager.Business.DataManager.VPlusSalesManagerModel context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
        }
    }
}
