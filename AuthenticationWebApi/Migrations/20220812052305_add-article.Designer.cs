// <auto-generated />
using System;
using AuthenticationWebApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AuthenticationWebApi.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20220812052305_add-article")]
    partial class addarticle
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.8");

            modelBuilder.Entity("AuthenticationWebApi.Models.Article", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Article");
                });

            modelBuilder.Entity("AuthenticationWebApi.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Deleted")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("BLOB");

                    b.Property<byte[]>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("BLOB");

                    b.Property<string>("RefreshToken")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("TokenCreated")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("TokenExpires")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("AuthenticationWebApi.Models.Article", b =>
                {
                    b.HasOne("AuthenticationWebApi.Models.User", "User")
                        .WithMany("Articles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("AuthenticationWebApi.Models.User", b =>
                {
                    b.Navigation("Articles");
                });
#pragma warning restore 612, 618
        }
    }
}
