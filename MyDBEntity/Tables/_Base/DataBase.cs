using Newtonsoft.Json.Linq;
using System.Linq.Expressions;
using Util;

namespace MyDbEntity;

/// <summary>
/// 基础表基类,会附带一个修改记录表以及修改方法
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class DataBase<T> : DbBase
{
    /// <summary>
    /// 增加修改记录
    /// </summary> 1 保存2 删除3新增
    /// <param name="user">用户</param>
    /// <param name="field">字段名称</param>
    /// <param name="oldValue">旧值</param>
    /// <param name="newValue">新值</param>
    /// <param name="Type">类型,1新增,2修改,3删除</param>
    public void AddRecord(string user, string field, string oldValue, string newValue, int type)
    {
        //Record<T> Record = new Record<T>()
        //{
        //    OperateUser = user,
        //    Field = field,
        //    OldValue = oldValue,
        //    NewValue = newValue,
        //    Type = Type,
        //};
        //if (Records == null) Records = new List<Record<T>>();
        //Records.Add(Record);
        LogHelper.WriteLog($"User:{user}  ID:{Id} Type:{type}  Field:{field}  OldValue:{oldValue}  NewValue:{newValue}", "logs/mytables", typeof(T).Name + ".table");
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="user"></param>
    public void Delete(string user)
    {
        DeleteFlag = 1;
        DeleteTime = DateTime.Now;
    }

    /// <summary>
    /// 根据Json更新此条数据,返回修改的字段数量
    /// </summary>
    /// <param name="jObject"></param>
    public int Update(string user, JObject jObject)
    {
        int i = 0;
        foreach (KeyValuePair<string, JToken> keyValue in jObject)
        {
            if (keyValue.Key == "ID") continue;
            try
            {
                string field = keyValue.Key;
                var oldValue = this.GetPropertyValue(field);
                Type tpye = this.GetPropertyType(field);
                var newValue = keyValue.Value.ToObject(tpye);
                if (oldValue.TryToNumberString() != newValue.TryToNumberString())
                {
                    this.SetPropertyValue(field, newValue);
                    string oldvalue = oldValue.TryToNumberString();
                    string newvalue = newValue.TryToNumberString();
                    AddRecord(user, field, oldvalue?.Length > 255 ? oldvalue.Substring(0, 255) : oldvalue, newvalue?.Length > 255 ? newvalue.Substring(0, 255) : newvalue, 2);
                    UpdateTime = DateTime.Now;
                    i++;
                }
            }
            catch { throw new Exception(keyValue.Key + "  更新失败"); }
        }
        return i;
    }

    /// <summary>
    /// 更新指定字段
    /// </summary>
    /// <typeparam name="R"></typeparam>
    /// <param name="user"></param>
    /// <param name="json"></param>
    /// <param name="fields"></param>
    /// <returns></returns>
    public int Update<R>(string user, JObject json, Func<T, R> fields) where R : class
    {
        int i = 0;
        R r = json.ToObject<R>();
        foreach (System.Reflection.PropertyInfo propertyInfo in r.GetType().GetProperties())
        {
            string field = propertyInfo.Name;
            try
            {
                JProperty jProperty = json.Property(field, StringComparison.OrdinalIgnoreCase);
                if (jProperty == null) continue;

                Type type = this.GetPropertyType(field);
                object newValue = jProperty.Value.ToObject(type);

                string oldvalue = this.GetPropertyValue(field).TryToNumberString();
                string newvalue = newValue.TryToNumberString();

                if (oldvalue != newvalue)
                {
                    this.SetPropertyValue(field, newValue);
                    AddRecord(user, field, oldvalue?.Length > 255 ? oldvalue.Substring(0, 255) : oldvalue, newvalue?.Length > 255 ? newvalue.Substring(0, 255) : newvalue, 2);
                    UpdateTime = DateTime.Now;
                    i++;
                }
            }
            catch { throw new Exception(field + "  更新失败"); }
        }
        return i;
    }

    public int Update(string user, string field, object newValue)
    {
        int i = 0;
        try
        {
            var oldValue = this.GetPropertyValue(field);
            Type tpye = oldValue.GetType();
            if (oldValue.ToString() != newValue.ToString())
            {
                this.SetPropertyValue(field, newValue);
                string oldvalue = oldValue.ToString();
                string newvalue = newValue.ToString();
                AddRecord(user, field, oldvalue.Length > 255 ? oldvalue.Substring(0, 255) : oldvalue, newvalue.Length > 255 ? newvalue.Substring(0, 255) : newvalue, 2);
                UpdateTime = DateTime.Now;
                i++;
            }
        }
        catch { throw new Exception(field + "  更新失败"); }

        return i;
    }

    public int Update<R>(string user, Expression<Func<T, R>> lab, R newValue)
    {
        int i = 0;
        string field = lab.Body.ToString().Split('.')[1].Split(',')[0];
        try
        {
            object oldValue = this.GetPropertyValue(field);
            Type tpye = this.GetPropertyType(field);
            string oldValueStr = oldValue?.ToString();
            string newValueStr = newValue?.ToString();

            if (oldValueStr != newValueStr)
            {
                this.SetPropertyValue(field, newValue);
                AddRecord(user, field, oldValueStr, newValueStr, 2);
                UpdateTime = DateTime.Now;
                i++;
            }
        }
        catch { throw new Exception(field + "  更新失败"); }

        return i;
    }
}