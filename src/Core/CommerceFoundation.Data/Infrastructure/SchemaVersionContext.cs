using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.ComponentModel.DataAnnotations;
using System.Data.Services.Common;
using System.Data.Common;

namespace VirtoCommerce.Foundation.Data
{
    [DataServiceKey("ModelId")]
    public class SchemaVersionRow
    {
        public DateTime CreatedOn { get; set; }
        [Required, StringLength(20)]
        public string VersionId { get; set; }
        [Required, StringLength(20), Key]
        public string ModelId { get; set; }

        public SchemaVersionRow()
        {
            CreatedOn = DateTime.UtcNow;
        }
    }

    public class SchemaVersionContext : DbContext
    {
        private const string TableName = "__SchemaVersion";

        public virtual IDbSet<SchemaVersionRow> Version { get; set; }

        public SchemaVersionContext(string nameOrConnectionString)
			: base(nameOrConnectionString)
		{
            Database.SetInitializer<SchemaVersionContext>(null);
		}

        public SchemaVersionContext(DbConnection existingConnection) : base(existingConnection, false)
        {
            Database.SetInitializer<SchemaVersionContext>(null);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Entity<SchemaVersionRow>().ToTable(TableName);            
            base.OnModelCreating(modelBuilder);
        }
    }
}
