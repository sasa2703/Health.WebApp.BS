﻿// <auto-generated />
using System;
using HealthManager.WebApp.BS.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Health.WebApp.BS.Repository.Migrations
{
    [DbContext(typeof(RepositoryContext))]
    [Migration("20250126155515_first_migration")]
    partial class first_migration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("HealthManager.WebApp.BS.Entities.Models.ApiAccessToken", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("Relational:JsonPropertyName", "id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("DtCreated")
                        .HasColumnType("datetime2")
                        .HasAnnotation("Relational:JsonPropertyName", "dtCreated");

                    b.Property<DateTime?>("DtExpireDate")
                        .HasColumnType("datetime2")
                        .HasAnnotation("Relational:JsonPropertyName", "dtExpireDate");

                    b.Property<string>("KeyVaultSecretId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasAnnotation("Relational:JsonPropertyName", "keyVaultSecretId");

                    b.Property<string>("LoginId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasAnnotation("Relational:JsonPropertyName", "loginId");

                    b.Property<string>("SubscriptionId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasAnnotation("Relational:JsonPropertyName", "subscriptionId");

                    b.Property<int>("SubscriptionId1")
                        .HasColumnType("int");

                    b.Property<string>("SubscriptionName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasAnnotation("Relational:JsonPropertyName", "subscriptionName");

                    b.HasKey("Id");

                    b.HasIndex("SubscriptionId1");

                    b.ToTable("ApiAccessToken");

                    b.HasAnnotation("Relational:JsonPropertyName", "apiAccessTokens");
                });

            modelBuilder.Entity("HealthManager.WebApp.BS.Entities.Models.ClinicalTrialMetadata", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("Relational:JsonPropertyName", "id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("ClinicalTrialsCollectionId")
                        .HasColumnType("int");

                    b.Property<int?>("Duration")
                        .HasColumnType("int")
                        .HasAnnotation("Relational:JsonPropertyName", "duration");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("datetime2")
                        .HasAnnotation("Relational:JsonPropertyName", "endDate");

                    b.Property<int?>("Participants")
                        .HasColumnType("int")
                        .HasAnnotation("Relational:JsonPropertyName", "participants");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2")
                        .HasAnnotation("Relational:JsonPropertyName", "startDate");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasAnnotation("Relational:JsonPropertyName", "status");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasAnnotation("Relational:JsonPropertyName", "title");

                    b.Property<string>("TrialId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasAnnotation("Relational:JsonPropertyName", "trialId");

                    b.HasKey("Id");

                    b.HasIndex("ClinicalTrialsCollectionId");

                    b.ToTable("Health");

                    b.HasAnnotation("Relational:JsonPropertyName", "trials");
                });

            modelBuilder.Entity("HealthManager.WebApp.BS.Entities.Models.ClinicalTrialsCollection", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("Relational:JsonPropertyName", "id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("DisplayIndex")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasAnnotation("Relational:JsonPropertyName", "displayIndex");

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasAnnotation("Relational:JsonPropertyName", "displayName");

                    b.HasKey("Id");

                    b.ToTable("HealthCollection");
                });

            modelBuilder.Entity("HealthManager.WebApp.BS.Entities.Models.Subscription", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("Relational:JsonPropertyName", "id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("DtCreated")
                        .HasColumnType("datetime2")
                        .HasAnnotation("Relational:JsonPropertyName", "dtCreated");

                    b.Property<DateTime?>("DtLastUpdate")
                        .HasColumnType("datetime2")
                        .HasAnnotation("Relational:JsonPropertyName", "dtLastUpdate");

                    b.Property<int?>("IUserCategoryId")
                        .HasColumnType("int");

                    b.Property<string>("ProjectCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasAnnotation("Relational:JsonPropertyName", "projectCode");

                    b.Property<string>("SubscriptionName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasAnnotation("Relational:JsonPropertyName", "subscriptionName");

                    b.HasKey("Id");

                    b.HasIndex("IUserCategoryId");

                    b.ToTable("Subscription");

                    b.HasAnnotation("Relational:JsonPropertyName", "subscription");
                });

            modelBuilder.Entity("HealthManager.WebApp.BS.Entities.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("Relational:JsonPropertyName", "id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("CountryCode")
                        .HasColumnType("nvarchar(max)")
                        .HasAnnotation("Relational:JsonPropertyName", "countryCode");

                    b.Property<bool>("Deleted")
                        .HasColumnType("bit")
                        .HasAnnotation("Relational:JsonPropertyName", "deleted");

                    b.Property<DateTime?>("DtCreated")
                        .HasColumnType("datetime2")
                        .HasAnnotation("Relational:JsonPropertyName", "dtCreated");

                    b.Property<DateTime?>("DtLastUpdate")
                        .HasColumnType("datetime2")
                        .HasAnnotation("Relational:JsonPropertyName", "dtLastUpdate");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)")
                        .HasAnnotation("Relational:JsonPropertyName", "name");

                    b.Property<string>("Phone")
                        .HasColumnType("nvarchar(max)")
                        .HasAnnotation("Relational:JsonPropertyName", "phone");

                    b.Property<int?>("RoleId")
                        .HasColumnType("int")
                        .HasAnnotation("Relational:JsonPropertyName", "roleId");

                    b.Property<int>("StatusId")
                        .HasColumnType("int")
                        .HasAnnotation("Relational:JsonPropertyName", "statusId");

                    b.Property<string>("SubscriptionId")
                        .HasColumnType("nvarchar(max)")
                        .HasAnnotation("Relational:JsonPropertyName", "subscriptionId");

                    b.Property<int?>("SubscriptionId1")
                        .HasColumnType("int");

                    b.Property<string>("TimeZone")
                        .HasColumnType("nvarchar(max)")
                        .HasAnnotation("Relational:JsonPropertyName", "timeZone");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasAnnotation("Relational:JsonPropertyName", "title");

                    b.Property<int>("UserCategory")
                        .HasColumnType("int")
                        .HasAnnotation("Relational:JsonPropertyName", "userCategory");

                    b.Property<int>("UserCategoryNavigationId")
                        .HasColumnType("int");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasAnnotation("Relational:JsonPropertyName", "username");

                    b.HasKey("Id");

                    b.HasIndex("StatusId");

                    b.HasIndex("SubscriptionId1");

                    b.HasIndex("UserCategoryNavigationId");

                    b.ToTable("User");

                    b.HasAnnotation("Relational:JsonPropertyName", "users");
                });

            modelBuilder.Entity("HealthManager.WebApp.BS.Entities.Models.UserCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("Relational:JsonPropertyName", "id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("UserCategoryName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasAnnotation("Relational:JsonPropertyName", "userCategoryName");

                    b.HasKey("Id");

                    b.ToTable("UserCategory");

                    b.HasAnnotation("Relational:JsonPropertyName", "userCategoryNavigation");
                });

            modelBuilder.Entity("HealthManager.WebApp.BS.Entities.Models.UserStatus", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("Relational:JsonPropertyName", "id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("StatusName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasAnnotation("Relational:JsonPropertyName", "statusName");

                    b.HasKey("Id");

                    b.ToTable("UserStatus");

                    b.HasAnnotation("Relational:JsonPropertyName", "status");
                });

            modelBuilder.Entity("HealthManager.WebApp.BS.Entities.Models.ApiAccessToken", b =>
                {
                    b.HasOne("HealthManager.WebApp.BS.Entities.Models.Subscription", "Subscription")
                        .WithMany("ApiAccessTokens")
                        .HasForeignKey("SubscriptionId1")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Subscription");
                });

            modelBuilder.Entity("HealthManager.WebApp.BS.Entities.Models.ClinicalTrialMetadata", b =>
                {
                    b.HasOne("HealthManager.WebApp.BS.Entities.Models.ClinicalTrialsCollection", null)
                        .WithMany("Trials")
                        .HasForeignKey("ClinicalTrialsCollectionId");
                });

            modelBuilder.Entity("HealthManager.WebApp.BS.Entities.Models.Subscription", b =>
                {
                    b.HasOne("HealthManager.WebApp.BS.Entities.Models.UserCategory", "IUserCategory")
                        .WithMany("Subscriptions")
                        .HasForeignKey("IUserCategoryId");

                    b.Navigation("IUserCategory");
                });

            modelBuilder.Entity("HealthManager.WebApp.BS.Entities.Models.User", b =>
                {
                    b.HasOne("HealthManager.WebApp.BS.Entities.Models.UserStatus", "Status")
                        .WithMany("Users")
                        .HasForeignKey("StatusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HealthManager.WebApp.BS.Entities.Models.Subscription", "Subscription")
                        .WithMany("Users")
                        .HasForeignKey("SubscriptionId1");

                    b.HasOne("HealthManager.WebApp.BS.Entities.Models.UserCategory", "UserCategoryNavigation")
                        .WithMany("Users")
                        .HasForeignKey("UserCategoryNavigationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Status");

                    b.Navigation("Subscription");

                    b.Navigation("UserCategoryNavigation");
                });

            modelBuilder.Entity("HealthManager.WebApp.BS.Entities.Models.ClinicalTrialsCollection", b =>
                {
                    b.Navigation("Trials");
                });

            modelBuilder.Entity("HealthManager.WebApp.BS.Entities.Models.Subscription", b =>
                {
                    b.Navigation("ApiAccessTokens");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("HealthManager.WebApp.BS.Entities.Models.UserCategory", b =>
                {
                    b.Navigation("Subscriptions");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("HealthManager.WebApp.BS.Entities.Models.UserStatus", b =>
                {
                    b.Navigation("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
