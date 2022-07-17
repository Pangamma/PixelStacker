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
    public class SchematicFormatter : IExportFormatter
    {
        //        public static void writeBlueprint(string filePath, BlueprintPA blueprint)
        //        {
        //        }

        //        private static void WriteBlockDataAndIDs(NbtCompound nbt, BlueprintPA blueprint, bool isSideView)
        //        {
        //            Dictionary<string, short> schematicaDic = new Dictionary<string, short>();
        //            //int depth = Options.Get.IsMultiLayer ? 3 : 1;
        //            byte[] bs = new byte[blueprint.Mapper.GetXLength(isSideView) * blueprint.Mapper.GetYLength(isSideView) * blueprint.Mapper.GetZLength(isSideView)];
        //            byte[] bsD = new byte[blueprint.Mapper.GetXLength(isSideView) * blueprint.Mapper.GetYLength(isSideView) * blueprint.Mapper.GetZLength(isSideView)];
        //            int ti = 0;
        //            //-z = + y
        //            for (int y = blueprint.Mapper.GetYLength(isSideView) - 1; y >= 0; y--)
        //            {
        //                for (int z = 0; z < blueprint.Mapper.GetZLength(isSideView); z++)
        //                {
        //                    for (int x = 0; x < blueprint.Mapper.GetXLength(isSideView); x++)
        //                    {
        //                        Material ma = blueprint.Mapper.GetMaterialAt(isSideView, x, y, z);
        //                        schematicaDic[ma.SchematicaMaterialName ?? "minecraft:barrier"] = Convert.ToInt16(ma.BlockID);

        //                        bsD[ti] = Convert.ToByte(ma?.Data ?? 0);
        //                        bs[ti++] = Convert.ToByte(ma?.BlockID ?? 0);
        //                    }

        //                }
        //            }

        //            var schematicaIcon = new NbtCompound("Icon", new List<NbtTag>() {
        //                new NbtByte("Count", 1),
        //                new NbtShort("Damage", 0),
        //                new NbtString("id", "minecraft:white_wool")
        //            });

        //            nbt.Add(schematicaIcon);

        //            var schematicaMapping = new NbtCompound("SchematicaMapping");
        //            schematicaDic.ToList().ForEach(kvp => {
        //                schematicaMapping.Add(new NbtShort(kvp.Key, kvp.Value));
        //            });
        //            nbt.Add(schematicaMapping);

        //            nbt.Add(new NbtByteArray("Blocks", bs));
        //            nbt.Add(new NbtByteArray("Data", bsD));
        //        }



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
            var details = Schem2Formatter.ConvertCanvasToDetails(canvas);

            var nbt = new NbtCompound("Schematic");

            nbt.Add(new NbtShort("Width", (short)details.WidthX));
            nbt.Add(new NbtShort("Height", (short)details.HeightY));
            nbt.Add(new NbtShort("Length", (short)details.LengthZ));

            nbt.Add(new NbtString("Materials", "Alpha"));
            nbt.Add(new NbtList("Entities", NbtTagType.End));
            nbt.Add(new NbtList("TileEntities", NbtTagType.End));

            if (canvas.WorldEditOrigin != null)
            {
                nbt.Add(new NbtInt("WEOriginX", details.MetaData.WEOriginX));
                nbt.Add(new NbtInt("WEOriginY", details.MetaData.WEOriginY));
                nbt.Add(new NbtInt("WEOriginZ", details.MetaData.WEOriginZ));
                nbt.Add(new NbtInt("WEOffsetX", details.MetaData.WEOffsetX ?? 0));
                nbt.Add(new NbtInt("WEOffsetY", details.MetaData.WEOffsetY ?? 0));
                nbt.Add(new NbtInt("WEOffsetZ", details.MetaData.WEOffsetZ ?? 0));
            }


            var schematicaIcon = new NbtCompound("Icon", new List<NbtTag>() {
                            new NbtByte("Count", 1),
                            new NbtShort("Damage", 0),
                            new NbtString("id", "minecraft:white_wool")
                        });
            nbt.Add(schematicaIcon);



            int size = details.WidthX * details.HeightY * details.LengthZ;
            byte[] localBlocks = new byte[size];
            byte[] localMetadata = new byte[size];
            byte[] extraBlocks = new byte[size];
            byte[] extraBlocksNibble = new byte[(int)Math.Ceiling(size / 2.0)];
            bool extra = false;

            Dictionary<string, short> mappings = new Dictionary<string, short>();
            for (int y = 0; y < details.HeightY; y++)
            {
                for (int z = 0; z < details.LengthZ; z++)
                {
                    for (int x = 0; x < details.WidthX; x++)
                    {
                        int index = (size - 1) - ((y * details.LengthZ + z) * details.WidthX + x);
                        Material block = details.RegionXYZ[x][y][z];

                        int blockId = block.BlockID;
                        if (index >= size || index < 0)
                        {

                        }
                        localBlocks[index] = (byte)block.BlockID;
                        localMetadata[index] = (byte)block.Data;

                        // Not checked for C#. Coming from JAVA code.
                        if (((byte)(blockId >> 8)) > 0)
                        {
                            extraBlocks[index] = (byte)(blockId >> 8);
                            extra = true;
                        }

                        string name = block.SchematicaMaterialName ?? "minecraft:barrier";
                        if (!mappings.ContainsKey(name))
                        {
                            mappings[name] = (short)blockId;
                        }
                    }
                }
            }

            // NOT YET CHECKED FOR C#. COMING FROM JAVA.
            for (int i = 0; i < extraBlocksNibble.Length; i++)
            {
                if (i * 2 + 1 < extraBlocks.Length)
                {
                    extraBlocksNibble[i] = (byte)((extraBlocks[i * 2 + 0] << 4) | extraBlocks[i * 2 + 1]);
                }
                else
                {
                    extraBlocksNibble[i] = (byte)(extraBlocks[i * 2 + 0] << 4);
                }
            }



            //int count = 20;
            //NBTTagList tileEntitiesList = new NBTTagList();
            //for (TileEntity tileEntity : schematic.getTileEntities())
            //{
            //    try
            //    {
            //        NBTTagCompound tileEntityTagCompound = NBTHelper.writeTileEntityToCompound(tileEntity);
            //        tileEntitiesList.appendTag(tileEntityTagCompound);
            //    }
            //    catch (Exception e) {
            //        BlockPos tePos = tileEntity.getPos();
            //        int index = tePos.getX() + (tePos.getY() * schematic.getLength() + tePos.getZ()) * schematic.getWidth();
            //        if (--count > 0)
            //        {
            //            IBlockState blockState = schematic.getBlockState(tePos);
            //            Block block = blockState.getBlock();
            //            Reference.logger.error("Block {}[{}] with TileEntity {} failed to save! Replacing with bedrock...", block, block != null ? Block.REGISTRY.getNameForObject(block) : "?", tileEntity.getClass().getName(), e);
            //        }
            //        localBlocks[index] = (byte)Block.REGISTRY.getIDForObject(Blocks.BEDROCK);
            //        localMetadata[index] = 0;
            //        extraBlocks[index] = 0;
            //    }
            //}

            NbtCompound nbtMapping = new NbtCompound("SchematicaMapping");
            foreach (var entry in mappings)
            {
                nbtMapping.Add(new NbtShort(entry.Key, entry.Value));
            }

            nbt.Add(new NbtByteArray("Blocks", localBlocks));
            nbt.Add(new NbtByteArray("Data", localMetadata));

            if (extra)
            {
                nbt.Add(new NbtByteArray("AddBlocks", extraBlocksNibble));
            }

            nbt.Add(nbtMapping);


            //        private static void WriteBlockDataAndIDs(NbtCompound nbt, BlueprintPA blueprint, bool isSideView)
            //        {
            //            //int depth = Options.Get.IsMultiLayer ? 3 : 1;
            //            byte[] bs = new byte[blueprint.Mapper.GetXLength(isSideView) * blueprint.Mapper.GetYLength(isSideView) * blueprint.Mapper.GetZLength(isSideView)];
            //            byte[] bsD = new byte[blueprint.Mapper.GetXLength(isSideView) * blueprint.Mapper.GetYLength(isSideView) * blueprint.Mapper.GetZLength(isSideView)];
            //            int ti = 0;
            //            //-z = + y
            //            for (int y = blueprint.Mapper.GetYLength(isSideView) - 1; y >= 0; y--)
            //            {
            //                for (int z = 0; z < blueprint.Mapper.GetZLength(isSideView); z++)
            //                {
            //                    for (int x = 0; x < blueprint.Mapper.GetXLength(isSideView); x++)
            //                    {
            //                        Material ma = blueprint.Mapper.GetMaterialAt(isSideView, x, y, z);
            //                        schematicaDic[ma.SchematicaMaterialName ?? "minecraft:barrier"] = Convert.ToInt16(ma.BlockID);

            //                        bsD[ti] = Convert.ToByte(ma?.Data ?? 0);
            //                        bs[ti++] = Convert.ToByte(ma?.BlockID ?? 0);
            //                    }

            //                }
            //            }


            //            var schematicaMapping = new NbtCompound("SchematicaMapping");
            //            schematicaDic.ToList().ForEach(kvp => {
            //                schematicaMapping.Add(new NbtShort(kvp.Key, kvp.Value));
            //            });
            //            nbt.Add(schematicaMapping);

            //            nbt.Add(new NbtByteArray("Blocks", bs));
            //            nbt.Add(new NbtByteArray("Data", bsD));
            //        }

            //            var serverFile = new NbtFile(nbt);
            //            serverFile.SaveToFile(filePath, NbtCompression.GZip);
            var serverFile = new NbtFile(nbt);
            using var ms2 = new MemoryStream();
            serverFile.SaveToStream(ms2, NbtCompression.GZip);
            return Task.FromResult(ms2.ToArray());
        }
    }
}
