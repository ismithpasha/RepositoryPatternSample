using RepositoryPatternSample.Entities.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPatternSample.Entities.Data
{
	public class ApplicationDbContext : IdentityDbContext<ApplicationUser, Role, int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }


        public DbSet<UserToken> UserTokens { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<UserType> UserTypes { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<RoleMenuPermission> RoleMenuPermissions { get; set; }
        public DbSet<ForgetPasswordToken> ForgetPasswordTokens { get; set; }

         

        
         
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            

            //Set Default Value "GETDATE()" For Every Model That Contains CreatedAt
            //Set Default Value "GETUTCDATE()" GMT Time For Every Model That Contains CreatedAt
            var entityTypesWithCreatedAt = builder.Model.GetEntityTypes().Where(t => t.ClrType.GetProperties().Any(p => p.Name == "CreatedAt"));
            foreach (var entityType in entityTypesWithCreatedAt)
            {
                foreach (var property in entityType.GetProperties().Where(p => p.Name == "CreatedAt" && p.ClrType == typeof(DateTime)))
                {
                    property.SetDefaultValueSql("GETUTCDATE()");
                }
            }

            //Set Default Value "1" For Every Model That Contains StatusId
            var entityTypesWithStatusId = builder.Model.GetEntityTypes().Where(t => t.ClrType.GetProperties().Any(p => p.Name == "StatusId"));
            foreach (var entityType in entityTypesWithStatusId)
            {
                foreach (var property in entityType.GetProperties().Where(p => p.Name == "StatusId" && p.ClrType == typeof(byte)))
                {
                    property.SetDefaultValueSql("1");
                }
            }

            //Set ON CASCADE DELETE to restrict for all model 
            foreach (var relationship in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            builder.Entity<Menu>().Navigation(t => t.RoleMenuPermission).AutoInclude();
            builder.Entity<RoleMenuPermission>().Navigation(t => t.ApplicationRole).AutoInclude();
            builder.Entity<RoleMenuPermission>().Navigation(t => t.Menu).AutoInclude();
        }
    }
}
