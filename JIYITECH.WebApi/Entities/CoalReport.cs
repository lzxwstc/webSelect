using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JIYITECH.WebApi.Entities
{
    public class CoalReport
    {
        /// <summary>
        /// 化验数据类型
        /// </summary>
        public enum CoalReportType : int
        {
            /// <summary>
            /// 前港化验
            /// </summary>
            LastPort = 0,
            /// <summary>
            /// 厂内化验
            /// </summary>
            Local = 1,
            /// <summary>
            /// 第三方化验
            /// </summary>
            ThirdPart = 2,
        }
        /// <summary>
        /// 煤种Id
        /// </summary>
        public long coalVarietyId { get; set; }
        /// <summary>
        /// 煤种  
        /// </summary>
        public CoalVariety coalVariety { get; set; }
        /// <summary>
        /// 煤价
        /// </summary>
        public float? coalPrice { get; set; }
        /// <summary>
        /// 煤船Id
        /// </summary>
        public long shipId { get; set; }
        /// <summary>
        /// 煤船
        /// </summary>
        public Ship ship { get; set; }
        /// <summary>
        /// 船次
        /// </summary>
        public int shipTime { get; set; }
        /// <summary>
        /// 化验单类型
        /// </summary>
        public CoalReportType type { get; set; }
        /// <summary>
        /// 船运单Id
        /// </summary>
        public long shipListId { get; set; }
        /// <summary>
        /// 船运单
        /// </summary>
        public ShipList shipList { get; set; }
        /// <summary>
        /// 化验数据有效
        /// </summary>
        public bool validate { get; set; }
        /// <summary>
        /// 煤样Id
        /// </summary>
        public long coalSampleId { get; set; }
        /// <summary>
        /// 煤样
        /// </summary>
        public CoalSample coalSample { get; set; }

        #region 热值
        /// <summary>
        /// 收到基低位发热量（MJ/KG）
        /// </summary>
        public float? qnetarMJ { get; set; }
        /// <summary>
        /// 收到基高位发热量（MJ/KG）
        /// </summary>
        public float? qgrarMJ { get; set; }
        /// <summary>
        /// 收到基低位发热量（kcal/kg）
        /// </summary>
        public float? qnetarKcal { get; set; }
        /// <summary>
        /// 收到基高位发热量（kcal/kg）
        /// </summary>
        public float? qgrarKcal { get; set; }
        /// <summary>
        /// 空气干燥基低位发热量（MJ/KG）
        /// </summary>
        public float? qnetadMJ { get; set; }
        /// <summary>
        /// 空气干燥基高位发热量（MJ/KG）
        /// </summary>
        public float? qgradMJ { get; set; }
        /// <summary>
        /// 空气干燥基低位发热量（kcal/kg）
        /// </summary>
        public float? qnetadKcal { get; set; }
        /// <summary>
        /// 空气干燥基高位发热量（kcal/kg）
        /// </summary>
        public float? qgradKcal { get; set; }
        /// <summary>
        /// 干燥基低位发热量（MJ/KG）
        /// </summary>
        public float? qnetdMJ { get; set; }
        /// <summary>
        /// 干燥基高位发热量（MJ/KG）
        /// </summary>
        public float? qgrdMJ { get; set; }
        /// <summary>
        /// 干燥基低位发热量（kcal/kg）
        /// </summary>
        public float? qnetdKcal { get; set; }
        /// <summary>
        /// 干燥基高位发热量（kcal/kg）
        /// </summary>
        public float? qgrdKcal { get; set; }
        /// <summary>
        /// 干燥无灰基低位发热量（MJ/KG）
        /// </summary>
        public float? qnetdafMJ { get; set; }
        /// <summary>
        /// 干燥无灰基高位发热量（MJ/KG）
        /// </summary>
        public float? qgrdafMJ { get; set; }
        /// <summary>
        /// 干燥无灰基低位发热量（kcal/kg）
        /// </summary>
        public float? qnetdafKcal { get; set; }
        /// <summary>
        /// 干燥无灰基高位发热量（kcal/kg）
        /// </summary>
        public float? qgrdafKcal { get; set; }
        #endregion

        #region 元素分析
        /// <summary>
        /// 空气干燥基碳含量
        /// </summary>
        public float? cad { get; set; }
        /// <summary>
        /// 收到基碳含量
        /// </summary>
        public float? car { get; set; }
        /// <summary>
        /// 干燥基碳含量
        /// </summary>
        public float? cd { get; set; }
        /// <summary>
        /// 干燥无灰基碳含量
        /// </summary>
        public float? cdaf { get; set; }
        /// <summary>
        /// 空气干燥基氢含量
        /// </summary>
        public float? had { get; set; }
        /// <summary>
        /// 收到基氢含量
        /// </summary>
        public float? har { get; set; }
        /// <summary>
        /// 干燥基氢含量
        /// </summary>
        public float? hd { get; set; }
        /// <summary>
        /// 干燥无灰基氢含量
        /// </summary>
        public float? hdaf { get; set; }
        /// <summary>
        /// 干燥基氮含量
        /// </summary>
        public float? nd { get; set; }
        /// <summary>
        /// 空气干燥基氮含量
        /// </summary>
        public float? nad { get; set; }
        /// <summary>
        /// 收到基氮含量
        /// </summary>
        public float? nar { get; set; }
        /// <summary>
        /// 干燥无灰基氮含量
        /// </summary>
        public float? ndaf { get; set; }
        /// <summary>
        /// 空气干燥基氧含量
        /// </summary>
        public float? oad { get; set; }
        /// <summary>
        /// 收到基氧含量
        /// </summary>
        public float? oar { get; set; }
        /// <summary>
        /// 干燥基氧含量
        /// </summary>
        public float? od { get; set; }
        /// <summary>
        /// 干燥无灰基氧含量
        /// </summary>
        public float? odaf { get; set; }
        /// <summary>
        /// 干燥基硫含量
        /// </summary>
        public float? std { get; set; }
        /// <summary>
        /// 收到基硫含量
        /// </summary>
        public float? star { get; set; }
        /// <summary>
        /// 空气干燥基硫含量
        /// </summary>
        public float? stad { get; set; }
        /// <summary>
        /// 干燥无灰基硫含量
        /// </summary>
        public float? stdaf { get; set; }
        #endregion

        #region 灰熔融性
        /// <summary>
        /// 软化温度
        /// </summary>
        public int? st { get; set; }
        /// <summary>
        /// 变形温度
        /// </summary>
        public int? dt { get; set; }
        /// <summary>
        /// 流动温度
        /// </summary>
        public int? ft { get; set; }
        /// <summary>
        /// 半球温度
        /// </summary>
        public int? ht { get; set; }
        #endregion

        #region 工业分析
        /// <summary>
        /// 干燥基灰分
        /// </summary>
        public float? ad { get; set; }
        /// <summary>
        /// 空气干燥基灰分
        /// </summary>
        public float? aad { get; set; }
        /// <summary>
        /// 收到基灰分
        /// </summary>
        public float? aar { get; set; }
        /// <summary>
        /// 干燥基固定碳
        /// </summary>
        public float? fcd { get; set; }
        /// <summary>
        /// 干燥无灰基固定碳
        /// </summary>
        public float? fcdaf { get; set; }
        /// <summary>
        /// 收到基固定碳
        /// </summary>
        public float? fcar { get; set; }
        /// <summary>
        /// 空气干燥基固定碳
        /// </summary>
        public float? fcad { get; set; }
        /// <summary>
        /// 干燥无灰基挥发分
        /// </summary>
        public float? vdaf { get; set; }
        /// <summary>
        /// 空气干燥基挥发分
        /// </summary>
        public float? vad { get; set; }
        /// <summary>
        /// 干燥基挥发分
        /// </summary>
        public float? vd { get; set; }
        /// <summary>
        /// 收到基挥发分
        /// </summary>
        public float? var { get; set; }
        /// <summary>
        /// 内水分
        /// </summary>
        public float? mad { get; set; }
        /// <summary>
        /// 全水分
        /// </summary>
        public float? mt { get; set; }
        #endregion

        #region 焦渣特性
        public int crc { get; set; }
        #endregion

        #region 可磨性系数
        public float hgi { get; set; }
        #endregion
    }
}
