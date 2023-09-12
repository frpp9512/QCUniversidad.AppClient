﻿// <auto-generated />
using System;
using IdServer.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace IdServer.Migrations
{
    [DbContext(typeof(IdServerDataContext))]
    [Migration("20220824142248_CreatingDatabase")]
    partial class CreatingDatabase
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.8");

            modelBuilder.Entity("IdServer.Data.Models.StoredClient", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("ClientId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Clients");
                });

            modelBuilder.Entity("IdServer.Data.Models.StoredClientAllowedScope", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("AllowedScope")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("ClientId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.ToTable("StoredClientAllowedScope");
                });

            modelBuilder.Entity("IdServer.Data.Models.StoredClientGrantType", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("ClientId")
                        .HasColumnType("TEXT");

                    b.Property<string>("GrantType")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.ToTable("StoredClientGrantType");
                });

            modelBuilder.Entity("IdServer.Data.Models.StoredClientSecret", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("ClientId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("Expiration")
                        .HasColumnType("TEXT");

                    b.Property<string>("Value")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.ToTable("ClientSecrets");
                });

            modelBuilder.Entity("IdServer.Data.Models.StoredClientAllowedScope", b =>
                {
                    b.HasOne("IdServer.Data.Models.StoredClient", "Client")
                        .WithMany("AllowedScopes")
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.Navigation("Client");
                });

            modelBuilder.Entity("IdServer.Data.Models.StoredClientGrantType", b =>
                {
                    b.HasOne("IdServer.Data.Models.StoredClient", "Client")
                        .WithMany("GrantTypes")
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.Navigation("Client");
                });

            modelBuilder.Entity("IdServer.Data.Models.StoredClientSecret", b =>
                {
                    b.HasOne("IdServer.Data.Models.StoredClient", "Client")
                        .WithMany("Secrets")
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Client");
                });

            modelBuilder.Entity("IdServer.Data.Models.StoredClient", b =>
                {
                    b.Navigation("AllowedScopes");

                    b.Navigation("GrantTypes");

                    b.Navigation("Secrets");
                });
#pragma warning restore 612, 618
        }
    }
}