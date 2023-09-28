using System;
using Molecula;
using StereoKit;

class SKLoader
{
	static void Main(string[] args)
	{
		// This will allow the App constructor to call a few SK methods
		// before Initialize is called.
		SK.PreLoadLibrary();

		// If the app has a constructor that takes a string array, then
		// we'll use that, and pass the command line arguments into it on
		// creation
		Type appType = typeof(StereoApp);
		StereoApp  app     = appType.GetConstructor(new Type[] { typeof(string[]) }) != null
			? (StereoApp)Activator.CreateInstance(appType, new object[] { args })
			: (StereoApp)Activator.CreateInstance(appType);
		if (app == null)
			throw new Exception("StereoKit loader couldn't construct an instance of the App!");

		// Initialize StereoKit, and the app
		if (!SK.Initialize(app.Settings))
			Environment.Exit(1);
		app.Init();

		// Now loop until finished, and then shut down
		while (SK.Step(app.Step)) { }
		SK.Shutdown();
	}
}