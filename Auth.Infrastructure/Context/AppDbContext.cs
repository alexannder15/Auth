﻿using Auth.Domain.Models;
using Auth.Domain.Models.Common;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Auth.Infrastructure.Context;

public class AppDbContext : IdentityDbContext<User, Role, int, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
{
    //private readonly IUserContextService _userContextService;

    public AppDbContext(
        DbContextOptions<AppDbContext> options
        //IUserContextService userContextService
        ) : base(options)
    {
        //_userContextService = userContextService;
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<User>(b =>
        {
            b.ToTable("Users");
            b.Property(u => u.FirstName).HasMaxLength(128);
            b.Property(u => u.LastName).HasMaxLength(128);
            b.Property(u => u.UserName).HasMaxLength(128);
            b.Property(u => u.NormalizedUserName).HasMaxLength(128);
            b.Property(u => u.Email).HasMaxLength(128);
            b.Property(u => u.NormalizedEmail).HasMaxLength(128);

            // Each User can have many UserClaims
            b.HasMany(e => e.Claims)
                .WithOne(e => e.User)
                .HasForeignKey(uc => uc.UserId)
                .IsRequired();

            // Each User can have many UserLogins
            b.HasMany(e => e.Logins)
                .WithOne(e => e.User)
                .HasForeignKey(ul => ul.UserId)
                .IsRequired();

            // Each User can have many UserTokens
            b.HasMany(e => e.Tokens)
                .WithOne(e => e.User)
                .HasForeignKey(ut => ut.UserId)
                .IsRequired();

            // Each User can have many entries in the UserRole join table
            b.HasMany(e => e.UserRoles)
                .WithOne(e => e.User)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();

            b.HasOne(e => e.CreatedBy)
            .WithMany()
            .HasForeignKey(e => e.CreatedById);

            b.HasOne(e => e.UpdatedBy)
            .WithMany()
            .HasForeignKey(e => e.UpdatedById);
        });

        builder.Entity<Role>(b =>
        {
            b.ToTable("Roles");

            // Each Role can have many entries in the UserRole join table
            b.HasMany(e => e.UserRoles)
                .WithOne(e => e.Role)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();

            // Each Role can have many associated RoleClaims
            b.HasMany(e => e.RoleClaims)
                .WithOne(e => e.Role)
                .HasForeignKey(rc => rc.RoleId)
                .IsRequired();

            b.HasOne(e => e.CreatedBy)
            .WithMany()
            .HasForeignKey(e => e.CreatedById);

            b.HasOne(e => e.UpdatedBy)
            .WithMany()
            .HasForeignKey(e => e.UpdatedById);
        });

        builder.Entity<UserClaim>(b =>
        {
            b.ToTable("UserClaims");

            b.HasOne(e => e.CreatedBy)
            .WithMany()
            .HasForeignKey(e => e.CreatedById);

            b.HasOne(e => e.UpdatedBy)
            .WithMany()
            .HasForeignKey(e => e.UpdatedById);
        });

        builder.Entity<UserLogin>(b =>
        {
            b.ToTable("UserLogins");

            b.HasOne(e => e.CreatedBy)
            .WithMany()
            .HasForeignKey(e => e.CreatedById);

            b.HasOne(e => e.UpdatedBy)
            .WithMany()
            .HasForeignKey(e => e.UpdatedById);
        });

        builder.Entity<RoleClaim>(b =>
        {
            b.ToTable("RoleClaims");

            b.HasOne(e => e.CreatedBy)
            .WithMany()
            .HasForeignKey(e => e.CreatedById);

            b.HasOne(e => e.UpdatedBy)
            .WithMany()
            .HasForeignKey(e => e.UpdatedById);
        });

        builder.Entity<UserRole>(b =>
        {
            b.ToTable("UserRoles");

            b.HasOne(e => e.CreatedBy)
            .WithMany()
            .HasForeignKey(e => e.CreatedById);

            b.HasOne(e => e.UpdatedBy)
            .WithMany()
            .HasForeignKey(e => e.UpdatedById);
        });

        builder.Entity<UserToken>(b =>
        {
            b.ToTable("UserTokens");

            b.HasOne(e => e.CreatedBy)
            .WithMany()
            .HasForeignKey(e => e.CreatedById);

            b.HasOne(e => e.UpdatedBy)
            .WithMany()
            .HasForeignKey(e => e.UpdatedById);
        });
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
    {
        // Get all the entities that inherit from AuditableEntity
        // and have a state of Added or Modified
        var entries = ChangeTracker
            .Entries()
            .Where(e => e.Entity is IAuditable && (
                    e.State == EntityState.Added
                    || e.State == EntityState.Modified))
            ;

        User user = new User();
        user.Id = 1;

        // For each entity we will set the Audit properties
        foreach (var entityEntry in entries)
        {
            // If the entity state is Added let's set
            // the CreatedOn and CreatedById properties
            if (entityEntry.State == EntityState.Added)
            {
                ((IAuditable)entityEntry.Entity).CreatedOn = DateTime.UtcNow;
                ((IAuditable)entityEntry.Entity).CreatedById = user.Id;
            }


            // If the entity state is Modified let's set
            // the UpdatedOn and UpdatedById properties
            if (entityEntry.State == EntityState.Modified)
            {
                ((IAuditable)entityEntry.Entity).UpdatedOn = DateTime.UtcNow;
                ((IAuditable)entityEntry.Entity).UpdatedById = user.Id;
            }
        }

        // After we set all the needed properties
        // we call the base implementation of SaveChangesAsync
        // to actually save our entities in the database
        return await base.SaveChangesAsync(cancellationToken);
    }


}
