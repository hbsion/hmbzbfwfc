using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using marr.BusinessRule;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using UI.hmtools;

namespace UI.app
{
    /// <summary>
    /// $codebehindclassname$ 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class appService : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string cu_no = context.Request["cu_no"];
            string xtcu_no = context.Request["xtcu_no"];
            string p_no = context.Request["p_no"];
            string po_no = context.Request["po_no"];
            string unitcode = context.Request["unitcode"];
            string key = context.Request["key"]; //key值，区分不同的查询表
            string query = context.Request["query"];//查询条件
            string dataf = context.Request["datef"];//开始时间
            string datat = context.Request["datet"];//结束时间


            string wheresql = context.Request["wheresql"];
            if  (wheresql==null)
            {
                wheresql = "";
            }

            

            int PageIndex = 0;
            int PageSize = 0;
            try { PageIndex = int.Parse(context.Request["PageIndex"]); }
            catch { PageIndex = 1; }

            try { PageSize = int.Parse(context.Request["PageSize"]); }
            catch { PageSize = 20; }

            string indate = "";//时间字段
            string[] ArrnWhere = null;//搜索字段
            /* 自定义where  */

            SqlQuery sqlQuery = new SqlQuery();


            if (key == "invstlist")// 库明细
            {
                sqlQuery.TableName = "(select a.fid,a.p_no,a.pname,a.type,b.st_no,b.stqty from inv_part a ,inv_stqty b where a.p_no=b.p_no and b.stqty>0 ) as invstqty ";//表名
                sqlQuery.PrimaryKey = "fid";//主键
                sqlQuery.Order = "in_date";//排序
                sqlQuery.SortType = 2;//排序规则
                indate = "ship_date";//时间字段
                ArrnWhere = new string[] { "pname", "st_no", "p_no" };//查询字段
                sqlQuery.Where = wheresql;
            }


            if (key == "ScanFgilist")//入库明细
            {
                sqlQuery.TableName = "sal_fgi";//表名
                sqlQuery.PrimaryKey = "fid";//主键
                sqlQuery.Order = "ship_date";//排序
                sqlQuery.SortType = 2;//排序规则
                indate = "ship_date";//时间字段
                ArrnWhere = new string[] { "bsnno", "ship_no",  "p_no" };//查询字段
                sqlQuery.Where = wheresql;
            }


            if (key == "ScanShiplist")//出库明细
            {
                sqlQuery.TableName = "sal_ship";//表名
                sqlQuery.PrimaryKey = "fid";//主键
                sqlQuery.Order = "ship_date";//排序
                sqlQuery.SortType = 2;//排序规则
                indate = "ship_date";//时间字段
                ArrnWhere = new string[] { "bsnno", "ship_no", "p_no","cu_no" };//查询字段
                sqlQuery.Where = wheresql;
            }
            if (key == "ScanRelist")//退库明细
            {
                sqlQuery.TableName = "sal_re";//表名
                sqlQuery.PrimaryKey = "fid";//主键
                sqlQuery.Order = "ship_date";//排序
                sqlQuery.SortType = 2;//排序规则
                indate = "ship_date";//时间字段
                ArrnWhere = new string[] { "bsnno", "ship_no", "p_no","cu_no" };//查询字段
                sqlQuery.Where = wheresql;
            }

            if (key == "ScanTstlist")//调拨明细
            {
                sqlQuery.TableName = "sal_tst";//表名
                sqlQuery.PrimaryKey = "fid";//主键
                sqlQuery.Order = "ship_date";//排序
                sqlQuery.SortType = 2;//排序规则
                indate = "ship_date";//时间字段
                ArrnWhere = new string[] { "bsnno", "ship_no", "p_no" };//查询字段
                sqlQuery.Where = wheresql;
            }

            if (key == "st")//库别
            {
                sqlQuery.TableName = "inv_store";//表名
                sqlQuery.PrimaryKey = "fid";//主键
                sqlQuery.Order = "fid";//排序
                sqlQuery.SortType = 2;//排序规则
                indate = "in_date";//时间字段
                ArrnWhere = new string[] { "st_no", "st_name" };//查询字段
                sqlQuery.Where = wheresql;
            }

            if (key == "dianhblist")//促销员红包明细
            {
                sqlQuery.TableName = "hob_yuerout";//表名
                sqlQuery.PrimaryKey = "fid";//主键
                sqlQuery.Order = "in_date";//排序
                sqlQuery.SortType = 2;//排序规则
                indate = "in_date";//时间字段
                ArrnWhere = new string[] { "wx_id", "fwcode" };//查询字段
                sqlQuery.Where = wheresql;
            }

