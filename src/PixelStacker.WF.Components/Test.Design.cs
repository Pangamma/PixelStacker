using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace MarqueeControlLibrary.Design
{
    [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")] 
    public class MarqueeBorderDesigner : ParentControlDesigner
    {
        public MarqueeBorderDesigner()
        {
            Trace.WriteLine("MarqueeBorderDesigner");
        }

        public bool Visible
        {
            get
            {
                return (bool)ShadowProperties["Visible"];
            }
            set
            {
                this.ShadowProperties["Visible"] = value;
            }
        }

        public bool Enabled
        {
            get
            {
                return (bool)ShadowProperties["Enabled"];
            }
            set
            {
                this.ShadowProperties["Enabled"] = value;
            }
        }

        protected override void PreFilterProperties(IDictionary properties)
        {
            base.PreFilterProperties(properties);

            if (properties.Contains("Padding"))
            {
                properties.Remove("Padding");
            }

            properties["Visible"] = TypeDescriptor.CreateProperty(
                typeof(MarqueeBorderDesigner),
                (PropertyDescriptor)properties["Visible"],
                new Attribute[0]);

            properties["Enabled"] = TypeDescriptor.CreateProperty(
                typeof(MarqueeBorderDesigner),
                (PropertyDescriptor)properties["Enabled"],
                new Attribute[0]);
        }

        private void OnVerbRunTest(object sender, EventArgs e)
        {
            IMarqueeWidget widget = this.Control as IMarqueeWidget;

            widget.StartMarquee();
        }

        private void OnVerbStopTest(object sender, EventArgs e)
        {
            IMarqueeWidget widget = this.Control as IMarqueeWidget;

            widget.StopMarquee();
        }
    }

    [ToolboxItemFilter("MarqueeControlLibrary.MarqueeBorder", ToolboxItemFilterType.Require)]
    [ToolboxItemFilter("MarqueeControlLibrary.MarqueeText", ToolboxItemFilterType.Require)]
    [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")] 
    public class MarqueeControlRootDesigner : DocumentDesigner
    {

        public MarqueeControlRootDesigner()
        {
            Trace.WriteLine("MarqueeControlRootDesigner ctor");
        }

        public override void Initialize(IComponent component)
        {
            base.Initialize(component);

            IComponentChangeService cs =
                GetService(typeof(IComponentChangeService)) 
                as IComponentChangeService;

            if (cs != null)
            {
                cs.ComponentChanged +=
                    new ComponentChangedEventHandler(OnComponentChanged);
            }

            this.Verbs.Add(
                new DesignerVerb("Run Test",
                new EventHandler(OnVerbRunTest))
                );

            this.Verbs.Add(
                new DesignerVerb("Stop Test",
                new EventHandler(OnVerbStopTest))
                );
        }

        private void OnComponentChanged(
            object sender,
            ComponentChangedEventArgs e)
        {
            if (e.Component is IMarqueeWidget)
            {
                this.Control.Refresh();
            }
        }

        private void OnVerbRunTest(object sender, EventArgs e)
        {
            MarqueeControl c = this.Control as MarqueeControl;

            c.Start();
        }

        private void OnVerbStopTest(object sender, EventArgs e)
        {
            MarqueeControl c = this.Control as MarqueeControl;

            c.Stop();
        }
    }
}