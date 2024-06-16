using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Server_.Models.EntityModel;

public partial class MedicalSearchEngineContext : DbContext
{
    public MedicalSearchEngineContext()
    {
    }

    public MedicalSearchEngineContext(DbContextOptions<MedicalSearchEngineContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<Appointment> Appointments { get; set; }

    public virtual DbSet<Doctor> Doctors { get; set; }

    public virtual DbSet<MedicalRecord> MedicalRecords { get; set; }

    public virtual DbSet<Patient> Patients { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=tcp:sql-server-rabin.database.windows.net,1433;Initial Catalog=MedicalSearchEngine;Persist Security Info=False;User ID=test;Password=Technology123#;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.AdminId).HasName("PK__Admin__43AA414193777C4F");

            entity.ToTable("Admin");

            entity.HasIndex(e => e.Email, "admin_email_constraint").IsUnique();

            entity.Property(e => e.AdminId)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("admin_id");
            entity.Property(e => e.ContactNumber)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("contact_number");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.ProfileImgUrl)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("profile_img_url");
        });

        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.HasKey(e => e.AppointmentId).HasName("PK__Appointm__A50828FC125C4B3B");

            entity.ToTable("Appointment");

            entity.Property(e => e.AppointmentId).HasColumnName("appointment_id");
            entity.Property(e => e.AppointmentDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("appointment_date");
            entity.Property(e => e.AppointmentTime)
                .HasDefaultValueSql("(CONVERT([time],getdate()))")
                .HasColumnName("appointment_time");
            entity.Property(e => e.DoctorId)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("doctor_id");
            entity.Property(e => e.MeetingUrl)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("meeting_url");
            entity.Property(e => e.Notes)
                .HasMaxLength(400)
                .IsUnicode(false)
                .HasColumnName("notes");
            entity.Property(e => e.PatientId)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("patient_id");
            entity.Property(e => e.Purpose)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("purpose");
            entity.Property(e => e.Status)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasDefaultValue("pending")
                .HasColumnName("status");

            entity.HasOne(d => d.Doctor).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.DoctorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Appointme__docto__66603565");

            entity.HasOne(d => d.Patient).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.PatientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Appointme__patie__6754599E");
        });

        modelBuilder.Entity<Doctor>(entity =>
        {
            entity.HasKey(e => e.DoctorId).HasName("PK__Doctor__F3993564DE463479");

            entity.ToTable("Doctor");

            entity.HasIndex(e => e.Email, "doctor_email_constraint").IsUnique();

            entity.Property(e => e.DoctorId)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("doctor_id");
            entity.Property(e => e.Availability)
                .HasDefaultValue(1)
                .HasColumnName("availability");
            entity.Property(e => e.ContactNumber)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("contact_number");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.ProfileImgUrl)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("profile_img_url");
            entity.Property(e => e.Speciality)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("speciality");
        });

        modelBuilder.Entity<MedicalRecord>(entity =>
        {
            entity.HasKey(e => e.RecordId).HasName("PK__MedicalR__BFCFB4DD19BFE398");

            entity.ToTable("MedicalRecord");

            entity.Property(e => e.RecordId).HasColumnName("record_id");
            entity.Property(e => e.Description)
                .HasMaxLength(400)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.DoctorId)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("doctor_id");
            entity.Property(e => e.PatientId)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("patient_id");
            entity.Property(e => e.RecordDate).HasColumnName("record_date");

            entity.HasOne(d => d.Doctor).WithMany(p => p.MedicalRecords)
                .HasForeignKey(d => d.DoctorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MedicalRe__docto__6C190EBB");

            entity.HasOne(d => d.Patient).WithMany(p => p.MedicalRecords)
                .HasForeignKey(d => d.PatientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MedicalRe__patie__6B24EA82");
        });

        modelBuilder.Entity<Patient>(entity =>
        {
            entity.HasKey(e => e.PatientId).HasName("PK__Patient__4D5CE476F5230552");

            entity.ToTable("Patient");

            entity.HasIndex(e => e.Email, "email_constraint").IsUnique();

            entity.Property(e => e.PatientId)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("patient_id");
            entity.Property(e => e.Address)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("address");
            entity.Property(e => e.BirthDate).HasColumnName("birth_date");
            entity.Property(e => e.ContactNumber)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("contact_number");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.MedicalHistory)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("medical_history");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.ProfileImgUrl)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("profile_img_url");
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasKey(e => e.UserRoleId).HasName("PK__UserRole__B8D9ABA259CE4BD9");

            entity.Property(e => e.UserRoleId).HasColumnName("user_role_id");
            entity.Property(e => e.Role)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("role");
            entity.Property(e => e.UserId)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("user_id");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
