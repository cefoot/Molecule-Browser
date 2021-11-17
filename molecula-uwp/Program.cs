using molecula_shared;
using StereoKit;
using System;

namespace molecula_uwp
{
    class Program
    {
        static void Main(string[] args)
        {
            // Initialize StereoKit
            SKSettings settings = new SKSettings
            {
                appName = "molecula_uwp",
                assetsFolder = "Assets",
            };
            if (!SK.Initialize(settings))
                Environment.Exit(1);
            new App().Loop();
            SK.Shutdown();
        }
    }
}
