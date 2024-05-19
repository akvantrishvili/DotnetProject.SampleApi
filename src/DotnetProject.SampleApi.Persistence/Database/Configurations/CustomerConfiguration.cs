

using DotnetProject.SampleApi.Domain.Customers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DotnetProject.SampleApi.Persistence.Database.Configurations
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.OwnsOne(x => x.ActualAddress);
            builder.OwnsOne(x => x.LegalAddress);
            builder.HasMany(x => x.IdentityDocuments)
                .WithOne(x => x.Customer)
                .HasForeignKey(x => x.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
