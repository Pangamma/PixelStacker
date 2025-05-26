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
    public class Schem2Formatter : IExportFormatter
    {
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
