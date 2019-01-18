﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Model;

namespace Model.Migrations
{
    [DbContext(typeof(DataBaseContext))]
    partial class DataBaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.0-rtm-35687");

            modelBuilder.Entity("Model.Characteristic", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<int>("ProjectId");

                    b.HasKey("Id");

                    b.HasAlternateKey("Name");

                    b.HasIndex("ProjectId");

                    b.ToTable("Characteristics");
                });

            modelBuilder.Entity("Model.CharacteristicValue", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CharacteristicId");

                    b.Property<int>("CompanyId");

                    b.Property<string>("Value");

                    b.HasKey("Id");

                    b.HasIndex("CharacteristicId");

                    b.HasIndex("CompanyId");

                    b.ToTable("CharacteristicValues");
                });

            modelBuilder.Entity("Model.Company", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Date");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Companies");
                });

            modelBuilder.Entity("Model.Metric", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<int>("ProjectId");

                    b.Property<double>("Weight");

                    b.HasKey("Id");

                    b.HasAlternateKey("Name");

                    b.HasIndex("ProjectId");

                    b.ToTable("Metrics");
                });

            modelBuilder.Entity("Model.MetricToSegmentConditionalProbability", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<double>("ConditionalProbability");

                    b.Property<int>("MetricId");

                    b.Property<int>("SegmentId");

                    b.HasKey("Id");

                    b.HasIndex("MetricId");

                    b.HasIndex("SegmentId");

                    b.ToTable("Probabilities");
                });

            modelBuilder.Entity("Model.MetricValue", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CompanyId");

                    b.Property<int>("MetricId");

                    b.Property<double>("Value");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.HasIndex("MetricId");

                    b.ToTable("MetricValues");
                });

            modelBuilder.Entity("Model.Project", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<DateTime>("ProjectDate");

                    b.HasKey("Id");

                    b.HasAlternateKey("Name");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("Model.Segment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<double>("Probability");

                    b.Property<int>("ProjectId");

                    b.HasKey("Id");

                    b.HasAlternateKey("Name");

                    b.HasIndex("ProjectId");

                    b.ToTable("Segments");
                });

            modelBuilder.Entity("Model.Characteristic", b =>
                {
                    b.HasOne("Model.Project", "Project")
                        .WithMany("Characteristics")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Model.CharacteristicValue", b =>
                {
                    b.HasOne("Model.Characteristic", "Characteristic")
                        .WithMany()
                        .HasForeignKey("CharacteristicId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Model.Company", "Company")
                        .WithMany("CharacteristicValues")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Model.Metric", b =>
                {
                    b.HasOne("Model.Project", "Project")
                        .WithMany("Metrics")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Model.MetricToSegmentConditionalProbability", b =>
                {
                    b.HasOne("Model.Metric", "Metric")
                        .WithMany()
                        .HasForeignKey("MetricId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Model.Segment", "Segment")
                        .WithMany()
                        .HasForeignKey("SegmentId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Model.MetricValue", b =>
                {
                    b.HasOne("Model.Company", "Company")
                        .WithMany("MetricValues")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Model.Metric", "Metric")
                        .WithMany()
                        .HasForeignKey("MetricId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Model.Segment", b =>
                {
                    b.HasOne("Model.Project", "Project")
                        .WithMany()
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
