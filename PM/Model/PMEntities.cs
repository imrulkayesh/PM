using PM.ModelsV;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace PM.Model
{
    public class PMEntities : DbContext
    {
        public PMEntities() : base("AioOraDb_1st")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // throw new UnintentionalCodeFirstException();
            modelBuilder.HasDefaultSchema("PM");
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Ignore<VW_ASET_BY_USERS>();
            //modelBuilder.Ignore<VW_ASSETUSERMANAGERVM>();
            //modelBuilder.Ignore<VW_BNDL_INSP>();
            //modelBuilder.Ignore<VW_BNDL_JOBC>();
            //modelBuilder.Ignore<VW_DATE_NOTIF>();
            modelBuilder.Ignore<VW_INSURANCE_S1>();
            modelBuilder.Ignore<VW_INSURANCE_S2>();
            modelBuilder.Ignore<VW_IQAMA_NOTIF_V1>();
            modelBuilder.Ignore<VW_IQAMA_NOTIF_V2>();
            modelBuilder.Ignore<VW_KSA_IQUMA_S1>();
            modelBuilder.Ignore<VW_KSA_IQUMA_S2>();
            modelBuilder.Ignore<VW_NOTIF_MAIL>();
            modelBuilder.Ignore<VW_NOTIF_MAIL_METER>();
            modelBuilder.Ignore<VW_OWNR_BY_USERS>();
            //modelBuilder.Ignore<VW_PMENU_CMENU>();
            //modelBuilder.Ignore<VW_RPT_ASET_EXP>();
            //modelBuilder.Ignore<VW_RPT_ASET_USER>();
            //modelBuilder.Ignore<VW_RPT_BILL>();
            //modelBuilder.Ignore<VW_RPT_BILL_SUM>();
            //modelBuilder.Ignore<VW_RPT_INSP_SUM>();
            //modelBuilder.Ignore<VW_RPT_INSURANCE>();
            //modelBuilder.Ignore<VW_RPT_JOB_SUM>();
            modelBuilder.Ignore<VW_RPT_JOBD_S1>();
            //modelBuilder.Ignore<VW_RPT_POLICE_CASE>();
            //modelBuilder.Ignore<VW_USER_ASSET_ID_STRING>();
            //modelBuilder.Ignore<VW_USER_ASSET_LIST>();
            //modelBuilder.Entity<VW_USER_ASSET_LIST>().ToTable("VW_USER_ASSET_LIST");
        }

        public virtual DbSet<A_ACCE_ITEMS> A_ACCE_ITEMS { get; set; }
        public virtual DbSet<A_AREA> A_AREA { get; set; }
        public virtual DbSet<A_ASET_ACCE> A_ASET_ACCE { get; set; }
        public virtual DbSet<A_ASET_APP> A_ASET_APP { get; set; }
        public virtual DbSet<A_ASET_DATA> A_ASET_DATA { get; set; }
        public virtual DbSet<A_ASET_USER> A_ASET_USER { get; set; }
        public virtual DbSet<A_ASET_USER_IMG> A_ASET_USER_IMG { get; set; }
        public virtual DbSet<A_ASET_USR_ACE> A_ASET_USR_ACE { get; set; }
        public virtual DbSet<A_BILL> A_BILL { get; set; }
        public virtual DbSet<A_BODY> A_BODY { get; set; }
        public virtual DbSet<A_BRAND> A_BRAND { get; set; }
        public virtual DbSet<A_BUSINESS> A_BUSINESS { get; set; }
        public virtual DbSet<A_COLOR> A_COLOR { get; set; }
        public virtual DbSet<A_COMPANY> A_COMPANY { get; set; }
        public virtual DbSet<A_COUNTRY> A_COUNTRY { get; set; }
        public virtual DbSet<A_DEPARTMENT> A_DEPARTMENT { get; set; }
        public virtual DbSet<A_DISTRICT> A_DISTRICT { get; set; }
        public virtual DbSet<A_EMAIL_LOG> A_EMAIL_LOG { get; set; }
        public virtual DbSet<A_FUEL> A_FUEL { get; set; }
        public virtual DbSet<A_GROUP> A_GROUP { get; set; }
        public virtual DbSet<A_INSP_JOB_IMG> A_INSP_JOB_IMG { get; set; }
        public virtual DbSet<A_INSP_TYPE> A_INSP_TYPE { get; set; }
        public virtual DbSet<A_INSPD> A_INSPD { get; set; }
        public virtual DbSet<A_INSPH> A_INSPH { get; set; }
        public virtual DbSet<A_INSUD> A_INSUD { get; set; }
        public virtual DbSet<A_INSUH> A_INSUH { get; set; }
        public virtual DbSet<A_ITEM_LIST> A_ITEM_LIST { get; set; }
        public virtual DbSet<A_JOBD> A_JOBD { get; set; }
        public virtual DbSet<A_JOBH> A_JOBH { get; set; }
        public virtual DbSet<A_MENUC> A_MENUC { get; set; }
        public virtual DbSet<A_MENUP> A_MENUP { get; set; }
        public virtual DbSet<A_NOTIF_SET> A_NOTIF_SET { get; set; }
        public virtual DbSet<A_OWNR> A_OWNR { get; set; }
        public virtual DbSet<A_OWNR_BILL> A_OWNR_BILL { get; set; }
        public virtual DbSet<A_PLEA> A_PLEA { get; set; }
        public virtual DbSet<A_PLEA_REASON> A_PLEA_REASON { get; set; }
        public virtual DbSet<A_REMINDER> A_REMINDER { get; set; }
        public virtual DbSet<A_REMINDER_IMG> A_REMINDER_IMG { get; set; }
        public virtual DbSet<A_ROLES> A_ROLES { get; set; }
        public virtual DbSet<A_SUMMARY_VIEW> A_SUMMARY_VIEW { get; set; }
        public virtual DbSet<A_TBL_SEQ> A_TBL_SEQ { get; set; }
        public virtual DbSet<A_THANA> A_THANA { get; set; }
        public virtual DbSet<A_USERS> A_USERS { get; set; }
        public virtual DbSet<A_USERS_AREA> A_USERS_AREA { get; set; }
        public virtual DbSet<A_USERS_BUS> A_USERS_BUS { get; set; }
        public virtual DbSet<A_USERS_COUNTRY> A_USERS_COUNTRY { get; set; }
        public virtual DbSet<A_USERS_GROUP> A_USERS_GROUP { get; set; }
        public virtual DbSet<A_USERS_LOGIN> A_USERS_LOGIN { get; set; }
        public virtual DbSet<A_USERS_MENUC> A_USERS_MENUC { get; set; }
        public virtual DbSet<A_USERS_ROLES> A_USERS_ROLES { get; set; }
        public virtual DbSet<A_EMAIL> A_EMAIL { get; set; }
        public virtual DbSet<A_STAFF_TYPE> A_STAFF_TYPE { get; set; }
        public virtual DbSet<INS_INSURANCE_TYPE> INS_INSRANCE_TYPE { get; set; }
        public virtual DbSet<INS_INSURANCE_COMPANY> INS_INSURANCE_COMPANY { get; set; }
        public virtual DbSet<INS_INSURANCE_MST> INS_INSURANCE_MST { get; set; }
        public virtual DbSet<INS_INSURANCE_IMG> INS_INSURANCE_IMG { get; set; }
        public virtual DbSet<INS_KICK_BACK_STATUS> INS_KICK_BACK_STATUS { get; set; }
        public virtual DbSet<INS_CLAIM_TYPE> INS_CLAIM_TYPE { get; set; }
        public virtual DbSet<INS_CLAIM_STATUS> INS_CLAIM_STATUS { get; set; }
        public virtual DbSet<INS_CLAIM> INS_CLAIM { get; set; }
        public virtual DbSet<INS_COMPANY> INS_COMPANY { get; set; }
        public virtual DbSet<INS_CLAIM_RECEIVE> INS_CLAIM_RECEIVE { get; set; }
        public virtual DbSet<LIC_ISSUING_AUTHORITY> LIC_ISSUING_AUTHORITY { get; set; }
        public virtual DbSet<LIC_LICENSE_NAME> LIC_LICENSE_NAME { get; set; }
        public virtual DbSet<LIC_LICENSE_TYPE> LIC_LICENSE_TYPE { get; set; }
        public virtual DbSet<LIC_STATUS> LIC_STATUS { get; set; }
        public virtual DbSet<A_ITEM_MASTER> A_ITEM_MASTER { get; set; }
        public virtual DbSet<LIC_LICENSE> LIC_LICENSE { get; set; }
        public virtual DbSet<LIC_LICENSE_IMG> LIC_LICENSE_IMG { get; set; }
        public virtual DbSet<LIC_LOCATION> LIC_LOCATION { get; set; }
        public virtual DbSet<LIC_LICENSE_DOCUMENT> LIC_LICENSE_DOCUMENT { get; set; }
        public virtual DbSet<AUDIT_AUDITOR> AUDIT_AUDITOR { get; set; }
        public virtual DbSet<AUDIT_TASK_STATUS> AUDIT_TASK_STATUS { get; set; }
        public virtual DbSet<AUDIT_RPT_FINAL_STATUS> AUDIT_RPT_FINAL_STATUS { get; set; }
        public virtual DbSet<AUDIT_TASK> AUDIT_TASK { get; set; }
        public virtual DbSet<C_LOCATION> C_LOCATION { get; set; }
        public virtual DbSet<C_COMPANY_GROUP> C_COMPANY_GROUP { get; set; }
        public virtual DbSet<C_COMPANY> C_COMPANY { get; set; }
        public virtual DbSet<C_DEPARTMENT> C_DEPARTMENT { get; set; }
        public virtual DbSet<AUDIT_TASK_CATEGORY_TYPE> AUDIT_TASK_CATEGORY_TYPE { get; set; }
        public virtual DbSet<AUDIT_TASK_COMMUNICATION_BY> AUDIT_TASK_COMMUNICATION_BY { get; set; }
        public virtual DbSet<AUDIT_TASK_ASSIGN_BY> AUDIT_TASK_ASSIGN_BY { get; set; }
        public virtual DbSet<AUDIT_MANAGEMENT_SOURCE> AUDIT_MANAGEMENT_SOURCE { get; set; }
        public virtual DbSet<AUDIT_TASK_TYPE> AUDIT_TASK_TYPE { get; set; }
        public virtual DbSet<AUDIT_TASK_PRIORITY> AUDIT_TASK_PRIORITY { get; set; }
        public virtual DbSet<AUDIT_TASK_CATEGORY> AUDIT_TASK_CATEGORY { get; set; }
        public virtual DbSet<AUDIT_TASK_IMG> AUDIT_TASK_IMG { get; set; }
        public virtual DbSet<LIC_COMPANY> LIC_COMPANY { get; set; }
        public virtual DbSet<AUDIT_REPORT_STATUS> AUDIT_REPORT_STATUS { get; set; }
        public virtual DbSet<AUDIT_REPORT_ACTION> AUDIT_REPORT_ACTION { get; set; }
        public virtual DbSet<AUDIT_REPORT> AUDIT_REPORT { get; set; }
        public virtual DbSet<AUDIT_REPORT_DETAIL> AUDIT_REPORT_DETAIL { get; set; }
        public virtual DbSet<AUDIT_RPT_SAVING_NAME> AUDIT_RPT_SAVING_NAME { get; set; }
        public virtual DbSet<AUDIT_SAVING_TYPE> AUDIT_SAVING_TYPE { get; set; }
        public virtual DbSet<AUDIT_REPORT_IMG> AUDIT_REPORT_IMG { get; set; }
        public virtual DbSet<AUDIT_RISK_TYPE> AUDIT_RISK_TYPE { get; set; }
        public virtual DbSet<AUDIT_FOLLOWUP_REQUIREMENT> AUDIT_FOLLOWUP_REQUIREMENT { get; set; }
        public virtual DbSet<AUDIT_RISK_MITIGATION> AUDIT_RISK_MITIGATION { get; set; }
        public virtual DbSet<AUDIT_REPORT_FOLLOWUP> AUDIT_REPORT_FOLLOWUP { get; set; }
        public virtual DbSet<AUDIT_MANAGEMENT_ACTION> AUDIT_MANAGEMENT_ACTION { get; set; }
        public virtual DbSet<AUDIT_FOLLOWUP_TYPE> AUDIT_FOLLOWUP_TYPE { get; set; }
        public virtual DbSet<AUDIT_LOG> AUDIT_LOG { get; set; }
        public virtual DbSet<LIC_LICENSE_RENEWAL_HISTORY> LIC_LICENSE_RENEWAL_HISTORY { get; set; }
        public virtual DbSet<AUDIT_REPORT_ASSIST_BY> AUDIT_REPORT_ASSIST_BY { get; set; }
        





























        public virtual DbSet<VW_ASET_BY_USERS> VW_ASET_BY_USERS { get; set; }
        public virtual DbSet<VW_ASSETUSERMANAGERVM> VW_ASSETUSERMANAGERVM { get; set; }
        public virtual DbSet<VW_BNDL_INSP> VW_BNDL_INSP { get; set; }
        public virtual DbSet<VW_BNDL_JOBC> VW_BNDL_JOBC { get; set; }
        public virtual DbSet<VW_DATE_NOTIF> VW_DATE_NOTIF { get; set; }
        public virtual DbSet<VW_INSURANCE_S1> VW_INSURANCE_S1 { get; set; }
        public virtual DbSet<VW_INSURANCE_S2> VW_INSURANCE_S2 { get; set; }
        public virtual DbSet<VW_IQAMA_NOTIF_V1> VW_IQAMA_NOTIF_V1 { get; set; }
        public virtual DbSet<VW_IQAMA_NOTIF_V2> VW_IQAMA_NOTIF_V2 { get; set; }
        public virtual DbSet<VW_KSA_IQUMA_S1> VW_KSA_IQUMA_S1 { get; set; }
        public virtual DbSet<VW_KSA_IQUMA_S2> VW_KSA_IQUMA_S2 { get; set; }
        public virtual DbSet<VW_NOTIF_MAIL> VW_NOTIF_MAIL { get; set; }
        public virtual DbSet<VW_NOTIF_MAIL_METER> VW_NOTIF_MAIL_METER { get; set; }
        public virtual DbSet<VW_OWNR_BY_USERS> VW_OWNR_BY_USERS { get; set; }
        public virtual DbSet<VW_PMENU_CMENU> VW_PMENU_CMENU { get; set; }
        public virtual DbSet<VW_RPT_ASET_EXP> VW_RPT_ASET_EXP { get; set; }
        public virtual DbSet<VW_RPT_ASET_USER> VW_RPT_ASET_USER { get; set; }
        public virtual DbSet<VW_RPT_BILL> VW_RPT_BILL { get; set; }
        public virtual DbSet<VW_RPT_BILL_SUM> VW_RPT_BILL_SUM { get; set; }
        public virtual DbSet<VW_RPT_INSP_SUM> VW_RPT_INSP_SUM { get; set; }
        public virtual DbSet<VW_RPT_INSURANCE> VW_RPT_INSURANCE { get; set; }
        public virtual DbSet<VW_RPT_JOB_SUM> VW_RPT_JOB_SUM { get; set; }
        public virtual DbSet<VW_RPT_JOBD_S1> VW_RPT_JOBD_S1 { get; set; }
        public virtual DbSet<VW_RPT_POLICE_CASE> VW_RPT_POLICE_CASE { get; set; }
        public virtual DbSet<VW_USER_ASSET_ID_STRING> VW_USER_ASSET_ID_STRING { get; set; }
        public virtual DbSet<VW_USER_ASSET_LIST> VW_USER_ASSET_LIST { get; set; }
        public virtual DbSet<VehicleVM> VehicleVM { get; set; }

        public virtual DbSet<LicenseVM> LicenseVM { get; set; }
    }
}
