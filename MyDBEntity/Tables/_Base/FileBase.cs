using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace MyDbEntity;

/// <summary>
/// 文件表基表
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class FileBase<T> : DataBase<T>
{
    /// <summary>
    /// 文件显示名称
    /// </summary>
    [MaxLength(127)]
    [Comment("文件显示名称")]
    public string Name { get; set; }

    /// <summary>
    /// 区域
    /// </summary>
    [MaxLength(31)]
    [Comment("区域")]
    public string OssRegion { get; set; }

    /// <summary>
    /// Bucket
    /// </summary>
    [MaxLength(31)]
    [Comment("Bucket")]
    public string OssBucket { get; set; }

    /// <summary>
    /// OSS存储路径
    /// </summary>
    [MaxLength(63)]
    [Comment("OSS存储路径")]
    public string OssPath { get; set; }

    /// <summary>
    /// OSS存储文件名
    /// </summary>
    [MaxLength(63)]
    [Comment("OSS存储文件名")]
    public string OssName { get; set; }

    /// <summary>
    /// 文件扩展名
    /// </summary>
    [MaxLength(15)]
    [Comment("文件扩展名")]
    public string OssExtention { get; set; }

    /// <summary>
    /// 文件类型, 1 RTF, 2 图片, 3 PPT,PPTX, 4 音频, 5 视频 , 6 LOGO , 7 Json ,8 语音标准文件 9 DOC,DOCX  10 PDF,  11 ZIP(H5)
    /// </summary>
    [Comment("文件类型, 1 RTF, 2 图片, 3 PPT,PPTX, 4 音频, 5 视频 , 6 LOGO , 7 Json ,8 语音标准文件 9 DOC,DOCX  10 PDF,  11 ZIP(H5)")]
    public int FileType { get; set; }

    /// <summary>
    /// 文件大小
    /// </summary>
    [Comment("文件大小")]
    public long Size { get; set; }

    /// <summary>
    /// 文件MD5
    /// </summary>
    [MaxLength(63)]
    [Comment("文件MD5")]
    public string FileMD5 { get; set; }

    /// <summary>
    /// 加密文件的MD5（为空表示未加密）
    /// </summary>
    [MaxLength(63)]
    [Comment("加密文件的MD5（为空表示未加密）")]
    public string EncryptedFileMD5 { get; set; }
}