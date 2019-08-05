using Dapper;
using JIYITECH.WebApi.Configs;
using ServiceStack.DataAnnotations;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace JIYITECH.WebApi.Entities
{
    /// <summary>
    /// DB表基础属性
    /// </summary>
    public abstract class BaseEntity<T>
    {
        public BaseEntity()
        {

        }
        /// <summary>
        /// 主键Id
        /// </summary>

        [PrimaryKey, DataMember]
        [AutoIncrement]
        [Alias("id")]
        public T id { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Default("getdate()"), ReadOnly(true)]
        public DateTime? createTime { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string createBy { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? updateTime { get; set; }
        /// <summary>
        /// 更新人
        /// </summary>
        public string updateBy { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string description { get; set; }
    }
}
