using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using eQuartz.Models;
using eQuartz.Services;
using Quartz;
using Quartz.Impl;

namespace eQuartz
{
    public class QuartzConfig
    {
        /// <summary></summary>
        public static void RegisterJob()
        {
            if (!QuartzGlobal.Scheduler.IsStarted)
                QuartzGlobal.Scheduler.Start();
            //List<JobEntity> jobs = new List<JobEntity>();
            //jobs.Add(new JobEntity
            //{
            //    JobId = 110,
            //    JobName = "Testing",
            //    IsEnabled = true,
            //    RunTime = "30 * * * * ?",
            //    JobType = 1,
            //    Content = "select * from table"
            //});
            // initilize job task
            foreach (JobEntity job in new ORMContext().Job)
            {
                if (job.IsEnabled)
                {                    
                    QuartzGlobal.RegisterJob<SampleJob>(job);
                }
            }
        }
    }

    /// <summary></summary>
    public class QuartzGlobal
    {
        private static IScheduler _scheduler = new StdSchedulerFactory().GetScheduler().Result;
        /// <summary></summary>
        public static IScheduler Scheduler { get { return _scheduler; } }

        /// <summary></summary>
        /// <param name="myjob"></param>
        public static void RegisterJob<T>(JobEntity myjob,string mapKey="task_job") where T : IJob
        {
            if (_scheduler.CheckExists(new JobKey(myjob.JobId.ToString())).Result)
                _scheduler.DeleteJob(new JobKey(myjob.JobId.ToString()));

            JobDataMap map = new JobDataMap();
            map.Put(mapKey, myjob);
            IJobDetail job = JobBuilder.Create<T>().UsingJobData(map).WithIdentity(myjob.JobId.ToString()).Build();

            var trigger_list = new HashSet<ITrigger>();
            foreach (string rt in myjob.RunTime.Split(';'))
            {
                trigger_list.Add(TriggerBuilder.Create().StartNow().WithCronSchedule(rt).Build());
            }
            _scheduler.ScheduleJob(job, trigger_list, true);
        }

    }
    
}