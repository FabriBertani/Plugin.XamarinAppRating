using System;
using AppKit;
using Foundation;

namespace TestApp.MacOS
{
    public partial class ViewController : NSViewController
    {
        public ViewController(IntPtr handle)
            : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
        }

        public override NSObject RepresentedObject
        {
            get => base.RepresentedObject;            
            set
            {
                base.RepresentedObject = value;
            }
        }
    }
}