﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using EDIS.Areas.BMED.Models;
using EDIS.Areas.BMED.Models.RepairModels;
using EDIS.Models.Identity;

namespace EDIS.Areas.BMED.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            
        }

        //public DbSet<DocIdStore> DocIdStores { get; set; }
        public DbSet<RepairModel> BMEDRepairs { get; set; }
        public DbSet<RepairDtlModel> BMEDRepairDtls { get; set; }
        public DbSet<RepairFlowModel> BMEDRepairFlows { get; set; }
        public DbSet<AppUserModel> AppUsers { get; set; }
        public DbSet<AppRoleModel> AppRoles { get; set; }
        public DbSet<DepartmentModel> Departments { get; set; }
        public DbSet<UsersInRolesModel> UsersInRoles { get; set; }
        public DbSet<AssetModel> BMEDAssets { get; set; }
        public DbSet<DealStatusModel> BMEDDealStatuses { get; set; }
        public DbSet<FailFactorModel> BMEDFailFactors { get; set; }
        public DbSet<RepairEmpModel> BMEDRepairEmps { get; set; }
        public DbSet<RepairCostModel> BMEDRepairCosts { get; set; }
        public DbSet<TicketModel> BMEDTickets { get; set; }
        public DbSet<TicketDtlModel> BMEDTicketDtls { get; set; }
        public DbSet<Ticket_seq_tmpModel> BMEDTicket_seq_tmps { get; set; }
        public DbSet<AttainFileModel> BMEDAttainFiles { get; set; }
        public DbSet<DeptStockModel> BMEDDeptStocks { get; set; }
        public DbSet<VendorModel> BMEDVendors { get; set; }
        //public DbSet<ExternalUserModel> ExternalUsers { get; set; }
        public DbSet<EngsInAssetsModel> BMEDEngsInAssets { get; set; }
        public DbSet<EngSubStaff> EngSubStaff { get; set; }
        public DbSet<ScrapAssetModel> ScrapAssets { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<AppUserModel>().HasKey(c => c.Id);
            builder.Entity<AppRoleModel>().HasKey(c => c.RoleId);
            builder.Entity<UsersInRolesModel>().HasKey(c => new {c.UserId, c.RoleId});
            builder.Entity<RepairFlowModel>().HasKey(c => new { c.DocId, c.StepId });
            //builder.Entity<DocIdStore>().HasKey(c => new { c.DocType, c.DocId});
            builder.Entity<RepairEmpModel>().HasKey(c => new { c.DocId, c.UserId });
            builder.Entity<RepairCostModel>().HasKey(c => new { c.DocId, c.SeqNo });
            builder.Entity<TicketDtlModel>().HasKey(c => new { c.TicketDtlNo, c.SeqNo });
            builder.Entity<TicketModel>().HasKey(c => c.TicketNo);
            builder.Entity<Ticket_seq_tmpModel>().HasKey(c => c.YYYMM);
            builder.Entity<AttainFileModel>().HasKey(c => new { c.DocType, c.DocId, c.SeqNo });
            builder.Entity<DeptStockModel>().HasKey(c => new { c.StockId });
            builder.Entity<VendorModel>().HasKey(c => new { c.VendorId });
            //builder.Entity<ExternalUserModel>().HasKey(c => new { c.Id });
            builder.Entity<EngsInAssetsModel>().HasKey(c => new { c.EngId, c.DeviceNo });
            builder.Entity<EngSubStaff>().HasKey(c => new { c.EngId });
            builder.Entity<ScrapAssetModel>().HasKey(c => new { c.DocId, c.DeviceNo, c.AssetNo });
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
