namespace DTO.Comm;

/// <summary>
/// 文件模型
/// </summary>
public class FileModel : IDNameDTO
{
    /// <summary>
    /// 文件名
    /// </summary>
    public string FileName { get; set; }
    /// <summary>
    /// 桶
    /// </summary>
    public string FileBucket { get; set; }
    /// <summary>
    /// 文件路径
    /// </summary>
    public string FilePath { get; set; }
    /// <summary>
    /// 扩展
    /// </summary>
    public string FileExtention { get; set; }
    /// <summary>
    /// 文件MD5
    /// </summary>
    public string FileMD5 { get; set; }
}

/// <summary>
/// 入参文件模型
/// </summary>
public class InFile
{
    /// <summary>
    /// 文件名
    /// </summary>
    public string FileName { get; set; }

    /// <summary>
    /// 入参MD5
    /// </summary>
    public string MD5 { get; set; }
}