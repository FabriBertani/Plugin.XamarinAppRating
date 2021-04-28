// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace TestAppNative.MacOS
{
	[Register ("ViewController")]
	partial class ViewController
	{
		[Outlet]
		AppKit.NSButton rateAppBtn { get; set; }

		[Action ("rateAppOnStoreButton:")]
		partial void rateAppOnStoreButton (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (rateAppBtn != null) {
				rateAppBtn.Dispose ();
				rateAppBtn = null;
			}
		}
	}
}
