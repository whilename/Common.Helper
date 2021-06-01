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
    /*
    public class QuartzConfig
    {
        private static IScheduler _scheduler = StdSchedulerFactory.GetDefaultScheduler();
        /// <summary></summary>
        public static IScheduler Scheduler { get { return _scheduler; } }

        /// <summary>Register jobs and trigger</summary>
        public static void RegisterJobTrigger()
        {
            try
            {
                LogHelper.WriteInfoLog("------------------ Quartz Scheduler Start ------------------", "Quartz");
                Scheduler.Start();
                JobScheduleManager manager = new JobScheduleManager();
                // Get all enabled jobs
                List<JobScheduleEntity> items = manager.FindByEnabled();
                //List<JobScheduleEntity> items = manager.FindByKeyWord("KTC"); //ktc email
                LogHelper.WriteInfoLog("Register jobs " + items.Count(), "Quartz");
                for (int i = 0; i < items.Count; i++)
                {
                    RegisterJobTrigger(items[i]);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex, "Quartz");
            }
        }


        /// <summary>Register jobs and trigger</summary>
        /// <param name="entity">job schedule info</param>
        public static void RegisterJobTrigger(JobScheduleEntity entity)
        {
            try
            {
                // check exists job name, if true delete job
                if (Scheduler.CheckExists(new JobKey(entity.JobName, entity.JobGroup)))
                    Scheduler.DeleteJob(new JobKey(entity.JobName));
                // JobDataMap map = new JobDataMap();
                // map.Put(Key, Object);
                // define the job and tie it to our MyJob class
                IJobDetail job = JobBuilder.Create(Type.GetType(entity.JobType))//.UsingJobData(map)
                    .WithIdentity(entity.JobName, entity.JobGroup).Build();

                TriggerKey trigger = new TriggerKey(entity.JobName + "Trigger", entity.JobName + "TriggerGroup");
                // Trigger the job to run now
                ITrigger JobTrigger = TriggerBuilder.Create().WithIdentity(trigger)
                    //.WithCronSchedule("30 * * * * ?").Build();
                    .WithCronSchedule(entity.CronExpression).Build();
                if (Scheduler.GetTrigger(trigger) != null)
                {
                    // Tell quartz to schedule the job using our new trigger
                    Scheduler.RescheduleJob(trigger, JobTrigger);
                }
                else
                {
                    // Tell quartz to schedule the job using our trigger
                    Scheduler.ScheduleJob(job, JobTrigger);
                }
                LogHelper.WriteInfoLog("Register job " + entity.JobName, "Quartz");
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex, "Quartz");
            }

        }
    }*/
}
