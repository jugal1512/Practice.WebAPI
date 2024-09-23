using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Practice.Domain.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice.EFCore.Configuration
{
    public class EmployeeConfiguration:IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.ToTable("Employees");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.FirstName).IsRequired();
            builder.Property(e => e.LastName).IsRequired();
            builder.Property(e => e.Email).IsRequired();
            builder.Property(e => e.Phone).IsRequired();
            builder.Property(e => e.Gender).IsRequired();
            builder.Property(e => e.Designation).IsRequired();
            builder.Property(e => e.ProfileImage);
            builder.Property(e => e.Address).IsRequired().HasMaxLength(250);

            builder
                .HasMany(s => s.Skills)
                .WithOne(e => e.Employee)
                .HasForeignKey(x => x.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}