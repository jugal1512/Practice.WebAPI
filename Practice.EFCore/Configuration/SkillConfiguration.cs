using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Practice.Domain.Skills;


namespace Practice.EFCore.Configuration
{
    public class SkillConfiguration : IEntityTypeConfiguration<Skill>
    {
        public void Configure(EntityTypeBuilder<Skill> builder)
        {
            builder.ToTable("Skills");
            builder.HasKey(s => s.SkillId);
            builder.Property(s => s.SkillName).IsRequired();
            builder.Property(s => s.EmployeeId);
        }
    }
}