            if (key == "hy")//会员信息
            {
                sqlQuery.TableName = "hy_hy";//表名
                sqlQuery.PrimaryKey = "fid";//主键
                sqlQuery.Order = "in_date";//排序
                sqlQuery.SortType = 2;//排序规则
                indate = "in_date";//时间字段
                ArrnWhere =new string[] {"hy_no","hy_cn","mobile","province","city"};//查询字段
                sqlQuery.Where = wheresql ;
            }

            if (key == "prod")//产品信息
            {
                sqlQuery.TableName = "Inv_Part";//表名
                sqlQuery.PrimaryKey = "fid";//主键
                sqlQuery.Order = "p_no";//排序
                sqlQuery.SortType = 1;//排序规则
                indate = "in_date";//时间字段
                ArrnWhere = new string[] { "p_no", "pname", "type","remark" };//查询字段
                sqlQuery.Where = wheresql;
            }

            if (key == "cust")//经销商信息
            {
                sqlQuery.TableName = "customer";//表名
                sqlQuery.PrimaryKey = "fid";//主键
                sqlQuery.Order = "cu_no";//排序
                sqlQuery.SortType = 1;//排序规则
                indate = "in_date";//时间字段
                ArrnWhere = new string[] { "cu_no", "cu_name", "remark", "province", "city", "addr", "phone" };//查询字段
                if (cu_no.Trim().Length == 0)
                {
                    sqlQuery.Where = " 1=1 ";
                }
                else
                {
                    sqlQuery.Where = " 1=1 ";
                   // sqlQuery.Where = "xtcu_no='" + cu_no.Trim() + "'  ";
                }

            }
            if (key == "vender")//经销商信息
            {
                sqlQuery.TableName = "vender";//表名
                sqlQuery.PrimaryKey = "fid";//主键
                sqlQuery.Order = "cu_no";//排序
                sqlQuery.SortType = 1;//排序规则
                indate = "in_date";//时间字段
                ArrnWhere = new string[] { "cu_no", "cu_name", "remark", "province", "city", "addr", "phone" };//查询字段
                if (cu_no.Trim().Length == 0)
                {
                    sqlQuery.Where = " 1=1 ";
                }
                else
                {
                    sqlQuery.Where = "xtcu_no='" + cu_no.Trim() + "'  ";
                }

            }
            if (key == "JF")//积分记录
            {
                sqlQuery.TableName = "hy_jflist";//表名
                sqlQuery.PrimaryKey = "fid";//主键
                sqlQuery.Order = "in_date";//排序
                sqlQuery.SortType = 2;//排序规则
                indate = "in_date";//时间字段
                ArrnWhere = new string[] { "hy_no", "hy_cn", "fwcode", "p_no", "pname", "jf" };//搜索字段
                sqlQuery.Where = wheresql;
            }
            if (key == "hob")//红包记录
            {
                sqlQuery.TableName = "hob_yuerout";//表名
                sqlQuery.PrimaryKey = "fid";//主键
                sqlQuery.Order = "in_date";//排序
                sqlQuery.SortType = 2;//排序规则
                indate = "in_date";//时间字段
                ArrnWhere = new string[] { "fwcode", "wx_id", "wx_name", "mqty" };//搜索字段
                sqlQuery.Where = wheresql;
            }
            if (key == "ship1")//总部出货
            {
                sqlQuery.TableName = "sal_ship";
                sqlQuery.PrimaryKey = "fid";
                sqlQuery.Order = "in_date";
                sqlQuery.SortType = 2;
                indate = "in_date";
                ArrnWhere = new string[] { "bsnno", "cu_no", "cu_name", "p_no", "pname", "ship_no" };
            }
            if (key == "ship2")//经销商出货
            {
                sqlQuery.TableName = "sal_cuship";
                sqlQuery.PrimaryKey = "fid";
                sqlQuery.Order = "in_date";
                sqlQuery.SortType = 2;
                indate = "in_date";
                ArrnWhere = new string[] { "bsnno","cu_no","cu_name","p_no","pname","ship_no"};
                sqlQuery.Where = " xtcu_no='"+cu_no+"'";
            }
            /* 自定义where End */

            /* 单条数据 */
               string fid = context.Request["fid"];
                if (fid != null && fid != "")
                {
                    if (sqlQuery.Where == null || sqlQuery.Where == "")
                    {
                        sqlQuery.Where = " fid=" + fid+" ";
                    }
                    else
                    {
                        sqlQuery.Where = "("+sqlQuery.Where + ") and fid=" + fid+" ";
                        //sqlQuery.Where = "";
                    }
                }
            /* 单条数据 End */

