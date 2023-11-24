using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepoAndService
{
    public class MainDBContext : DbContext
    {
        public MainDBContext(DbContextOptions options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<UserType> UserTypes { get; set; }
        public DbSet<CustomerItem> CustomerItem { get; set; }
        public DbSet<ItemSupplier> ItemSupplier { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Catagory> Catagories { get; set; }
        public DbSet<ItemImages> ItemImages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasOne(d => d.UserType)
                .WithMany(d => d.Users)
                .HasForeignKey(d => d.UsertypeId)
                .IsRequired();

            modelBuilder.Entity<ItemSupplier>()
                .HasOne(d => d.User)
                .WithMany(d => d.ItemSuppliers)
                .HasForeignKey(d => d.UserId)
                .IsRequired();

            modelBuilder.Entity<CustomerItem>()
                .HasOne(d => d.User)
                .WithMany(d => d.CustomerItems)
                .HasForeignKey(d => d.UserId)
                .IsRequired();

            modelBuilder.Entity<ItemSupplier>()
                .HasOne(d=>d.Item)
                .WithOne(d=>d.ItemSupplier)
                .HasForeignKey<ItemSupplier>(d => d.ItemId)
                .IsRequired();

            modelBuilder.Entity<CustomerItem>()
                .HasOne(d => d.Item)
                .WithOne(d => d.CustomerItem)
                .HasForeignKey<CustomerItem>(d => d.ItemId)
                .IsRequired();

            modelBuilder.Entity<ItemImages>()
                .HasOne(d => d.Item)
                .WithMany(d => d.ItemImages)
                .HasForeignKey(d => d.ItemId)
                .IsRequired();

            modelBuilder.Entity<Catagory>()
                .HasMany(d=>d.Items)
                .WithOne(d=>d.Catagory)
                .HasForeignKey(d=>d.CatagoryId)
                .IsRequired();
                
        }

    }
}