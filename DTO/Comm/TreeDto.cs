namespace DTO.Comm;

/// <summary>
/// 树形结构DTO
/// </summary>
public class TreeDto : IDNameDTO
{
    /// <summary>
    /// 子集
    /// </summary>
    public IEnumerable<TreeDto> Children { get; set; }
}