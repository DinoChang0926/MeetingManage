﻿// <auto-generated />
using MeetingManage.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace MeetingManage.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20220107014521_migration0000")]
    partial class migration0000
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("MeetingManage.Models.Meeting", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<string>("Account")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Account");

                    b.Property<string>("ETime")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("ETime");

                    b.Property<string>("Event")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Event");

                    b.Property<string>("Remarks")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Remarks");

                    b.Property<string>("Room")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Room");

                    b.Property<string>("STime")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("STime");

                    b.Property<long>("userId")
                        .HasColumnType("bigint")
                        .HasColumnName("user_id");

                    b.HasKey("Id");

                    b.HasIndex("userId");

                    b.ToTable("Meetings");
                });

            modelBuilder.Entity("MeetingManage.Models.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<string>("Account")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Account");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("password");

                    b.Property<byte>("Role")
                        .HasColumnType("tinyint")
                        .HasColumnName("Role");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("userName");

                    b.HasKey("Id");

                    b.ToTable("UserData");
                });

            modelBuilder.Entity("MeetingManage.Models.Meeting", b =>
                {
                    b.HasOne("MeetingManage.Models.User", "User")
                        .WithMany("Meetings")
                        .HasForeignKey("userId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("MeetingManage.Models.User", b =>
                {
                    b.Navigation("Meetings");
                });
#pragma warning restore 612, 618
        }
    }
}
