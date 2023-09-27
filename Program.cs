using StereoKit;

namespace Molecula;

class Program
{
	static void Main(string[] args)
	{

		var app = new StereoKitApp();
		if (!SK.Initialize(app.Settings))
			return;
		app.Init();

		// Core application loop
		SK.Run(app.Step);
	}
}