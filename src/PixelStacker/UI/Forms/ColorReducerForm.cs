using PixelStacker.Logic.Engine.Quantizer;
using PixelStacker.Logic.IO.Config;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace PixelStacker.UI.Forms
{
    /// <summary>
    /// TODO: Localize
    /// </summary>
    public partial class ColorReducerForm : Form
    {
        private MainForm MainForm;
        private Options Options;
        private bool IsNoisy = false;
        private Dictionary<string, int> MaxUniqueColorOptions = new Dictionary<string, int>()
        {
            ["255^3"] = 1,
            ["51^3"] = 5,
            ["17^3"] = 15,
            ["15^3"] = 17,
            ["5^3"] = 51
        };

        public ColorReducerForm()
        {
            InitializeComponent();
        }

        public ColorReducerForm(MainForm mf, Options options)
        {
            this.MainForm = mf;
            this.Options = options;
            InitializeComponent();
            InitializeCustom();
            RevalidateOptions(options);
            ApplyLocalization();
        }

        private void InitializeCustom()
        {
            cbxEnableQuantizer.Checked = this.Options.Preprocessor.QuantizerSettings?.IsEnabled == true;
            bool isChecked = this.Options.Preprocessor.QuantizerSettings?.IsEnabled == true;
            ddlAlgorithm.Enabled = isChecked;
            ddlDither.Enabled = isChecked;
            ddlColorCount.Enabled = isChecked;
            ddlParallel.Enabled = isChecked;
        }

        private void RevalidateOptions(Options options)
        {
            IsNoisy = true;
            var preproc = options.Preprocessor;
            var quant = preproc.QuantizerSettings;
            string algo = quant.Algorithm;
            var qOpts = QuantizerEngine.GetQuantizerAlgorithmOptions(algo);
            if (!quant.IsValid(qOpts, true))
            {
                throw new Exception("Invalid quantizer options detected.");
            }

            string[] quantizerAlgorithms = QuantizerEngine.GetQuantizerAlgorithms();
            {
                ddlAlgorithm.Items.Clear();
                ddlAlgorithm.Items.AddRange(quantizerAlgorithms);
                ddlAlgorithm.SelectedItem = quant.Algorithm;
                ddlAlgorithm.Enabled = quant.IsEnabled;
            }
            {
                ddlRgbBucketSize.Items.Clear();
                ddlRgbBucketSize.Items.AddRange(MaxUniqueColorOptions.Keys.ToArray());
                ddlRgbBucketSize.SelectedItem = MaxUniqueColorOptions.Where(x => x.Value == preproc.RgbBucketSize).Select(x => x.Key).First();
            }
            {
                ddlParallel.Items.Clear();
                ddlParallel.Items.AddRange(qOpts.MaxParallelProcessesList.Select(x => x.ToString()).ToArray());
                ddlParallel.SelectedItem = qOpts.MaxParallelProcessesList
                    .Where(x => x == quant.MaxParallelProcesses)
                    .DefaultIfEmpty(1).First().ToString();
                ddlParallel.Enabled = qOpts.MaxParallelProcessesList.Count > 1 && quant.IsEnabled;
            }
            {
                ddlDither.Items.Clear();
                ddlDither.Items.AddRange(qOpts.DithererList.Keys.ToArray());
                ddlDither.SelectedItem
                    = qOpts.DithererList.Keys.FirstOrDefault(x => x == quant.DitherAlgorithm)
                    ?? qOpts.DithererList.Keys.First();
                ddlDither.Enabled = qOpts.DithererList.Count > 1 && quant.IsEnabled;
            }
            {
                ddlColorCount.Items.Clear();
                ddlColorCount.Items.AddRange(qOpts.MaxColorCountsList.Select(x => x.ToString()).ToArray());
                ddlColorCount.SelectedItem = qOpts.MaxColorCountsList.Where(x => x == quant.MaxColorCount)
                    .DefaultIfEmpty(qOpts.MaxColorCountsList.First()).First().ToString();
                ddlColorCount.Enabled = qOpts.MaxColorCountsList.Count > 1 && quant.IsEnabled;
            }
            IsNoisy = false;
        }
    }
}
