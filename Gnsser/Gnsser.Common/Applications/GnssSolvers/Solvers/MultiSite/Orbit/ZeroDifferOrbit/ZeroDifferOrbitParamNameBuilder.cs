﻿//2018.10.24, czs, create in hmx, 非差轨道确定


using System;
using System.Collections.Generic;
using System.Text;
using Gnsser.Domain;
using Gnsser.Data.Sinex;
using Gnsser.Data.Rinex;
using Gnsser.Times;
using Geo.Algorithm;
using Geo.Coordinates;
using  Geo.Algorithm.Adjust;
using Geo;

namespace Gnsser.Service
{
    /// <summary>
    /// 非差轨道确定参数命名器
    /// </summary>
    public class ZeroDifferOrbitParamNameBuilder : RangeOrbitParamNameBuilder
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ZeroDifferOrbitParamNameBuilder( GnssProcessOption option)  :base(option){ }
           
        /// <summary>
        /// 生成
        /// </summary>
        /// <returns></returns>
        public override List<string> Build()
        {
            List<string> paramNames = new List<string>();
            var mInfo = MultiSiteEpochInfo;
            //卫星坐标
            foreach (var prn in this.EnabledPrns)
            {
                paramNames.AddRange(GetSatDxyz(prn));
            }
            //卫星钟差
            foreach (var prn in this.EnabledPrns)
            {
                paramNames.Add(this.GetSatClockParamName(prn));
            }
            //接收机钟差
            foreach (var site in mInfo)
            {
                paramNames.Add(GetReceiverClockParamName(site.SiteName));
            }
            //对流层天定距湿延迟
            foreach (var site in mInfo)
            {
                paramNames.Add(GetSiteWetTropZpdName(site.SiteName));
            }
            //模糊度参数
            foreach (var site in mInfo)
            {
                foreach (var prn in site.EnabledPrns)
                {
                    paramNames.Add(GetSiteSatAmbiguityParamName(site.SiteName, prn));
                }
            }
            return paramNames;
        } 

        public override  string GetParamName(SatelliteNumber sat)
        {
            return GetSatClockParamName(sat);
        }

    }
}