using DAL.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Configurations.Identity
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            // تكوين الجدول
            builder.ToTable("RefreshTokens", "Identity");

            // تكوين المفتاح الأساسي
            builder.HasKey(rt => rt.Id);

            builder.Property(rt => rt.Id)
                .HasMaxLength(450)
                .IsRequired();

            // تكوين الخصائص
            builder.Property(rt => rt.Token)
                .HasMaxLength(500)
                .IsRequired()
                .HasComment("قيمة الـ Refresh Token");

            builder.Property(rt => rt.UserId)
                .HasMaxLength(450)
                .IsRequired()
                .HasComment("معرف المستخدم");

            builder.Property(rt => rt.ExpiryDate)
                .HasColumnType("datetime2")
                .IsRequired()
                .HasComment("تاريخ انتهاء صلاحية الـ Token");

            builder.Property(rt => rt.IsActive)
                .IsRequired()
                .HasDefaultValue(true)
                .HasComment("حالة نشاط الـ Token");

            builder.Property(rt => rt.IsRevoked)
                .IsRequired()
                .HasDefaultValue(false)
                .HasComment("هل تم إلغاء الـ Token");

            builder.Property(rt => rt.RevokedAt)
                .HasColumnType("datetime2")
                .IsRequired(false)
                .HasComment("تاريخ إلغاء الـ Token");

            builder.Property(rt => rt.RevokedBy)
                .HasMaxLength(450)
                .IsRequired(false)
                .HasComment("من قام بإلغاء الـ Token");

            builder.Property(rt => rt.ReplacedByToken)
                .HasMaxLength(500)
                .IsRequired(false)
                .HasComment("الـ Token الذي حل محل هذا الـ Token");

            builder.Property(rt => rt.ReasonRevoked)
                .HasMaxLength(500)
                .IsRequired(false)
                .HasComment("سبب إلغاء الـ Token");

            // تكوين خصائص Auditing
            builder.Property(rt => rt.CreatedAt)
                .HasColumnType("datetime2")
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()")
                .HasComment("تاريخ إنشاء السجل");

            builder.Property(rt => rt.CreatedBy)
                .HasMaxLength(450)
                .IsRequired()
                .HasDefaultValue("System")
                .HasComment("من أنشأ السجل");

            builder.Property(rt => rt.LastModifiedAt)
                .HasColumnType("datetime2")
                .IsRequired()
                .HasComment("تاريخ آخر تعديل");

            builder.Property(rt => rt.LastModifiedBy)
                .HasMaxLength(450)
                .IsRequired()
                .HasComment("من قام بآخر تعديل");

            // تكوين العلاقات
            builder.HasOne(rt => rt.User)
                .WithMany()
                .HasForeignKey(rt => rt.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_RefreshToken_ApplicationUser");

            // تكوين الفهارس
            builder.HasIndex(rt => rt.Token)
                .HasDatabaseName("IX_RefreshToken_Token")
                .IsUnique();

            builder.HasIndex(rt => rt.UserId)
                .HasDatabaseName("IX_RefreshToken_UserId");

            builder.HasIndex(rt => rt.ExpiryDate)
                .HasDatabaseName("IX_RefreshToken_ExpiryDate");

            builder.HasIndex(rt => rt.IsActive)
                .HasDatabaseName("IX_RefreshToken_IsActive");

            builder.HasIndex(rt => rt.IsRevoked)
                .HasDatabaseName("IX_RefreshToken_IsRevoked");

            builder.HasIndex(rt => new { rt.UserId, rt.IsActive, rt.IsRevoked })
                .HasDatabaseName("IX_RefreshToken_UserId_IsActive_IsRevoked");
        }
    }
}
