using PixelStacker.Logic.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PixelStacker.Logic.IO.Config;

namespace PixelStacker.Tests
{
    [TestClass]
    public class MaterialCombinationTests
    {
        public MaterialPalette Palette { get; }
        private readonly MaterialCombination McAir;
        private readonly MaterialCombination McSolid;
        private readonly MaterialCombination McGlass;
        private readonly MaterialCombination McAir2;
        private readonly MaterialCombination McSolid2;
        private readonly MaterialCombination McGlass2;

        public MaterialCombinationTests()
        {
            this.Palette = MaterialPalette.FromResx();
            this.McAir = this.Palette.GetMaterialCombinationByMaterials(Materials.Air, Materials.Air);
            this.McSolid = this.Palette.GetMaterialCombinationByMaterials(Materials.FromPixelStackerID("DIRT"), Materials.FromPixelStackerID("DIRT"));
            this.McGlass = this.Palette.GetMaterialCombinationByMaterials(Materials.FromPixelStackerID("STONE"), Materials.FromPixelStackerID("GLASS_00"));

            this.McAir2 = this.Palette.GetMaterialCombinationByMaterials(Materials.Air, Materials.Air);
            this.McSolid2 = this.Palette.GetMaterialCombinationByMaterials(Materials.FromPixelStackerID("WOOL_05"), Materials.FromPixelStackerID("WOOL_05"));
            this.McGlass2 = this.Palette.GetMaterialCombinationByMaterials(Materials.FromPixelStackerID("WOOL_00"), Materials.FromPixelStackerID("GLASS_01"));

        }

        #region TOP: X on Solid
        [TestMethod("Top: Air on solid")]
        [TestCategory("Unit")]
        public void AirOnTopOfSolid_ZT()
        {
            var above = this.McAir;
            var below = this.McSolid2;

            var mc = MaterialCombination.GetMcToPaintWith(ZLayer.Top, this.Palette, this.McAir, below);
            Assert.AreEqual(below.Bottom.PixelStackerID, mc.Bottom.PixelStackerID, "Bottom shouldn't change.");
            Assert.AreEqual(below.Top.PixelStackerID, mc.Top.PixelStackerID, "Top shouldn't change.");
            Assert.AreEqual(mc.Bottom.PixelStackerID, mc.Top.PixelStackerID, "Top should match the Bottom.");
            Assert.AreNotEqual(above.Top.PixelStackerID, mc.Top.PixelStackerID, "Top should not have been set to air.");
        }

        [TestMethod("Top: Glass on solid")]
        [TestCategory("Unit")]
        public void GlassOnTopOfSolid_ZT()
        {
            var above = this.McGlass;
            var below = this.McSolid2;
            var mc = MaterialCombination.GetMcToPaintWith(ZLayer.Top, this.Palette, above, below);
            Assert.AreEqual(below.Bottom.PixelStackerID, mc.Bottom.PixelStackerID, "Bottom shouldn't change.");
            Assert.AreEqual(above.Top.PixelStackerID, mc.Top.PixelStackerID, "Glass should be applied to top.");
            Assert.AreNotEqual(mc.Bottom.PixelStackerID, mc.Top.PixelStackerID, "Top should NOT match the Bottom.");
        }

        [TestMethod("Top: Solid on solid")]
        [TestCategory("Unit")]
        public void SolidOnTopOfSolid_ZT()
        {
            var above = this.McSolid;
            var below = this.McSolid2;
            var mc = MaterialCombination.GetMcToPaintWith(ZLayer.Top, this.Palette, above, below);
            Assert.AreEqual(below.Bottom.PixelStackerID, mc.Bottom.PixelStackerID, "Bottom1 != Bottom2");
            Assert.AreEqual(below.Bottom.PixelStackerID, mc.Top.PixelStackerID, "Top1 != Bottom2");
        }
        #endregion TOP: X on Solid

        #region TOP: X on Glass
        [TestMethod("Top: Air on glass")]
        [TestCategory("Unit")]
        public void AirOnTopOfGlass_ZT()
        {
            var above = this.McAir;
            var below = this.McGlass2;
            var mc = MaterialCombination.GetMcToPaintWith(ZLayer.Top, this.Palette, above, below);
            Assert.AreEqual(below.Bottom.PixelStackerID, mc.Bottom.PixelStackerID, "Bottom shouldn't change.");
            Assert.AreNotEqual(above.Top.PixelStackerID, mc.Top.PixelStackerID, "Top should not have been set to air.");
            Assert.AreEqual(mc.Bottom.PixelStackerID, mc.Top.PixelStackerID, "Glass should be stripped off top.");
        }

