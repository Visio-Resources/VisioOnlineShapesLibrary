namespace VisioAddin
{
    partial class AddinRibbonComponent : Microsoft.Office.Tools.Ribbon.RibbonBase
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        public AddinRibbonComponent()
            : base(Globals.Factory.GetRibbonFactory())
        {
            InitializeComponent();
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Tab1 = this.Factory.CreateRibbonTab();
            this.Group1 = this.Factory.CreateRibbonGroup();
            this.btnOnlineShapes = this.Factory.CreateRibbonButton();
            this.Tab1.SuspendLayout();
            this.Group1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Tab1
            // 
            this.Tab1.ControlId.ControlIdType = Microsoft.Office.Tools.Ribbon.RibbonControlIdType.Office;
            this.Tab1.ControlId.OfficeId = "TabHome";
            this.Tab1.Groups.Add(this.Group1);
            this.Tab1.Label = "TabHome";
            this.Tab1.Name = "Tab1";
            // 
            // Group1
            // 
            this.Group1.Items.Add(this.btnOnlineShapes);
            this.Group1.Label = "Online Shapes";
            this.Group1.Name = "Group1";
            // 
            // btnOnlineShapes
            // 
            this.btnOnlineShapes.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.btnOnlineShapes.Label = "Online Shapes";
            this.btnOnlineShapes.Name = "btnOnlineShapes";
            this.btnOnlineShapes.OfficeImageId = "ControlPropertyDisplayTextFieldName";
            this.btnOnlineShapes.ScreenTip = "Online shapes library";
            this.btnOnlineShapes.ShowImage = true;
            this.btnOnlineShapes.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnOnlineShapes_Click);
            // 
            // AddinRibbonComponent
            // 
            this.Name = "AddinRibbonComponent";
            this.RibbonType = "Microsoft.Visio.Drawing";
            this.Tabs.Add(this.Tab1);
            this.Tab1.ResumeLayout(false);
            this.Tab1.PerformLayout();
            this.Group1.ResumeLayout(false);
            this.Group1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        internal Microsoft.Office.Tools.Ribbon.RibbonTab Tab1;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup Group1;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnOnlineShapes;
    }

    partial class ThisRibbonCollection
    {
        internal AddinRibbonComponent Ribbon
        {
            get { return this.GetRibbon<AddinRibbonComponent>(); }
        }
    }
}
