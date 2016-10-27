using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace MyMinions.Model
{
    public class Context : DbContext, IDisposable
    {
        public Context()
            : base("name=MyMinions.Model.Db")
        {
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<JobDescriptor> JobDescriptors { get; set; }
        public DbSet<Job> Jobs { get; set; }

        static Context()
        {
            System.Data.Entity.Database.SetInitializer(new DbInitializer());
        }

        class DbInitializer : DropCreateDatabaseAlways<Context>
        {
            protected override void Seed(Context context)
            {
                var descriptors = new List<JobDescriptor>
                {
                    new JobDescriptor { Descriptor = "Trim a bush" },
                    new JobDescriptor { Descriptor = "Mow a lawn" },
                    new JobDescriptor { Descriptor = "Wax a helmet" },
                    new JobDescriptor { Descriptor = "Fill a crack" },
                    new JobDescriptor { Descriptor = "Fly a helicopter" },
                    new JobDescriptor { Descriptor = "Dig a tunnel to the moon" },
                    new JobDescriptor { Descriptor = "Carry a piano" }
                };
                descriptors.ForEach(d => context.JobDescriptors.Add(d));
                context.SaveChanges();

                var customers = new List<Customer>
                {
                    new Customer { Id = "Test0001", FirstName = "John", LastName = "Smith", Age = 43, Reputation = "Unorthodox" },
                    new Customer { Id = "Test0002", FirstName = "Templeton", LastName = "Peck", Age = 32, Reputation = "Smarmy" },
                    new Customer { Id = "Test0003", FirstName = "Howling Mad", LastName = "Murdock", Age = 37, Reputation = "Insane" },
                    new Customer { Id = "Test0004", FirstName = "Bosco", LastName = "Baracus", Age = 33, Reputation = "Angry" }
                };
                customers.ForEach(c => context.Customers.Add(c));
                context.SaveChanges();

                var jobs = new List<Job>
                {
                    new Job { Customer = customers[0], Descriptor = descriptors[5], StartDate = DateTime.Now },
                    new Job { Customer = customers[1], Descriptor = descriptors[0], StartDate = DateTime.Now },
                    new Job { Customer = customers[1], Descriptor = descriptors[3], StartDate = DateTime.Now },
                    new Job { Customer = customers[2], Descriptor = descriptors[4], StartDate = DateTime.Now },
                    new Job { Customer = customers[3], Descriptor = descriptors[6], StartDate = DateTime.Now }
                };
                jobs.ForEach(j => context.Jobs.Add(j));
                context.SaveChanges();

                base.Seed(context);
            }
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<JobDescriptor>()
                        .Property(d => d.Descriptor)
                        .IsRequired();

            modelBuilder.Entity<Customer>()
                        .HasKey(c => c.Id);
            modelBuilder.Entity<Customer>()
                        .Property(c => c.FirstName)
                        .IsRequired();
            modelBuilder.Entity<Customer>()
                        .Property(c => c.LastName)
                        .IsRequired();
            modelBuilder.Entity<Customer>()
                        .Property(c => c.Reputation)
                        .IsRequired();
            modelBuilder.Entity<Customer>()
                        .HasMany(c => c.Jobs)
                        .WithRequired();

            modelBuilder.Entity<Job>()
                        .HasRequired(j => j.Customer)
                        .WithMany(c => c.Jobs);
            modelBuilder.Entity<Job>()
                        .HasRequired(j => j.Descriptor)
                        .WithMany();

            base.OnModelCreating(modelBuilder);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
