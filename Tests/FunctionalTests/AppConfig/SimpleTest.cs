using System;
using System.Collections.Generic;
using System.Linq;

namespace FunctionalTests.AppConfig
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity;
    using Microsoft.WindowsAzure;

    using VirtoCommerce.Foundation.Data.AppConfig;
    using VirtoCommerce.Foundation.Data.Azure.Common;

    using Xunit;

    public class SimpleTest
    {
        //[Fact]
        public void ef_memory_leak()
        {
            // this call causes memory leak for every entity framework request after
            var isAzure = AzureCommonHelper.IsAzureEnvironment();
            //CloudConfigurationManager.GetSetting("SchoolContext");

            Database.SetInitializer(new MySampleInitialize());
            
            new SchoolContext().Database.Initialize(false);
            for (int index = 1; index < 500; index++)
            {
                var db2 = new SchoolContext();
                var all = db2.Students.ToArray();
                db2.Dispose();
            }

            for (int index = 1; index < 500; index++)
            {
                var db2 = new EFAppConfigRepository();
                var all = db2.Localizations.ToArray();
                db2.Dispose();
            }
        }
    }

    public class SchoolContext : BaseRepository
    {
        public IQueryable<Student> Students { get { return GetAsQueryable<Student>(); } }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>().Map(m=>m.ToTable("Students"));
        }
    }

    public class BaseRepository : DbContext
    {
        public IQueryable<T> GetAsQueryable<T>() where T : class
        {
            return Set<T>();
        }
    }

    public class Student
    {
        public int ID { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "First name cannot be longer than 50 characters.")]
        [Column("FirstName")]
        [Display(Name = "First Name")]
        public string FirstMidName { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Enrollment Date")]
        public DateTime EnrollmentDate { get; set; }

        [Display(Name = "Full Name")]
        public string FullName
        {
            get
            {
                return LastName + ", " + FirstMidName;
            }
        }
    }

    public class MySampleInitialize : CreateDatabaseIfNotExists<SchoolContext>
    {
        protected override void Seed(SchoolContext context)
        {
            var bulkstudents = new List<Student>();

            for (int index = 0; index < 1000; index++)
            {
                bulkstudents.Add(new Student
                {
                    FirstMidName = "Carson",
                    LastName = "Alexander",
                    EnrollmentDate = DateTime.Parse("2005-09-01")
                });
            }

            bulkstudents.ForEach(x=>context.Set<Student>().Add(x));
            context.SaveChanges();
            base.Seed(context);
        }
    }
}