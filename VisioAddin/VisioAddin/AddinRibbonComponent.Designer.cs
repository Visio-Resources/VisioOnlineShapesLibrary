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
            Microsoft.Office.Tools.Ribbon.RibbonDropDownItem ribbonDropDownItemImpl1 = this.Factory.CreateRibbonDropDownItem();
            Microsoft.Office.Tools.Ribbon.RibbonDropDownItem ribbonDropDownItemImpl2 = this.Factory.CreateRibbonDropDownItem();
            this.Tab1 = this.Factory.CreateRibbonTab();
            this.Group1 = this.Factory.CreateRibbonGroup();
            this.dropDownServer = this.Factory.CreateRibbonDropDown();
            this.btnOnlineShapes = this.Factory.CreateRibbonButton();
            this.sBtnContribute = this.Factory.CreateRibbonSplitButton();
            this.btnContributeStencil = this.Factory.CreateRibbonButton();
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
            this.Group1.Items.Add(this.dropDownServer);
            this.Group1.Items.Add(this.btnOnlineShapes);
            this.Group1.Items.Add(this.sBtnContribute);
            this.Group1.Label = "Online Shapes";
            this.Group1.Name = "Group1";
            // 
            // dropDownServer
            // 
            ribbonDropDownItemImpl1.Label = "https://www.visio-shapes.com";
            ribbonDropDownItemImpl2.Label = "http://127.0.0.1:5000";
            this.dropDownServer.Items.Add(ribbonDropDownItemImpl1);
            this.dropDownServer.Items.Add(ribbonDropDownItemImpl2);
            this.dropDownServer.Label = "Server";
            this.dropDownServer.Name = "dropDownServer";
            this.dropDownServer.SizeString = "https://www.visio-shapes.com";
            this.dropDownServer.SelectionChanged += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.dropDownServer_SelectionChanged);
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
            // sBtnContribute
            // 
            this.sBtnContribute.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.sBtnContribute.Items.Add(this.btnContributeStencil);
            this.sBtnContribute.Label = "Contribute Shape";
            this.sBtnContribute.Name = "sBtnContribute";
            this.sBtnContribute.OfficeImageId = "ShareThisNotebook";
            this.sBtnContribute.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.sBtnContribute_Click);
            // 
            // btnContributeStencil
            // 
            this.btnContributeStencil.Label = "Contribute Stencil";
            this.btnContributeStencil.Name = "btnContributeStencil";
            this.btnContributeStencil.ShowImage = true;
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
        internal Microsoft.Office.Tools.Ribbon.RibbonSplitButton sBtnContribute;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnContributeStencil;
        internal Microsoft.Office.Tools.Ribbon.RibbonDropDown dropDownServer;
    }

    partial class ThisRibbonCollection
    {
        internal AddinRibbonComponent Ribbon
        {
            get { return this.GetRibbon<AddinRibbonComponent>(); }
        }
    }
}
