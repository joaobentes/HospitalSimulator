using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using HospitalSimulator.Models;

namespace HospitalSimulator.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.1")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("HospitalSimulator.Models.Consultation", b =>
                {
                    b.Property<int>("ConsultationID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("ConsultationDate");

                    b.Property<int>("DoctorID");

                    b.Property<int>("PatientID");

                    b.Property<DateTime>("RegistrationDate");

                    b.Property<string>("TreatmentRoomName")
                        .IsRequired();

                    b.HasKey("ConsultationID");

                    b.HasIndex("DoctorID");

                    b.HasIndex("PatientID");

                    b.HasIndex("TreatmentRoomName");

                    b.ToTable("Consultations");
                });

            modelBuilder.Entity("HospitalSimulator.Models.Doctor", b =>
                {
                    b.Property<int>("DoctorID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("DoctorID");

                    b.ToTable("Doctors");
                });

            modelBuilder.Entity("HospitalSimulator.Models.DoctorRole", b =>
                {
                    b.Property<int>("DoctorID");

                    b.Property<int>("RoleID");

                    b.HasKey("DoctorID", "RoleID");

                    b.HasIndex("RoleID");

                    b.ToTable("DoctorRoles");
                });

            modelBuilder.Entity("HospitalSimulator.Models.Patient", b =>
                {
                    b.Property<int>("PatientID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Condition")
                        .IsRequired();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("PatientID");

                    b.ToTable("Patients");
                });

            modelBuilder.Entity("HospitalSimulator.Models.Role", b =>
                {
                    b.Property<int>("RoleID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("RoleID");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("HospitalSimulator.Models.TreatmentMachine", b =>
                {
                    b.Property<string>("Name")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Capability")
                        .IsRequired();

                    b.HasKey("Name");

                    b.ToTable("TreatmentMachines");
                });

            modelBuilder.Entity("HospitalSimulator.Models.TreatmentRoom", b =>
                {
                    b.Property<string>("Name")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("MachineName");

                    b.HasKey("Name");

                    b.HasIndex("MachineName")
                        .IsUnique();

                    b.ToTable("TreatmentRooms");
                });

            modelBuilder.Entity("HospitalSimulator.Models.Consultation", b =>
                {
                    b.HasOne("HospitalSimulator.Models.Doctor", "Doctor")
                        .WithMany()
                        .HasForeignKey("DoctorID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("HospitalSimulator.Models.Patient", "Patient")
                        .WithMany()
                        .HasForeignKey("PatientID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("HospitalSimulator.Models.TreatmentRoom", "TreatmentRoom")
                        .WithMany()
                        .HasForeignKey("TreatmentRoomName")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("HospitalSimulator.Models.DoctorRole", b =>
                {
                    b.HasOne("HospitalSimulator.Models.Doctor", "Doctor")
                        .WithMany("DoctorRoles")
                        .HasForeignKey("DoctorID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("HospitalSimulator.Models.Role", "Role")
                        .WithMany("DoctorRoles")
                        .HasForeignKey("RoleID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("HospitalSimulator.Models.TreatmentRoom", b =>
                {
                    b.HasOne("HospitalSimulator.Models.TreatmentMachine", "Machine")
                        .WithOne()
                        .HasForeignKey("HospitalSimulator.Models.TreatmentRoom", "MachineName");
                });
        }
    }
}
