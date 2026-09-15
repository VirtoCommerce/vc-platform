using System;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Security;

namespace VirtoCommerce.Platform.Security.Model;

/// <summary>
/// Append-only audit row. Derives from <see cref="Entity"/> rather than AuditableEntity:
/// rows are never modified, so ModifiedBy/ModifiedDate would be dead columns on the
/// highest-write table in the system.
/// </summary>
public class UserSignInLogEntity : Entity
{
    public DateTime CreatedDate { get; set; }
    public string UserName { get; set; }
    public string UserId { get; set; }
    public bool Succeeded { get; set; }
    public string FailureReason { get; set; }
    public string SignInType { get; set; }
    public string Provider { get; set; }
    public string OperatorUserId { get; set; }
    public string OperatorUserName { get; set; }
    public string IpAddress { get; set; }
    public string UserAgent { get; set; }
    public string ClientId { get; set; }
    public string SessionId { get; set; }
    public string StoreId { get; set; }
    public string StoreName { get; set; }
    public string MemberId { get; set; }
    public string OrganizationId { get; set; }
    public string OrganizationName { get; set; }

    public virtual UserSignInLogEntity FromModel(UserSignInLog record, PrimaryKeyResolvingMap pkMap)
    {
        ArgumentNullException.ThrowIfNull(record);

        pkMap.AddPair(record, this);

        Id = record.Id;
        CreatedDate = record.CreatedDate;
        UserName = record.UserName;
        UserId = record.UserId;
        Succeeded = record.Succeeded;
        FailureReason = record.FailureReason;
        SignInType = record.SignInType;
        Provider = record.Provider;
        OperatorUserId = record.OperatorUserId;
        OperatorUserName = record.OperatorUserName;
        IpAddress = record.IpAddress;
        UserAgent = record.UserAgent;
        ClientId = record.ClientId;
        SessionId = record.SessionId;
        StoreId = record.StoreId;
        StoreName = record.StoreName;
        MemberId = record.MemberId;
        OrganizationId = record.OrganizationId;
        OrganizationName = record.OrganizationName;

        return this;
    }

    public virtual UserSignInLog ToModel(UserSignInLog record)
    {
        ArgumentNullException.ThrowIfNull(record);

        record.Id = Id;
        record.CreatedDate = CreatedDate;
        record.UserName = UserName;
        record.UserId = UserId;
        record.Succeeded = Succeeded;
        record.FailureReason = FailureReason;
        record.SignInType = SignInType;
        record.Provider = Provider;
        record.OperatorUserId = OperatorUserId;
        record.OperatorUserName = OperatorUserName;
        record.IpAddress = IpAddress;
        record.UserAgent = UserAgent;
        record.ClientId = ClientId;
        record.SessionId = SessionId;
        record.StoreId = StoreId;
        record.StoreName = StoreName;
        record.MemberId = MemberId;
        record.OrganizationId = OrganizationId;
        record.OrganizationName = OrganizationName;

        return record;
    }
}
