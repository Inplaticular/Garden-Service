﻿// <auto-generated />
using System;
using Inplanticular.Garden_Service.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Inplanticular.Garden_Service.WebAPI.Migrations
{
    [DbContext(typeof(GardenContext))]
    [Migration("20220323191444_DbMig01")]
    partial class DbMig01
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Inplanticular.Garden_Service.Core.Models.Garden", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<DateTime>("DateOfCreation")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Gardens");
                });

            modelBuilder.Entity("Inplanticular.Garden_Service.Core.Models.Plant", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("GardenId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PlantDataId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("GardenId");

                    b.HasIndex("PlantDataId");

                    b.ToTable("Plants");
                });

            modelBuilder.Entity("Inplanticular.Garden_Service.Core.Models.PlantData", b =>
                {
                    b.Property<string>("BotanicalName")
                        .HasColumnType("text");

                    b.Property<double>("AvgFruitWeight")
                        .HasColumnType("double precision");

                    b.Property<int>("DaysToMature")
                        .HasColumnType("integer");

                    b.Property<string>("FriendlyName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<double>("GrowthPerDay")
                        .HasColumnType("double precision");

                    b.HasKey("BotanicalName");

                    b.ToTable("PlantData");
                });

            modelBuilder.Entity("Inplanticular.Garden_Service.Core.Models.Plant", b =>
                {
                    b.HasOne("Inplanticular.Garden_Service.Core.Models.Garden", null)
                        .WithMany("Plants")
                        .HasForeignKey("GardenId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Inplanticular.Garden_Service.Core.Models.PlantData", "PlantData")
                        .WithMany()
                        .HasForeignKey("PlantDataId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PlantData");
                });

            modelBuilder.Entity("Inplanticular.Garden_Service.Core.Models.Garden", b =>
                {
                    b.Navigation("Plants");
                });
#pragma warning restore 612, 618
        }
    }
}
