using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Util;

namespace MyDbEntity;

/// <summary>
/// 所有表的公共字段
/// </summary>
[Index(nameof(DeleteFlag))]
public abstract class DbBase
{
    /// <summary>
    /// 主键Id
    /// </summary>
    [Key, MaxLength(63)]
    [Comment("主键Id")]
    public string Id { get; set; } = GuidHelper.GenerateGuid();

    /// <summary>
    /// 创建时间
    /// </summary>
    [Comment("创建时间")]
    public DateTime? CreateTime { get; set; } = DateTime.Now;

    /// <summary>
    /// 更新时间
    /// </summary>
    [Comment("更新时间")]
    public DateTime? UpdateTime { get; set; }

    /// <summary>
    /// 删除时间
    /// </summary>
    [Comment("删除时间")]
    public DateTime? DeleteTime { get; set; }

    /// <summary>
    /// 删除标志: 0正常,1删除
    /// </summary>
    [DefaultValue(0)]
    [Comment("删除标志: 0正常,1删除")]
    public int DeleteFlag { get; set; }

    /// <summary>
    /// 操作人id
    /// </summary>
    [MaxLength(63)]
    [Comment("操作人id")]
    public string OperatorId { get; set; }

    /// <summary>
    /// 操作人姓名
    /// </summary>
    [MaxLength(63)]
    [Comment("操作人姓名")]
    public string OperatorName { get; set; }

    /// <summary>
    /// 创建人id
    /// </summary>
    [MaxLength(63)]
    [Comment("创建人id")]
    public string CreateId { get; set; }

    /// <summary>
    /// 创建人姓名
    /// </summary>
    [MaxLength(63)]
    [Comment("创建人姓名")]
    public string CreateName { get; set; }
}