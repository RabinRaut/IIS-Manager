using IIS_Manager_Framework.Model;
using Microsoft.Web.Administration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MicrosoftApplicationPool = Microsoft.Web.Administration.ApplicationPool;
using ApplicationPoolModel = IIS_Manager_Framework.Model.ApplicationPool;

namespace IIS_Manager_Framework
{
    public class Program
    {

        static void Main(string[] args)
        {
            ServerManager serverManager = new ServerManager();
            #region Model Initalization
            ManageServer manageServer = new ManageServer();
            List<string> applicationSiteStartedList = new List<string>();
            List<string> applicationSiteStoppedList = new List<string>();
            List<string> applicationpoolStartedList = new List<string>();
            List<string> applicationpoolStoppedList = new List<string>();

            ApplicationSetting applicationSetting = Library.Utilities.LoadJsonConfigFile<ApplicationSetting>(); // fetch appsetting.json datas
            #endregion
            if (applicationSetting != null &&
                (
                    ((applicationSetting.ManageServer.ApplicationPool.Start.Count() + applicationSetting.ManageServer.ApplicationPool.Stop.Count()) > 0)
                    || ((applicationSetting.ManageServer.ApplicationSite.Start.Count() + applicationSetting.ManageServer.ApplicationSite.Stop.Count()) > 0))
                )
            {
                SiteCollection sites = serverManager.Sites;
                var applicationSiteManagementResponse = ManageApplicationSite(sites, applicationSetting.ManageServer.ApplicationSite);
                var applicationPools = serverManager.ApplicationPools;
                ApplicationPoolModel applicationPoolModelManagementResponse = ManageApplicationPool(applicationPools, applicationSetting.ManageServer.ApplicationPool);


                manageServer.ApplicationSite = applicationSiteManagementResponse;
                manageServer.ApplicationPool = applicationPoolModelManagementResponse;
                if (
                    (manageServer.ApplicationSite.Start.Count() + manageServer.ApplicationSite.Stop.Count()) < 1
                    || (manageServer.ApplicationPool.Start.Count() + manageServer.ApplicationPool.Stop.Count()) < 1
                    )
                {
                    Console.WriteLine("Something went wrong. Please try again later.");
                }
                else
                {
                    if ((manageServer.ApplicationSite.Start.Count() + manageServer.ApplicationSite.Stop.Count()) >= 1)
                    {
                        if (manageServer.ApplicationSite.Start.Count() > 0)
                        {
                            Console.WriteLine("********* Started Application Site *********");
                            Parallel.ForEach(manageServer.ApplicationSite.Start, item => { Console.WriteLine(item); });
                        }
                        if (manageServer.ApplicationSite.Stop.Count() > 0)
                        {
                            Console.WriteLine("********* Stopped Application Site *********");
                            Parallel.ForEach(manageServer.ApplicationSite.Stop, item => { Console.WriteLine(item); });
                        }
                    }
                    if ((manageServer.ApplicationPool.Start.Count() + manageServer.ApplicationPool.Stop.Count()) >= 1)
                    {
                        if (manageServer.ApplicationPool.Start.Count() > 0)
                        {
                            Console.WriteLine("********* Started Application Pool *********");
                            Parallel.ForEach(manageServer.ApplicationPool.Start, item => { Console.WriteLine(item); });
                        }
                        if (manageServer.ApplicationPool.Stop.Count() > 0)
                        {
                            Console.WriteLine("********* Stopped Application Pool *********");
                            Parallel.ForEach(manageServer.ApplicationPool.Stop, item => { Console.WriteLine(item); });
                        }
                    }
                }
                Console.ReadLine();
                return;
            }
            else
            {
                Console.WriteLine("No data found to process. Please provide valid data and try again later.");
                Console.ReadLine();
                return;
            }
        }
        private static ApplicationSite ManageApplicationSite(SiteCollection sites, ApplicationSite application_site)
        {
            ApplicationSite resonse = new ApplicationSite();
            List<string> applicationSiteStartedList = new List<string>();
            List<string> applicationSiteStoppedList = new List<string>();
            foreach (Site site in sites)
            {
                if (site.State.ToString().ToLower() != "started")
                {
                    foreach (var item in application_site.Start)
                    {
                        if (item.ToLower() == site.Name.ToString().ToLower())
                        {
                            site.Start();
                            if (site.State.ToString().ToLower() == "started")
                            {
                                applicationSiteStartedList.Add(item);
                            }
                        }
                    }
                }
                else if (site.State.ToString().ToLower() != "stopped")
                {
                    foreach (var item in application_site.Stop)
                    {
                        if (item.ToLower() == site.Name.ToString().ToLower())
                        {
                            site.Stop();
                            if (site.State.ToString().ToLower() == "stopped")
                            {
                                applicationSiteStoppedList.Add(item);
                            }
                        }
                    }
                }
                else { }
            }
            resonse = new ApplicationSite()
            {
                Start = applicationSiteStartedList,
                Stop = applicationSiteStoppedList
            };
            return resonse;
        }
        private static ApplicationPoolModel ManageApplicationPool(ApplicationPoolCollection pool_collection, ApplicationPoolModel application_pool)
        {
            ApplicationPoolModel resonse = new ApplicationPoolModel();
            List<string> applicationpoolStartedList = new List<string>();
            List<string> applicationpoolStoppedList = new List<string>();
            foreach (MicrosoftApplicationPool pool in pool_collection)
            {
                if (pool.State.ToString().ToLower() != "started")
                {
                    foreach (var item in application_pool.Start)
                    {
                        if (item.ToLower() == pool.Name.ToString().ToLower())
                        {
                            pool.Start();
                            if (pool.State.ToString().ToLower() == "started")
                            {
                                applicationpoolStartedList.Add(item);
                            }
                        }
                    }
                }
                else if (pool.State.ToString().ToLower() != "stopped")
                {
                    foreach (var item in application_pool.Stop)
                    {
                        if (item.ToLower() == pool.Name.ToString().ToLower())
                        {
                            pool.Stop();
                            if (pool.State.ToString().ToLower() == "stopped")
                            {
                                applicationpoolStoppedList.Add(item);
                            }
                        }
                    }
                }
                else { }
            }
            resonse = new ApplicationPoolModel()
            {
                Start = applicationpoolStartedList,
                Stop = applicationpoolStoppedList
            };
            return resonse;
        }
    }
}
