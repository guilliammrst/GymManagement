﻿// <auto-generated />
using System;
using GymManagement.Infrastructure.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GymManagement.Infrastructure.Persistence.Migrations
{
    [DbContext(typeof(BaseGymDbContext))]
    [Migration("20250625135528_DeleteMembershipsCascadeWhenUserDelete")]
    partial class DeleteMembershipsCascadeWhenUserDelete
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("GymManagement.Infrastructure.Persistence.Models.AddressModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("city");

                    b.Property<int>("Country")
                        .HasColumnType("integer")
                        .HasColumnName("country");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<string>("Number")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("house_number");

                    b.Property<string>("PostalCode")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("postal_code");

                    b.Property<string>("Street")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("street");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("Id");

                    b.ToTable("addresses");
                });

            modelBuilder.Entity("GymManagement.Infrastructure.Persistence.Models.AttendanceModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CheckInTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("check_in_time");

                    b.Property<int>("ClubId")
                        .HasColumnType("integer")
                        .HasColumnName("club_id");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<int>("MemberId")
                        .HasColumnType("integer")
                        .HasColumnName("member_id");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("Id");

                    b.HasIndex("ClubId");

                    b.HasIndex("MemberId");

                    b.ToTable("attendances");
                });

            modelBuilder.Entity("GymManagement.Infrastructure.Persistence.Models.ClubModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("AddressId")
                        .HasColumnType("integer")
                        .HasColumnName("address_id");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<int?>("ManagerId")
                        .HasColumnType("integer")
                        .HasColumnName("manager_id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("Id");

                    b.HasIndex("AddressId")
                        .IsUnique();

                    b.HasIndex("ManagerId");

                    b.ToTable("clubs");
                });

            modelBuilder.Entity("GymManagement.Infrastructure.Persistence.Models.MembershipModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("end_date");

                    b.Property<int>("HomeClubId")
                        .HasColumnType("integer")
                        .HasColumnName("home_club_id");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean")
                        .HasColumnName("is_active");

                    b.Property<int>("MemberId")
                        .HasColumnType("integer")
                        .HasColumnName("member_id");

                    b.Property<int>("MembershipPlanId")
                        .HasColumnType("integer")
                        .HasColumnName("membership_plan_id");

                    b.Property<int>("PaymentDetailId")
                        .HasColumnType("integer")
                        .HasColumnName("payment_detail_id");

                    b.Property<bool>("RenewWhenExpiry")
                        .HasColumnType("boolean")
                        .HasColumnName("renew_when_expiry");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("start_date");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("Id");

                    b.HasIndex("HomeClubId");

                    b.HasIndex("MemberId");

                    b.HasIndex("MembershipPlanId");

                    b.HasIndex("PaymentDetailId")
                        .IsUnique();

                    b.ToTable("memberships");
                });

            modelBuilder.Entity("GymManagement.Infrastructure.Persistence.Models.MembershipPlanModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<decimal>("BasePrice")
                        .HasColumnType("numeric")
                        .HasColumnName("base_price");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<bool>("IsValid")
                        .HasColumnType("boolean")
                        .HasColumnName("is_valid");

                    b.Property<int>("MembershipPlanType")
                        .HasColumnType("integer")
                        .HasColumnName("membership_plan_type");

                    b.Property<decimal>("RegistrationFees")
                        .HasColumnType("numeric")
                        .HasColumnName("registration_fees");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.Property<decimal>("YearlyDiscount")
                        .HasColumnType("numeric")
                        .HasColumnName("yearly_discount");

                    b.HasKey("Id");

                    b.ToTable("membership_plans");
                });

            modelBuilder.Entity("GymManagement.Infrastructure.Persistence.Models.PaymentDetailModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<decimal>("Amount")
                        .HasColumnType("numeric")
                        .HasColumnName("amount");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<DateTime?>("PaymentDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("payment_date");

                    b.Property<int>("PaymentMethod")
                        .HasColumnType("integer")
                        .HasColumnName("payment_method");

                    b.Property<int>("PaymentStatus")
                        .HasColumnType("integer")
                        .HasColumnName("payment_status");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("Id");

                    b.ToTable("payment_details");
                });

            modelBuilder.Entity("GymManagement.Infrastructure.Persistence.Models.UserModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int?>("AddressId")
                        .HasColumnType("integer")
                        .HasColumnName("address_id");

                    b.Property<DateTime>("Birthdate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("birthdate");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("email");

                    b.Property<int>("Gender")
                        .HasColumnType("integer")
                        .HasColumnName("gender");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("password");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("phone_number");

                    b.Property<int>("Role")
                        .HasColumnType("integer")
                        .HasColumnName("role");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("surname");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("Id");

                    b.HasIndex("AddressId")
                        .IsUnique();

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("PhoneNumber")
                        .IsUnique();

                    b.ToTable("users");
                });

            modelBuilder.Entity("GymManagement.Infrastructure.Persistence.Models.AttendanceModel", b =>
                {
                    b.HasOne("GymManagement.Infrastructure.Persistence.Models.ClubModel", "Club")
                        .WithMany("Attendances")
                        .HasForeignKey("ClubId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GymManagement.Infrastructure.Persistence.Models.UserModel", "Member")
                        .WithMany("Attendances")
                        .HasForeignKey("MemberId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Club");

                    b.Navigation("Member");
                });

            modelBuilder.Entity("GymManagement.Infrastructure.Persistence.Models.ClubModel", b =>
                {
                    b.HasOne("GymManagement.Infrastructure.Persistence.Models.AddressModel", "Address")
                        .WithOne()
                        .HasForeignKey("GymManagement.Infrastructure.Persistence.Models.ClubModel", "AddressId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GymManagement.Infrastructure.Persistence.Models.UserModel", "Manager")
                        .WithMany()
                        .HasForeignKey("ManagerId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("Address");

                    b.Navigation("Manager");
                });

            modelBuilder.Entity("GymManagement.Infrastructure.Persistence.Models.MembershipModel", b =>
                {
                    b.HasOne("GymManagement.Infrastructure.Persistence.Models.ClubModel", "HomeClub")
                        .WithMany("Memberships")
                        .HasForeignKey("HomeClubId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .IsRequired();

                    b.HasOne("GymManagement.Infrastructure.Persistence.Models.UserModel", "Member")
                        .WithMany("Memberships")
                        .HasForeignKey("MemberId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GymManagement.Infrastructure.Persistence.Models.MembershipPlanModel", "MembershipPlan")
                        .WithMany("Memberships")
                        .HasForeignKey("MembershipPlanId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .IsRequired();

                    b.HasOne("GymManagement.Infrastructure.Persistence.Models.PaymentDetailModel", "PaymentDetail")
                        .WithOne("Membership")
                        .HasForeignKey("GymManagement.Infrastructure.Persistence.Models.MembershipModel", "PaymentDetailId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("HomeClub");

                    b.Navigation("Member");

                    b.Navigation("MembershipPlan");

                    b.Navigation("PaymentDetail");
                });

            modelBuilder.Entity("GymManagement.Infrastructure.Persistence.Models.UserModel", b =>
                {
                    b.HasOne("GymManagement.Infrastructure.Persistence.Models.AddressModel", "Address")
                        .WithOne()
                        .HasForeignKey("GymManagement.Infrastructure.Persistence.Models.UserModel", "AddressId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Address");
                });

            modelBuilder.Entity("GymManagement.Infrastructure.Persistence.Models.ClubModel", b =>
                {
                    b.Navigation("Attendances");

                    b.Navigation("Memberships");
                });

            modelBuilder.Entity("GymManagement.Infrastructure.Persistence.Models.MembershipPlanModel", b =>
                {
                    b.Navigation("Memberships");
                });

            modelBuilder.Entity("GymManagement.Infrastructure.Persistence.Models.PaymentDetailModel", b =>
                {
                    b.Navigation("Membership");
                });

            modelBuilder.Entity("GymManagement.Infrastructure.Persistence.Models.UserModel", b =>
                {
                    b.Navigation("Attendances");

                    b.Navigation("Memberships");
                });
#pragma warning restore 612, 618
        }
    }
}
