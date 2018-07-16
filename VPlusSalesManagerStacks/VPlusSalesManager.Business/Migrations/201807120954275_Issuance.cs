namespace VPlusSalesManager.Business.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Issuance : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "VPlusSales.Beneficiary",
                c => new
                    {
                        BeneficiaryId = c.Int(nullable: false, identity: true),
                        BeneficiaryAccountId = c.Int(nullable: false),
                        Fullname = c.String(nullable: false, maxLength: 80, storeType: "varchar"),
                        MobileNumber = c.String(nullable: false, maxLength: 11, storeType: "varchar"),
                        Email = c.String(nullable: false, maxLength: 50, storeType: "varchar"),
                        Address = c.String(nullable: false, maxLength: 150, storeType: "varchar"),
                        TimeStampRegistered = c.String(nullable: false, maxLength: 35, storeType: "varchar"),
                        ApprovedBy = c.Int(nullable: false),
                        ApproverComment = c.String(nullable: false, maxLength: 150, storeType: "varchar"),
                        TimeStampApproved = c.String(nullable: false, maxLength: 35, storeType: "varchar"),
                        BeneficiaryType = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.BeneficiaryId)
                .ForeignKey("VPlusSales.BeneficiaryAccount", t => t.BeneficiaryAccountId, cascadeDelete: true)
                .Index(t => t.BeneficiaryAccountId);
            
            CreateTable(
                "VPlusSales.BeneficiaryAccount",
                c => new
                    {
                        BeneficiaryAccountId = c.Int(nullable: false, identity: true),
                        AvailableBalance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreditLimit = c.Decimal(nullable: false, precision: 18, scale: 2),
                        LastTransactionAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        LastTransactionType = c.Int(nullable: false),
                        LastTransactionId = c.Long(nullable: false),
                        LastTransactionTimeStamp = c.String(nullable: false, maxLength: 35, storeType: "varchar"),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.BeneficiaryAccountId);
            
            CreateTable(
                "VPlusSales.BeneficiaryAccountTransaction",
                c => new
                    {
                        BeneficiaryAccountTransactionId = c.Long(nullable: false, identity: true),
                        BeneficiaryAccountId = c.Int(nullable: false),
                        BeneficiaryId = c.Int(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PreviousBalance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        NewBalance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RegisteredBy = c.Int(nullable: false),
                        TimeStampRegistered = c.String(nullable: false, maxLength: 35, storeType: "varchar"),
                        TransactionType = c.Int(nullable: false),
                        TransactionSource = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.BeneficiaryAccountTransactionId)
                .ForeignKey("VPlusSales.BeneficiaryAccount", t => t.BeneficiaryAccountId, cascadeDelete: true)
                .Index(t => t.BeneficiaryAccountId);
            
            CreateTable(
                "VPlusSales.CardRequisition",
                c => new
                    {
                        CardRequisitionId = c.Long(nullable: false, identity: true),
                        BeneficiaryId = c.Int(nullable: false),
                        RequisitionTitle = c.String(nullable: false, maxLength: 150, storeType: "varchar"),
                        TotalQuantityRequested = c.Int(nullable: false),
                        RequestedBy = c.Int(nullable: false),
                        TimeStampRequested = c.String(nullable: false, maxLength: 35, storeType: "varchar"),
                        ApprovedBy = c.Int(nullable: false),
                        ApproverComment = c.String(nullable: false, maxLength: 150, storeType: "varchar"),
                        TimeStampApproved = c.String(nullable: false, maxLength: 35, storeType: "varchar"),
                        QuantityApproved = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CardRequisitionId)
                .ForeignKey("VPlusSales.Beneficiary", t => t.BeneficiaryId, cascadeDelete: true)
                .Index(t => t.BeneficiaryId);
            
            CreateTable(
                "VPlusSales.CardRequisitionItem",
                c => new
                    {
                        CardRequisitionItemId = c.Long(nullable: false, identity: true),
                        CardRequisitionId = c.Long(nullable: false),
                        CardCommissionId = c.Int(nullable: false),
                        BeneficiaryId = c.Int(nullable: false),
                        CardTypeId = c.Int(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Quantity = c.Int(nullable: false),
                        CommissionRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CommissionQuantity = c.Int(nullable: false),
                        CommissionAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ExcessBalance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        UnitPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RequestedBy = c.Int(nullable: false),
                        TimeStampRequested = c.String(nullable: false, maxLength: 35, storeType: "varchar"),
                        ApprovedBy = c.Int(nullable: false),
                        QuantityApproved = c.Int(nullable: false),
                        TimeStampApproved = c.String(nullable: false, maxLength: 35, storeType: "varchar"),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CardRequisitionItemId)
                .ForeignKey("VPlusSales.CardRequisition", t => t.CardRequisitionId, cascadeDelete: true)
                .Index(t => t.CardRequisitionId);
            
            CreateTable(
                "VPlusSales.BeneficiaryPayment",
                c => new
                    {
                        BeneficiaryPaymentId = c.Long(nullable: false, identity: true),
                        BeneficiaryAccountTransactionId = c.Long(nullable: false),
                        BeneficiaryAccountId = c.Int(nullable: false),
                        BeneficiaryId = c.Int(nullable: false),
                        AmountPaid = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PaymentSource = c.Int(nullable: false),
                        PaymentSourceName = c.String(nullable: false, maxLength: 80, storeType: "varchar"),
                        PaymentReference = c.String(nullable: false, maxLength: 18, storeType: "varchar"),
                        PaymentDate = c.String(nullable: false, maxLength: 35, storeType: "varchar"),
                        RegisteredBy = c.Int(nullable: false),
                        TimeStampRegistered = c.String(nullable: false, maxLength: 35, storeType: "varchar"),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.BeneficiaryPaymentId)
                .ForeignKey("VPlusSales.BeneficiaryAccountTransaction", t => t.BeneficiaryAccountTransactionId, cascadeDelete: true)
                .Index(t => t.BeneficiaryAccountTransactionId);
            
            CreateTable(
                "VPlusSales.CardCommission",
                c => new
                    {
                        CardCommissionId = c.Int(nullable: false, identity: true),
                        CardTypeId = c.Int(nullable: false),
                        LowerAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        UpperAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CommissionRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CardCommissionId)
                .ForeignKey("VPlusSales.CardType", t => t.CardTypeId, cascadeDelete: true)
                .Index(t => t.CardTypeId);
            
            CreateTable(
                "VPlusSales.CardType",
                c => new
                    {
                        CardTypeId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 150, storeType: "varchar"),
                        FaceValue = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CardTypeId);
            
            CreateTable(
                "VPlusSales.Card",
                c => new
                    {
                        CardId = c.Int(nullable: false, identity: true),
                        CardTitle = c.String(nullable: false, maxLength: 200, storeType: "varchar"),
                        CardTypeId = c.Int(nullable: false),
                        TotalQuantity = c.Int(nullable: false),
                        BatchKey = c.String(nullable: false, maxLength: 2, storeType: "varchar"),
                        StartBatchId = c.String(nullable: false, maxLength: 5, storeType: "varchar"),
                        StopBatchId = c.String(nullable: false, maxLength: 5, storeType: "varchar"),
                        NumberOfBatches = c.Int(nullable: false),
                        QuantityPerBatch = c.Int(nullable: false),
                        TimeStampRegistered = c.String(nullable: false, maxLength: 35, storeType: "varchar"),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CardId)
                .ForeignKey("VPlusSales.CardType", t => t.CardTypeId, cascadeDelete: true)
                .Index(t => t.CardTypeId);
            
            CreateTable(
                "VPlusSales.CardItem",
                c => new
                    {
                        CardItemId = c.Long(nullable: false, identity: true),
                        CardId = c.Int(nullable: false),
                        CardTypeId = c.Int(nullable: false),
                        BatchId = c.String(nullable: false, maxLength: 5, storeType: "varchar"),
                        BatchStartNumber = c.String(nullable: false, maxLength: 10, storeType: "varchar"),
                        BatchStopNumber = c.String(nullable: false, maxLength: 10, storeType: "varchar"),
                        DefectiveBatchNumbers = c.String(maxLength: 500, storeType: "varchar"),
                        BatchQuantity = c.Int(nullable: false),
                        MissingQuantity = c.Int(nullable: false),
                        DefectiveQuantity = c.Int(nullable: false),
                        DeliveredQuantity = c.Int(nullable: false),
                        AvailableQuantity = c.Int(nullable: false),
                        IssuedQuantity = c.Int(nullable: false),
                        TimeStampRegistered = c.String(nullable: false, maxLength: 35, storeType: "varchar"),
                        TimeStampDelivered = c.String(nullable: false, maxLength: 35, storeType: "varchar"),
                        TimeStampLastIssued = c.String(nullable: false, maxLength: 35, storeType: "varchar"),
                        RegisteredBy = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CardItemId)
                .ForeignKey("VPlusSales.Card", t => t.CardId, cascadeDelete: true)
                .Index(t => t.CardId);
            
            CreateTable(
                "VPlusSales.CardDelivery",
                c => new
                    {
                        CardDeliveryId = c.Long(nullable: false, identity: true),
                        CardItemId = c.Long(nullable: false),
                        CardId = c.Int(nullable: false),
                        CardTypeId = c.Int(nullable: false),
                        BatchId = c.String(nullable: false, maxLength: 5, storeType: "varchar"),
                        StartBatchNumber = c.String(nullable: false, maxLength: 12, storeType: "varchar"),
                        StopBatchNumber = c.String(nullable: false, maxLength: 12, storeType: "varchar"),
                        QuantityDelivered = c.Int(nullable: false),
                        MissingQuantity = c.Int(nullable: false),
                        DefectiveQuantity = c.Int(nullable: false),
                        DeliveryDate = c.String(maxLength: 35, storeType: "varchar"),
                        TimeStampRegistered = c.String(nullable: false, maxLength: 35, storeType: "varchar"),
                        ApprovedBy = c.Int(nullable: false),
                        ApproverComment = c.String(nullable: false, maxLength: 150, storeType: "varchar"),
                        TimeStampApproved = c.String(nullable: false, maxLength: 35, storeType: "varchar"),
                        RecievedBy = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CardDeliveryId)
                .ForeignKey("VPlusSales.CardItem", t => t.CardItemId, cascadeDelete: true)
                .Index(t => t.CardItemId);
            
            CreateTable(
                "VPlusSales.CardIssuance",
                c => new
                    {
                        CardIssuanceId = c.Long(nullable: false, identity: true),
                        CardRequisitionId = c.Long(nullable: false),
                        CardRequisitionItemId = c.Long(nullable: false),
                        CardTypeId = c.Int(nullable: false),
                        BeneficiaryId = c.Int(nullable: false),
                        CardItemId = c.Long(nullable: false),
                        BatchId = c.String(nullable: false, maxLength: 300, storeType: "varchar"),
                        StartBatchNumber = c.String(nullable: false, maxLength: 300, storeType: "varchar"),
                        StopBatchNumber = c.String(nullable: false, maxLength: 300, storeType: "varchar"),
                        QuantityIssued = c.Int(nullable: false),
                        IssuedBy = c.Int(nullable: false),
                        TimeStampIssued = c.String(nullable: false, maxLength: 35, storeType: "varchar"),
                    })
                .PrimaryKey(t => t.CardIssuanceId)
                .ForeignKey("VPlusSales.CardItem", t => t.CardItemId, cascadeDelete: true)
                .ForeignKey("VPlusSales.CardRequisitionItem", t => t.CardRequisitionItemId, cascadeDelete: true)
                .Index(t => t.CardRequisitionItemId)
                .Index(t => t.CardItemId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("VPlusSales.CardIssuance", "CardRequisitionItemId", "VPlusSales.CardRequisitionItem");
            DropForeignKey("VPlusSales.CardIssuance", "CardItemId", "VPlusSales.CardItem");
            DropForeignKey("VPlusSales.CardDelivery", "CardItemId", "VPlusSales.CardItem");
            DropForeignKey("VPlusSales.Card", "CardTypeId", "VPlusSales.CardType");
            DropForeignKey("VPlusSales.CardItem", "CardId", "VPlusSales.Card");
            DropForeignKey("VPlusSales.CardCommission", "CardTypeId", "VPlusSales.CardType");
            DropForeignKey("VPlusSales.BeneficiaryPayment", "BeneficiaryAccountTransactionId", "VPlusSales.BeneficiaryAccountTransaction");
            DropForeignKey("VPlusSales.CardRequisitionItem", "CardRequisitionId", "VPlusSales.CardRequisition");
            DropForeignKey("VPlusSales.CardRequisition", "BeneficiaryId", "VPlusSales.Beneficiary");
            DropForeignKey("VPlusSales.Beneficiary", "BeneficiaryAccountId", "VPlusSales.BeneficiaryAccount");
            DropForeignKey("VPlusSales.BeneficiaryAccountTransaction", "BeneficiaryAccountId", "VPlusSales.BeneficiaryAccount");
            DropIndex("VPlusSales.CardIssuance", new[] { "CardItemId" });
            DropIndex("VPlusSales.CardIssuance", new[] { "CardRequisitionItemId" });
            DropIndex("VPlusSales.CardDelivery", new[] { "CardItemId" });
            DropIndex("VPlusSales.CardItem", new[] { "CardId" });
            DropIndex("VPlusSales.Card", new[] { "CardTypeId" });
            DropIndex("VPlusSales.CardCommission", new[] { "CardTypeId" });
            DropIndex("VPlusSales.BeneficiaryPayment", new[] { "BeneficiaryAccountTransactionId" });
            DropIndex("VPlusSales.CardRequisitionItem", new[] { "CardRequisitionId" });
            DropIndex("VPlusSales.CardRequisition", new[] { "BeneficiaryId" });
            DropIndex("VPlusSales.BeneficiaryAccountTransaction", new[] { "BeneficiaryAccountId" });
            DropIndex("VPlusSales.Beneficiary", new[] { "BeneficiaryAccountId" });
            DropTable("VPlusSales.CardIssuance");
            DropTable("VPlusSales.CardDelivery");
            DropTable("VPlusSales.CardItem");
            DropTable("VPlusSales.Card");
            DropTable("VPlusSales.CardType");
            DropTable("VPlusSales.CardCommission");
            DropTable("VPlusSales.BeneficiaryPayment");
            DropTable("VPlusSales.CardRequisitionItem");
            DropTable("VPlusSales.CardRequisition");
            DropTable("VPlusSales.BeneficiaryAccountTransaction");
            DropTable("VPlusSales.BeneficiaryAccount");
            DropTable("VPlusSales.Beneficiary");
        }
    }
}
