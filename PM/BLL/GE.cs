using PM.Model;
using System;
using System.Data.Entity;
using System.Linq;

namespace PM.BLL
{
    //General Execution
    public class GE
    {

        private PMEntities db = new PMEntities();
        //Update Sequence and update Sequennce Number
        public int GetSeq(int id)
        {
            A_TBL_SEQ seq = db.A_TBL_SEQ.Find(id);
            seq.TABLE_SEQ = seq.TABLE_SEQ + 1 ?? 1;
            int N = seq.TABLE_SEQ ?? 1;
            db.Entry(seq).State = EntityState.Modified;
            db.SaveChanges();
            //
            return N;
        }
        //Only Update
        public int GetSeqNumber(int id)
        {
            A_TBL_SEQ sequen = db.A_TBL_SEQ.Find(id);
            sequen.TABLE_SEQ = sequen.TABLE_SEQ + 1 ?? 1;
            int NId = sequen.TABLE_SEQ ?? 1;
            //
            return NId;
        }
        public int GetSeqNew(int id)
        {
            A_TBL_SEQ seq = db.A_TBL_SEQ.Find(id);
            seq.TABLE_SEQ = seq.TABLE_SEQ ?? 1;
            int N = seq.TABLE_SEQ ?? 1;
            db.Entry(seq).State = EntityState.Modified;
            db.SaveChanges();
            //
            return N;
        }
        public void PostSeq(int id, int val)
        {
            A_TBL_SEQ seq = db.A_TBL_SEQ.Find(id);
            seq.TABLE_SEQ = val;
            db.Entry(seq).State = EntityState.Modified;
            db.SaveChanges();
        }

        public string GetImgPath(int id)
        {
            A_TBL_SEQ seq = db.A_TBL_SEQ.Find(id);
            return seq.TABLE_NAME;
        }

        //Associate Asset Accesories to User
        //public void UpdateAcceItems(string TransferId, string AssetId, string[] Notes)
        //{
        //    var AI = db.A_ASET_ACCE.Where(a => a.ASET_ID == AssetId && a.ACT == true).OrderBy(b => b.ACCE_ID).ToList();
        //    if (AI.Count != 0)
        //    {
        //        A_ASET_UA AU = new A_ASET_UA();
        //        foreach (var item in AI)
        //        {
        //            int i = 0;
        //            AU.AUA_AU = TransferId;
        //            AU.AUA_ACCE = item.ACCE_ID;
        //            AU.AUA_DESC = Notes.ElementAt(i);
        //            AU.AUA_QTY = item.AAI_QUANTITY;
        //            AU.AUA_RATE = item.AAI_RATE;
        //            AU.AUA_TYPE = "T";
        //            AU.ACT = true;
        //            i = i + 1;
        //            db.A_ASET_UA.Add(AU);
        //        }
        //        db.SaveChanges();
        //    }
        //}


        //Active\Inactive Asset(s)
        public bool OnOffAsset(string AssetId, bool VAL)
        {
            bool Result = false;
            A_ASET_DATA AD = db.A_ASET_DATA.Find(AssetId);
            if (AD != null)
            {
                AD.ACT = VAL;
                db.Entry(AD).State = EntityState.Modified;
                db.SaveChanges();
                Result = true;
            }
            return Result;
        }

        //Booked\Unbooked Asset(s)

        public bool SetAssetUsedUnused(string AssetId, bool VAL)
        {
            A_ASET_DATA AD = db.A_ASET_DATA.Find(AssetId);
            if (AD == null)
            {
                return false;
            }
            else
            {
                AD.ASET_USED = VAL;
                db.Entry(AD).State = EntityState.Modified;
                db.SaveChanges();
                return true;
            }
        }
        public bool SetOwnerBookUnbook(string OwnrId, bool VAL)
        {
            A_OWNR AO = db.A_OWNR.Find(OwnrId);
            if (AO == null)
            {
                return false;
            }
            else
            {
                AO.OWNR_POST = VAL;
                db.Entry(AO).State = EntityState.Modified;
                db.SaveChanges();
                return true;
            }
        }

