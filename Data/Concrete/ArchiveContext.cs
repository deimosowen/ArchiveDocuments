using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using ArchiveDocuments.Data.Entities.Models;

namespace ArchiveDocuments.Data.Concrete
{
    public partial class ArchiveContext : DbContext
    {
        public ArchiveContext()
        {
        }

        public ArchiveContext(DbContextOptions<ArchiveContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUserRole> AspNetUserRoles { get; set; }
        public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; }
        public virtual DbSet<DFolder> DFolders { get; set; }
        public virtual DbSet<DService> DServices { get; set; }
        public virtual DbSet<DServicesDocument> DServicesDocuments { get; set; }
        public virtual DbSet<DServicesDocumentFile> DServicesDocumentFiles { get; set; }
        public virtual DbSet<DServicesMetadatum> DServicesMetadata { get; set; }
        public virtual DbSet<DeviceCode> DeviceCodes { get; set; }
        public virtual DbSet<PersistedGrant> PersistedGrants { get; set; }
        public virtual DbSet<SDocument> SDocuments { get; set; }
        public virtual DbSet<SDocumentMetadatum> SDocumentMetadata { get; set; }
        public virtual DbSet<SOrganization> SOrganizations { get; set; }
        public virtual DbSet<SSetting> SSettings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("uuid-ossp")
                .HasAnnotation("Relational:Collation", "Russian_Russia.1251");

