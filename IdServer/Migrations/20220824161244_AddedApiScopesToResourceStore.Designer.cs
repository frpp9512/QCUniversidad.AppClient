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
    [Migration("20220824161244_AddedApiScopesToResourceStore")]
    partial class AddedApiScopesToResourceStore
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.8");

            modelBuilder.Entity("IdServer.Data.Models.StoredApiResource", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("DisplayName")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("ApiResources");
                });

            modelBuilder.Entity("IdServer.Data.Models.StoredApiResourceScope", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Scope")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("StoredApiResourceId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("StoredApiResourceId");

                    b.ToTable("ApiResourceScopes");
                });

            modelBuilder.Entity("IdServer.Data.Models.StoredApiResourceUserClaim", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("ApiResourceId")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("StoredApiResourceId")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserClaim")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("StoredApiResourceId");

                    b.ToTable("apiResourceUserClaims");
                });

            modelBuilder.Entity("IdServer.Data.Models.StoredApiScope", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("DisplayName")
                        .HasColumnType("TEXT");

                    b.Property<bool>("Emphasize")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<bool>("Required")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("ApiScopes");
                });

            modelBuilder.Entity("IdServer.Data.Models.StoredApiScopeUserClaim", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("StoredApiScopeId")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserClaim")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("StoredApiScopeId");

                    b.ToTable("storedApiScopeUserClaims");
                });

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

                    b.ToTable("ClientAllowedScopes");
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

                    b.ToTable("ClientGrantTypes");
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

            modelBuilder.Entity("IdServer.Data.Models.StoredIdentityResource", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("DisplayName")
                        .HasColumnType("TEXT");

                    b.Property<bool>("Emphasize")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<bool>("Required")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("IdentityResources");
                });

            modelBuilder.Entity("IdServer.Data.Models.StoredIdentityResourceUserClaim", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("StoredIdentityResourceId")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserClaim")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("StoredIdentityResourceId");

                    b.ToTable("IdentityResourceUserClaims");
                });

            modelBuilder.Entity("IdServer.Data.Models.StoredApiResourceScope", b =>
                {
                    b.HasOne("IdServer.Data.Models.StoredApiResource", "StoredApiResource")
                        .WithMany("Scopes")
                        .HasForeignKey("StoredApiResourceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("StoredApiResource");
                });

            modelBuilder.Entity("IdServer.Data.Models.StoredApiResourceUserClaim", b =>
                {
                    b.HasOne("IdServer.Data.Models.StoredApiResource", "StoredApiResource")
                        .WithMany("UserClaims")
                        .HasForeignKey("StoredApiResourceId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("StoredApiResource");
                });

            modelBuilder.Entity("IdServer.Data.Models.StoredApiScopeUserClaim", b =>
                {
                    b.HasOne("IdServer.Data.Models.StoredApiScope", "StoredApiScope")
                        .WithMany("UserClaims")
                        .HasForeignKey("StoredApiScopeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("StoredApiScope");
                });

            modelBuilder.Entity("IdServer.Data.Models.StoredClientAllowedScope", b =>
                {
                    b.HasOne("IdServer.Data.Models.StoredClient", "Client")
                        .WithMany("AllowedScopes")
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Client");
                });

            modelBuilder.Entity("IdServer.Data.Models.StoredClientGrantType", b =>
                {
                    b.HasOne("IdServer.Data.Models.StoredClient", "Client")
                        .WithMany("GrantTypes")
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade)
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

            modelBuilder.Entity("IdServer.Data.Models.StoredIdentityResourceUserClaim", b =>
                {
                    b.HasOne("IdServer.Data.Models.StoredIdentityResource", "StoredIdentityResource")
                        .WithMany("UserClaims")
                        .HasForeignKey("StoredIdentityResourceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("StoredIdentityResource");
                });

            modelBuilder.Entity("IdServer.Data.Models.StoredApiResource", b =>
                {
                    b.Navigation("Scopes");

                    b.Navigation("UserClaims");
                });

            modelBuilder.Entity("IdServer.Data.Models.StoredApiScope", b =>
                {
                    b.Navigation("UserClaims");
                });

            modelBuilder.Entity("IdServer.Data.Models.StoredClient", b =>
                {
                    b.Navigation("AllowedScopes");

                    b.Navigation("GrantTypes");

                    b.Navigation("Secrets");
                });

            modelBuilder.Entity("IdServer.Data.Models.StoredIdentityResource", b =>
                {
                    b.Navigation("UserClaims");
                });
#pragma warning restore 612, 618
        }
    }
}
