using FNBT;
using FNBT.Tags;
using PixelStacker.Logic.IO.Config;
using PixelStacker.Logic.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PixelStacker.Logic.IO.Formatters
{
    #region Supporting Models
    public class Schem2Details
    {
        public SchemMetaData MetaData { get; set; }

        // X Y Z
        public Material[][][] RegionXYZ { get; set; }
        public int WidthX { get; set; }
        public int LengthZ { get; set; }
        public int HeightY { get; set; }

        public Schem2Details()
        {
            MetaData = new SchemMetaData();
        }
    }

    public class SchemMetaData
    {
        public string Name { get; set; } = "NameOfSchematic";
        public string Author { get; set; } = "Taylor Love";
        public int WEOriginX => 1;
        public int WEOriginY => 1;
        public int WEOriginZ => 1;
        public int? WEOffsetX { get; set; }
        public int? WEOffsetY { get; set; }
        public int? WEOffsetZ { get; set; }
    }

    #endregion 

    public class Schem2Formatter : IExportFormatter
    {
        //public static void writeBlueprint(string filePath, BlueprintPA blueprint)
        //{
        //    bool isv = Options.Get.IsSideView;
        //    bool isMultiLayer = Options.Get.IsMultiLayer;
        //    Material[][][] region;
        //    var details = new Schem2Details();

        //    int xD, yD, zD;

        //    if (isv)
        //    {
        //        xD = canvasData.GetLength(0);
        //        yD = canvasData.GetLength(1);
        //        zD = isMultiLayer ? 3 : 1;

        //        if (blueprint.WorldEditOrigin != null)
        //        {
        //            details.MetaData.WEOffsetX = -blueprint.WorldEditOrigin.X;
        //            details.MetaData.WEOffsetY = blueprint.WorldEditOrigin.Y - yD;
        //            details.MetaData.WEOffsetZ = -zD;
        //        }
        //    }
        //    else
        //    {
        //        xD = canvasData.GetLength(0);
        //        yD = isMultiLayer ? 2 : 1;
        //        zD = canvasData.GetLength(1);

        //        if (blueprint.WorldEditOrigin != null)
        //        {
        //            details.MetaData.WEOffsetX = -blueprint.WorldEditOrigin.X;
        //            details.MetaData.WEOffsetY = 0;
        //            details.MetaData.WEOffsetZ = -blueprint.WorldEditOrigin.Y;
        //        }
        //    }

        //    region = new Material[xD][][];
        //    for (int xi = 0; xi < xD; xi++)
        //    {
        //        region[xi] = new Material[yD][];
        //        for (int yi = 0; yi < yD; yi++)
        //        {
        //            region[xi][yi] = new Material[zD];
        //        }
        //    }

        //    details.RegionXYZ = region;
        //    details.WidthX = xD;
        //    details.HeightY = yD;
        //    details.LengthZ = zD;

        //    // TODO: Populate based on ISV
        //    if (isv)
        //    {
        //        for (int xr = 0; xr < xD; xr++)
        //        {
        //            for (int yr = 0; yr < yD; yr++)
        //            {
        //                int ci = canvasData[xr, yD - 1 - yr];
        //                Color c = Color.FromArgb(ci);
        //                var mm = (ColorMatcher.Get.ColorToMaterialMap.TryGetValue(c, out Material[] found) ? found : null) ?? new Material[] { Materials.Air };

        //                if (isMultiLayer)
        //                {
        //                    region[xr][yr][0] = mm.Last();
        //                    region[xr][yr][1] = mm.First();
        //                    region[xr][yr][2] = mm.Last();
        //                }
        //                else
        //                {
        //                    region[xr][yD - yr - 1][0] = mm.First();
        //                }
        //            }
        //        }
        //    }
        //    else
        //    {
        //        for (int xr = 0; xr < xD; xr++)
        //        {
        //            for (int zr = 0; zr < zD; zr++)
        //            {
        //                int ci = canvasData[xr, zr]; // WARN: Maybe this needs to be zD - 1 - zr
        //                Color c = Color.FromArgb(ci);
        //                var mm = (ColorMatcher.Get.ColorToMaterialMap.TryGetValue(c, out Material[] found) ? found : null) ?? new Material[] { Materials.Air };

        //                if (isMultiLayer)
        //                {
        //                    region[xr][0][zr] = mm.First(); // If this turns out inside-out, then swap First with Last calls. 
        //                    region[xr][1][zr] = mm.Last();
        //                }
        //                else
        //                {
        //                    region[xr][0][zr] = mm.First();
        //                }
        //            }
        //        }
        //    }

        //    writeBlueprintDirect(filePath, details);
        //}

        public static byte[] SchematicToNBT(Schem2Details details, bool isMultilayer, bool isv)
        {
            var nbt = new NbtCompound("Schematic");
            nbt.Add(new NbtInt("Version", 2)); // Schematic format version
            nbt.Add(new NbtInt("DataVersion", Constants.DataVersion));

            {
                var metadata = new NbtCompound("Metadata");
                metadata.Add(new NbtString("Name", details.MetaData.Name));
                metadata.Add(new NbtString("Author", details.MetaData.Author));
                metadata.Add(new NbtString("Generator", "PixelStacker (" + Constants.Version + ")"));
                metadata.Add(new NbtString("Generator Website", Constants.Website));
                metadata.Add(new NbtList("RequiredMods", new List<NbtTag>(), NbtTagType.String));
                metadata.Add(new NbtLong("Date", DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()));
                metadata.Add(new NbtInt("WEOriginX", 1));
                metadata.Add(new NbtInt("WEOriginY", 1));
                metadata.Add(new NbtInt("WEOriginZ", 1));

                if (details.MetaData.WEOffsetX != null)
                {
                    metadata.Add(new NbtInt("WEOffsetX", details.MetaData.WEOffsetX.Value));
                    metadata.Add(new NbtInt("WEOffsetY", details.MetaData.WEOffsetY.Value));
                    metadata.Add(new NbtInt("WEOffsetZ", details.MetaData.WEOffsetZ.Value));
                }

                nbt.Add(metadata);
            }

            {
                nbt.Add(new NbtIntArray("Offset", new int[] { 0, 0, 0 }));
                nbt.Add(new NbtShort("Width", (short)details.WidthX));
                nbt.Add(new NbtShort("Height", (short)details.HeightY));
                nbt.Add(new NbtShort("Length", (short)details.LengthZ));
            }

            var palette = new Dictionary<string, int>();
            var tileEntities = new List<NbtCompound>();

            //Required.Specifies the main storage array which contains Width *Height * Length entries.Each entry is specified 
            //as a varint and refers to an index within the Palette.The entries are indexed by 
            //x +z * Width + y * Width * Length.
            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter buffer = new BinaryWriter(ms))
                {
                    int xMax = details.WidthX;
                    int yMax = details.HeightY;
                    int zMax = details.LengthZ;

                    var r = details.RegionXYZ;
                    for (int y = 0; y < yMax; y++)
                    {
                        for (int z = 0; z < zMax; z++)
                        {
                            for (int x = 0; x < xMax; x++)
                            {
                                var mat = r[x][y][z];
                                string blockKey = mat.GetBlockNameAndData(isv);
                                if (!palette.ContainsKey(blockKey))
                                {
                                    palette.Add(blockKey, palette.Count);
                                }

                                int blockID = palette[blockKey];

                                while ((blockID & -128) != 0)
                                {
                                    buffer.Write((byte)(blockID & 127 | 128));
                                    blockID = (int)((uint)blockID >> 7);
                                }
                                buffer.Write((byte)blockID);
                            }
                        }
                    }
                }

                // size of block palette in number of bytes needed for the maximum  palette index. Implementations may use
                // this as a hint for the case that the palette data fits within a datatype smaller than a 32 - bit integer
                // that they may allocate a smaller sized array.
                nbt.Add(new NbtInt("PaletteMax", palette.Count));
                var paletteTag = new NbtCompound("Palette");
                var paletteList = palette.OrderBy(kvp => kvp.Value);
                foreach (var kvp in paletteList)
                {
                    paletteTag.Add(new NbtInt(kvp.Key, kvp.Value));
                }

                nbt.Add(paletteTag);
                var blockData = ms.ToArray();
                nbt.Add(new NbtByteArray("BlockData", blockData));
            }

            nbt.Add(new NbtList("TileEntities", NbtTagType.End));

            var serverFile = new NbtFile(nbt);
            using var ms2 = new MemoryStream();
            serverFile.SaveToStream(ms2, NbtCompression.GZip);
            return ms2.ToArray();
        }

        public async Task ExportAsync(string filePath, PixelStackerProjectData canvas, CancellationToken? worker = null)
        {
            if (File.Exists(filePath)) File.Delete(filePath);
            byte[] data = await ExportAsync(canvas, worker);
            File.WriteAllBytes(filePath, data);
        }

        public Task<byte[]> ExportAsync(PixelStackerProjectData canvas, CancellationToken? worker = null)
        {
            bool isv = canvas.IsSideView;
            bool isMultiLayer = canvas.CanvasData.Any(x => canvas.MaterialPalette[x.PaletteID].IsMultiLayer);
            Material[][][] region;
            var details = new Schem2Details();
            var canvasData = canvas.CanvasData;

            int xD, yD, zD;

            if (isv)
            {
                xD = canvasData.Width;
                yD = canvasData.Height;
                zD = isMultiLayer ? 3 : 1;

                if (canvas.WorldEditOrigin != null)
                {
                    details.MetaData.WEOffsetX = -canvas.WorldEditOrigin.X;
                    details.MetaData.WEOffsetY = canvas.WorldEditOrigin.Y - yD;
                    details.MetaData.WEOffsetZ = -zD;
                }
            }
            else
            {
                xD = canvasData.Width;
                yD = isMultiLayer ? 2 : 1;
                zD = canvasData.Height;

                if (canvas.WorldEditOrigin != null)
                {
                    details.MetaData.WEOffsetX = -canvas.WorldEditOrigin.X;
                    details.MetaData.WEOffsetY = 0;
                    details.MetaData.WEOffsetZ = -canvas.WorldEditOrigin.Y;
                }
            }

            region = new Material[xD][][];
            for (int xi = 0; xi < xD; xi++)
            {
                region[xi] = new Material[yD][];
                for (int yi = 0; yi < yD; yi++)
                {
                    region[xi][yi] = new Material[zD];
                }
            }

            details.RegionXYZ = region;
            details.WidthX = xD;
            details.HeightY = yD;
            details.LengthZ = zD;

            if (isv)
            {
                for (int xr = 0; xr < xD; xr++)
                {
                    for (int yr = 0; yr < yD; yr++)
                    {
                        MaterialCombination mm = canvasData[xr, yD - 1 - yr];

                        if (isMultiLayer)
                        {
                            region[xr][yr][0] = mm.Top;
                            region[xr][yr][1] = mm.Bottom;
                            region[xr][yr][2] = mm.Top;
                        }
                        else
                        {
                            region[xr][yD - yr - 1][0] = mm.Bottom;
                        }
                    }
                }
            }
            else
            {
                for (int xr = 0; xr < xD; xr++)
                {
                    for (int zr = 0; zr < zD; zr++)
                    {
                        MaterialCombination mm = canvasData[xr, zr]; // WARN: Maybe this needs to be zD - 1 - zr

                        if (isMultiLayer)
                        {
                            region[xr][0][zr] = mm.Bottom; // If this turns out inside-out, then swap First with Last calls. 
                            region[xr][1][zr] = mm.Top;
                        }
                        else
                        {
                            region[xr][0][zr] = mm.Bottom;
                        }
                    }
                }
            }

            byte[] bbData = SchematicToNBT(details, isMultiLayer, isv);
            return Task.FromResult(bbData);
        }
    }
}
