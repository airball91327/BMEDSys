﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using EDIS.Areas.BMED.Models;
using EDIS.Areas.BMED.Models.RepairModels;
using EDIS.Areas.BMED.Models.KeepModels;
using EDIS.Areas.BMED.Models.DeliveryModels;
using EDIS.Models.Identity;
using EDIS.Areas.BMED.Models.BuyEvaluateModels;
using EDIS.Models;

namespace EDIS.Areas.BMED.Data
{
    public class BMEDDbContext : IdentityDbContext<ApplicationUser>
    {
        public BMEDDbContext(DbContextOptions<BMEDDbContext> options)
            : base(options)
        {
            
        }
        public DbSet<DocIdStore> DocIdStores { get; set; }
        public DbSet<AppUserModel> AppUsers { get; set; }
        public DbSet<AppRoleModel> AppRoles { get; set; }
        public DbSet<DepartmentModel> Departments { get; set; }
        public DbSet<UsersInRolesModel> UsersInRoles { get; set; }
        public DbSet<RepairModel> BMEDRepairs { get; set; }
        public DbSet<RepairDtlModel> BMEDRepairDtls { get; set; }
        public DbSet<RepairFlowModel> BMEDRepairFlows { get; set; }
        public DbSet<AssetModel> BMEDAssets { get; set; }
        public DbSet<DealStatusModel> BMEDDealStatuses { get; set; }
        public DbSet<FailFactorModel> BMEDFailFactors { get; set; }
        public DbSet<RepairEmpModel> BMEDRepairEmps { get; set; }
        public DbSet<RepairCostModel> BMEDRepairCosts { get; set; }
        public DbSet<TicketDtlModel> BMEDTicketDtls { get; set; }
        public DbSet<TicketModel> BMEDTickets { get; set; }
        public DbSet<Ticket_seq_tmpModel> BMEDTicket_seq_tmps { get; set; }
        public DbSet<AttainFileModel> BMEDAttainFiles { get; set; }
        public DbSet<DeptStockModel> BMEDDeptStocks { get; set; }
        public DbSet<VendorModel> BMEDVendors { get; set; }
        //public DbSet<ExternalUserModel> ExternalUsers { get; set; }
        //public DbSet<EngsInAssetsModel> BMEDEngsInAssets { get; set; }
        //public DbSet<EngSubStaff> EngSubStaff { get; set; }
        //public DbSet<ScrapAssetModel> ScrapAssets { get; set; }
        public DbSet<KeepModel> BMEDKeeps { get; set; }
        public DbSet<KeepDtlModel> BMEDKeepDtls { get; set; }
        public DbSet<KeepRecordModel> BMEDKeepRecords { get; set; }
        public DbSet<KeepEmpModel> BMEDKeepEmps { get; set; }
        public DbSet<KeepCostModel> BMEDKeepCosts { get; set; }
        public DbSet<KeepFlowModel> BMEDKeepFlows { get; set; }
        public DbSet<KeepFormatModel> BMEDKeepFormats { get; set; }
        public DbSet<KeepFormatDtlModel> BMEDKeepFormatDtls { get; set; }
        public DbSet<KeepResultModel> BMEDKeepResults { get; set; }
        public DbSet<AssetKeepModel> BMEDAssetKeeps { get; set; }
        public DbSet<DeviceClassCode> BMEDDeviceClassCodes { get; set; }
        public DbSet<ExceptDeviceModel> ExceptDevice { get; set; }

