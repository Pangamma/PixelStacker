using PixelStacker.Extensions;
using PixelStacker.Logic.IO.Config;
using PixelStacker.Logic.Model;
using PixelStacker.UI.Controls.Pickers;
using PixelStacker.UI.Forms;
using System.Reflection;

namespace PixelStacker.Tests.UI
{
    [TestFixture]
    [Apartment(ApartmentState.STA)]
    public class Tests
    {
        private Dictionary<string, AsyncLazy<RenderedCanvas>> Canvases = new Dictionary<string, AsyncLazy<RenderedCanvas>>();
        private Options Options;
        private MaterialPalette Palette;
        private MaterialPickerForm MaterialPickerForm;

        // The IDE is WRONG. Let me do my job.
#pragma warning disable NUnit1032 // An IDisposable field/property should be Disposed in a TearDown method
        private MaterialCombination McAir;
        private MaterialCombination McSolid;
        private MaterialCombination McGlass;
        private MaterialCombination McPureGlass;
        private MaterialCombination McAir2;
        private MaterialCombination McSolid2;
        private MaterialCombination McGlass2;
        private MaterialCombination McPureGlass2;
#pragma warning restore NUnit1032 // An IDisposable field/property should be Disposed in a TearDown method


        [SetUp]
        public void Setup()
        {
            Options = new MemoryOptionsProvider().Load();
            MaterialPickerForm = new MaterialPickerForm(Options);
            this.Palette = MaterialPalette.FromResx();
            this.McAir = this.Palette.GetMaterialCombinationByMaterials(Materials.Air, Materials.Air);
            this.McSolid = this.Palette.GetMaterialCombinationByMaterials(Materials.FromPixelStackerID("DIRT"), Materials.FromPixelStackerID("DIRT"));
            this.McGlass = this.Palette.GetMaterialCombinationByMaterials(Materials.FromPixelStackerID("STONE"), Materials.FromPixelStackerID("GLASS_00"));
            this.McPureGlass = this.Palette.GetMaterialCombinationByMaterials(Materials.FromPixelStackerID("GLASS_00"), Materials.FromPixelStackerID("GLASS_00"));

            this.McAir2 = this.Palette.GetMaterialCombinationByMaterials(Materials.Air, Materials.Air);
            this.McSolid2 = this.Palette.GetMaterialCombinationByMaterials(Materials.FromPixelStackerID("WOOL_05"), Materials.FromPixelStackerID("WOOL_05"));
            this.McGlass2 = this.Palette.GetMaterialCombinationByMaterials(Materials.FromPixelStackerID("WOOL_00"), Materials.FromPixelStackerID("GLASS_01"));
            this.McPureGlass2 = this.Palette.GetMaterialCombinationByMaterials(Materials.FromPixelStackerID("GLASS_01"), Materials.FromPixelStackerID("GLASS_01"));
        }

        [TearDown]
        public void CleanUp()
        {
            this.MaterialPickerForm.Dispose();
            McAir.DisposeSafely();
            McSolid.DisposeSafely();
            McGlass.DisposeSafely();
            McPureGlass.DisposeSafely();
            McAir2.DisposeSafely();
            McSolid2.DisposeSafely();
            McGlass2.DisposeSafely();
            McPureGlass2.DisposeSafely();
        }

        [Test]
        public void Top_Air_to_PureGlass()
        {
            var A = this.McAir;
            var B = this.McPureGlass2;
            this.MaterialPickerForm.SelectedCombo = A;
            this.MaterialPickerForm.pnlTopMats_TileClicked(null, new ImageButtonClickEventArgs()
            {
                ImageButtonData = new ImageButtonData()
                {
                    Data = B.Top,
                    Text = null,
                    Image = null
                }
            });

            var C = this.MaterialPickerForm.SelectedCombo;
            Assert.AreEqual(B.Bottom.PixelStackerID, C.Bottom.PixelStackerID);
            Assert.AreEqual(B.Top.PixelStackerID, C.Top.PixelStackerID);
            Assert.AreEqual(C.Top.PixelStackerID, C.Bottom.PixelStackerID);
        }

        [Test]
        public void Top_Air_to_Glass()
        {
            var A = this.McAir;
            var B = this.McGlass2;
            this.MaterialPickerForm.SelectedCombo = A;
            this.MaterialPickerForm.pnlTopMats_TileClicked(null, new ImageButtonClickEventArgs()
            {
                ImageButtonData = new ImageButtonData()
                {
                    Data = B.Top,
                    Text = null,
                    Image = null
                }
            });

            var C = this.MaterialPickerForm.SelectedCombo;
            Assert.That(C.Bottom.PixelStackerID, Is.EqualTo(B.Top.PixelStackerID));
            Assert.That(C.Top.PixelStackerID, Is.EqualTo(B.Top.PixelStackerID));
            Assert.That(C.Bottom.PixelStackerID, Is.EqualTo(C.Top.PixelStackerID));
        }
    }
}