            /*  时间范围判断  */
            if (indate != "")
            {
                string dateWhere = "";
                if (dataf != null && dataf != "")
                {
                    try { dataf=DateTime.Parse(dataf).ToString(); }
                    catch
                    {
                        context.Response.Write(Mes("开始时间格式有误"));
                        context.Response.End();
                    }
                    dateWhere =" "+ indate+" >= '" + dataf + "' ";
                }
                if (datat != null && datat != "")
                {
                    try { datat = DateTime.Parse(datat).AddDays(1).ToString(); }
                    catch
                    {
                        context.Response.Write(Mes("开始时间格式有误"));
                        context.Response.End();
                    }
                    if (dateWhere != "") dateWhere += " and ";
                    dateWhere += " " + indate + " <= '" + datat + "' ";
                }
                if (dateWhere != "" && sqlQuery.Where != "")
                {
                    sqlQuery.Where = "(" + dateWhere + ") and (" + sqlQuery.Where + ")";
                }
                else
                    sqlQuery.Where += dateWhere;
            } 
            /*  时间范围判断 End */

            /*  搜索 */
            if (query!=null&&ArrnWhere != null&&query!="")
            {
                query = FilteSQLStr(query);
                if (query.Length > 20) query = query.Substring(0, 20);
                string temArray = "";
                foreach (string t in ArrnWhere)
                {
                    if (temArray != "")
                        temArray += " or ";

                    temArray += t + " LIKE '%"+query+"%' ";
                }
                if (temArray != "" && sqlQuery.Where != "")
                {
                    sqlQuery.Where = "(" + temArray + ") and (" + sqlQuery.Where + ")";
                }
                else
                    sqlQuery.Where += temArray;
            }
            /*  搜索 End */
            context.Response.Write(Page(sqlQuery,PageSize,PageIndex));
            context.Response.End();
        }

        #region 分页方法
        /// <summary>
        /// 
        /// </summary>  
        public StringBuilder Page(SqlQuery sqlQuery, int PageSize, int PageIndex)
        {
            StringBuilder sb = new StringBuilder();
            //@TableName VARCHAR(200),     --表名          
            //@FieldList VARCHAR(2000),    --显示列名，如果是全部字段则为*          
            //@PrimaryKey VARCHAR(100),    --单一主键或唯一值键          
            //@Where VARCHAR(2000),        --查询条件 不含'where'字符，如id>10 and len(userid)>9          
            //@Order VARCHAR(1000),        --排序 不含'order by'字符，如id asc,userid desc，必须指定asc或desc          
            //--注意当@SortType=3时生效，记住一定要在最后加上主键，否则会让你比较郁闷          
            //@SortType INT,               --排序规则 1:正序asc 2:倒序desc 3:多列排序方法          
            //@RecorderCount INT,          --记录总数 0:会返回总记录          
            //@PageSize INT,               --每页输出的记录数          
            //@PageIndex INT,              --当前页数          
            //@TotalCount INT OUTPUT ,      --记返回总记录          
            //@TotalPageCount INT OUTPUT   --返回总页数 
            if (sqlQuery.TableName == "")
                return sb;
            SqlParameter[] gtparameter = new SqlParameter[11];

            gtparameter[0] = new SqlParameter("@TableName", SqlDbType.VarChar);
            gtparameter[0].Value = sqlQuery.TableName;//表名

            gtparameter[1] = new SqlParameter("@FieldList", SqlDbType.VarChar);
            gtparameter[1].Value = sqlQuery.FieldList;//列名

            gtparameter[2] = new SqlParameter("@PrimaryKey", SqlDbType.VarChar);
            gtparameter[2].Value = sqlQuery.PrimaryKey;//单一主键或唯一值键   

            gtparameter[3] = new SqlParameter("@Where", SqlDbType.VarChar);
            gtparameter[3].Value = sqlQuery.Where;//查询条件

            gtparameter[4] = new SqlParameter("@Order", SqlDbType.VarChar);
            gtparameter[4].Value = sqlQuery.Order;//排序 

            gtparameter[5] = new SqlParameter("@SortType", SqlDbType.Int);
            gtparameter[5].Value = sqlQuery.SortType;//排序规则 1:正序asc 2:倒序desc 3:多列排序方法  

            gtparameter[6] = new SqlParameter("@RecorderCount", SqlDbType.Int);
            gtparameter[6].Value = 0;//记录总数

            gtparameter[7] = new SqlParameter("@PageSize", SqlDbType.Int);
            gtparameter[7].Value = PageSize;//每页输出的记录数

            gtparameter[8] = new SqlParameter("@PageIndex", SqlDbType.Int);
            gtparameter[8].Value = PageIndex;//当前页数

            gtparameter[9] = new SqlParameter("@TotalCount", SqlDbType.Int);
            gtparameter[9].Value = 0;//记返回总记录
            gtparameter[9].Direction = ParameterDirection.Output;

            gtparameter[10] = new SqlParameter("@TotalPageCount", SqlDbType.Int);
            gtparameter[10].Value = 0;//返回总页数
            gtparameter[10].Direction = ParameterDirection.Output;

            DataSet ds = new DataSet();
            string connectionString = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["ConnectionString"]);
            SqlConnection connection = new SqlConnection(connectionString);//打开连接
            SqlCommand cmd = new SqlCommand();//执行sql或exec
            cmd.Connection = connection;
            cmd.CommandText = "P_viewPage";
            cmd.CommandType = CommandType.StoredProcedure;

            // SqlCommand com = new SqlCommand("P_viewPage", connection);

            foreach (SqlParameter parameter in gtparameter)
            cmd.Parameters.Add(parameter);

            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(ds);
            int TotalCount=0;
            int TotalPageCount = 0;
            try
            {
                TotalCount = (int)cmd.Parameters["@TotalCount"].Value;
                TotalPageCount = (int)cmd.Parameters["@TotalPageCount"].Value;
            }
            catch { }
            if (TotalCount == 0)
            {
                sb.Append(Mes("抱歉，没有符合条件的记录"));
                return sb;
            }

            if (PageIndex > TotalPageCount)
            {
                sb.Append(Mes("亲，没有更多记录了"));
                return sb;
            }
            json JSON = new json();
            sb.Append("{\"success\":true,\"Message\":\"\",\"PageIndex\":" + PageIndex + ",\"PageSize\":" + PageSize + ",\"PageCount\":"+(PageIndex*PageSize)+",\"count\":" + TotalCount + ",\"Page\":" + TotalPageCount + ",\"cont\":" + JSON.ToJson(ds.Tables[0]) + "}");
            return sb;
        }
        #endregion
        #region key返回值构造类
        /// <summary>
        /// 
        /// </summary>  
        public class SqlQuery
        {
            public string TableName { get; set; }//表名
            public string FieldList { get; set; }//列名
            public string PrimaryKey { get; set; }//主键
            public string Where { get; set; }//查询语句
            public string Order { get; set; }//排序
            public int SortType { get; set; }//排序规则
            public SqlQuery()
            {
                TableName = "";
                FieldList = "*";
                PrimaryKey = "fid";
                Where = "";
                Order = "";
                SortType = 1;
            }
        }
        #endregion

        #region 错误通用回复
        /// <summary>
        /// 
        /// </summary> 
        public string Mes(string s)
        {
            // string callback = Request["callback"];
            return "{\"success\":false,\"Message\":\"" + s + "\"}";
        }
        #endregion

        #region 过滤不安全的字符串
        /// <summary>
        /// 过滤不安全的字符串
        /// </summary>
        /// <param name="Str"></param>
        /// <returns></returns>
        public static string FilteSQLStr(string Str)
        {

            Str = Str.Replace("'", "");
            Str = Str.Replace("\"", "");
            Str = Str.Replace("&", "&amp");
            Str = Str.Replace("<", "&lt");
            Str = Str.Replace(">", "&gt");

            Str = Str.Replace("delete", "");
            Str = Str.Replace("update", "");
            Str = Str.Replace("insert", "");

            Str = SqlFilter(Str);

            return Str;
        }
         #endregion

        #region 过滤 Sql 语句字符串中的注入脚本
        /// <summary>
        /// 过滤 Sql 语句字符串中的注入脚本
        /// </summary>
        /// <param name="source">传入的字符串</param>
        /// <returns>过滤后的字符串</returns>
        public static string SqlFilter(string source)
        {
            //单引号替换成两个单引号
            source = source.Replace("'", "''");

            //半角封号替换为全角封号，防止多语句执行
            source = source.Replace(";", "；");

            //半角括号替换为全角括号
            source = source.Replace("(", "（");
            source = source.Replace(")", "）");

            ///////////////要用正则表达式替换，防止字母大小写得情况////////////////////

            //去除执行存储过程的命令关键字
            source = source.Replace("Exec", "");
            source = source.Replace("Execute", "");

            //去除系统存储过程或扩展存储过程关键字
            source = source.Replace("xp_", "x p_");
            source = source.Replace("sp_", "s p_");

            //防止16进制注入
            source = source.Replace("0x", "0 x");

            return source;
        }
        #endregion

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