        public DbSet<DeliveryModel> Deliveries { get; set; }
        public DbSet<DelivFlowModel> DelivFlows { get; set; }
        public DbSet<DelivCodeModel> DelivCodes { get; set; }
        public DbSet<AssetFileModel> AssetFiles { get; set; }
        public DbSet<NeedFileModel> NeedFiles { get; set; }
        public DbSet<BudgetModel> Budgets { get; set; }
        public DbSet<BuyEvaluateModel> BuyEvaluates { get; set; }
        public DbSet<BuyFlowModel> BuyFlows { get; set; }
        public DbSet<BuySFlowModel> BuySFlows { get; set; }
        public DbSet<BuyVendorModel> BuyVendors { get; set; }
        public DbQuery<UnSignListVModel> UnSignListVModelQuery { get; set; }
        public DbSet<EngsInDptsModel> EngsInDpts { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<AppUserModel>().HasKey(c => c.Id);
            builder.Entity<AppRoleModel>().HasKey(c => c.RoleId);
            builder.Entity<DocIdStore>().HasKey(c => new { c.DocType, c.DocId });
            builder.Entity<UsersInRolesModel>().HasKey(c => new {c.UserId, c.RoleId});
            builder.Entity<RepairModel>().HasKey(c => new { c.DocId });
            builder.Entity<RepairDtlModel>().HasKey(c => new { c.DocId });
            builder.Entity<RepairFlowModel>().HasKey(c => new { c.DocId, c.StepId });
            builder.Entity<AssetModel>().HasKey(c => new { c.AssetNo });
            builder.Entity<DealStatusModel>().HasKey(c => new { c.Id });
            builder.Entity<FailFactorModel>().HasKey(c => new { c.Id });
            builder.Entity<RepairEmpModel>().HasKey(c => new { c.DocId, c.UserId });
            builder.Entity<RepairCostModel>().HasKey(c => new { c.DocId, c.SeqNo });
            builder.Entity<TicketDtlModel>().HasKey(c => new { c.TicketDtlNo, c.SeqNo });
            builder.Entity<TicketModel>().HasKey(c => c.TicketNo);
            builder.Entity<Ticket_seq_tmpModel>().HasKey(c => c.YYYMM);
            builder.Entity<AttainFileModel>().HasKey(c => new { c.DocType, c.DocId, c.SeqNo });
            builder.Entity<DeptStockModel>().HasKey(c => new { c.StockId });
            builder.Entity<VendorModel>().HasKey(c => new { c.VendorId });
            //builder.Entity<ExternalUserModel>().HasKey(c => new { c.Id });
            //builder.Entity<EngsInAssetsModel>().HasKey(c => new { c.EngId, c.AssetNo });
            //builder.Entity<EngSubStaff>().HasKey(c => new { c.EngId });
            //builder.Entity<ScrapAssetModel>().HasKey(c => new { c.DocId, c.DeviceNo, c.AssetNo });
            builder.Entity<KeepModel>().HasKey(c => new { c.DocId });
            builder.Entity<KeepDtlModel>().HasKey(c => new { c.DocId });
            builder.Entity<KeepRecordModel>().HasKey(c => new { c.DocId, c.FormatId, c.Sno });
            builder.Entity<KeepEmpModel>().HasKey(c => new { c.DocId, c.UserId });
            builder.Entity<KeepCostModel>().HasKey(c => new { c.DocId, c.SeqNo });
            builder.Entity<KeepFlowModel>().HasKey(c => new { c.DocId, c.StepId });
            builder.Entity<KeepFormatModel>().HasKey(c => new { c.FormatId });
            builder.Entity<KeepFormatDtlModel>().HasKey(c => new { c.FormatId, c.Sno });
            builder.Entity<KeepResultModel>().HasKey(c => new { c.Id });
            builder.Entity<AssetKeepModel>().HasKey(c => new { c.AssetNo });
            builder.Entity<DeviceClassCode>().HasKey(c => new { c.M_code });
            builder.Entity<ExceptDeviceModel>().HasKey(c => new { c.AssetNo });

            builder.Entity<DeliveryModel>().HasKey(c => new { c.DocId });
            builder.Entity<DelivFlowModel>().HasKey(c => new { c.DocId, c.StepId });
            builder.Entity<DelivCodeModel>().HasKey(c => new { c.Code });
            builder.Entity<AssetFileModel>().HasKey(c => new { c.AssetNo, c.SeqNo, c.Fid });
            builder.Entity<NeedFileModel>().HasKey(c => new { c.SeqNo });
            builder.Entity<BudgetModel>().HasKey(c => new { c.DocId });
            builder.Entity<BuyEvaluateModel>().HasKey(c => new { c.DocId });
            builder.Entity<BuyFlowModel>().HasKey(c => new { c.DocId, c.StepId });
            builder.Entity<BuySFlowModel>().HasKey(c => new { c.DocId, c.StepId, c.DocSid });
            builder.Entity<BuyVendorModel>().HasKey(c => new { c.DocId, c.VendorNo });
            builder.Entity<EngsInDptsModel>().HasKey(c => new { c.AccDptId, c.EngId });

            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
