using DTO.API.Cache;
using MyDbEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicLibrary.Comm
{
    public static class FileServer
    {
        public static T CreateFile<T>(this OssPath file, string fileName, string MD5 = null, string extention = null) where T : FileBase<T>, new()
        {
            return new T
            {
                OssBucket = file.Bucket,
                OssPath = file.Path,
                OssExtention = extention ?? file.Extention,
                OssName = fileName,
                FileMD5 = MD5 ?? fileName,
            };
        }
    }
}
