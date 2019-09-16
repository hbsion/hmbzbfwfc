using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace hmbzbfwfc.Models
{
  
        /// <summary>
        /// 分页信息类
        /// </summary>
        public class PageinationInfo
        {
            /// <summary>
            /// 页索引
            /// </summary>            
            public int PageIndex { get; set; }
            /// <summary>
            /// 页大小
            /// </summary>
            public int PageSize { get; set; }
            /// <summary>
            /// 总数
            /// </summary>
            public int TotalCount { get; set; }
            /// <summary>
            /// 总页数
            /// </summary>
            public int TotalPages { get; set; }
            /// <summary>
            /// 表名
            /// </summary>
            public string TableName { get; set; }
            /// <summary>
            /// 主键
            /// </summary>
            public string KeyName { get; set; }
            /// <summary>
            /// 查询字段
            /// </summary>
            public string FieldName { get; set; }
            /// <summary>
            /// where条件（不带'where'）
            /// </summary>
            public string WhereCondition { get; set; }
            /// <summary>
            /// 排序条件，如：id desc ，或者：id desc,name asc
            /// </summary>
            public string OrderCondition { get; set; }
        }

        /// <summary>
        /// 分页数据类
        /// 用以将分页集合和总数，总页数等打包一起返回
        /// </summary>
        public class PageinationData
        {
            /// <summary>
            /// 总数
            /// </summary>
            public int TotalCount { get; set; }
            /// <summary>
            /// 总页数
            /// </summary>
            public int TotalPages { get; set; }

            /// <summary>
            /// 集合
            /// </summary>
            public dynamic DataList { get; set; }
        }

        /// <summary>
        /// 分页信息类
        /// </summary>
        public class PageinationInfo2
        {
            /// <summary>
            /// 页索引
            /// </summary>            
            public int PageIndex { get; set; }
            /// <summary>
            /// 页大小
            /// </summary>
            public int PageSize { get; set; }
            /// <summary>
            /// 总数
            /// </summary>
            public int TotalCount { get; set; }
            /// <summary>
            /// 总页数
            /// </summary>
            public int TotalPageCount { get; set; }
            /// <summary>
            /// 表名
            /// </summary>
            public string TableName { get; set; }
            /// <summary>
            /// 主键
            /// </summary>
            public string PrimaryKey { get; set; }
            /// <summary>
            /// 查询字段
            /// </summary>
            public string FieldList { get; set; }
            /// <summary>
            /// where条件（不带'where'）
            /// </summary>
            public string Where { get; set; }
            /// <summary>
            /// 排序条件，如：id desc ，或者：id desc,name asc
            /// </summary>
            public string Order { get; set; }

            public int SortType { get; set; }//           --排序规则 1:正序asc 2:倒序desc 3:多列排序方法          
            public int RecorderCount { get; set; }//          --记录总数 0:会返回总记录      
        }

        /// <summary>
        /// 分页数据类
        /// 用以将分页集合和总数，总页数等打包一起返回
        /// </summary>
        public class PageinationData2
        {
            /// <summary>
            /// 总数
            /// </summary>
            public int TotalCount { get; set; }
            /// <summary>
            /// 总页数
            /// </summary>
            public int TotalPages { get; set; }

            /// <summary>
            /// 集合
            /// </summary>
            public dynamic DataList { get; set; }
        }
}