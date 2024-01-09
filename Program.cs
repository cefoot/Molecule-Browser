using StereoKit;
using StereoKit.Framework;

namespace Molecula
{

	class Program
	{
		static void Main(string[] args)
		{

			var passthrough = SK.GetOrCreateStepper<PassthroughMetaExt>();
			var app = new StereoKitApp(passthrough);
			if (!SK.Initialize(app.Settings))
				return;
			passthrough.Enabled = true;
			app.Init();

			// Core application loop
			SK.Run(app.Step);
		}
	}
}