using System;
using System.ComponentModel.DataAnnotations;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Security;

namespace VirtoCommerce.Platform.Data.Model
{
    public class RefreshTokenEntity : Entity
    {
        [Required]
        [MaxLength(128)]
        public string Subject { get; set; }
        public DateTime IssuedUtc { get; set; }
        public DateTime ExpiresUtc { get; set; }
        [Required]
        public string ProtectedTicket { get; set; }

        public virtual RefreshToken ToModel(RefreshToken model)
        {
            model.Id = Id;
            model.Subject = Subject;
            model.IssuedUtc = IssuedUtc;
            model.ExpiresUtc = ExpiresUtc;
            model.ProtectedTicket = ProtectedTicket;

            return model;
        }

        public virtual RefreshTokenEntity FromModel(RefreshToken model)
        {
            Id = model.Id;
            Subject = model.Subject;
            IssuedUtc = model.IssuedUtc;
            ExpiresUtc = model.ExpiresUtc;
            ProtectedTicket = model.ProtectedTicket;

            return this;
        }

        public virtual void Patch(RefreshTokenEntity target)
        {
            target.Subject = Subject;
            target.IssuedUtc = IssuedUtc;
            target.ExpiresUtc = ExpiresUtc;
            target.ProtectedTicket = ProtectedTicket;
        }
    }
}