        //public void BookUnbookAsset(string AssetId, bool VAL)
        //{
        //    A_ASET_DATA AD = db.A_ASET_DATA.Find(AssetId);
        //    AD.ASET_USED = VAL;
        //    db.Entry(AD).State = EntityState.Modified;
        //    db.SaveChanges();
        //}

        //Owner Booked\Unbooked
        //public void OwnerPostUnpost(string OwnrId, bool VAL)
        //{
        //    A_OWNR AO = db.A_OWNR.Find(OwnrId);
        //    AO.OWNR_POST = VAL;
        //    db.Entry(AO).State = EntityState.Modified;
        //    db.SaveChanges();
        //}



        public string FifteenDigitId(string Pref, string Id, DateTime dt)
        {
            string PL = dt.Year.ToString() + dt.Month.ToString().PadLeft(2, '0') + dt.Day.ToString().PadLeft(2, '0') + Id.ToString() + dt.Hour.ToString().PadLeft(2, '0');
            string XXX = Pref + PL.PadLeft(15, '0');
            return XXX;
        }
        public string TwentyDigitId(string Pref, string Id, DateTime dt)
        {
            dt = DateTime.Now;
            string PL = dt.Year.ToString() + dt.Month.ToString().PadLeft(2, '0') + dt.Day.ToString().PadLeft(2, '0') + dt.Hour.ToString().PadLeft(2, '0') + dt.Minute.ToString().PadLeft(2, '0') + dt.Second.ToString().PadLeft(2, '0') + Id.ToString();
            string XXX = Pref + PL.PadLeft(20, '0');
            return XXX;
        }





        public short GetAssetServiceMonth(string id, DateTime ADATE)
        {
            short SM = 0;
            A_ASET_DATA AD = db.A_ASET_DATA.Find(id);
            if (AD != null)
            {
                DateTime REG = AD.ASET_REG_DATE ?? DateTime.Now.Date;
                SM = Convert.ToInt16(((ADATE.Year - REG.Year) * 12) + ADATE.Month - REG.Month);
            }
            return SM;
        }


        public bool IsExist_ASSET_CODE_NAME(string CODE_NAME)
        {
            bool AD = db.A_ASET_DATA.Where(x => x.ASET_CODE_NAME == CODE_NAME).Any();
            return AD;
        }
        //new
        public bool IsExist_BILL_OWNR(string CODE_NAME)
        {
            bool AD = db.A_OWNR.Where(x => x.OWNR_TEXT == CODE_NAME).Any();
            return AD;
        }

        public bool IsExist_OUTLET_NAME(string CODE_NAME)
        {
            //bool ON = db.A_OWNR.Where(o => o.OWNR_TEXT == CODE_NAME && o.OWNR_POST == false && o.ACT==true).Any();
            bool ON = db.A_OWNR.Where(o => o.OWNR_TEXT == CODE_NAME).Any();
            return ON;
        }

        public string Asset_Id_By_Fridge_Code(string AssetCode)
        {
            string rt = "";
            A_ASET_DATA AD = db.A_ASET_DATA.Where(a => a.ASET_CODE_NAME == AssetCode).FirstOrDefault();
            if (AD != null)
            {
                rt = AD.ASET_ID;
            }
            return rt;
        }
        public string Outlet_Id_By_Outlet_Code(string OutletCode)
        {
            string rt = "";
            A_OWNR AO = db.A_OWNR.Where(a => a.OWNR_TEXT == OutletCode).FirstOrDefault();
            if (AO != null)
            {
                rt = AO.OWNR_ID;
            }
            return rt;
        }
        public AsetId_DepartmentId_AreaId ADA_Fridge_Code(string AssetCode)
        {
            A_ASET_DATA AD = db.A_ASET_DATA.Where(a => a.ASET_CODE_NAME == AssetCode).FirstOrDefault();
            AsetId_DepartmentId_AreaId x = new AsetId_DepartmentId_AreaId();
            if (AD != null)
            {
                x.ASET_ID = AD.ASET_ID;
                x.DEPARTMENT_ID = AD.ASET_DEPARTMENT;
                x.AREA_ID = AD.ASET_AREA;
            }
            return x;
        }

    }
    public class AsetId_DepartmentId_AreaId
    {
        public string ASET_ID { get; set; }
        public string AREA_ID { get; set; }
        public string DEPARTMENT_ID { get; set; }
    }
}