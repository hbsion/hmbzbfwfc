using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using hmbzbfwfc.Models;
using System.Data.SqlClient;
using System.Data;

namespace hmbzbfwfc.Commons
{
    public class PagesImpl
    {

        //数据库上下文实例
        private ConnectionStringEntities db;

        public PagesImpl(ConnectionStringEntities dbContext)
        {
            this.db = dbContext;
        }

        /// <summary>
        /// 读取分页数据
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="pageinationInfo">分页信息类</param>
        /// <returns>分页数据</returns>
        public PageinationData GetPageinationData<T>(PageinationInfo pageinationInfo) where T : class
        {
            dynamic result = null;
            try
            {
                #region SqlParameter参数
                SqlParameter[] paras = new SqlParameter[8];
                //页索引
                paras[0] = new SqlParameter("pageIndex", DbType.Int32);
                paras[0].Value = pageinationInfo.PageIndex;
                //页大小
                paras[1] = new SqlParameter("pageSize", DbType.Int32);
                paras[1].Value = pageinationInfo.PageSize;
                //表名
                paras[2] = new SqlParameter("tableName", DbType.String);
                paras[2].Value = pageinationInfo.TableName;
                //查询字段
                //EF仅支持返回返回某个表的全部字段，以便转换成对应的实体，无法支持返回部分字段的情况...？？？
                paras[3] = new SqlParameter("fieldName", DbType.String);
                paras[3].Value = pageinationInfo.FieldName;
                //where条件
                paras[4] = new SqlParameter("whereCondition", DbType.String);
                paras[4].Value = pageinationInfo.WhereCondition;
                //order条件
                paras[5] = new SqlParameter("orderCondition", DbType.String);
                paras[5].Value = pageinationInfo.OrderCondition;
                //总数
                paras[6] = new SqlParameter("totalCount", DbType.Int32);
                paras[6].Value = pageinationInfo.TotalCount;
                paras[6].Direction = ParameterDirection.Output;
                //总页数
                paras[7] = new SqlParameter("totalPages", DbType.Int32);
                paras[7].Value = pageinationInfo.TotalPages;
                paras[7].Direction = ParameterDirection.Output;
                #endregion

                string sql = "P_SpiltPage @pageIndex,@pageSize,@tableName,@fieldName,@whereCondition,@orderCondition,@totalCount output,@totalPages output";
                var list = db.Database.SqlQuery<T>(sql, paras).ToList();

                PageinationData data = new PageinationData();
                data.TotalCount = (int)paras[6].Value;
                data.TotalPages = (int)paras[7].Value;
                data.DataList = list;

                result = data;
            }
            catch (Exception e)
            {
                throw;
            }

            return result;
        }

        public PageinationData GetPageDate<T>(string tableName, int pageno, int pagesize, string fieldName, string order, string where) where T : class
        {
            PagesImpl pageImpl = new PagesImpl(db);
            PageinationInfo pageination = new PageinationInfo();
            pageination.TableName = tableName;
            pageination.PageIndex = pageno;
            pageination.PageSize = pagesize;
            pageination.FieldName = fieldName;
            pageination.OrderCondition = order;
            pageination.WhereCondition = where;
            var data = pageImpl.GetPageinationData<T>(pageination);
            return data;
        }

