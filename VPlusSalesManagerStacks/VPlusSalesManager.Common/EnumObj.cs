using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VPlusSalesManager.Common
{
    public enum Status { Deleted = -100,Inactive = 0,Active}

    public enum ApprovalStatus { Deleted = -100, Pending = 0, Approved }
    
    public enum CardStatus { Deleted = -100,Unknown = 0,Registered, Available,Defective,Issued }

    public enum CardRequisitionStatus { Deleted = -100,Unknown = 0,Registered,Approved,Denied,Issued,Closed}

    public enum PaySource { Unknown = 0,Bank,Cash }

    public enum BeneficiaryType { Unknown = 0, LR_Office, LR_Marketer, LR_Dealer, LR_Customer}

    public enum TransactionType { Unknown = 0, Credit, Debit }

    public enum TransactionSourceType { Unknown = 0, Account_TopUp, Card_Purchase,Chart_Purchase}

}
