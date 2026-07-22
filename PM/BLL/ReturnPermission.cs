using PM.Model;
using PM.ModelsV;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PM.BLL
{
    public class ReturnPermission
    {
        private PMEntities db = new PMEntities();
        public List<string> MyGroupString(string UserId, string UserType)
        {
            var GroupPermission = from r in db.A_USERS_GROUP
                                  join g in db.A_GROUP on r.UG_GROUP equals g.GROUP_ID
                                  where r.UG_USER == UserId && r.ACT == true
                                  select r.UG_GROUP;
            List<string> Group = GroupPermission.ToList();
            return Group;
        }
        public List<string> MyDeptListString(string UserId, string UserType)
        {
            var UserDeptList = from d in db.A_DEPARTMENT
                               join b in db.A_BUSINESS on d.DEPARTMENT_BUSINESS equals b.BUSINESS_ID
                               join p in db.A_USERS_BUS on b.BUSINESS_ID equals p.UB_BUSINESS
                               where d.ACT == true && b.ACT == true && p.ACT == true
                               && p.UB_USER == UserId
                               select d.DEPARTMENT_ID;
            List<string> Department = UserDeptList.ToList();
            return Department;
        }
        public List<string> MyCountryListString(string UserId, string UserType)
        {
            var CountryList = from c in db.A_USERS_COUNTRY
                              where c.ACT == true
                              && c.USER_TEXT == UserId
                              select c.COUNTRY_ID;
            List<string> Country = CountryList.ToList();
            return Country;
        }
        public List<string> MyAssetListString(string UserId, string UserType)
        {
            var Groups = (from r in db.A_USERS_GROUP
                          join g in db.A_GROUP on r.UG_GROUP equals g.GROUP_ID
                          where r.UG_USER == UserId && r.ACT == true
                          select r.UG_GROUP).ToList();
            var Departments = (from d in db.A_DEPARTMENT
                               join b in db.A_BUSINESS on d.DEPARTMENT_BUSINESS equals b.BUSINESS_ID
                               join p in db.A_USERS_BUS on b.BUSINESS_ID equals p.UB_BUSINESS
                               where d.ACT == true && b.ACT == true && p.ACT == true
                               && p.UB_USER == UserId
                               select d.DEPARTMENT_ID).ToList();

            List<string> Country = MyCountryListString(UserId: UserId, UserType: UserType);
            List<string> Areas = new List<string>();

            if (!Country.Any())
            {
                Areas = (from l in db.A_USERS_AREA
                         join m in db.A_AREA on l.UA_AREA equals m.AREA_ID
                         where l.ACT == true && m.ACT == true && l.UA_USER == UserId
                         select m.AREA_ID).ToList();
            }
            else
            {
                Areas = (from a in db.A_AREA
                         where a.ACT == true && Country.Contains(a.A_THANA.A_DISTRICT.A_COUNTRY.COUNTRY_ID)
                         select a.AREA_ID).ToList();
            }
            var AssetList = from a in db.A_ASET_DATA
                            where a.ACT == true
                            && Departments.Contains(a.ASET_DEPARTMENT)
                            && Groups.Contains(a.ASET_GROUP)
                            && Areas.Contains(a.ASET_AREA)
                            select a.ASET_ID;
            List<string> Assets = AssetList.ToList();

            //   var aset = db.VW_USER_ASSET_ID_STRING.Where(u => u.UG_USER == UserId).ToList();
            // List<string> Assets = aset.Select(s=>s.ASET_ID).ToList();
            return Assets;
        }

        public List<AssetManager_ID> MyAssetListString_New(string UserId, string UserType)
        {
            var Groups = (from r in db.A_USERS_GROUP
                          join g in db.A_GROUP on r.UG_GROUP equals g.GROUP_ID
                          where r.UG_USER == UserId && r.ACT == true
                          select r.UG_GROUP).ToList();
            var Departments = (from d in db.A_DEPARTMENT
                               join b in db.A_BUSINESS on d.DEPARTMENT_BUSINESS equals b.BUSINESS_ID
                               join p in db.A_USERS_BUS on b.BUSINESS_ID equals p.UB_BUSINESS
                               where d.ACT == true && b.ACT == true && p.ACT == true
                               && p.UB_USER == UserId
                               select d.DEPARTMENT_ID).ToList();

            List<string> Country = MyCountryListString(UserId: UserId, UserType: UserType);
            List<string> Areas = new List<string>();

            if (!Country.Any())
            {
                Areas = (from l in db.A_USERS_AREA
                         join m in db.A_AREA on l.UA_AREA equals m.AREA_ID
                         where l.ACT == true && m.ACT == true && l.UA_USER == UserId
                         select m.AREA_ID).ToList();
            }
            else
            {
                Areas = (from a in db.A_AREA
                         where a.ACT == true && Country.Contains(a.A_THANA.A_DISTRICT.A_COUNTRY.COUNTRY_ID)
                         select a.AREA_ID).ToList();
            }
            var AssetList = (from a in db.A_ASET_DATA
                             where a.ACT == true
                             && Departments.Contains(a.ASET_DEPARTMENT)
                             && Groups.Contains(a.ASET_GROUP)
                             && Areas.Contains(a.ASET_AREA)
                             select new AssetManager_ID
                             {
                                 AsetID = a.ASET_ID
                             }).ToList();
            // List<string> Assets = AssetList.ToList();
            return AssetList;
        }
        public List<string> MyBusinessString(string UserId)
        {
            var BusinessPermission = from r in db.A_USERS_BUS
                                     where r.ACT == true && r.UB_USER == UserId
                                     select r.UB_BUSINESS;
            List<string> Business = BusinessPermission.ToList();
            return Business;
        }
        public List<string> MyAreaListString(string UserId, string UserType)
        {
            List<string> Country = MyCountryListString(UserId: UserId, UserType: UserType);

            if (!Country.Any())
            {
                var AreaList = from a in db.A_AREA
                               join p in db.A_USERS_AREA on a.AREA_ID equals p.UA_AREA
                               where a.ACT == true && p.ACT == true && p.UA_USER == UserId
                               select a.AREA_ID;
                return AreaList.ToList();
            }
            else
            {
                var AreaList = from a in db.A_AREA
                               where a.ACT == true && Country.Contains(a.A_THANA.A_DISTRICT.A_COUNTRY.COUNTRY_ID)
                               select a.AREA_ID;
                return AreaList.ToList();
            }
        }

        public List<string> FREE_MyAssetListString(string UserId, string UserType)
        {

            var Groups = (from r in db.A_USERS_GROUP
                          join g in db.A_GROUP on r.UG_GROUP equals g.GROUP_ID
                          where r.UG_USER == UserId && r.ACT == true
                          select r.UG_GROUP).ToList();
            var Departments = (from d in db.A_DEPARTMENT
                               join b in db.A_BUSINESS on d.DEPARTMENT_BUSINESS equals b.BUSINESS_ID
                               join p in db.A_USERS_BUS on b.BUSINESS_ID equals p.UB_BUSINESS
                               where d.ACT == true && b.ACT == true && p.ACT == true
                               && p.UB_USER == UserId
                               select d.DEPARTMENT_ID).ToList();

            List<string> Country = MyCountryListString(UserId: UserId, UserType: UserType);
            List<string> Areas = new List<string>();

            if (!Country.Any())
            {
                Areas = (from l in db.A_USERS_AREA
                         join m in db.A_AREA on l.UA_AREA equals m.AREA_ID
                         where l.ACT == true && m.ACT == true && l.UA_USER == UserId
                         select m.AREA_ID).ToList();
            }
            else
            {
                Areas = (from a in db.A_AREA
                         where a.ACT == true && Country.Contains(a.A_THANA.A_DISTRICT.A_COUNTRY.COUNTRY_ID)
                         select a.AREA_ID).ToList();
            }
            var AssetList = from a in db.A_ASET_DATA
                            where a.ACT == true
                            && a.ASET_USED == false
                            && Departments.Contains(a.ASET_DEPARTMENT)
                            && Groups.Contains(a.ASET_GROUP)
                            && Areas.Contains(a.ASET_AREA)
                            select a.ASET_ID;
            List<string> Assets = AssetList.ToList();
            return Assets;
        }
        public List<string> FREE_Retailers(string UserId, string UserType)
        {

            var Departments = (from d in db.A_DEPARTMENT
                               join b in db.A_BUSINESS on d.DEPARTMENT_BUSINESS equals b.BUSINESS_ID
                               join p in db.A_USERS_BUS on b.BUSINESS_ID equals p.UB_BUSINESS
                               where d.ACT == true && b.ACT == true && p.ACT == true
                               && p.UB_USER == UserId
                               select d.DEPARTMENT_ID).ToList();

            List<string> Country = MyCountryListString(UserId: UserId, UserType: UserType);
            List<string> Areas = new List<string>();

            if (!Country.Any())
            {
                Areas = (from l in db.A_USERS_AREA
                         join m in db.A_AREA on l.UA_AREA equals m.AREA_ID
                         where l.ACT == true && m.ACT == true && l.UA_USER == UserId
                         select m.AREA_ID).ToList();
            }
            else
            {
                Areas = (from a in db.A_AREA
                         where a.ACT == true && Country.Contains(a.A_THANA.A_DISTRICT.A_COUNTRY.COUNTRY_ID)
                         select a.AREA_ID).ToList();
            }
            var AssetList = from a in db.A_OWNR
                            where a.ACT == true
                            && a.OWNR_POST == false
                            && a.OWNR_TYPE == "Retailer"
                            && a.OWNR_RESIGN == "N"
                            && Departments.Contains(a.OWNR_DEPT)
                            && Areas.Contains(a.OWNR_AREA)
                            select a.OWNR_ID;
            List<string> Assets = AssetList.ToList();
            return Assets;
        }


        public List<string> MyInspectionForJob(string id)
        {
            var Insp = from i in db.A_INSPH
                       where i.ACT == true
                       && i.INSPH_JOB == "N"
                       && i.INSPH_APP == "Y"
                       && i.A_INSP_TYPE.ITYPE_ID != "B"
                       && i.INSPH_ASET == id
                       select i.INSPH_ID;
            List<string> INSPH_ID = Insp.ToList();
            return INSPH_ID;
        }
        public List<string> MyJobForBill(string id)
        {
            var Jobs = from i in db.A_JOBH
                       where i.ACT == true
                       && i.JOBH_BILL == "N"
                       && i.JOBH_APP == "Y"
                       && i.A_INSP_TYPE.ITYPE_ID != "B"
                       && i.JOBH_ASET == id
                       select i.JOBH_ID;
            List<string> JOBH_ID = Jobs.ToList();
            return JOBH_ID;
        }
        public string InspectionAndJobData(string d, string a, string u)
        {
            var Dept = db.A_DEPARTMENT.Find(d).DEPARTMENT_NAME;
            var Area = db.A_AREA.Find(a).AREA_NAME;
            var User = db.A_OWNR.Find(u).OWNR_USER;
            string rd = string.Format("{0}:{1}-{2}:{3}-{4}:{5}", u, User, d, Dept, a, Area);
            return rd;
        }

        public List<MyGroup> MyGroup(string UserId, string UserType)
        {
            var GroupPermission = from r in db.A_USERS_GROUP
                                  join g in db.A_GROUP on r.UG_GROUP equals g.GROUP_ID
                                  where r.UG_USER == UserId && r.ACT == true
                                  orderby g.GROUP_ID
                                  select new MyGroup
                                  {
                                      Group_ID = r.UG_GROUP,
                                      Group_Name = g.GROUP_NAME
                                  };
            return GroupPermission.ToList();
        }
        public List<MyBusiness> MyBusiness(string UserId)
        {
            var BusinessPermission = from r in db.A_USERS_BUS
                                     where r.ACT == true && r.UB_USER == UserId
                                     orderby r.UB_BUSINESS
                                     select new MyBusiness
                                     {
                                         BUSINESS_ID = r.UB_BUSINESS,
                                         BUSINESS_NAME = r.A_BUSINESS.BUSINESS_NAME
                                     };
            return BusinessPermission.ToList();
        }

        public List<CompanyList> CompanyList()
        {
            var CompanyList = from r in db.INS_COMPANY
                              where r.ACT == true
                              orderby r.COMPANY_NAME
                              select new CompanyList
                              {
                                  INS_COMPANY_ID = r.INS_COMPANY_ID,
                                  COMPANY_NAME = r.COMPANY_NAME
                              };
            return CompanyList.ToList();
        }
        
        public List<LICCompanyList> LICCompanyList()
        {
            var LICCompanyList = from r in db.LIC_COMPANY
                              where r.ACT == true
                              orderby r.COMPANY_NAME
                              select new LICCompanyList
                              {
                                  COMPANY_ID = r.COMPANY_ID,
                                  COMPANY_NAME = r.COMPANY_NAME
                              };
            return LICCompanyList.ToList();
        }

        public List<LocationList> LocationList()
        {
            var LocationList = from r in db.LIC_LOCATION
                               where r.ACT == true
                               orderby r.LOCATION_NAME
                               select new LocationList
                               {
                                   LIC_LOCATION_ID = r.LIC_LOCATION_ID,
                                   LOCATION_NAME = r.LOCATION_NAME
                               };
            return LocationList.ToList();
        }
        public List<UserList> UserList()
        {
            var UserList = from r in db.A_USERS
                           where r.ACT == true
                               orderby r.USER_NAME
                           select new UserList
                           {
                               USER_TEXT = r.USER_TEXT,
                               USER_NAME = r.USER_NAME
                           };
            return UserList.ToList();
        }

        public List<IssuingAuthorityList> IssuingAuthorityList()
        {
            var IssuingAuthorityList = from r in db.LIC_ISSUING_AUTHORITY
                                       where r.ACT == true
                                       orderby r.AUTHORITY_NAME
                                       select new IssuingAuthorityList
                                       {
                                           ISSUING_AUTHORITY_ID = r.ISSUING_AUTHORITY_ID,
                                           AUTHORITY_NAME = r.AUTHORITY_NAME
                                       };
            return IssuingAuthorityList.ToList();
        }
        public List<LicenseTypeList> LicenseTypeList()
        {
            var LicenseTypeList = from r in db.LIC_LICENSE_NAME
                                  where r.ACT == true
                                  orderby r.LICENSE_NAME
                                  select new LicenseTypeList
                                  {
                                      LICENSE_NAME_ID = r.LICENSE_NAME_ID,
                                      LICENSE_NAME = r.LICENSE_NAME
                                  };
            return LicenseTypeList.ToList();
        }
        
        public List<LicenseTypeNameList> LicenseTypeNameList()
        {
            var LicenseTypeNameList = from r in db.LIC_LICENSE_TYPE
                                      where r.ACT == true
                                  orderby r.LICENSE_TYPE_NAME
                                      select new LicenseTypeNameList
                                  {
                                          LICENSE_TYPE_ID = r.LICENSE_TYPE_ID,
                                          LICENSE_TYPE_NAME = r.LICENSE_TYPE_NAME
                                      };
            return LicenseTypeNameList.ToList();
        }

        public List<LicenseStatusList> LicenseStatusList()
        {
            var LicenseStatusList = from r in db.LIC_STATUS
                                    where r.ACT == true
                                    orderby r.LICENSE_STATUS_NAME
                                    select new LicenseStatusList
                                    {
                                        LICENSE_STATUS_ID = r.LICENSE_STATUS_ID,
                                        LICENSE_STATUS_NAME = r.LICENSE_STATUS_NAME
                                    };
            return LicenseStatusList.ToList();
        }

        public List<ItemList> ItemList()
        {
            var ItemList = from r in db.A_ITEM_MASTER
                           where r.ACT == true
                           orderby r.ITEM_NAME
                           select new ItemList
                           {
                               ITEM_ID = r.ITEM_ID,
                               ITEM_NAME = r.ITEM_NAME
                           };
            return ItemList.ToList();
        }

        public List<InsuranceTypeList> InsuranceTypeList()
        {
            var InsuranceTypeList = from a in db.INS_INSRANCE_TYPE
                                    where a.ACT == true
                                    orderby a.INSURANCE_TYPE_ID
                                    select new InsuranceTypeList
                                    {
                                        INSURANCE_TYPE_ID = a.INSURANCE_TYPE_ID,
                                        INSURANCE_TYPE_NAME = a.INSURANCE_TYPE_NAME
                                    };
            return InsuranceTypeList.ToList();
        }

        public List<KickBackStatusList> KickBackStatusList()
        {
            var KickBackStatusList = from a in db.INS_KICK_BACK_STATUS
                                     where a.ACT == true
                                     orderby a.KICK_BACK_STATUS_ID
                                     select new KickBackStatusList
                                     {
                                         KICK_BACK_STATUS_ID = a.KICK_BACK_STATUS_ID,
                                         KICK_BACK_STATUS_NAME = a.KICK_BACK_STATUS_NAME
                                     };
            return KickBackStatusList.ToList();
        }
        public List<ClaimTypeList> ClaimTypeList()
        {
            var ClaimTypeList = from a in db.INS_CLAIM_TYPE
                                where a.ACT == true
                                orderby a.CLAIM_TYPE_ID
                                select new ClaimTypeList
                                {
                                    CLAIM_TYPE_ID = a.CLAIM_TYPE_ID,
                                    CLAIM_NAME = a.CLAIM_NAME
                                };
            return ClaimTypeList.ToList();
        }

        public List<ClaimStatusList> ClaimStatusList()
        {
            var ClaimStatusList = from a in db.INS_CLAIM_STATUS
                                  where a.ACT == true
                                  orderby a.CLAIM_STATUS_ID
                                  select new ClaimStatusList
                                  {
                                      CLAIM_STATUS_ID = a.CLAIM_STATUS_ID,
                                      CLAIM_STATUS_NAME = a.CLAIM_STATUS_NAME
                                  };
            return ClaimStatusList.ToList();
        }
        public List<InsuranceCompanyList> InsuranceCompanyList()
        {
            var InsuranceCompanyList = from a in db.INS_INSURANCE_COMPANY
                                       where a.ACT == true
                                       orderby a.INSURANCE_COMPANY_ID
                                       select new InsuranceCompanyList
                                       {
                                           INSURANCE_COMPANY_ID = a.INSURANCE_COMPANY_ID,
                                           INS_COMPANY_NAME = a.INS_COMPANY_NAME
                                       };
            return InsuranceCompanyList.ToList();
        }

        public List<AuditorList> AuditorList()
        {
            var AuditorList = from r in db.AUDIT_AUDITOR
                              where r.ACT == true
                              orderby r.AUDITOR_NAME
                              select new AuditorList
                              {
                                  AUDITOR_ID = r.AUDITOR_ID,
                                  AUDITOR_NAME = r.AUDITOR_NAME
                              };
            return AuditorList.ToList();
        }

        public List<AuditorListNew> AuditorListNew()
        {
            var AuditorListNew = from r in db.AUDIT_AUDITOR
                                 where r.ACT == true
                                 orderby r.AUDITOR_NAME
                                 select new AuditorListNew
                                 {
                                     AUDITOR_STAFF_ID = r.AUDITOR_STAFF_ID,
                                     AUDITOR_NAME = r.AUDITOR_NAME
                                 };
            return AuditorListNew.ToList();
        }

        public List<RiskTypeList> RiskTypeList()
        {
            var RiskTypeList = from r in db.AUDIT_RISK_TYPE
                              where r.ACT == true
                              orderby r.RISK_TYPE_NAME
                              select new RiskTypeList
                              {
                                  RISK_TYPE_ID = r.RISK_TYPE_ID,
                                  RISK_TYPE_NAME = r.RISK_TYPE_NAME
                              };
            return RiskTypeList.ToList();
        }
        
       public List<FollowUpRequirementList> FollowUpRequirementList()
        {
            var FollowUpRequirementList = from r in db.AUDIT_FOLLOWUP_REQUIREMENT
                               where r.ACT == true
                               orderby r.FOLLOWUP_REQUIREMENT_NAME
                                 select new FollowUpRequirementList
                               {
                                   FOLLOWUP_REQUIREMENT_ID = r.FOLLOWUP_REQUIREMENT_ID,
                                   FOLLOWUP_REQUIREMENT_NAME = r.FOLLOWUP_REQUIREMENT_NAME
                               };
            return FollowUpRequirementList.ToList();
        }

        public List<C_LocationList> C_LocationList()
        {
            var C_LocationList = from r in db.C_LOCATION
                               where r.ACT == true
                              orderby r.LOCATION_NAME
                               select new C_LocationList
                               {
                                  LOCATION_ID = r.LOCATION_ID,
                                  LOCATION_NAME = r.LOCATION_NAME
                              };
            return C_LocationList.ToList();
        }
        
        public List<CompanyGroupList> CompanyGroupList()
        {
            var CompanyGroupList = from r in db.C_COMPANY_GROUP
                                   where r.ACT == true
                                 orderby r.COMPANY_GROUP_NAME
                                   select new CompanyGroupList
                                 {
                                     COMPANY_GROUP_ID = r.COMPANY_GROUP_ID,
                                     COMPANY_GROUP_NAME = r.COMPANY_GROUP_NAME
                                 };
            return CompanyGroupList.ToList();
        }
        
        public List<C_DepartmentList> C_DepartmentList()
        {
            var C_DepartmentList = from r in db.C_DEPARTMENT
                                   where r.ACT == true
                                orderby r.DEPARTMENT_NAME
                                   select new C_DepartmentList
                                {
                                    DEPARTMENT_ID = r.DEPARTMENT_ID,
                                    DEPARTMENT_NAME = r.DEPARTMENT_NAME
                                };
            return C_DepartmentList.ToList();
        }
        
        public List<TaskCategoryTypeList> TaskCategoryTypeList()
        {
            var TaskCategoryTypeList = from r in db.AUDIT_TASK_CATEGORY_TYPE
                                         where r.ACT == true
                                   orderby r.TASK_CATEGORY_TYPE_NAME
                                         select new TaskCategoryTypeList
                                         {
                                       TASK_CATEGORY_TYPE_ID = r.TASK_CATEGORY_TYPE_ID,
                                       TASK_CATEGORY_TYPE_NAME = r.TASK_CATEGORY_TYPE_NAME
                                   };
            return TaskCategoryTypeList.ToList();
        }
        
        public List<RiskMitigationList> RiskMitigationList()
        {
            var RiskMitigationList = from r in db.AUDIT_RISK_MITIGATION
                                     where r.ACT == true
                                       orderby r.RISK_MITIGATION_NAME
                                     select new RiskMitigationList
                                       {
                                           RISK_MITIGATION_ID = r.RISK_MITIGATION_ID,
                                           RISK_MITIGATION_NAME = r.RISK_MITIGATION_NAME
                                       };
            return RiskMitigationList.ToList();
        }
        public List<RptSavingTypeList> RptSavingTypeList()
        {
            var RptSavingTypeList = from r in db.AUDIT_SAVING_TYPE
                                    where r.ACT == true
                                       orderby r.AUDIT_SAVING_TYPE_NAME
                                    select new RptSavingTypeList
                                       {
                                           AUDIT_SAVING_TYPE_ID = r.AUDIT_SAVING_TYPE_ID,
                                           AUDIT_SAVING_TYPE_NAME = r.AUDIT_SAVING_TYPE_NAME
                                       };
            return RptSavingTypeList.ToList();
        }

        public List<TaskList> TaskList(string UserId)
        {
            var allowedStatusIds = new[] { "1", "3" };
            var TaskList = from r in db.AUDIT_TASK
                           where r.ACT == true && r.CDU == UserId && r.TASK_STATUS == "5" && r.REPORT_CREATE_STATUS == "0" && allowedStatusIds.Contains(r.REPORT_STATUS_ID)
                           orderby r.TASK_ID
                           select new TaskList
                           {
                               TASK_ID = r.TASK_ID,
                               TASK_DETAILS = r.TASK_DETAILS
                           };
            return TaskList.ToList();
        }

        //public List<TaskList> TaskList(string UserId)
        //{
        //    var TaskList = from r in db.AUDIT_TASK
        //                               where r.ACT == true && r.CDU == UserId && r.TASK_STATUS == "5" && r.REPORT_CREATE_STATUS == "0" &&  r.REPORT_STATUS_ID == "1"
        //                   orderby r.TASK_ID
        //                              select new TaskList
        //                               {
        //                                   TASK_ID = r.TASK_ID,
        //                                   TASK_DETAILS = r.TASK_DETAILS
        //                              };
        //    return TaskList.ToList();
        //}
        public List<FirstFollowUpReportList> SearchFirstFollowUpReportList(string UserId)
        {
            var FirstFollowUpReportList = from r in db.AUDIT_REPORT
                           where r.ACT == true && r.CDU == UserId && r.FIRST_FOLLOWUP_STATUS == "N" && r.REPORT_STATUS_ID == "1" && r.MANAGEMENT_ACTION_ID != "4" && r.FIRST_FOLLOWUP_DATE <= DateTime.Today
                                          orderby r.AUDIT_RPT_ID
                                          select new FirstFollowUpReportList
                           {
                               AUDIT_RPT_ID = r.AUDIT_RPT_ID,
                               AUDIT_RPT_NAME = r.AUDIT_RPT_NAME
                           };
            return FirstFollowUpReportList.ToList();
        }
        public List<FirstFollowUpReportList> SearchSecondFollowUpReportList(string UserId)
        {
            var FirstFollowUpReportList = from r in db.AUDIT_REPORT
                                          where r.ACT == true && r.CDU == UserId && r.FIRST_FOLLOWUP_STATUS == "Y" && r.SECOND_FOLLOWUP_STATUS == "N" && r.REPORT_STATUS_ID == "1" && r.SECOND_FOLLOWUP_DATE <= DateTime.Today
                                          orderby r.AUDIT_RPT_ID
                                          select new FirstFollowUpReportList
                                          {
                                              AUDIT_RPT_ID = r.AUDIT_RPT_ID,
                                              AUDIT_RPT_NAME = r.AUDIT_RPT_NAME
                                          };
            return FirstFollowUpReportList.ToList();
        }
        public List<FirstFollowUpReportList> SearchThirdFollowUpReportList(string UserId)
        {
            var FirstFollowUpReportList = from r in db.AUDIT_REPORT
                                          where r.ACT == true && r.CDU == UserId && r.FIRST_FOLLOWUP_STATUS == "Y" && r.SECOND_FOLLOWUP_STATUS == "Y"  && r.THIRD_FOLLOWUP_STATUS == "N" && r.REPORT_STATUS_ID == "1" && r.THIRD_FOLLOWUP_DATE <= DateTime.Today
                                          orderby r.AUDIT_RPT_ID
                                          select new FirstFollowUpReportList
                                          {
                                              AUDIT_RPT_ID = r.AUDIT_RPT_ID,
                                              AUDIT_RPT_NAME = r.AUDIT_RPT_NAME
                                          };
            return FirstFollowUpReportList.ToList();
        }
        public List<TaskList> TaskListWithoutRpt(string UserId)
        {
            var allowedWithoutRptStatusIds = new[] { "2", "4" };
            var TaskList = from r in db.AUDIT_TASK
                           where r.ACT == true && r.CDU == UserId && r.TASK_STATUS == "5" && r.REPORT_CREATE_STATUS == "0" && allowedWithoutRptStatusIds.Contains(r.REPORT_STATUS_ID)
                           orderby r.TASK_ID
                           select new TaskList
                           {
                               TASK_ID = r.TASK_ID,
                               TASK_DETAILS = r.TASK_DETAILS
                           };
            return TaskList.ToList();
        }

        public List<TaskList> ReportTaskList(string UserId)
        {
            var TaskList = from r in db.AUDIT_TASK
                           where r.ACT == true && r.CDU == UserId && r.TASK_STATUS == "5" && r.REPORT_CREATE_STATUS == "1" &&  r.REPORT_STATUS_ID == "1"
                           orderby r.TASK_ID
                           select new TaskList
                           {
                               TASK_ID = r.TASK_ID,
                               TASK_DETAILS = r.TASK_DETAILS
                           };
            return TaskList.ToList();
        }

        public List<TaskCommunicationByList> TaskCommunicationByList()
        {
            var TaskCommunicationByList = from r in db.AUDIT_TASK_COMMUNICATION_BY
                                            where r.ACT == true
                                         orderby r.TASK_COMMUNICATION_BY_NAME
                                            select new TaskCommunicationByList
                                            {
                                             TASK_COMMUNICATION_BY_ID = r.TASK_COMMUNICATION_BY_ID,
                                             TASK_COMMUNICATION_BY_NAME = r.TASK_COMMUNICATION_BY_NAME
                                         };
            return TaskCommunicationByList.ToList();
        }
        
        public List<TaskAssignByList> TaskAssignByList()
        {
            var TaskAssignByList = from r in db.AUDIT_TASK_ASSIGN_BY
                                     where r.ACT == true
                                            orderby r.TASK_ASSIGN_BY_NAME
                                              select new TaskAssignByList
                                              {
                                                TASK_ASSIGN_BY_ID = r.TASK_ASSIGN_BY_ID,
                                                TASK_ASSIGN_BY_NAME = r.TASK_ASSIGN_BY_NAME
                                            };
            return TaskAssignByList.ToList();
        }
        
        public List<ManagementSourceList> ManagementSourceList()
        {
            var ManagementSourceList = from r in db.AUDIT_MANAGEMENT_SOURCE
                                       where r.ACT == true
                                   orderby r.MANAGEMENT_SOURCE_NAME
                                       select new ManagementSourceList
                                   {
                                       MANAGEMENT_SOURCE_ID = r.MANAGEMENT_SOURCE_ID,
                                       MANAGEMENT_SOURCE_NAME = r.MANAGEMENT_SOURCE_NAME
                                   };
            return ManagementSourceList.ToList();
        }
        
        public List<TaskTypeList> TaskTypeList()
        {
            var TaskTypeList = from r in db.AUDIT_TASK_TYPE
                                       where r.ACT == true
                                       orderby r.TASK_TYPE_NAME
                                          select new TaskTypeList
                                       {
                                           TASK_TYPE_ID = r.TASK_TYPE_ID,
                                           TASK_TYPE_NAME = r.TASK_TYPE_NAME
                                       };
            return TaskTypeList.ToList();
        }
        
        public List<TaskPriorityList> TaskPriorityList()
        {
            var TaskPriorityList = from r in db.AUDIT_TASK_PRIORITY
                                   where r.ACT == true
                               orderby r.TASK_PRIORITY_NAME
                                   select new TaskPriorityList
                               {
                                   TASK_PRIORITY_ID = r.TASK_PRIORITY_ID,
                                   TASK_PRIORITY_NAME = r.TASK_PRIORITY_NAME
                               };
            return TaskPriorityList.ToList();
        }
        
        public List<ReportStatusList> ReportStatusList()
        {
            var ReportStatusList = from r in db.AUDIT_REPORT_STATUS
                                   where r.ACT == true
                                   orderby r.REPORT_STATUS_NAME
                                   select new ReportStatusList
                                   {
                                       REPORT_STATUS_ID = r.REPORT_STATUS_ID,
                                       REPORT_STATUS_NAME = r.REPORT_STATUS_NAME
                                   };
            return ReportStatusList.ToList();
        }

        //public List<ReportStatusList> AuditorList()
        //{
        //    var AuditorList = from r in db.AUDIT_AUDITOR
        //                      where r.ACT == true
        //                      orderby r.AUDITOR_NAME
        //                      select new ReportStatusList
        //                      {
        //                          REPORT_STATUS_ID = r.AUDITOR_ID,
        //                          REPORT_STATUS_NAME = r.AUDITOR_NAME
        //                      };
        //    return AuditorList.ToList();
        //}

        public List<C_CompanyList> C_CompanyList()
        {
            var C_CompanyList = from r in db.C_COMPANY
                                   where r.ACT == true
                                   orderby r.COMPANY_NAME
                                   select new C_CompanyList
                                   {
                                       COMPANY_ID = r.COMPANY_ID,
                                       COMPANY_NAME = r.COMPANY_NAME
                                   };
            return C_CompanyList.ToList();
        }

        public List<TaskStatusList> TaskStatusList()
        {
            var TaskStatusList = from r in db.AUDIT_TASK_STATUS
                                 where r.ACT == true
                              orderby r.TASK_STATUS_NAME
                                 select new TaskStatusList
                              {
                                  TASK_STATUS_ID = r.TASK_STATUS_ID,
                                  TASK_STATUS_NAME = r.TASK_STATUS_NAME
                              };
            return TaskStatusList.ToList();
        }
        
        public List<RptFinalStatusList> RptFinalStatusList()
        {
            var RptFinalStatusList = from r in db.AUDIT_RPT_FINAL_STATUS
                                     where r.ACT == true
                                 orderby r.FINAL_RPT_STATUS_NAME
                                     select new RptFinalStatusList
                                 {
                                     FINAL_RPT_STATUS_ID = r.FINAL_RPT_STATUS_ID,
                                     FINAL_RPT_STATUS_NAME = r.FINAL_RPT_STATUS_NAME
                                 };
            return RptFinalStatusList.ToList();
        }
        public List<DepartmentList> DepartmentList(string BusId)
        {
            var DepartmentPermission = from r in db.A_DEPARTMENT
                                       where r.ACT == true && r.DEPARTMENT_BUSINESS == BusId
                                       select new DepartmentList
                                       {
                                           DEPARTMENT_ID = r.DEPARTMENT_ID,
                                           DEPARTMENT_NAME = r.DEPARTMENT_NAME
                                       };
            return DepartmentPermission.ToList();
        }

        public List<DepartmentList> DepartmentList()
        {
            var DepartmentPermission = from r in db.A_DEPARTMENT
                                       where r.ACT == true 
                                       select new DepartmentList
                                       {
                                           DEPARTMENT_ID = r.DEPARTMENT_ID,
                                           DEPARTMENT_NAME = r.DEPARTMENT_NAME
                                       };
            return DepartmentPermission.ToList();
        }

        public List<GroupCompanyList> GCompanyList(string GrpId)
        {
            var CompanyPermission = from r in db.C_COMPANY
                                       where r.ACT == true && r.COMPANY_GROUP_ID == GrpId
                                    select new GroupCompanyList
                                    {
                                           COMPANY_ID = r.COMPANY_ID,
                                           COMPANY_NAME = r.COMPANY_NAME
                                       };
            return CompanyPermission.ToList();
        }
        
        public List<ItemList> ItemList(string LicNameId)
        {
            var ItemPermission = from r in db.A_ITEM_MASTER
                                    where r.ACT == true && r.LICENSE_NAME_ID == LicNameId
                                    select new ItemList
                                    {
                                        ITEM_ID = r.ITEM_ID,
                                        ITEM_NAME = r.ITEM_NAME
                                    };
            return ItemPermission.ToList();
        }

        public List<TCategoryList> TaskCategoryList(string CaTypeId)
        {
            var TaskCategoryPermission = from r in db.AUDIT_TASK_CATEGORY
                                         where r.ACT == true && r.TASK_CATEGORY_TYPE_ID == CaTypeId
                                    select new TCategoryList
                                    {
                                        TASK_CATEGORY_ID = r.TASK_CATEGORY_ID,
                                        TASK_CATEGORY_NAME = r.TASK_CATEGORY_NAME
                                    };
            return TaskCategoryPermission.ToList();
        }

        public List<TSavingNameList> SavingNameList(string SavingTypeId)
        {
            var SavingNameListPermission = from r in db.AUDIT_RPT_SAVING_NAME
                                           where r.ACT == true && r.AUDIT_SAVING_TYPE_ID == SavingTypeId
                                         select new TSavingNameList
                                         {
                                             AUDIT_SAVING_NAME_ID = r.AUDIT_SAVING_NAME_ID,
                                             AUDIT_SAVING_NAME = r.AUDIT_SAVING_NAME
                                         };
            return SavingNameListPermission.ToList();
        }

        public List<TaskAllCategoryList> TaskAllCategoryList()
        {
            var TaskAllCategoryList = from r in db.AUDIT_TASK_CATEGORY
                                      where r.ACT == true
                                       orderby r.TASK_CATEGORY_NAME
                                      select new TaskAllCategoryList
                                       {
                                           TASK_CATEGORY_ID = r.TASK_CATEGORY_ID,
                                           TASK_CATEGORY_NAME = r.TASK_CATEGORY_NAME
                                       };
            return TaskAllCategoryList.ToList();
        }
        
        public List<RptSavingNameList> RptSavingNameList()
        {
            var RptSavingNameList = from r in db.AUDIT_RPT_SAVING_NAME
                                    where r.ACT == true
                                      orderby r.AUDIT_SAVING_NAME
                                    select new RptSavingNameList
                                      {
                                          AUDIT_SAVING_NAME_ID = r.AUDIT_SAVING_NAME_ID,
                                          AUDIT_SAVING_NAME = r.AUDIT_SAVING_NAME
                                      };
            return RptSavingNameList.ToList();
        }

        public List<RptSavingNameList> DirectSavingNameList()
        {
            var DirectSavingNameList = from r in db.AUDIT_RPT_SAVING_NAME
                                    where r.ACT == true && r.AUDIT_SAVING_TYPE_ID == "1"
                                    orderby r.AUDIT_SAVING_NAME
                                    select new RptSavingNameList
                                    {
                                        AUDIT_SAVING_NAME_ID = r.AUDIT_SAVING_NAME_ID,
                                        AUDIT_SAVING_NAME = r.AUDIT_SAVING_NAME
                                    };
            return DirectSavingNameList.ToList();
        }
        
        public List<InDirectSavingNameList> INDirectSavingNameList()
        {
            var INDirectSavingNameList = from r in db.AUDIT_RPT_SAVING_NAME
                                       where r.ACT == true && r.AUDIT_SAVING_TYPE_ID == "2"
                                       orderby r.AUDIT_SAVING_NAME
                                       select new InDirectSavingNameList
                                       {
                                           AUDIT_SAVING_NAME_ID = r.AUDIT_SAVING_NAME_ID,
                                           AUDIT_SAVING_NAME = r.AUDIT_SAVING_NAME
                                       };
            return INDirectSavingNameList.ToList();
        }

        public List<AuditActionList> AuditActionList()
        {
            var AuditActionList = from r in db.AUDIT_REPORT_ACTION
                                  where r.ACT == true
                                      orderby r.ACTION_NAME
                                  select new AuditActionList
                                      {
                                          REPORT_ACTION_ID = r.REPORT_ACTION_ID,
                                          ACTION_NAME = r.ACTION_NAME
                                      };
            return AuditActionList.ToList();
        }
        public List<DirectSavingTypeList> DirectSavingTypeList()
        {
            var DirectSavingTypeList = from r in db.AUDIT_RPT_SAVING_NAME
                                  where r.ACT == true && r.AUDIT_SAVING_TYPE_ID == "1"
                                       orderby r.AUDIT_SAVING_NAME
                                select new DirectSavingTypeList
                                {
                                      AUDIT_SAVING_NAME_ID = r.AUDIT_SAVING_NAME_ID,
                                      AUDIT_SAVING_NAME = r.AUDIT_SAVING_NAME
                                  };
            return DirectSavingTypeList.ToList();
        }
        public List<InDirectSavingTypeList> InDirectSavingTypeList()
        {
            var InDirectSavingTypeList = from r in db.AUDIT_RPT_SAVING_NAME
                                       where r.ACT == true && r.AUDIT_SAVING_TYPE_ID == "2"
                                       orderby r.AUDIT_SAVING_NAME
                                       select new InDirectSavingTypeList
                                       {
                                           AUDIT_SAVING_NAME_ID = r.AUDIT_SAVING_NAME_ID,
                                           AUDIT_SAVING_NAME = r.AUDIT_SAVING_NAME
                                       };
            return InDirectSavingTypeList.ToList();
        }
        
        public List<ManagementActionList> ManagementActionList()
        {
            var ManagementActionList = from r in db.AUDIT_MANAGEMENT_ACTION
                                       where r.ACT == true 
                                         orderby r.MANAGEMENT_ACTION_NAME
                                       select new ManagementActionList
                                         {
                                             MANAGEMENT_ACTION_ID = r.MANAGEMENT_ACTION_ID,
                                             MANAGEMENT_ACTION_NAME = r.MANAGEMENT_ACTION_NAME
                                         };
            return ManagementActionList.ToList();
        }


        public List<DepartmentList> DepartmentById(string DeptId)
        {
            var DepartmentPermission = from r in db.A_DEPARTMENT
                                       where r.ACT == true && r.DEPARTMENT_ID == DeptId
                                       select new DepartmentList
                                       {
                                           DEPARTMENT_ID = r.DEPARTMENT_ID,
                                           DEPARTMENT_NAME = r.DEPARTMENT_NAME
                                       };
            return DepartmentPermission.ToList();
        }
        public List<KickBackStatusList> KickBackStatusById(string KbId)
        {
            var KickBackStatusList = from r in db.INS_KICK_BACK_STATUS
                                     where r.ACT == true && r.KICK_BACK_STATUS_ID == KbId
                                     select new KickBackStatusList
                                     {
                                         KICK_BACK_STATUS_ID = r.KICK_BACK_STATUS_ID,
                                         KICK_BACK_STATUS_NAME = r.KICK_BACK_STATUS_NAME
                                     };
            return KickBackStatusList.ToList();
        }


        public List<AreaList> MyAreaList(string UserId, string UserType)
        {
            List<string> Country = MyCountryListString(UserId: UserId, UserType: UserType);

            if (!Country.Any())
            {
                var AreaList = from a in db.A_AREA
                               join p in db.A_USERS_AREA on a.AREA_ID equals p.UA_AREA
                               where a.ACT == true && p.ACT == true && p.UA_USER == UserId
                               select new AreaList
                               {
                                   AREA_ID = a.AREA_ID,
                                   AREA_NAME = a.AREA_NAME
                               };
                return AreaList.ToList();
            }
            else
            {
                var AreaList = from a in db.A_AREA
                               where a.ACT == true && Country.Contains(a.A_THANA.A_DISTRICT.A_COUNTRY.COUNTRY_ID)
                               select new AreaList
                               {
                                   AREA_ID = a.AREA_ID,
                                   AREA_NAME = a.AREA_NAME
                               };
                return AreaList.ToList();
            }
        }

        public List<AreaList> MyAreaListByID(string AreaId)
        {

            var AreaList = from a in db.A_AREA
                           where a.ACT == true && a.AREA_ID == AreaId
                           select new AreaList
                           {
                               AREA_ID = a.AREA_ID,
                               AREA_NAME = a.AREA_NAME
                           };
            return AreaList.ToList();
        }

        public List<AreaList> AreaList(List<string> Country)
        {
            var AreaList = from a in db.A_AREA
                           where a.ACT == true && Country.Contains(a.A_THANA.A_DISTRICT.A_COUNTRY.COUNTRY_ID)
                           select new AreaList
                           {
                               AREA_ID = a.AREA_ID,
                               AREA_NAME = a.AREA_NAME
                           };
            return AreaList.ToList();
        }
        public List<BodyList> BodyList()
        {
            var BodyList = from a in db.A_BODY
                           where a.ACT == true
                           orderby a.BODY_ID
                           select new BodyList
                           {
                               BODY_ID = a.BODY_ID,
                               BODY_NAME = a.BODY_NAME
                           };
            return BodyList.ToList();
        }
        public List<StaffTypeList> StaffTypeList()
        {
            var StaffTypeList = from a in db.A_STAFF_TYPE
                                where a.ACT == true
                                orderby a.STAFF_TYPE_ID
                                select new StaffTypeList
                                {
                                    STAFF_TYPE_ID = a.STAFF_TYPE_ID,
                                    STAFF_TYPE_NAME = a.STAFF_TYPE_NAME
                                };
            return StaffTypeList.ToList();
        }
        public List<StaffTypeList> StaffTypeById(string StaffTypeId)
        {
            var StaffTypeList = from r in db.A_STAFF_TYPE
                                where r.ACT == true && r.STAFF_TYPE_ID == StaffTypeId
                                select new StaffTypeList
                                {
                                    STAFF_TYPE_ID = r.STAFF_TYPE_ID,
                                    STAFF_TYPE_NAME = r.STAFF_TYPE_NAME
                                };
            return StaffTypeList.ToList();
        }
        public List<BrandList> BrandList()
        {
            var BrandList = from a in db.A_BRAND
                            where a.ACT == true
                            orderby a.BRAND_ID
                            select new BrandList
                            {
                                BRAND_ID = a.BRAND_ID,
                                BRAND_NAME = a.BRAND_NAME
                            };
            return BrandList.ToList();
        }
        public List<ColorList> ColorList()
        {
            var ColorList = from a in db.A_COLOR
                            where a.ACT == true
                            orderby a.COLOR_ID
                            select new ColorList
                            {
                                COLOR_ID = a.COLOR_ID,
                                COLOR_NAME = a.COLOR_NAME
                            };
            return ColorList.ToList();
        }
        public List<FuelList> FuelList()
        {
            var FuelList = from a in db.A_FUEL
                           where a.ACT == true
                           select new FuelList
                           {
                               FUEL_ID = a.FUEL_ID,
                               FUEL_NAME = a.FUEL_NAME
                           };
            return FuelList.ToList();
        }
        public List<VehilceListByDept> VehilceListByDept(string DeptId)
        {
            var VehilceListByDept = from v in db.A_ASET_DATA
                                    where v.ACT == true && v.ASET_DEPARTMENT == DeptId
                                    select new VehilceListByDept
                                    {
                                        ASET_ID = v.ASET_ID,
                                        ASET_NAME_TAG = v.ASET_CODE_NAME
                                    };
            return VehilceListByDept.ToList();

        }
        public List<AccesoriesByVehicle> AccesoriesByAsset(string AsetId)
        {
            //Get Group Id from Aset
            var ASET_GROUP = db.A_ASET_DATA.Where(a => a.ASET_ID == AsetId).ToList();
            var getAcceGroup = ASET_GROUP.Select(s => s.ASET_GROUP);
            //Get Current AcceList by Asset
            var ASET_ACCE = from c in db.A_ASET_ACCE
                            where c.ASET_ID == AsetId
                            select new AccesoriesByVehicle
                            {
                                ACCE_ID = c.ACCE_ID
                            };

            var CurrentList = ASET_ACCE.ToList().Select(s => s.ACCE_ID);
            var AccesoriesByGroup = from i in db.A_ACCE_ITEMS
                                    where i.ACT == true && getAcceGroup.Contains(i.ACCE_GROUP)
                                    select new AccesoriesByVehicle
                                    {
                                        ACCE_ID = i.ACCE_ID,
                                        ACCE_NAME = i.ACCE_NAME
                                    };
            //Get Asset AcceList by Group
            return AccesoriesByGroup.Where(a => !CurrentList.Contains(a.ACCE_ID)).ToList();
        }
        public List<AccesoriesByVehicle> AcceAssociatedByAsset(string AsetId)
        {
            //Get Group Id from Aset
            var ASET_GROUP = db.A_ASET_DATA.Where(a => a.ASET_ID == AsetId).ToList();
            var getAcceGroup = ASET_GROUP.Select(s => s.ASET_GROUP);
            //Get Current AcceList by Asset
            var ASET_ACCE = from c in db.A_ASET_ACCE
                            where c.ASET_ID == AsetId
                            select new AccesoriesByVehicle
                            {
                                ACCE_ID = c.ACCE_ID
                            };

            var CurrentList = ASET_ACCE.ToList().Select(s => s.ACCE_ID);
            var AccesoriesByGroup = from i in db.A_ACCE_ITEMS
                                    where i.ACT == true && getAcceGroup.Contains(i.ACCE_GROUP)
                                    select new AccesoriesByVehicle
                                    {
                                        ACCE_ID = i.ACCE_ID,
                                        ACCE_NAME = i.ACCE_NAME
                                    };
            //Get Asset AcceList by Group
            return AccesoriesByGroup.Where(a => CurrentList.Contains(a.ACCE_ID)).ToList();
        }
        public List<FreeAsetByDeptGroup> FreeAsetByDeptGroup(string DeptId, string GroupId)
        {
            var FreeAsetByDeptGroup = from v in db.A_ASET_DATA
                                      where v.ACT == true
                                      && v.ASET_USED == false
                                      && v.ASET_DEPARTMENT == DeptId
                                      && v.ASET_GROUP == GroupId
                                      select new FreeAsetByDeptGroup
                                      {
                                          ASET_ID = v.ASET_ID,
                                          ASET_CODE_NAME = v.ASET_CODE_NAME
                                      };
            return FreeAsetByDeptGroup.ToList();
        }
        public List<FreeAsetByDeptGroup> ActiveAsetByDeptGroup(string DeptId, string GroupId)
        {
            var ActAsetByDeptGroup = from v in db.A_ASET_DATA
                                     where v.ACT == true
                                     && v.ASET_DEPARTMENT == DeptId
                                     && v.ASET_GROUP == GroupId
                                     select new FreeAsetByDeptGroup
                                     {
                                         ASET_ID = v.ASET_ID,
                                         ASET_CODE_NAME = v.ASET_CODE_NAME
                                     };
            return ActAsetByDeptGroup.ToList();
        }
        public List<OwnerList> OwnerList(string DeptId, string AreaId)
        {

            string Sql = string.Format(@"select o.ownr_id, (o.ownr_text || ' / ' || o.ownr_user) ownr_user
                            from a_ownr o
                            where o.ownr_resign = 'N' and o.ownr_dept = '{0}' and o.ownr_area = '{1}'", DeptId, AreaId);
            List<OwnerList> _ownerList = db.Database.SqlQuery<OwnerList>(sql: Sql).ToList();
            return _ownerList;

            //var OwnerList = from o in db.A_OWNR
            //                where o.ACT == true
            //                && o.OWNR_RESIGN == "N"
            //                && o.OWNR_DEPT == DeptId
            //                && o.OWNR_AREA == AreaId
            //                select new OwnerList
            //                {
            //                    OWNR_ID = o.OWNR_ID,
            //                    OWNR_USER = o.OWNR_USER
            //                };
            //return OwnerList.ToList();
        }
        public List<VehilceListByDept> AsetById(string AssetId)
        {
            var VehilceListByDept = from v in db.A_ASET_DATA
                                    where v.ACT == true && v.ASET_ID == AssetId
                                    select new VehilceListByDept
                                    {
                                        ASET_ID = v.ASET_ID,
                                        ASET_NAME_TAG = v.ASET_CODE_NAME
                                    };
            return VehilceListByDept.ToList();
        }
        public string AsetByName(string AssetName)
        {
            var AssetList = (from a in db.A_ASET_DATA
                             where a.ACT == true
                             && a.ASET_CODE_NAME == AssetName
                             select a.ASET_ID).ToList();
            if (AssetList.Count > 0)
            {
                return AssetList[0].ToString();
            }
            else
            {
                var XD = "0";
                return XD;
            }

        }
        public List<VehilceListByDept> AsetByListId(List<string> AssetId)
        {
            var VehilceListByDept = from v in db.A_ASET_DATA
                                    where v.ACT == true && AssetId.Contains(v.ASET_ID)
                                    select new VehilceListByDept
                                    {
                                        ASET_ID = v.ASET_ID,
                                        ASET_NAME_TAG = v.ASET_CODE_NAME
                                    };
            return VehilceListByDept.ToList();
        }
        public List<OwnerList> GetOwnerByAsset(string AssetId)
        {
            var OwnerName = (from u in db.A_OWNR
                             join a in db.A_ASET_USER on u.OWNR_ID equals a.AU_USER
                             where a.AU_ASET == AssetId
                             && a.AU_EAUSER == null
                             select new OwnerList
                             {
                                 OWNR_ID = u.OWNR_ID ?? "0",
                                 OWNR_USER = u.OWNR_USER ?? "No User"
                             }).ToList();
            return OwnerName;
        }
        public List<RemindItem> ReminderItem(string ig)
        {
            var Item = (from i in db.A_ITEM_LIST
                        where i.REMINDER_ENTRY == "Y"
                        && i.ITEM_GROUP == ig
                        select new RemindItem
                        {
                            ITEM_ID = i.ITEM_ID,
                            ITEM_NAME = i.ITEM_NAME
                        }).ToList();
            return Item;
        }
        public List<RemindItem> InspectionItem(string ig)
        {
            var Item = (from i in db.A_ITEM_LIST
                        where i.JOB_ENTRY == "I"
                        && i.ITEM_GROUP == ig
                        select new RemindItem
                        {
                            ITEM_ID = i.ITEM_ID,
                            ITEM_NAME = i.ITEM_NAME
                        }).ToList();
            return Item;
        }
        public List<RemindItem> InspectionItemNot(string ig, List<string> Exist)
        {
            var Item = (from i in db.A_ITEM_LIST
                        where i.JOB_ENTRY == "I"
                        && i.ITEM_GROUP == ig
                        && !Exist.Contains(i.ITEM_ID)
                        select new RemindItem
                        {
                            ITEM_ID = i.ITEM_ID,
                            ITEM_NAME = i.ITEM_NAME
                        }).ToList();
            return Item;
        }
        public List<RemindItem> BundleInspectionItem(string ig)
        {
            var Item = (from i in db.A_ITEM_LIST
                        where i.JOB_ENTRY == "I"
                        && i.ITEM_BASE == "B"
                        && i.ITEM_GROUP == ig
                        select new RemindItem
                        {
                            ITEM_ID = i.ITEM_ID,
                            ITEM_NAME = i.ITEM_NAME
                        }).ToList();
            return Item;
        }
        public List<string> TransferInfo(string Transfer_id)
        {
            List<string> T_I = new List<string>();
            A_ASET_USER TI = db.A_ASET_USER.Find(Transfer_id);


            if (TI != null)
            {
                T_I.Add(TI.AU_SMAILAGE); //mileage 0
                T_I.Add(TI.AU_SRUSER); //handover by 1
                T_I.Add(TI.AU_SRDATE.ToString()); //date 2
                T_I.Add(TI.AU_SNOTE); // note  3

                A_DEPARTMENT AD = db.A_DEPARTMENT.Find(TI.AU_DEPT);
                T_I.Add(AD.DEPARTMENT_NAME); //Take Working Department Name 4
                T_I.Add(AD.A_BUSINESS.BUSINESS_NAME); //Working Business 5
                T_I.Add(AD.A_BUSINESS.A_COMPANY.COMPANY_NAME); //Working Company 6
                A_AREA AA = db.A_AREA.Find(TI.AU_AREA);
                T_I.Add(AA.AREA_NAME); //Take Working Area Name 7

                T_I.Add(TI.A_ASET_DATA.ASET_TAG); // Aset Tag 8
                T_I.Add(TI.A_ASET_DATA.ASET_CODE_NAME); //aset Code 9
                T_I.Add(TI.A_ASET_DATA.A_DEPARTMENT.DEPARTMENT_NAME); //aset detaprtment 10
                T_I.Add(TI.A_ASET_DATA.A_DEPARTMENT.A_BUSINESS.BUSINESS_NAME); //aset business 11
                T_I.Add(TI.A_ASET_DATA.A_DEPARTMENT.A_BUSINESS.A_COMPANY.COMPANY_NAME); //aset company 12
                T_I.Add(TI.A_ASET_DATA.A_AREA.AREA_NAME);//aset area name 13

                T_I.Add(TI.A_OWNR.OWNR_TEXT); //user staff id 14
                T_I.Add(TI.A_OWNR.OWNR_USER); //staff name 15
                T_I.Add(TI.A_OWNR.OWNR_NAME);//Iqama 16
                T_I.Add(TI.A_OWNR.A_DEPARTMENT.DEPARTMENT_NAME);//user department 17
                T_I.Add(TI.A_OWNR.A_DEPARTMENT.A_BUSINESS.BUSINESS_NAME); //user business 18
                T_I.Add(TI.A_OWNR.A_DEPARTMENT.A_BUSINESS.A_COMPANY.COMPANY_NAME); //user company 19
                T_I.Add(TI.A_OWNR.A_AREA.AREA_NAME);//aset area name 20

                T_I.Add(AD.A_BUSINESS.BUSINESS_ID); //  Working BU Name 21
            };
            return T_I;
        }
        public List<Email_Id> EmailListByGroup(List<string> Group, List<string> BU, string TypeS)
        {
            var EMAIL_ID = (from e in db.A_EMAIL
                            where Group.Contains(e.MAIL_GROUP)
                            && BU.Contains(e.MAIL_BUSINESS)
                            && e.MAIL_TYPE == TypeS
                            && e.ACT == true
                            select new Email_Id
                            {
                                SMTP = e.EMAIL_ID,
                                KEEP_IN = e.KEEP_IN
                            }).ToList();
            return EMAIL_ID;
        }
        public List<Email_Id> EmailListByGroupArea(List<string> Group, string TypeS, string AreA)
        {
            var EMAIL_ID = (from e in db.A_EMAIL
                            where Group.Contains(e.MAIL_GROUP)
                            && e.MAIL_TYPE == TypeS
                            && e.AREA_ID == AreA
                            && e.ACT == true
                            select new Email_Id
                            {
                                SMTP = e.EMAIL_ID,
                                KEEP_IN = e.KEEP_IN
                            }).ToList();
            return EMAIL_ID;
        }
        public List<Email_Id> EmailListByGroupType(List<string> Group, string TypeS)
        {
            var EMAIL_ID = (from e in db.A_EMAIL
                            where Group.Contains(e.MAIL_GROUP)
                            && e.MAIL_TYPE == TypeS
                            && e.ACT == true
                            select new Email_Id
                            {
                                SMTP = e.EMAIL_ID,
                                KEEP_IN = e.KEEP_IN
                            }).ToList();
            return EMAIL_ID;
        }
        public List<string> InspectionApprovalData(string id)
        {
            List<string> IAD = new List<string>();
            A_INSPH I = db.A_INSPH.Find(id);
            if (I != null)
            {
                IAD.Add(I.INSPH_ID);// Inspection Id {0}
                IAD.Add(I.INSPH_NOTE);//Reason {1}
                IAD.Add(I.A_ASET_DATA.ASET_CODE_NAME); // aset code {2}
                IAD.Add(I.INSPH_IUSER);//Inspection User {3}
                IAD.Add(I.INSPH_DATE.Value.ToString("dddd, dd MMMM yyyy"));// Inspection Date {4}
            }
            A_OWNR AO = db.A_OWNR.Find(I.INSPH_OWNR);
            if (AO != null)
            {
                IAD.Add(AO.OWNR_USER); // Staff Name {5}
            }
            List<A_INSPD> D = db.A_INSPD.Where(d => d.INSPH_ID == I.INSPH_ID).ToList();
            decimal TotalAmount = 0;
            foreach (var item in D)
            {
                decimal SubTotal = (item.INSPD_IQTY * item.INSPD_IRATE) ?? 0;
                TotalAmount = TotalAmount + SubTotal;
            }
            IAD.Add(TotalAmount.ToString()); //Amount {6}
            return IAD;

        }
        public List<string> JobApprovalData(string id)
        {
            List<string> JAD = new List<string>();
            A_JOBH J = db.A_JOBH.Find(id);
            if (J != null)
            {
                JAD.Add(J.JOBH_ID); // Job Id {0}
                JAD.Add(J.JOBH_NOTE);//Reason/Note {1}
                JAD.Add(J.A_INSPH.A_ASET_DATA.ASET_CODE_NAME);//Aset name {2}
                JAD.Add(J.JOBH_DATE.Value.ToString("dddd, dd MMMM yyyy"));//Job Date {3}
                JAD.Add(J.INSPH_ID);//Inspection No {4}
            }
            List<A_JOBD> D = db.A_JOBD.Where(d => d.JOBH_ID == J.JOBH_ID).ToList();
            decimal IAmount = 0;
            decimal JAmount = 0;
            foreach (var item in D)
            {
                decimal SubIAmnt = (item.JOBD_IQTY * item.JOBD_IRATE) ?? 0;
                IAmount = IAmount + SubIAmnt;
                decimal SubJAmnt = (item.JOBD_JQTY * item.JOBD_JRATE) ?? 0;
                JAmount = JAmount + SubJAmnt;
            }
            JAD.Add(IAmount.ToString()); //I Amount {5}
            JAD.Add(JAmount.ToString()); // J Amonut {6}
            A_OWNR AO = db.A_OWNR.Find(J.JOBH_OWNR);
            if (AO != null)
            {
                JAD.Add(AO.OWNR_USER); // Staff Name {7}
            }
            return JAD;

        }

        public List<string> AsetIdListByUserPermission(string UserId)
        {
            string Sql = string.Format(@"select a.aset_id from vw_user_asset_id_string a where a.ug_user='{0}'", UserId);
            List<string> asset_id_list = db.Database.SqlQuery<string>(sql: Sql).ToList();
            return asset_id_list;
        }

    }


    public class MyGroup
    {
        public string Group_ID { get; set; }
        public string Group_Name { get; set; }
    }
    public class MyBusiness
    {
        public string BUSINESS_ID { get; set; }
        public string BUSINESS_NAME { get; set; }
    }
    public class InsuranceTypeList
    {
        public string INSURANCE_TYPE_ID { get; set; }
        public string INSURANCE_TYPE_NAME { get; set; }
    }
    public class CompanyList
    {
        public string INS_COMPANY_ID { get; set; }
        public string COMPANY_NAME { get; set; }
    }
    
    public class LICCompanyList
    {
        public string COMPANY_ID { get; set; }
        public string COMPANY_NAME { get; set; }
    }
    public class GroupCompanyList
    {
        public string COMPANY_ID { get; set; }
        public string COMPANY_NAME { get; set; }
    }
    
    public class TCategoryList
    {
        public string TASK_CATEGORY_ID { get; set; }
        public string TASK_CATEGORY_NAME { get; set; }
    }
    
    public class TSavingNameList
    {
        public string AUDIT_SAVING_NAME_ID { get; set; }
        public string AUDIT_SAVING_NAME { get; set; }
    }

    public class TaskAllCategoryList
    {
        public string TASK_CATEGORY_ID { get; set; }
        public string TASK_CATEGORY_NAME { get; set; }
    }
    
    public class RptSavingNameList
    {
        public string AUDIT_SAVING_NAME_ID { get; set; }
        public string AUDIT_SAVING_NAME { get; set; }
    }
    public class InDirectSavingNameList
    {
        public string AUDIT_SAVING_NAME_ID { get; set; }
        public string AUDIT_SAVING_NAME { get; set; }
    }


    public class AuditActionList
    {
        public string REPORT_ACTION_ID { get; set; }
        public string ACTION_NAME { get; set; }
    }
    
    public class DirectSavingTypeList
    {
        public string AUDIT_SAVING_NAME_ID { get; set; }
        public string AUDIT_SAVING_NAME { get; set; }
    }
    public class InDirectSavingTypeList
    {
        public string AUDIT_SAVING_NAME_ID { get; set; }
        public string AUDIT_SAVING_NAME { get; set; }
    }
    
    public class ManagementActionList
    {
        public string MANAGEMENT_ACTION_ID { get; set; }
        public string MANAGEMENT_ACTION_NAME { get; set; }
    }
    public class LocationList
    {
        public string LIC_LOCATION_ID { get; set; }
        public string LOCATION_NAME { get; set; }
    }
    
    public class UserList
    {
        public string USER_TEXT { get; set; }
        public string USER_NAME { get; set; }
    }

    public class C_LocationList
    {
        public string LOCATION_ID { get; set; }
        public string LOCATION_NAME { get; set; }
    }
    
    public class CompanyGroupList
    {
        public string COMPANY_GROUP_ID { get; set; }
        public string COMPANY_GROUP_NAME { get; set; }
    }

    public class C_DepartmentList
    {
        public string DEPARTMENT_ID { get; set; }
        public string DEPARTMENT_NAME { get; set; }
    }
    
    public class TaskCategoryTypeList
    {
        public string TASK_CATEGORY_TYPE_ID { get; set; }
        public string TASK_CATEGORY_TYPE_NAME { get; set; }
    }
    
    public class RiskMitigationList
    {
        public string RISK_MITIGATION_ID { get; set; }
        public string RISK_MITIGATION_NAME { get; set; }
    }

    public class RptSavingTypeList
    {
        public string AUDIT_SAVING_TYPE_ID { get; set; }
        public string AUDIT_SAVING_TYPE_NAME { get; set; }
    }

    public class TaskList
    {
        public string TASK_ID { get; set; }
        public string TASK_DETAILS { get; set; }
    }
    
    public class FirstFollowUpReportList
    {
        public string AUDIT_RPT_ID { get; set; }
        public string AUDIT_RPT_NAME { get; set; }
    }

    public class TaskCommunicationByList
    {
        public string TASK_COMMUNICATION_BY_ID { get; set; }
        public string TASK_COMMUNICATION_BY_NAME { get; set; }
    }
    
    public class TaskAssignByList
    {
        public string TASK_ASSIGN_BY_ID { get; set; }
        public string TASK_ASSIGN_BY_NAME { get; set; }
    }
    
    public class ManagementSourceList
    {
        public string MANAGEMENT_SOURCE_ID { get; set; }
        public string MANAGEMENT_SOURCE_NAME { get; set; }
    }
    
    public class TaskTypeList
    {
        public string TASK_TYPE_ID { get; set; }
        public string TASK_TYPE_NAME { get; set; }
    }
    
    public class TaskPriorityList
    {
        public string TASK_PRIORITY_ID { get; set; }
        public string TASK_PRIORITY_NAME { get; set; }
    }
    
    public class ReportStatusList
    {
        public string REPORT_STATUS_ID { get; set; }
        public string REPORT_STATUS_NAME { get; set; }
    }

    //public class AuditorList
    //{
    //    public string AUDITOR_ID { get; set; }
    //    public string AUDITOR_NAME { get; set; }
    //}
    public class C_CompanyList
    {
        public string COMPANY_ID { get; set; }
        public string COMPANY_NAME { get; set; }
    }


    public class IssuingAuthorityList
    {
        public string ISSUING_AUTHORITY_ID { get; set; }
        public string AUTHORITY_NAME { get; set; }
    }

    public class LicenseTypeList
    {
        public string LICENSE_NAME_ID { get; set; }
        public string LICENSE_NAME { get; set; }
    }
    
    public class LicenseTypeNameList
    {
        public string LICENSE_TYPE_ID { get; set; }
        public string LICENSE_TYPE_NAME { get; set; }
    }

    public class LicenseStatusList
    {
        public string LICENSE_STATUS_ID { get; set; }
        public string LICENSE_STATUS_NAME { get; set; }
    }

    public class ItemList
    {
        public string ITEM_ID { get; set; }
        public string ITEM_NAME { get; set; }
    }

    public class KickBackStatusList
    {
        public string KICK_BACK_STATUS_ID { get; set; }
        public string KICK_BACK_STATUS_NAME { get; set; }
    }

    public class ClaimTypeList
    {
        public string CLAIM_TYPE_ID { get; set; }
        public string CLAIM_NAME { get; set; }
    }

    public class ClaimStatusList
    {
        public string CLAIM_STATUS_ID { get; set; }
        public string CLAIM_STATUS_NAME { get; set; }
    }

    public class InsuranceCompanyList
    {
        public string INSURANCE_COMPANY_ID { get; set; }
        public string INS_COMPANY_NAME { get; set; }
    }
    
    public class AuditorList
    {
        public string AUDITOR_ID { get; set; }
        public string AUDITOR_NAME { get; set; }
    }
    public class AuditorListNew
    {
        public string AUDITOR_STAFF_ID { get; set; }
        public string AUDITOR_NAME { get; set; }
    }
    public class RiskTypeList
    {
        public string RISK_TYPE_ID { get; set; }
        public string RISK_TYPE_NAME { get; set; }
    }
    
    public class FollowUpRequirementList
    {
        public string FOLLOWUP_REQUIREMENT_ID { get; set; }
        public string FOLLOWUP_REQUIREMENT_NAME { get; set; }
    }


    public class TaskStatusList
    {
        public string TASK_STATUS_ID { get; set; }
        public string TASK_STATUS_NAME { get; set; }
    }
    
    public class RptFinalStatusList
    {
        public string FINAL_RPT_STATUS_ID { get; set; }
        public string FINAL_RPT_STATUS_NAME { get; set; }
    }

    public class DepartmentList
    {
        public string DEPARTMENT_ID { get; set; }
        public string DEPARTMENT_NAME { get; set; }
    }
    public class AreaList
    {
        public string AREA_ID { get; set; }
        public string AREA_NAME { get; set; }
    }
    public class StaffTypeList
    {
        public string STAFF_TYPE_ID { get; set; }
        public string STAFF_TYPE_NAME { get; set; }
    }

    public class BodyList
    {
        public string BODY_ID { get; set; }
        public string BODY_NAME { get; set; }
    }
    public class BrandList
    {
        public string BRAND_ID { get; set; }
        public string BRAND_NAME { get; set; }
    }
    public class ColorList
    {
        public string COLOR_ID { get; set; }
        public string COLOR_NAME { get; set; }
    }
    public class FuelList
    {
        public string FUEL_ID { get; set; }
        public string FUEL_NAME { get; set; }
    }
    public class VehilceListByDept
    {
        public string ASET_ID { get; set; }
        public string ASET_NAME_TAG { get; set; }
    }
    public class AccesoriesByVehicle
    {
        public string ACCE_ID { get; set; }
        public string ACCE_NAME { get; set; }
    }
    public class FreeAsetByDeptGroup
    {
        public string ASET_ID { get; set; }
        public string ASET_CODE_NAME { get; set; }
    }
    public class OwnerList
    {
        public string OWNR_ID { get; set; }
        public string OWNR_USER { get; set; }
    }
    public class RemindItem
    {
        public string ITEM_ID { get; set; }
        public string ITEM_NAME { get; set; }
    }
    public class Email_Id
    {
        public string SMTP { get; set; }
        public string KEEP_IN { get; set; }
    }
}