        /// <summary>
        /// 读取分页数据
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="pageinationInfo">分页信息类</param>
        /// <returns>分页数据</returns>
        public PageinationData2 GetPageinationData1<T>(PageinationInfo2 pageinationInfo) where T : class
        {
            dynamic result = null;
            try
            {
                #region SqlParameter参数
                SqlParameter[] paras = new SqlParameter[11];
                //表名
                paras[0] = new SqlParameter("TableName", DbType.String);
                paras[0].Value = pageinationInfo.TableName;
                //查询字段
                //EF仅支持返回返回某个表的全部字段，以便转换成对应的实体，无法支持返回部分字段的情况...？？？
                paras[1] = new SqlParameter("FieldList", DbType.String);
                paras[1].Value = pageinationInfo.FieldList;

                paras[2] = new SqlParameter("PrimaryKey", DbType.String);
                paras[2].Value = pageinationInfo.PrimaryKey;

                //where条件
                paras[3] = new SqlParameter("Where", DbType.String);
                paras[3].Value = pageinationInfo.Where;
                //order条件
                paras[4] = new SqlParameter("Order", DbType.String);
                paras[4].Value = pageinationInfo.Order;

                paras[5] = new SqlParameter("SortType", DbType.Int32);
                paras[5].Value = pageinationInfo.SortType;

                paras[6] = new SqlParameter("RecorderCount", DbType.Int32);
                paras[6].Value = pageinationInfo.RecorderCount;
                //页大小
                paras[7] = new SqlParameter("PageSize", DbType.Int32);
                paras[7].Value = pageinationInfo.PageSize;
                //页索引
                paras[8] = new SqlParameter("PageIndex", DbType.Int32);
                paras[8].Value = pageinationInfo.PageIndex;

                //总数
                paras[9] = new SqlParameter("TotalCount", DbType.Int32);
                paras[9].Value = pageinationInfo.TotalCount;
                paras[9].Direction = ParameterDirection.Output;
                //总页数
                paras[10] = new SqlParameter("TotalPageCount", DbType.Int32);
                paras[10].Value = pageinationInfo.TotalPageCount;
                paras[10].Direction = ParameterDirection.Output;
                #endregion

                string sql = "P_viewPage @TableName,@FieldList,@PrimaryKey,@Where,@Order,@SortType,@RecorderCount,@PageSize,@PageIndex,@TotalCount output,@TotalPageCount output";
                var list = db.Database.SqlQuery<T>(sql, paras).ToList();

                PageinationData2 data = new PageinationData2();
                data.TotalCount = (int)paras[9].Value;
                data.TotalPages = (int)paras[10].Value;
                data.DataList = list;

                result = data;
            }
            catch (Exception e)
            {
                throw;
            }

            return result;
        }

        /// <summary>
        /// 数据导入
        /// </summary>       
        public string PackDataImport(string txtfpath, string txtpack_no, string filetype, DateTime add_date, string cu_no)
        {
            try
            {

                db.Database.CommandTimeout = 3600;
                string sql = "exec psal_pack_cl '" + txtfpath + "','" + txtpack_no + "','" + filetype + "','" + add_date.ToString("yyyy-MM-dd HH:mm:ss.ff") + "','" + cu_no + "'";
                var list = db.Database.ExecuteSqlCommand(sql);

                return "ok";
            }
            catch (Exception e)
            {
                return e.Message;
            }

        }

        /// <summary>
        /// 数据导入
        /// </summary>       
        public string CuPackDataImport(string txtfpath, string txtpack_no, string filetype, string cu_no)
        {
            try
            {

                db.Database.CommandTimeout = 3600;
                string sql = "exec psal_cupack_cl '" + txtfpath + "','" + txtpack_no + "','" + filetype + "','" + cu_no + "' ";
                var list = db.Database.ExecuteSqlCommand(sql);

                return "ok";
            }
            catch (Exception e)
            {
                return e.Message;
            }

        }

        /// <summary>
        /// 数据撤销删除
        /// </summary>       
        public int FileDataDel(string txtfpath, string txtpack_no, string filetype, DateTime add_date)
        {
            try
            {


                string sql = "exec filedata_del '" + txtfpath + "','" + txtpack_no + "','" + filetype + "','" + add_date.ToString("yyyy-MM-dd HH:mm:ss.ff") + "'";
                var list = db.Database.ExecuteSqlCommand(sql);
                return list;

            }
            catch (Exception e)
            {
                return 0;
                throw;
            }

        }

        public string PackDataBatchImport(string filetype, DateTime date)
        {
            try
            {

                db.Database.CommandTimeout = 3600;
                string sql = "exec file_plcl '" + filetype + "','" + date.ToString("yyyy-MM-dd 00:00:00") + "'";
                var list = db.Database.ExecuteSqlCommand(sql);

                return "ok";
            }
            catch (Exception e)
            {
                return e.Message;
            }

        }

    }
}