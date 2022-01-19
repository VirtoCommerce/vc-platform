using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ArrayVsListBench
{
    public class InMemCtx : DbContext
    {

        public InMemCtx(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Something>(o =>
           {
               o.Property(o => o.Id);
           }
           );
        }
    }

    public class Repo
    {

        public InMemCtx Ctx { get; set; }

        public Repo()
        {
            Ctx = new InMemCtx(new DbContextOptionsBuilder<InMemCtx>()
                .UseInMemoryDatabase("UseInMemoryDatabase").Options);
        }

        public IQueryable<Something> Somethings => Ctx.Set<Something>();
    }
}
