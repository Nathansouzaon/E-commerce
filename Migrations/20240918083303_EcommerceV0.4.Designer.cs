﻿// <auto-generated />
using System;
using Ecommerce.data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Ecommerce.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240918083303_EcommerceV0.4")]
    partial class EcommerceV04
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.8");

            modelBuilder.Entity("Ecommerce.Clientes.Cliente", b =>
                {
                    b.Property<Guid>("clienteId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Categoria")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Cpf")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("Identificador")
                        .HasColumnType("TEXT");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("dataVenda")
                        .HasColumnType("TEXT");

                    b.HasKey("clienteId");

                    b.ToTable("Cliente");
                });

            modelBuilder.Entity("Ecommerce.Clientes.Item", b =>
                {
                    b.Property<int>("ProdutoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Descricao")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<decimal>("PrecoUnitario")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Quantidade")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Total")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("clienteId")
                        .HasColumnType("TEXT");

                    b.HasKey("ProdutoId");

                    b.HasIndex("clienteId");

                    b.ToTable("Itens");
                });

            modelBuilder.Entity("Ecommerce.Clientes.Item", b =>
                {
                    b.HasOne("Ecommerce.Clientes.Cliente", null)
                        .WithMany("Itens")
                        .HasForeignKey("clienteId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Ecommerce.Clientes.Cliente", b =>
                {
                    b.Navigation("Itens");
                });
#pragma warning restore 612, 618
        }
    }
}
