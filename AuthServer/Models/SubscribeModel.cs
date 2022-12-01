namespace AuthServer.Models;

public class SubscribeModel
{
    /// <summary>
    /// 发起设备名称
    /// </summary>
    public string EquipmentID { get; set; }

    /// <summary>
    /// 操作类型 
    /// </summary>
    public ActivityType ActivityType { get; set; }

    /// <summary>
    /// 信息
    /// </summary>
    public string Message { get; set; }
}