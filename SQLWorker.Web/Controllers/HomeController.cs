using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SQLWorker.BLL.ProvidersRepositories.Github;
using SQLWorker.Web.Models;

namespace SQLWorker.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _log;
        private readonly GithubPuller _puller;
        public HomeController(ILogger<HomeController> log, GithubPuller puller)
        {
            _log = log;
            _puller = puller;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task GetFileTree()
        {
            if (string.IsNullOrEmpty(HttpContext.User.Identity.Name))
                return;
            
            await Task.Run(() =>
            {
                
                string dir;
                if (string.IsNullOrEmpty(Request.Form["dir"]))
                    dir = "/";
                else
                    dir = Request.Form["dir"];
                DirectoryInfo di = new DirectoryInfo(dir);
                
                Response.WriteAsync("<ul class=\"jqueryFileTree\" style=\"display: none;\">\n");
                foreach (DirectoryInfo di_child in di.GetDirectories().Where(e => !e.Name.StartsWith('.')))
                {
                        Response.WriteAsync("\t<li class=\"directory collapsed\"><a href=\"#\" rel=\"" + dir +
                                            di_child.Name +
                                            "/\">" + di_child.Name + "</a></li>\n");
                }
                foreach (FileInfo fi in di.GetFiles().Where(e => e.Extension.Substring(1).ToLower() == "sql"))
                {
                    string ext = "";
                    if (fi.Extension.Length > 1)
                        ext = fi.Extension.Substring(1).ToLower();

                    Response.WriteAsync("\t<li class=\"file ext_" + ext + "\"><a href=\"#\" rel=\"" + dir + fi.Name +
                                        "\">" + fi.Name + "</a></li>\n");
                }
                Response.WriteAsync("</ul>");
            });
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}