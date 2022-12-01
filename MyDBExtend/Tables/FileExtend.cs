using DTO.Comm;
using MyDbEntity;
using MyDbEntity.Tables;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyDBExtend.Tables
{
    public static class FileExtend
    {
        /// <summary>
        /// 获取文件对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="file"></param>
        /// <returns></returns>
        public static FileModel GetFile<T>(this FileBase<T> file)
        {
            return file == null ? null : new FileModel
            {
                Id = file.Id,
                Name = file.Name,
                FileName = file.OssName,
                FileBucket = file.OssBucket,
                FilePath = file.OssPath,
                FileExtention = file.OssExtention,
                FileMD5 = file.FileMD5,
            };
        }
    }
}
