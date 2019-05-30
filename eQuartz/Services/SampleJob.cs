using eQuartz.Models;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace eQuartz.Services
{
    public class SampleJob : IJob
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Execute(IJobExecutionContext context)
        {
            JobEntity job = (JobEntity)context.JobDetail.JobDataMap.Get("task_job");
            switch (job.JobType)
            {
                case 1:
                    await SQLExecute(job);
                    break;
                case 2:
                    await EmailExecute(job);
                    break;
                case 3:
                    await ExportExecute(job);
                    break;
                default:
                    break;
            }
        }

        /// <summary></summary>
        /// <param name="job"></param>
        public Task SQLExecute(JobEntity job)
        {

            return null;
        }

        /// <summary></summary>
        /// <param name="job"></param>
        public Task EmailExecute(JobEntity job)
        {

            return null;
        }

        /// <summary></summary>
        /// <param name="job"></param>
        public Task ExportExecute(JobEntity job)
        {

            return null;
        }

    }
}