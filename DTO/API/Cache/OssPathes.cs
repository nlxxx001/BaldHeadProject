using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.API.Cache
{
    public class OssPathes
    {
        public OssPath File1 { get; set; } = new OssPath("bucket", "path", "extention", "注释");
        public OssPath File2 { get; set; } = new OssPath("bucket", "path", "extention", "注释");
    }
    public class OssPath
    {
        public OssPath(string bucket, string path, string extention, string explain)
        {
            Bucket = bucket;
            Path = path;
            Extention = extention;
            Explain = explain;
        }

        /// <summary>
        /// Bucket
        /// </summary>
        public string Bucket { get; set; }

        /// <summary>
        /// 路径
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 扩展名
        /// </summary>
        public string Extention { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        public string Explain { get; set; }
    }
}
