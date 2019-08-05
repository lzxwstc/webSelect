﻿using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;

namespace JIYITECH.WebApi.Tasks
{
    // Responsible for starting and gracefully stopping the scheduler.
    public class QuartzStartup
    {
        //private IScheduler _scheduler; // after Start, and until shutdown completes, references the scheduler object

        //// starts the scheduler, defines the jobs and the triggers
        //public void Start()
        //{
        //    if (_scheduler != null)
        //    {
        //        throw new InvalidOperationException("Already started.");
        //    }

        //    var properties = new NameValueCollection
        //    {
        //        // json serialization is the one supported under .NET Core (binary isn't)
        //        ["quartz.serializer.type"] = "json",

        //        // the following setup of job store is just for example and it didn't change from v2
        //        ["quartz.jobStore.type"] = "Quartz.Impl.AdoJobStore.JobStoreTX, Quartz",
        //        ["quartz.jobStore.useProperties"] = "false",
        //        ["quartz.jobStore.dataSource"] = "default",
        //        ["quartz.jobStore.tablePrefix"] = "QRTZ_",
        //        ["quartz.jobStore.driverDelegateType"] = "Quartz.Impl.AdoJobStore.SqlServerDelegate, Quartz",
        //        ["quartz.dataSource.default.provider"] = "SqlServer-41", // SqlServer-41 is the new provider for .NET Core
        //        ["quartz.dataSource.default.connectionString"] = @"Server=(localdb)\MSSQLLocalDB;Database=Quartz;Integrated Security=true"
        //    };

        //    var schedulerFactory = new StdSchedulerFactory(properties);
        //    _scheduler = schedulerFactory.GetScheduler().Result;
        //    _scheduler.Start().Wait();

        //    var userEmailsJob = JobBuilder.Create<SendUserEmailsJob>()
        //        .WithIdentity("SendUserEmails")
        //        .Build();
        //    var userEmailsTrigger = TriggerBuilder.Create()
        //        .WithIdentity("UserEmailsCron")
        //        .StartNow()
        //        .WithCronSchedule("0 0 17 ? * MON,TUE,WED")
        //        .Build();

        //    _scheduler.ScheduleJob(userEmailsJob, userEmailsTrigger).Wait();

        //    var adminEmailsJob = JobBuilder.Create<SendAdminEmailsJob>()
        //        .WithIdentity("SendAdminEmails")
        //        .Build();
        //    var adminEmailsTrigger = TriggerBuilder.Create()
        //        .WithIdentity("AdminEmailsCron")
        //        .StartNow()
        //        .WithCronSchedule("0 0 9 ? * THU,FRI")
        //        .Build();

        //    _scheduler.ScheduleJob(adminEmailsJob, adminEmailsTrigger).Wait();
        //}

        //// initiates shutdown of the scheduler, and waits until jobs exit gracefully (within allotted timeout)
        //public void Stop()
        //{
        //    if (_scheduler == null)
        //    {
        //        return;
        //    }

        //    // give running jobs 30 sec (for example) to stop gracefully
        //    if (_scheduler.Shutdown(waitForJobsToComplete: true).Wait(30000))
        //    {
        //        _scheduler = null;
        //    }
        //    else
        //    {
        //        // jobs didn't exit in timely fashion - log a warning...
        //    }
        //}
    }
}
