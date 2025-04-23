using FNBT;
using FNBT.Tags;
using PixelStacker.Logic.IO.Config;
using PixelStacker.Logic.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PixelStacker.Logic.IO.Formatters
{
    #region Supporting Models
    public class Schem3Details
    {
        public SchemMetaData MetaData { get; set; }

        public Material[][][] RegionXYZ { get; set; }
        public int WidthX { get; set; }
        public int LengthZ { get; set; }
        public int HeightY { get; set; }

        public Schem3Details()
        {
            MetaData = new SchemMetaData();
        }
    }
    #endregion

    /// <summary>
    /// Generated with ChatGPT-4o, following this spec:
    /// https://github.com/SpongePowered/Schematic-Specification/blob/master/versions/schematic-3.md
    /// </summary>
    public class Schem3Formatter : IExportFormatter
    {
        public static byte[] SchematicToNBT(Schem2Details details, bool isMultilayer, bool isv)
        {
            var nbt = new NbtCompound("Schematic");

            nbt.Add(new NbtInt("Version", 3));
            nbt.Add(new NbtInt("DataVersion", Constants.DataVersion));

            // Metadata
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

            // Dimensions
            nbt.Add(new NbtIntArray("Offset", new int[] { 0, 0, 0 }));
            nbt.Add(new NbtShort("Width", (short)details.WidthX));
            nbt.Add(new NbtShort("Height", (short)details.HeightY));
            nbt.Add(new NbtShort("Length", (short)details.LengthZ));

            // Block Data & Palette
            var palette = new Dictionary<string, int>();
            var paletteList = new List<NbtCompound>();

            using (MemoryStream ms = new MemoryStream())
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
                                palette[blockKey] = paletteList.Count;
                                var stateTag = new NbtCompound();
                                stateTag.Add(new NbtString("Name", blockKey));
                                stateTag.Add(new NbtCompound("Properties"));
                                paletteList.Add(stateTag);
                            }

                            int blockID = palette[blockKey];

                            // Write varint blockID
                            while ((blockID & -128) != 0)
                            {
                                buffer.Write((byte)(blockID & 127 | 128));
                                blockID = (int)((uint)blockID >> 7);
                            }
                            buffer.Write((byte)blockID);
                        }
                    }
                }

                nbt.Add(new NbtInt("PaletteMax", palette.Count));
                nbt.Add(new NbtList("Palette", paletteList, NbtTagType.Compound));
                nbt.Add(new NbtByteArray("BlockData", ms.ToArray()));
            }

            // Required for v3
            nbt.Add(new NbtList("BlockEntities", NbtTagType.End));
            nbt.Add(new NbtList("Entities", NbtTagType.End));

            var file = new NbtFile(nbt);
            using var msOut = new MemoryStream();
            file.SaveToStream(msOut, NbtCompression.GZip);
            return msOut.ToArray();
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
            Schem2Details details = IExportFormatter.ConvertCanvasToDetails(canvas);
            byte[] bbData = SchematicToNBT(details, isMultiLayer, isv);
            return Task.FromResult(bbData);
        }
    }
}
