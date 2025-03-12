﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using PixelStacker.Logic.Model;
using Newtonsoft.Json;
using System.IO;

namespace PixelStacker.CodeGenerator
{
    [TestClass]
    public class MaterialPaletteGenerator
    {
        private readonly string RootDir = AppDomain.CurrentDomain.BaseDirectory
            .Split(new string[] { "\\PixelStacker.CodeGenerator\\bin\\" }, StringSplitOptions.RemoveEmptyEntries)
            .FirstOrDefault();

        [TestMethod]
        [TestCategory("Generators")]
        public void GenerateMaterialCombinationPalette()
        {
            string filePath = RootDir + "\\PixelStacker.Resources\\Files\\materialPalette.json";
            MaterialPalette palette;
            {
                string paletteFile = filePath;
                string json = File.ReadAllText(paletteFile);
                palette = JsonConvert.DeserializeObject<MaterialPalette>(json) ?? new MaterialPalette();
            }

            var glasses = Materials.List.Where(m2 => m2.Category == "Glass");
            var solids = Materials.List.Where(m2 => m2.Category != "Glass" && m2.Category != "Air");

            bool isModified = false;
#pragma warning disable CS0618 // Type or member is obsolete
            isModified |= palette.AddCombination(new MaterialCombination(Materials.Air));
            foreach (var solid in solids)
            {
                isModified |= palette.AddCombination(new MaterialCombination(solid));
                foreach (var glass in glasses)
                {
                    isModified |= palette.AddCombination(new MaterialCombination(solid, glass));
                }
            }

            foreach (var glass in glasses)
            {
                isModified |= palette.AddCombination(new MaterialCombination(glass));
            }
#pragma warning restore CS0618 // Type or member is obsolete

            Console.WriteLine("Material Palette has been populated. Size = " + palette.Count);
            Console.WriteLine($"IsModified={isModified}");

            if (isModified)
            {
                string json = JsonConvert.SerializeObject(palette, Formatting.Indented);
                File.WriteAllText(filePath, json);
            }
        }
    }
}
