using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Comm
{
    /// <summary>
    /// 学期数据
    /// </summary>
    public class TermDataOutDto
    {
        /// <summary>
        /// 学期ID
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 学期code所有系统里面存储，统一用学期Code
        /// </summary>
        public int Code { get; set; }
        /// <summary>
        ///学期名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Alias { get; set; }
        /// <summary>
        /// 学年ID
        /// </summary>
        public string SchoolYearId { get; set; }
        /// <summary>
        /// 所属年度
        /// </summary>
        public int Year { get; set; }
        /// <summary>
        /// 1春季学期 2秋季学期
        /// </summary>
        public int Type { get; set; }
    }


}
