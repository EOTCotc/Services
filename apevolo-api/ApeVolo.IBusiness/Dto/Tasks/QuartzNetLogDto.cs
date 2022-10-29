﻿using ApeVolo.Common.AttributeExt;
using ApeVolo.Entity.Do.Tasks;

namespace ApeVolo.IBusiness.Dto.Tasks;

[AutoMapping(typeof(QuartzNetLog), typeof(QuartzNetLogDto))]
public class QuartzNetLogDto : EntityDtoRoot<long>
{
    /// <summary>
    /// 任务ID
    /// </summary>
    public string TaskId { get; set; }

    /// <summary>
    /// 任务名称
    /// </summary>
    public string TaskName { get; set; }

    /// <summary>
    /// 任务分组
    /// </summary>
    public string TaskGroup { get; set; }

    /// <summary>
    /// 程序集名称
    /// </summary>
    public string AssemblyName { get; set; }

    /// <summary>
    /// 任务所在类
    /// </summary>
    public string ClassName { get; set; }

    /// <summary>
    /// cron 表达式
    /// </summary>
    public string Cron { get; set; }

    /// <summary>
    /// 异常详情
    /// </summary>
    public string ExceptionDetail { get; set; }

    /// <summary>
    /// 执行耗时（毫秒）
    /// </summary>
    public long ExecutionDuration { get; set; }

    /// <summary>
    /// 执行传参
    /// </summary>
    public string RunParams { get; set; }

    /// <summary>
    /// 是否成功
    /// </summary>
    public bool IsSuccess { get; set; }
}