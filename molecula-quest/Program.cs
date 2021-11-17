using molecula_shared;
using StereoKit;
using System;

namespace molecula_quest
{
    class Program
    {
        static void Main(string[] args)
        {
            // Initialize StereoKit
            SKSettings settings = new SKSettings
            {
                appName = "molecula_quest",
                assetsFolder = "Assets",
            };
            if (!SK.Initialize(settings))
                Environment.Exit(1);


            App.Loop();
            SK.Shutdown();
        }
    }
}
