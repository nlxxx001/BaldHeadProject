using LeYiXue.Common.Util;
using System.Collections.Concurrent;

namespace Util;

/// <summary>
/// 日志帮助类
/// </summary>
public static class LogHelper
{
    private static readonly ConcurrentQueue<LogModel> _que = new ConcurrentQueue<LogModel>();

    private static readonly ManualResetEvent _mre = new ManualResetEvent(false);

    private static Thread _thread = new Thread(new ThreadStart(WriteLog)) { IsBackground = true, Priority = ThreadPriority.BelowNormal };

    /// <summary>
    /// 启动日志线程
    /// </summary>
    static LogHelper() => _thread.Start();

    /// <summary>
    /// 写日志
    /// </summary>
    public static void WriteLog(string log)
    {
        _que.Enqueue(new LogModel
        {
            Message = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}:{log}\r\n",
            Path = "logs/",
            FileName = DateTime.Now.ToString("yyyy_MM_dd") + ".log",
            LogType = LogMessageExtendType.Warn,
        });
        _mre.Set();
    }

    /// <summary>
    /// 写日志
    /// </summary>
    /// <param name="log"></param>
    /// <param name="path"></param>
    public static void WriteErrorLog(string log, string path, string fileName)
    {
        _que.Enqueue(new LogModel
        {
            Message = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}:{log}\r\n",
            Path = path,
            FileName = DateTime.Now.ToString("yyyy_MM_dd") + ".log",
            LogType = LogMessageExtendType.Error,
        });
        _mre.Set();
    }

    public static void WriteLog(string log, string path, string fileName)
    {
        _que.Enqueue(new LogModel
        {
            Message = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}:{log}\r\n",
            Path = path,
            FileName = fileName,
            LogType = LogMessageExtendType.Info,
        });
        _mre.Set();
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="serviceName">服务名称</param>
    /// <param name="methodName">方法名</param>
    /// <param name="type">日志类型</param>
    /// <param name="body">消息内容</param>
    /// <param name="filter1">过滤1</param>
    /// <param name="filter2"></param>
    public static void WriteContent(string serviceName, string methodName, LogMessageExtendType type, string body, string filter1, string filter2 = "")
    {
        GlobalStaticService.loggerService?.WriteCurrency("LogHelper", serviceName, methodName, type, body, filter1, filter2);
    }

    private static void WriteLog()
    {
        while (true)
        {
            _mre.WaitOne();
            while (_que.Count > 0 && _que.TryDequeue(out LogModel log))
            {
                //string dirPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, log.Path);

                GlobalStaticService.loggerService?.WriteCurrency("LogHelper", "LogHelper", "WriteLog", log.LogType, log.Message, log.Path.Trim('/'), log.TraceId ?? "");
                //if (!Directory.Exists(dirPath)) Directory.CreateDirectory(dirPath);
                //string filePath = Path.Combine(dirPath, log.FileName);
                //if (!File.Exists(filePath)) File.Create(filePath).Close();
                //try
                //{
                //    File.AppendAllText(filePath, log.Message);
                //}
                //catch
                //{
                //    File.AppendAllText(filePath + "__FailWrite", log.Message);
                //}
            }
            _mre.Reset();
        }
    }
}

public class LogModel
{
    /// <summary>
    /// 目录
    /// </summary>
    public string Path { get; set; }

    /// <summary>
    /// 文件名
    /// </summary>
    public string FileName { get; set; }

    /// <summary>
    /// 信息
    /// </summary>
    public string Message { get; set; }

    public LogMessageExtendType LogType { get; set; }
    /// <summary>
    /// 跟踪值
    /// </summary>
    public string TraceId { get; set; }
}