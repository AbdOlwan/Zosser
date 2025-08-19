using DAL.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace DAL.Configurations.Identity
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            // تكوين الجدول
            builder.ToTable("AspNetUsers", "Identity");

            // تكوين الخصائص الأساسية
            builder.HasKey(u => u.Id);

            builder.Property(u => u.Id)
                .HasMaxLength(450)
                .IsRequired();

            // تكوين الخصائص المخصصة
            builder.Property(u => u.Name)
                .HasMaxLength(100)
                .IsRequired()
                .HasComment("اسم المستخدم الكامل");

            builder.Property(u => u.Gender)
                .HasMaxLength(10)
                .IsRequired(false)
                .HasComment("جنس المستخدم");

            builder.Property(u => u.DateOfBirth)
                .HasColumnType("date")
                .IsRequired(false)
                .HasComment("تاريخ ميلاد المستخدم");

            builder.Property(u => u.IsActive)
                .IsRequired()
                .HasDefaultValue(true)
                .HasComment("حالة نشاط المستخدم");

            builder.Property(u => u.RegistrationDate)
                .HasColumnType("datetime2")
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()")
                .HasComment("تاريخ تسجيل الحساب");

            // تكوين خصائص Identity المُعاد تعريفها
            builder.Property(u => u.Email)
                .HasMaxLength(256)
                .IsRequired(false);

            builder.Property(u => u.PhoneNumber)
                .HasMaxLength(11)
                .IsRequired()
                .HasComment("رقم الهاتف المصري");

            // تكوين خصائص Auditing
            builder.Property(u => u.CreatedAt)
                .HasColumnType("datetime2")
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()")
                .HasComment("تاريخ إنشاء السجل");

            builder.Property(u => u.CreatedBy)
                .HasMaxLength(450)
                .IsRequired()
                .HasDefaultValue("System")
                .HasComment("هوية من أنشأ السجل");

            builder.Property(u => u.LastModifiedAt)
                .HasColumnType("datetime2")
                .IsRequired()
                .HasComment("تاريخ آخر تعديل");

            builder.Property(u => u.LastModifiedBy)
                .HasMaxLength(450)
                .IsRequired()
                .HasComment("هوية من قام بآخر تعديل");

            // تكوين العلاقات
            builder.HasOne(u => u.Customer)
                .WithOne(c => c.User)
                .HasForeignKey<Customer>(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Customer_ApplicationUser");

            // تكوين الفهارس
            builder.HasIndex(u => u.Email)
                .HasDatabaseName("IX_ApplicationUser_Email")
                .IsUnique(false);

            builder.HasIndex(u => u.PhoneNumber)
                .HasDatabaseName("IX_ApplicationUser_PhoneNumber")
                .IsUnique();

            builder.HasIndex(u => u.IsActive)
                .HasDatabaseName("IX_ApplicationUser_IsActive");

            builder.HasIndex(u => u.RegistrationDate)
                .HasDatabaseName("IX_ApplicationUser_RegistrationDate");


        }
    }
}
