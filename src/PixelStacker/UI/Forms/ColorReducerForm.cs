using PixelStacker.Logic.Engine.Quantizer;
using PixelStacker.Logic.Extensions;
using PixelStacker.Logic.IO.Config;
using PixelStacker.Resources.Localization;
using PixelStacker.UI.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PixelStacker.UI.Forms
{
    public partial class ColorReducerForm : Form, ILocalized
    {
        private Options Options;

        public ColorReducerForm()
        {
            InitializeComponent();
        }

        //private void RenderGridView()
        //{
        //    var opt = this.Options;
        //    gridView.Add<Options, bool>(opt, opt => opt.IsSideView);
        //    gridView.Add<Options, int>(opt, opt => opt.Preprocessor.RgbBucketSize);
        //    gridView.Add<Options, int?>(opt, opt => opt.Preprocessor.MaxHeight);
        //    gridView.Add<Options, int?>(opt, opt => opt.Preprocessor.MaxWidth);
        //    gridView.Add<Options, int>(opt, opt => opt.Preprocessor.QuantizerSettings.MaxColorCount);
        //    gridView.Add<Options, int>(opt, opt => opt.Preprocessor.QuantizerSettings.MaxParallelProcesses);
        //    gridView.Add<Options, string>(opt, opt => opt.Preprocessor.QuantizerSettings.DitherAlgorithm);
        //    gridView.Add<Options, string>(opt, opt => opt.Preprocessor.QuantizerSettings.ColorCache);
        //    gridView.Add<Options, string>(opt, opt => opt.Preprocessor.QuantizerSettings.Algorithm);
        //    propertyGrid.Refresh();
        //    this.Refresh();
        //}

        public ColorReducerForm(Options options)
        {
            this.Options = options;
            InitializeComponent();

            cbxEnableQuantizer.Checked = this.Options.Preprocessor.QuantizerSettings.IsEnabled;
            bool isChecked = this.Options.Preprocessor.QuantizerSettings.IsEnabled;
            ddlAlgorithm.Enabled = isChecked;
            ddlColorCache.Enabled = isChecked;
            ddlDither.Enabled = isChecked;
            ddlColorCount.Enabled = isChecked;
            ddlParallel.Enabled = isChecked;

            ApplyLocalization(CultureInfo.CurrentUICulture);
            //InitReduceOptions(options);
            ShowReduceOptions(options);
        }

        private void InitReduceOptions(Options options)
        {
            var preproc = options.Preprocessor;
            var quant = preproc.QuantizerSettings;
            string[] quantizerAlgorithms = QuantizerEngine.GetQuantizerAlgorithms();
            {
                ddlAlgorithm.Items.Clear();
                ddlAlgorithm.Items.AddRange(quantizerAlgorithms);
                int idx = quantizerAlgorithms.ToList().IndexOf(quant.Algorithm);
                if (idx == -1) idx = 0; ddlAlgorithm.SelectedIndex = idx;
            }

        }

        private void ShowReduceOptions(Options options)
        {
            var preproc = options.Preprocessor;
            var quant = preproc.QuantizerSettings;
            string algo = quant.Algorithm;
            var qOpts = QuantizerEngine.GetQuantizerAlgorithmOptions(algo);
            string[] quantizerAlgorithms = QuantizerEngine.GetQuantizerAlgorithms();
            {
                ddlAlgorithm.Items.Clear();
                ddlAlgorithm.Items.AddRange(quantizerAlgorithms);
                int idx = Math.Max(0, quantizerAlgorithms.ToList().IndexOf(quant.Algorithm));
                ddlAlgorithm.SelectedItem = quant.Algorithm;
            }
            {
                ddlRgbBucketSize.Items.Clear();
                var rgbBucketOptions = new string[] {
                "1  :  255^3"
                ,"5  :  51^3"
                ,"15  :  17^3"
                ,"17  :  15^3"
                ,"51  :  5^3"};

                ddlRgbBucketSize.Items.AddRange(rgbBucketOptions);
                ddlRgbBucketSize.SelectedItem = rgbBucketOptions.ToList().Find(x => x.StartsWith(preproc.RgbBucketSize.ToString() + " "));
            }

            {
                ddlParallel.Items.Clear();
                ddlParallel.Items.AddRange(qOpts.MaxParallelProcessesList.Select(x => x.ToString()).ToArray());
                ddlParallel.SelectedItem = quant.MaxParallelProcesses.ToString();
                ddlParallel.Enabled = qOpts.MaxParallelProcessesList.Count > 1;
            }
        }

        public void ApplyLocalization(CultureInfo locale)
        {
            lblInstructions.Text = Resources.Text.ColorReducer_Instructions;
            lblRgbBucketSize.Text = Resources.Text.ColorReducer_RgbBucketSize;
            this.toolTip.SetToolTip(this.lblRgbBucketSize, Resources.Text.ColorReducer_RgbBucketSize_Tooltip.AsTooltipText());
        }

        private void ddlAlgorithm_SelectedValueChanged(object sender, EventArgs e)
        {
            string val = ddlAlgorithm.SelectedItem as string;
            if (val == Options.Preprocessor.QuantizerSettings.Algorithm) return;
            if (val != null)
            {
                Options.Preprocessor.QuantizerSettings.Algorithm = val;
                ShowReduceOptions(Options);
            }
        }

        private void lblInstructionTitle_Click(object sender, EventArgs e)
        {
            lblInstructions.Text = Resources.Text.ColorReducer_Instructions;
        }

        private void lblRgbBucketSize_Click(object sender, EventArgs e)
        {
            lblInstructions.Text = Resources.Text.ColorReducer_RgbBucketSize_Tooltip;
        }

        private void cbxEnableQuantizer_CheckedChanged(object sender, EventArgs e)
        {
            bool isChecked = cbxEnableQuantizer.Checked;
            ddlAlgorithm.Enabled = isChecked;
            ddlColorCache.Enabled = isChecked;
            ddlDither.Enabled = isChecked;
            ddlColorCount.Enabled = isChecked;
            ddlParallel.Enabled = isChecked;
            Options.Preprocessor.QuantizerSettings.IsEnabled = isChecked;
            Options.Save();
        }
    }

    public class ColorReducerView : DynamicObject
    {
        private readonly IDictionary<string, object> dynamicProperties =
         new Dictionary<string, object>();

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            var memberName = binder.Name;
            return dynamicProperties.TryGetValue(memberName, out result);
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            var memberName = binder.Name;
            dynamicProperties[memberName] = value;
            return true;
        }
    }

    public class RgbBucketSizeConverter : Int32Converter
    {
        public override Boolean GetStandardValuesSupported(ITypeDescriptorContext context) { return true; }
        public override Boolean GetStandardValuesExclusive(ITypeDescriptorContext context) { return true; }
        public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            List<Int32> list = new List<Int32>();
            list.Add(1);
            list.Add(5);
            list.Add(15);
            list.Add(17);
            list.Add(51);
            list.Add(256);
            return new StandardValuesCollection(list);
        }
    }

    public class ColorReducerOptionsView
    {
        public object src = new CanvasPreprocessorSettings();

        [TypeConverter(typeof(RgbBucketSizeConverter))]
        public int RgbBucketSize { get; set; } = 1;

        private Options opts;
        
        public ColorReducerOptionsView(Options opts)
        {
            this.opts = opts;
        }
    }

}
