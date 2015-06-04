using System;

namespace VirtoCommerce.Foundation.Orders.Model
{
    /// <summary>
    /// Various transaction types that payment gateways can support. Not all payment gateways support all transaction types.
    /// </summary>
    [Flags]
    public enum TransactionType
    {
        /// <summary>
        /// Initiates an authorization-only request transaction.
        /// The transaction must be completed later with the Capture method (you may use the Sale method if you wish to authorize and capture in one step). 
        /// </summary>
        Authorization = 0x1,
        /// <summary>
        /// Captures a previously authorized transaction. The AuthorizationCode from Payment class indicates to the Gateway which transaction is to be captured, 
        /// and should contain the AuthorizationCode from the original transaction. The CaptureAmount parameter is the value to be captured from the customer's credit card, and can be different from the authorized amount. 
        /// </summary>
        Capture = 0x2,
        /// <summary>
        /// Initiates an Sale transaction (authorization and capture).
        /// </summary>
        Sale = 0x4,
        /// <summary>
        /// Credits a previously captured transaction. This method credits a transaction that has already been captured, or settled. 
        /// If the transaction is still outstanding use the Void method instead. The AuthorizationCode parameter indicates to the Gateway 
        /// which transaction is to be voided, and should contain the AuthorizationCode from the original transaction. The CreditAmount 
        /// parameter is the value to be credited back to the customer, and can be all or part of the original transaction Amount.
        /// </summary>
        Credit = 0x8,
        /// <summary>
        /// Voids a previously authorized transaction. This method voids a transaction that has been previously authorized, 
        /// but which has not yet gone to settlement, or been "captured". The AuthorizationCode parameter indicates to the Gateway 
        /// which transaction is to be voided, and should contain the AuthorizationCode from the original transaction. 
        /// </summary>
        Void = 0x10,
    }
}
