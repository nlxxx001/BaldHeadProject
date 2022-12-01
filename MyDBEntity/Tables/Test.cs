using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyDbEntity.Tables;

/// <summary>
/// 测试表
/// </summary>
[Table("Test_Main")]
[Comment("测试表")]
public class Test : FileBase<Test>
{
    /// <summary>
    /// 包类型 0完整包 1差异包
    /// </summary>
    [MaxLength(63)]
    [Comment("测试字段")]
    public int Type { get; set; }
}