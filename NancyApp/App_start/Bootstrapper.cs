using Common.Utility;
using Nancy;
using Nancy.Extensions;
using Nancy.ErrorHandling;
using Nancy.IO;
using System;
using System.IO;
using System.Text;
using System.Linq;
using Nancy.Conventions;

namespace NancyApp
{
    /// <summary></summary>
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        /// <summary>
        /// The bootstrapper enables you to reconfigure the composition of the framework,
        /// by overriding the various methods and properties.
        /// For more information https://github.com/NancyFx/Nancy/wiki/Bootstrapper
        /// </summary>
        /// <param name="container"></param>
        /// <param name="pipelines"></param>
        protected override void ApplicationStartup(Nancy.TinyIoc.TinyIoCContainer container, Nancy.Bootstrapper.IPipelines pipelines)
        {
            Log.Register();
            //Log.Info("Test Message");
            base.ApplicationStartup(container, pipelines);
            StaticConfiguration.EnableRequestTracing = true;
            StaticConfiguration.DisableErrorTraces = false;
        }

        /// <summary>重写自定义跟路径提供程序</summary>
        protected override IRootPathProvider RootPathProvider
        {
            get { return new CustomRootPathProvider(); }
        }

        /// <summary></summary>
        /// <param name="nancyConventions"></param>
        protected override void ConfigureConventions(NancyConventions nancyConventions)
        {
            base.ConfigureConventions(nancyConventions);
            nancyConventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("/", "Home"));
        }
        
    }

    /// <summary>自定义跟路径提供程序(主要想改变程序默认显示图标)</summary>
    public class CustomRootPathProvider : IRootPathProvider
    {
        /// <summary>获取跟路径</summary>
        /// <returns></returns>
        public string GetRootPath() { return AppDomain.CurrentDomain.GetData(".appPath").ToString(); }
    }
        
}