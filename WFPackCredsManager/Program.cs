using System;

namespace WFPackCredsManager
{
    class Program
    {
        static void Main(string[] args)
        {
            var opt = new Options();
            if (!CommandLine.Parser.Default.ParseArguments(args, opt))
            {
                opt.GetUsage();
                return;
            }

            var spClient = new SPClient(opt.Url, opt.Login, opt.Password);

            Console.WriteLine(spClient.RootSiteCredsExists
                    ? $"Default root site account is {spClient.DefaultLogin}"
                    : $"Root site {opt.Url} doesn't contain actions pack credentials information, you can't use update flag");

            foreach (var url in spClient.ActionsPackSites.Keys)
            {
                var info = spClient.ActionsPackSites[url];
                Console.WriteLine($"Site {url} uses {info?.DefaultLogin ?? "empty"} account");
            }

            if (opt.Force)
            {
                spClient.UpdateCredsOnChildSites();
            }
        }
    }
}
