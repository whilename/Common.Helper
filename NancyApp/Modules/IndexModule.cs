using Nancy;

namespace NancyApp
{
    /// <summary></summary>
    public class IndexModule : NancyModule
    {
        /// <summary></summary>
        public IndexModule()
        {
            Get["/"] = parameters => { return View["index"]; };

            Get["/Home"] = parameters => { return View["index"]; };

            Get["/Home/Index"] = parameters => { return View["index"]; };

        }
    }
}