        [TestMethod("Top: Glass on glass")]
        [TestCategory("Unit")]
        public void GlassOnTopOfGlass_ZT()
        {
            var above = this.McGlass;
            var below = this.McGlass2;
            var mc = MaterialCombination.GetMcToPaintWith(ZLayer.Top, this.Palette, above, below);
            Assert.AreEqual(below.Bottom.PixelStackerID, mc.Bottom.PixelStackerID, "Bottom should not change.");
            Assert.AreEqual(above.Top.PixelStackerID, mc.Top.PixelStackerID, "Top glass should be replaced.");
        }

        [TestMethod("Top: Solid on glass")]
        [TestCategory("Unit")]
        public void SolidOnTopOfGlass_ZT()
        {
            var above = this.McSolid;
            var below = this.McGlass2;
            var mc = MaterialCombination.GetMcToPaintWith(ZLayer.Top, this.Palette, above, below);
            Assert.AreEqual(below.Bottom.PixelStackerID, mc.Bottom.PixelStackerID, "Bottom1 != Bottom2");
            Assert.AreEqual(below.Bottom.PixelStackerID, mc.Top.PixelStackerID, "Top1 != Bottom2");
            Assert.AreEqual(mc.Top.PixelStackerID, mc.Bottom.PixelStackerID, "Top should match bottom.");
        }

        #endregion TOP: X on Glass

        #region TOP: X on Air
        [TestMethod("Top: Air on air")]
        [TestCategory("Unit")]
        public void AirOnTopOfAir_ZT()
        {
            var above = this.McAir;
            var below = this.McAir2;
            var mc = MaterialCombination.GetMcToPaintWith(ZLayer.Top, this.Palette, above, below);
            Assert.AreEqual(below.Bottom.PixelStackerID, mc.Bottom.PixelStackerID, "Bottom shouldn't change.");
            Assert.AreEqual(above.Top.PixelStackerID, mc.Top.PixelStackerID, "Top shouldn't change.");
        }

        [TestMethod("Top: Glass on air")]
        [TestCategory("Unit")]
        public void GlassOnTopOfAir_ZT()
        {
            var above = this.McGlass;
            var below = this.McAir2;
            var mc = MaterialCombination.GetMcToPaintWith(ZLayer.Top, this.Palette, above, below);
            Assert.AreEqual(below.Top.PixelStackerID, mc.Top.PixelStackerID, "Top shouldn't change.");
            Assert.AreEqual(below.Bottom.PixelStackerID, mc.Bottom.PixelStackerID, "Bottom shouldn't change.");
        }

        [TestMethod("Top: Solid on air")]
        [TestCategory("Unit")]
        public void SolidOnTopOfAir_ZT()
        {
            var above = this.McSolid;
            var below = this.McAir2;
            var mc = MaterialCombination.GetMcToPaintWith(ZLayer.Top, this.Palette, above, below);
            Assert.AreEqual(below.Bottom.PixelStackerID, mc.Bottom.PixelStackerID, "Bottom1 != Bottom2");
            Assert.AreEqual(below.Bottom.PixelStackerID, mc.Top.PixelStackerID, "Top1 != Bottom2");
            Assert.AreEqual(mc.Top.PixelStackerID, mc.Bottom.PixelStackerID, "Top should match bottom.");
        }

        #endregion TOP: X on Glass


        #region BOTTOM: X under Solid
        [TestMethod("Bottom: Air under solid")]
        [TestCategory("Unit")]
        public void AirOnTopOfSolid_ZB()
        {
            var above = this.McAir;
            var below = this.McSolid2;

            var mc = MaterialCombination.GetMcToPaintWith(ZLayer.Bottom, this.Palette, above, below);
            Assert.AreEqual(above.Bottom.PixelStackerID, mc.Bottom.PixelStackerID, "Bottom should match new bottom block.");
            Assert.AreEqual(above.Bottom.PixelStackerID, mc.Top.PixelStackerID, "Top should match new bottom block.");
        }

        [TestMethod("Bottom: Glass under solid")]
        [TestCategory("Unit")]
        public void GlassOnTopOfSolid_ZB()
        {
            var above = this.McGlass;
            var below = this.McSolid2;
            var mc = MaterialCombination.GetMcToPaintWith(ZLayer.Bottom, this.Palette, above, below);
            Assert.AreEqual(above.Bottom.PixelStackerID, mc.Bottom.PixelStackerID, "Bottom should match new bottom block.");
            Assert.AreEqual(above.Bottom.PixelStackerID, mc.Top.PixelStackerID, "Top should match new bottom block.");
        }

