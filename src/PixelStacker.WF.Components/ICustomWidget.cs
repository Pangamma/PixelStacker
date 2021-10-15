//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Forms.Design;

//namespace PixelStacker.WF.Components
//{
//    public interface ICustomWidget
//    {
//        public void CustomInitialize();
//    }

//    [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
//    public class CustomWidgetDesigner : ParentControlDesigner
//    {
//        public CustomWidgetDesigner()
//        {
//            System.Diagnostics.Trace.WriteLine("CustomWidgetDesigner");
//        }

//        //[Obsolete("I have no idea what this does", false)]
//        //public bool Visible
//        //{
//        //    get
//        //    {
//        //        return (bool)ShadowProperties["Visible"];
//        //    }
//        //    set
//        //    {
//        //        this.ShadowProperties["Visible"] = value;
//        //    }
//        //}

//        //[Obsolete("I have no idea what this does", false)]
//        //public bool Enabled
//        //{
//        //    get
//        //    {
//        //        return (bool)ShadowProperties["Enabled"];
//        //    }
//        //    set
//        //    {
//        //        this.ShadowProperties["Enabled"] = value;
//        //    }
//        //}

//        //[Obsolete("I have no idea what this does", false)]
//        //protected override void PreFilterProperties(IDictionary properties)
//        //{
//        //    base.PreFilterProperties(properties);

//        //    if (properties.Contains("Padding"))
//        //    {
//        //        properties.Remove("Padding");
//        //    }

//        //    properties["Visible"] = TypeDescriptor.CreateProperty(
//        //        typeof(CustomWidgetDesigner),
//        //        (PropertyDescriptor)properties["Visible"],
//        //        new Attribute[0]);

//        //    properties["Enabled"] = TypeDescriptor.CreateProperty(
//        //        typeof(CustomWidgetDesigner),
//        //        (PropertyDescriptor)properties["Enabled"],
//        //        new Attribute[0]);
//        //}

//        private void OnVerbCustomInitialize(object sender, EventArgs e)
//        {
//            ICustomWidget widget = this.Control as ICustomWidget;
//            widget.CustomInitialize();
//        }
//    }
//}
