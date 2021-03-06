using System;
using System.Collections.Concurrent;
using System.IO;
using System.Web.Hosting;
using System.Web.Mvc;

namespace HandlebarsDotNet.Mvc.ViewEngine
{
    public class HandlebarsMvcView : IView
    {
        static ConcurrentDictionary<string, Func<object, string>> compiledViews = new ConcurrentDictionary<string, Func<object, string>>();
        private Func<object, string> render;

        public HandlebarsMvcView(string physicalpath, IHandlebars handlebars)
        {
            var templatePath = physicalpath;
            var version = HostingEnvironment.VirtualPathProvider.GetCacheKey(templatePath);
            var key = templatePath + "_" + version;
            render = compiledViews.GetOrAdd(key, (k) =>
            {
                var compiledView = handlebars.CompileView(templatePath);
                return compiledView;
            });
        }

        public void Render(ViewContext viewContext, TextWriter writer)
        {
            writer.Write(render(viewContext.ViewData.Model));
        }
    }
}