            modelBuilder.Entity<AspNetRole>(entity =>
            {
                entity.HasIndex(e => e.NormalizedName, "RoleNameIndex")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.NormalizedName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetRoleClaim>(entity =>
            {
                entity.HasIndex(e => e.RoleId, "IX_AspNetRoleClaims_RoleId");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetRoleClaims)
                    .HasForeignKey(d => d.RoleId);
            });

            modelBuilder.Entity<AspNetUser>(entity =>
            {
                entity.HasIndex(e => e.NormalizedEmail, "EmailIndex");

                entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.LockoutEnd).HasColumnType("timestamp with time zone");

                entity.Property(e => e.EmployeesName).HasMaxLength(256);
                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

                entity.Property(e => e.SorganizationId).HasColumnName("SOrganizationId");

                entity.Property(e => e.UserName).HasMaxLength(256);

                entity.HasOne(d => d.Sorganization)
                    .WithMany(p => p.AspNetUsers)
                    .HasForeignKey(d => d.SorganizationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("AspNetUsers_SOrganizationId_fkey");
            });

            modelBuilder.Entity<AspNetUserClaim>(entity =>
            {
                entity.HasIndex(e => e.UserId, "IX_AspNetUserClaims_UserId");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserClaims)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserLogin>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

                entity.HasIndex(e => e.UserId, "IX_AspNetUserLogins_UserId");

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.ProviderKey).HasMaxLength(128);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserLogins)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserRole>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });

                entity.HasIndex(e => e.RoleId, "IX_AspNetUserRoles_RoleId");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.RoleId);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserToken>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.Name).HasMaxLength(128);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserTokens)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<DFolder>(entity =>
            {
                entity.ToTable("d_folder");

                entity.HasComment("Папки");

                entity.HasIndex(e => e.SOrganizationId, "d_folder_s_organization_id_idx");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("uuid_generate_v4()")
                    .HasComment("Первичный ключ");

                entity.Property(e => e.DateAdd)
                    .HasPrecision(6)
                    .HasColumnName("date_add")
                    .HasDefaultValueSql("now()")
                    .HasComment("Дата добавления");

                entity.Property(e => e.EmployeesNameAdd)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("employees_name_add")
                    .HasComment("Сотрудник добавивший");

                entity.Property(e => e.FolderName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("folder_name")
                    .HasComment("Наименование папки");

                entity.Property(e => e.SOrganizationId)
                    .HasColumnName("s_organization_id")
                    .HasComment("Организация");

                entity.HasOne(d => d.SOrganization)
                    .WithMany(p => p.DFolders)
                    .HasForeignKey(d => d.SOrganizationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("d_folder_s_organization_id_fkey");
            });

            modelBuilder.Entity<DService>(entity =>
            {
                entity.ToTable("d_services");

                entity.HasComment("Дела");

                entity.HasIndex(e => e.SDocumentId, "d_services_s_document_id_idx");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("uuid_generate_v4()")
                    .HasComment("Первичный ключ");

                entity.Property(e => e.CaseNumber)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("case_number")
                    .HasComment("Номер дела");

                entity.Property(e => e.CountPage)
                    .HasColumnName("count_page")
                    .HasComment("Количество страниц");

                entity.Property(e => e.DFolderId)
                    .HasColumnName("d_folder_id")
                    .HasComment("Папка");

                entity.Property(e => e.Description)
                    .HasMaxLength(500)
                    .HasColumnName("description")
                    .HasComment("Описание");

                entity.Property(e => e.SDocumentId)
                    .HasColumnName("s_document_id")
                    .HasComment("Документ");

                entity.Property(e => e.YearStart)
                    .HasColumnName("year_start")
                    .HasComment("Год начала");

                entity.Property(e => e.YearStop)
                    .HasColumnName("year_stop")
                    .HasComment("Год окончания");

                entity.HasOne(d => d.DFolder)
                    .WithMany(p => p.DServices)
                    .HasForeignKey(d => d.DFolderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("d_services_d_folder_id_fkey");

                entity.HasOne(d => d.SDocument)
                    .WithMany(p => p.DServices)
                    .HasForeignKey(d => d.SDocumentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("d_services_s_document_id_fkey");
            });

            modelBuilder.Entity<DServicesDocument>(entity =>
            {
                entity.ToTable("d_services_document");

                entity.HasComment("Документ к делу");

                entity.HasIndex(e => e.DServicesId, "d_services_document_d_services_id_idx");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("uuid_generate_v4()")
                    .HasComment("Первичный ключ");

                entity.Property(e => e.DServicesId)
                    .HasColumnName("d_services_id")
                    .HasComment("Дело");

                entity.Property(e => e.DateAdd)
                    .HasPrecision(6)
                    .HasColumnName("date_add")
                    .HasDefaultValueSql("now()")
                    .HasComment("Дата добавления");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(500)
                    .HasColumnName("description")
                    .HasComment("Описание");

                entity.Property(e => e.EmployeesNameAdd)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("employees_name_add")
                    .HasComment("Сотрудник добавивший");

                entity.HasOne(d => d.DServices)
                    .WithMany(p => p.DServicesDocuments)
                    .HasForeignKey(d => d.DServicesId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("d_services_document_d_services_id_fkey");
            });

            modelBuilder.Entity<DServicesDocumentFile>(entity =>
            {
                entity.ToTable("d_services_document_file");

                entity.HasComment("Файлы к документу");

                entity.HasIndex(e => e.DServicesDocumentId, "d_services_document_file_d_services_document_id_idx");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("uuid_generate_v4()")
                    .HasComment("Первичный ключ");

                entity.Property(e => e.DServicesDocumentId)
                    .HasColumnName("d_services_document_id")
                    .HasComment("Документ");

                entity.Property(e => e.DateAdd)
                    .HasPrecision(6)
                    .HasColumnName("date_add")
                    .HasDefaultValueSql("now()")
                    .HasComment("Дата добавления");

                entity.Property(e => e.EmployeesNameAdd)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("employees_name_add")
                    .HasComment("Сотрудник добавивший");

                entity.Property(e => e.FileExt)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("file_ext")
                    .HasComment("расширение файла");

                entity.Property(e => e.FileName)
                    .IsRequired()
                    .HasMaxLength(400)
                    .HasColumnName("file_name")
                    .HasComment("имя файла");

                entity.Property(e => e.FileSize)
                    .HasColumnName("file_size")
                    .HasComment("размер файла");

                entity.Property(e => e.FileContent)
                    .HasColumnName("file_content")
                    .HasComment("контент файла");

                entity.HasOne(d => d.DServicesDocument)
                    .WithMany(p => p.DServicesDocumentFiles)
                    .HasForeignKey(d => d.DServicesDocumentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("d_services_document_file_d_services_document_id_fkey");
            });

            modelBuilder.Entity<DServicesMetadatum>(entity =>
            {
                entity.ToTable("d_services_metadata");

                entity.HasComment("Метаданные к документу");

                entity.HasIndex(e => e.DServicesDocumentId, "d_services_metadata_d_services_document_id_idx");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("uuid_generate_v4()")
                    .HasComment("Первичный ключ");

                entity.Property(e => e.DServicesDocumentId)
                    .HasColumnName("d_services_document_id")
                    .HasComment("Документ");

                entity.Property(e => e.IsBinding)
                    .IsRequired()
                    .HasColumnName("is_binding")
                    .HasDefaultValueSql("true")
                    .HasComment("Обязательность");

                entity.Property(e => e.MetadataName)
                    .IsRequired()
                    .HasMaxLength(70)
                    .HasColumnName("metadata_name")
                    .HasComment("Наименование поля");

                entity.Property(e => e.MetadataValue)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("metadata_value")
                    .HasComment("Значение поля");

                entity.HasOne(d => d.DServicesDocument)
                    .WithMany(p => p.DServicesMetadata)
                    .HasForeignKey(d => d.DServicesDocumentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("d_services_metadata_d_services_document_id_fkey");
            });

            modelBuilder.Entity<DeviceCode>(entity =>
            {
                entity.HasKey(e => e.UserCode);

                entity.HasIndex(e => e.DeviceCode1, "IX_DeviceCodes_DeviceCode")
                    .IsUnique();

                entity.HasIndex(e => e.Expiration, "IX_DeviceCodes_Expiration");

                entity.Property(e => e.UserCode).HasMaxLength(200);

                entity.Property(e => e.ClientId)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Data)
                    .IsRequired()
                    .HasMaxLength(50000);

                entity.Property(e => e.Description).HasMaxLength(200);

                entity.Property(e => e.DeviceCode1)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("DeviceCode");

                entity.Property(e => e.SessionId).HasMaxLength(100);

                entity.Property(e => e.SubjectId).HasMaxLength(200);
            });

            modelBuilder.Entity<PersistedGrant>(entity =>
            {
                entity.HasKey(e => e.Key);

                entity.HasIndex(e => e.Expiration, "IX_PersistedGrants_Expiration");

                entity.HasIndex(e => new { e.SubjectId, e.ClientId, e.Type }, "IX_PersistedGrants_SubjectId_ClientId_Type");

                entity.HasIndex(e => new { e.SubjectId, e.SessionId, e.Type }, "IX_PersistedGrants_SubjectId_SessionId_Type");

                entity.Property(e => e.Key).HasMaxLength(200);

                entity.Property(e => e.ClientId)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Data)
                    .IsRequired()
                    .HasMaxLength(50000);

                entity.Property(e => e.Description).HasMaxLength(200);

                entity.Property(e => e.SessionId).HasMaxLength(100);

                entity.Property(e => e.SubjectId).HasMaxLength(200);

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<SDocument>(entity =>
            {
                entity.ToTable("s_document");

                entity.HasComment("Справочник документов");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("uuid_generate_v4()")
                    .HasComment("Первичный ключ");

                entity.Property(e => e.DateAdd)
                    .HasColumnName("date_add")
                    .HasDefaultValueSql("now()")
                    .HasComment("Дата добавления");

                entity.Property(e => e.DocumentName)
                    .IsRequired()
                    .HasMaxLength(70)
                    .HasColumnName("document_name")
                    .HasComment("Наименование документа");

                entity.Property(e => e.EmployeesNameAdd)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("employees_name_add")
                    .HasComment("Сотрудник добавивший");
            });

            modelBuilder.Entity<SDocumentMetadatum>(entity =>
            {
                entity.ToTable("s_document_metadata");

                entity.HasComment("Справочник Метаданных к документу");

                entity.HasIndex(e => e.SDocumentId, "s_document_metadata_s_document_id_idx");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("uuid_generate_v4()")
                    .HasComment("Первичный ключ");

                entity.Property(e => e.DateAdd)
                    .HasPrecision(6)
                    .HasColumnName("date_add")
                    .HasDefaultValueSql("now()")
                    .HasComment("Дата добавления");

                entity.Property(e => e.EmployeesNameAdd)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("employees_name_add")
                    .HasComment("Сотрудник добавивший");

                entity.Property(e => e.IsBinding)
                    .IsRequired()
                    .HasColumnName("is_binding")
                    .HasDefaultValueSql("true")
                    .HasComment("Обязательность");

                entity.Property(e => e.MetadataLength)
                    .IsRequired()
                    .HasColumnType("character varying")
                    .HasColumnName("metadata_length")
                    .HasComment("Длина поля");

                entity.Property(e => e.MetadataName)
                    .IsRequired()
                    .HasMaxLength(70)
                    .HasColumnName("metadata_name")
                    .HasComment("Наименование поля");

                entity.Property(e => e.SDocumentId)
                    .HasColumnName("s_document_id")
                    .HasComment("Документ");

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasComment("Тип (1 число 2 строка 3 дата 4 текст)");

                entity.HasOne(d => d.SDocument)
                    .WithMany(p => p.SDocumentMetadata)
                    .HasForeignKey(d => d.SDocumentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("s_document_metadata_s_document_id_fkey");
            });

            modelBuilder.Entity<SOrganization>(entity =>
            {
                entity.ToTable("s_organization");

                entity.HasComment("Справочник Организации");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("uuid_generate_v4()")
                    .HasComment("Первичный ключ");

                entity.Property(e => e.OrganizationAddress)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("organization_address")
                    .HasComment("Адрес");

                entity.Property(e => e.OrganizationEmail)
                    .IsRequired()
                    .HasMaxLength(70)
                    .HasColumnName("organization_email")
                    .HasComment("Электронная почта");

                entity.Property(e => e.OrganizationName)
                    .IsRequired()
                    .HasMaxLength(70)
                    .HasColumnName("organization_name")
                    .HasComment("Организация");

                entity.Property(e => e.OrganizationTel)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("organization_tel")
                    .HasComment("Телефон");
            });

            modelBuilder.Entity<SSetting>(entity =>
            {
                entity.ToTable("s_settings");

                entity.HasComment("Таблица с настройками");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasComment("Первичный ключ");

                entity.Property(e => e.ParamName)
                    .IsRequired()
                    .HasMaxLength(70)
                    .HasColumnName("param_name")
                    .HasComment("Наименование параметра");

                entity.Property(e => e.ParamValue)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("param_value")
                    .HasComment("Значение параметра");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
