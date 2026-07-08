using DotNet.EFCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DotNet.EFCore.Database
{
    public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("user", table =>
            {
                table.HasComment("用户表");
            });

            builder.Property(user => user.Id)
                .HasComment("主键");

            builder.Property(user => user.Name)
                .HasMaxLength(128)
                .IsRequired()
                .HasComment("名称");

            builder.Property(user => user.Age)
                .HasMaxLength(3)
                .IsRequired()
                .HasComment("年龄");

            builder.Property(user => user.Phone)
                .HasMaxLength(128)
                .IsRequired()
                .HasComment("电话");

            builder.Property(user => user.Email)
                .HasMaxLength(128)
                .HasComment("邮箱");

            builder.Property(user => user.CreationTime)
                .HasComment("创建时间");

            builder.Property(user => user.CreatorId)
                .HasComment("创建人");

            builder.Property(user => user.LastModificationTime)
                .HasComment("更新时间");

            builder.Property(user => user.LastModifierId)
                .HasComment("更新人");

            builder.Property(user => user.DeletionTime)
                .HasComment("删除时间");

            builder.Property(user => user.DeleterId)
                .HasComment("删除人");

            builder.Property(user => user.IsDeleted)
                .HasComment("是否删除，0：未删除，1：已删除");
        }
    }
}
