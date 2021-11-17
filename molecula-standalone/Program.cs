using molecula_shared;
using StereoKit;
using System;

namespace molecula_standalone
{
    class Program
    {
        static void Main(string[] args)
        {
            // Initialize StereoKit
            SKSettings settings = new SKSettings
            {
                appName = "molecula_standalone",
                assetsFolder = "Assets",
            };
            if (!SK.Initialize(settings))
                Environment.Exit(1);
            new App().Loop();
            SK.Shutdown();
        }
    }
}
