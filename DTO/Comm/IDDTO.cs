namespace DTO.Comm;

/// <summary>
/// 包含ID的模型
/// </summary>
public class IDDTO
{
    /// <summary>
    /// ID
    /// </summary>
    public string Id { get; set; }
}

/// <summary>
/// 包含ID和Name的模型
/// </summary>
public class IDNameDTO : IDDTO
{
    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; }
}