        [TestMethod("Bottom: Solid under solid")]
        [TestCategory("Unit")]
        public void SolidOnTopOfSolid_ZB()
        {
            var above = this.McSolid;
            var below = this.McSolid2;
            var mc = MaterialCombination.GetMcToPaintWith(ZLayer.Bottom, this.Palette, above, below);
            Assert.AreEqual(above.Bottom.PixelStackerID, mc.Bottom.PixelStackerID, "Bottom should match new bottom block.");
            Assert.AreEqual(above.Bottom.PixelStackerID, mc.Top.PixelStackerID, "Top should match new bottom block.");
        }
        #endregion BOTTOM: X under Solid

        #region BOTTOM: X under Glass
        [TestMethod("Bottom: Air under glass (Weird one)")]
        [TestCategory("Unit")]
        public void AirOnTopOfGlass_ZB()
        {
            // Special case since we do not support pure glass yet.
            var above = this.McAir;
            var below = this.McGlass2;
            var mc = MaterialCombination.GetMcToPaintWith(ZLayer.Bottom, this.Palette, above, below);
            Assert.AreEqual(above.Top.PixelStackerID, mc.Top.PixelStackerID, "Set top to air");
            Assert.AreEqual(above.Bottom.PixelStackerID, mc.Bottom.PixelStackerID, "Set bottom to air.");
        }

        [TestMethod("Bottom: Glass under glass")]
        [TestCategory("Unit")]
        public void GlassOnTopOfGlass_ZB()
        {
            var above = this.McGlass;
            var below = this.McGlass2;
            var mc = MaterialCombination.GetMcToPaintWith(ZLayer.Bottom, this.Palette, above, below);
            Assert.AreEqual(below.Top.PixelStackerID, mc.Top.PixelStackerID, "Do not change top glass.");
            Assert.AreEqual(above.Bottom.PixelStackerID, mc.Bottom.PixelStackerID, "Replace bottom layer.");
        }

        [TestMethod("Bottom: Solid under glass")]
        [TestCategory("Unit")]
        public void SolidOnTopOfGlass_ZB()
        {
            var above = this.McSolid;
            var below = this.McGlass2;
            var mc = MaterialCombination.GetMcToPaintWith(ZLayer.Bottom, this.Palette, above, below);
            Assert.AreEqual(below.Top.PixelStackerID, mc.Top.PixelStackerID, "Do not change top glass.");
            Assert.AreEqual(above.Bottom.PixelStackerID, mc.Bottom.PixelStackerID, "Replace bottom layer.");
        }

        #endregion BOTTOM: X under Glass

        #region BOTTOM: X under Air
        [TestMethod("Bottom: Air under air")]
        [TestCategory("Unit")]
        public void AirOnTopOfAir_ZB()
        {
            var above = this.McAir;
            var below = this.McAir2;
            var mc = MaterialCombination.GetMcToPaintWith(ZLayer.Bottom, this.Palette, above, below);
            Assert.AreEqual(below.Bottom.PixelStackerID, mc.Bottom.PixelStackerID, "Bottom shouldn't change.");
            Assert.AreEqual(above.Top.PixelStackerID, mc.Top.PixelStackerID, "Top shouldn't change.");
        }

        [TestMethod("Bottom: Glass under air")]
        [TestCategory("Unit")]
        public void GlassOnTopOfAir_ZB()
        {
            // Special case since we do not support pure glass yet.
            var above = this.McGlass;
            var below = this.McAir2;
            var mc = MaterialCombination.GetMcToPaintWith(ZLayer.Bottom, this.Palette, above, below);
            Assert.AreEqual(above.Bottom.PixelStackerID, mc.Top.PixelStackerID, "Top should be replaced.");
            Assert.AreEqual(above.Bottom.PixelStackerID, mc.Bottom.PixelStackerID, "Bottom should be replaced.");
            Assert.AreEqual(mc.Top.PixelStackerID, mc.Bottom.PixelStackerID, "Top should match bottom.");
        }

        [TestMethod("Bottom: Solid under air")]
        [TestCategory("Unit")]
        public void SolidOnTopOfAir_ZB()
        {
            var above = this.McSolid;
            var below = this.McAir2;
            var mc = MaterialCombination.GetMcToPaintWith(ZLayer.Bottom, this.Palette, above, below);
            Assert.AreEqual(above.Top.PixelStackerID, mc.Top.PixelStackerID, "Top should be replaced.");
            Assert.AreEqual(above.Bottom.PixelStackerID, mc.Bottom.PixelStackerID, "Bottom should be replaced.");
            Assert.AreEqual(mc.Top.PixelStackerID, mc.Bottom.PixelStackerID, "Top should match bottom.");
        }

        #endregion BOTTOM: X under Glass
    }
}
