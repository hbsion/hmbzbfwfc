using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using marr.DataLayer;

namespace marr.BusinessRule
{
    public class brBillNumber : IDisposable
    {
        public brBillNumber()
        {
            db = new dbDataContext(System.Configuration.ConfigurationSettings.AppSettings["ConnectionString"]);
        }

        private dbDataContext db;

        public void Dispose()
        {
            db.Dispose();
            db = null;
        }

        void IDisposable.Dispose()
        {
            this.Dispose();
        }

        public string getBillNumber(string inbillcode,int addint)
        {
            Gy_BillNumber mybillnumber= db.Gy_BillNumber.FirstOrDefault(x => x.BillCode == inbillcode);
            if (mybillnumber != null)
            {
                int tmpint = (int) mybillnumber.seq1  + 1;
                if (addint == 1)
                {
                    if (tmpint==99999)
                        db.ExecuteCommand("update Gy_BillNumber  set seq1=0 where BillCode ='" + inbillcode + "'");
                    else
                        db.ExecuteCommand("update Gy_BillNumber  set seq1=seq1+1 where BillCode ='" + inbillcode+ "'");

                }
                return mybillnumber.Profix1.Trim() + string.Format("{0:yyMMdd}", System.DateTime.Now) + string.Format("{0:0000}", tmpint);
            }
            return "";

        }

        public void Update(Gy_BillNumber entity)
        {
            lock (db)
            {
                bool closeConn = false;
                if (db.Connection.State != ConnectionState.Open)
                {
                    db.Connection.Open();
                    closeConn = true;
                }

                try
                {
                    db.Transaction = db.Connection.BeginTransaction();
                    Gy_BillNumber dbdata = db.Gy_BillNumber.FirstOrDefault(x => x.fid == entity.fid);
                    if (dbdata != null)
                        EntityUtilities.CloneEntity<Gy_BillNumber>(entity, dbdata);
                    else
                        throw new Exception("更新失败，该记录可能已被其他用户删除。");
                    db.SubmitChanges();

                    db.Transaction.Commit();
                }
                catch
                {
                    if (db.Transaction != null) db.Transaction.Rollback();
                    throw;
                }
                finally
                {
                    db.Transaction = null;
                    if (closeConn) db.Connection.Close();
                }
            }
        }

    }
}
