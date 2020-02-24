using fNbt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelStacker.Logic
{
    class SchematicFormatter
    {
        public static void writeBlueprint(string filePath, BlueprintPA blueprint)
        {
            var nbt = new NbtCompound("Schematic");
            bool isv = Options.Get.IsSideView;
            nbt.Add(new NbtShort("Width", (short)blueprint.Mapper.GetXLength(isv)));
            nbt.Add(new NbtShort("Height", (short)blueprint.Mapper.GetYLength(isv)));
            nbt.Add(new NbtShort("Length", (short)blueprint.Mapper.GetZLength(isv)));
            if (Options.Get.IsSideView)
            {
                //-z = + y
                nbt.Add(new NbtString("Materials", "Alpha"));
                WriteBlockDataAndIDs(nbt, blueprint, Options.Get.IsSideView);
                nbt.Add(new NbtList("Entities", NbtTagType.End));
                nbt.Add(new NbtList("TileEntities", NbtTagType.End));
                if (blueprint.WorldEditOrigin != null)
                {
                    nbt.Add(new NbtInt("WEOriginX", 1));
                    nbt.Add(new NbtInt("WEOriginY", 1));
                    nbt.Add(new NbtInt("WEOriginZ", 1));
                    nbt.Add(new NbtInt("WEOffsetX", -blueprint.WorldEditOrigin.X));
                    nbt.Add(new NbtInt("WEOffsetY", blueprint.WorldEditOrigin.Y-blueprint.Height));
                    nbt.Add(new NbtInt("WEOffsetZ", Options.Get.IsMultiLayer ? -3 : -1));
                }
            }
            else
            {
                nbt.Add(new NbtString("Materials", "Alpha"));
                WriteBlockDataAndIDs(nbt, blueprint,Options.Get.IsSideView);
                nbt.Add(new NbtList("Entities", NbtTagType.End));
                nbt.Add(new NbtList("TileEntities", NbtTagType.End));
                if (blueprint.WorldEditOrigin != null)
                {
                    nbt.Add(new NbtInt("WEOriginX", 1));
                    nbt.Add(new NbtInt("WEOriginY", 1));
                    nbt.Add(new NbtInt("WEOriginZ", 1));
                    nbt.Add(new NbtInt("WEOffsetX", -blueprint.WorldEditOrigin.X));
                    nbt.Add(new NbtInt("WEOffsetY", 0));
                    nbt.Add(new NbtInt("WEOffsetZ", -blueprint.WorldEditOrigin.Y));
                }
            }

            var serverFile = new NbtFile(nbt);
            serverFile.SaveToFile(filePath, NbtCompression.GZip);
        }

        private static void WriteBlockDataAndIDs(NbtCompound nbt, BlueprintPA blueprint, bool isSideView)
        {
            Dictionary<string, short> schematicaDic = new Dictionary<string, short>();
            //int depth = Options.Get.IsMultiLayer ? 3 : 1;
            byte[] bs = new byte[blueprint.Mapper.GetXLength(isSideView) * blueprint.Mapper.GetYLength(isSideView) * blueprint.Mapper.GetZLength(isSideView)];
            byte[] bsD = new byte[blueprint.Mapper.GetXLength(isSideView) * blueprint.Mapper.GetYLength(isSideView) * blueprint.Mapper.GetZLength(isSideView)];
            int ti = 0;
            //-z = + y
            for (int y = blueprint.Mapper.GetYLength(isSideView) - 1; y >= 0; y--)
            {
                for (int z = 0; z < blueprint.Mapper.GetZLength(isSideView); z++)
                {
                    for (int x = 0; x < blueprint.Mapper.GetXLength(isSideView); x++)
                    {
                        Material ma = blueprint.Mapper.GetMaterialAt(isSideView, x, y, z);
                        schematicaDic[ma.SchematicaMaterialName ?? "minecraft:barrier"] = Convert.ToInt16(ma.BlockID);

                        bsD[ti] = Convert.ToByte(ma?.Data ?? 0);
                        bs[ti++] = Convert.ToByte(ma?.BlockID ?? 0);
                    }

                }
            }

            var schematicaIcon = new NbtCompound("Icon", new List<NbtTag>() {
                new NbtByte("Count", 1),
                new NbtShort("Damage", 0),
                new NbtString("id", "minecraft:white_wool")
            });

            nbt.Add(schematicaIcon);

            var schematicaMapping = new NbtCompound("SchematicaMapping");
            schematicaDic.ToList().ForEach(kvp => {
                schematicaMapping.Add(new NbtShort(kvp.Key, kvp.Value));
            });
            nbt.Add(schematicaMapping);

            nbt.Add(new NbtByteArray("Blocks", bs));
            nbt.Add(new NbtByteArray("Data", bsD));
        }


//        @Override
//        public boolean writeToNBT(final NBTTagCompound tagCompound, final ISchematic schematic)
//        {
//            final NBTTagCompound tagCompoundIcon = new NBTTagCompound();
//            final ItemStack icon = schematic.getIcon();
//            icon.writeToNBT(tagCompoundIcon);
//            tagCompound.setTag(Names.NBT.ICON, tagCompoundIcon);

//            tagCompound.setShort(Names.NBT.WIDTH, (short) schematic.getWidth());
//            tagCompound.setShort(Names.NBT.LENGTH, (short) schematic.getLength());
//            tagCompound.setShort(Names.NBT.HEIGHT, (short) schematic.getHeight());

//            final int size = schematic.getWidth() * schematic.getLength() * schematic.getHeight();
//            final byte[] localBlocks = new byte[size];
//            final byte[] localMetadata = new byte[size];
//            final byte[] extraBlocks = new byte[size];
//            final byte[] extraBlocksNibble = new byte[(int) Math.ceil(size / 2.0)];
//            boolean extra = false;

//            final MBlockPos pos = new MBlockPos();
//            final Map<String, Short> mappings = new HashMap<String, Short>();
//            for (int x = 0; x < schematic.getWidth(); x++)
//            {
//                for (int y = 0; y < schematic.getHeight(); y++)
//                {
//                    for (int z = 0; z < schematic.getLength(); z++)
//                    {
//                        final int index = x + (y * schematic.getLength() + z) * schematic.getWidth();
//                        final IBlockState blockState = schematic.getBlockState(pos.set(x, y, z));
//                        final Block block = blockState.getBlock();
//                        final int blockId = Block.REGISTRY.getIDForObject(block);
//                        localBlocks[index] = (byte) blockId;
//                        localMetadata[index] = (byte) block.getMetaFromState(blockState);
//                        if ((extraBlocks[index] = (byte) (blockId >> 8)) > 0)
//                        {
//                            extra = true;
//                        }

//                        final String name = String.valueOf(Block.REGISTRY.getNameForObject(block));
//                        if (!mappings.containsKey(name))
//                        {
//                            mappings.put(name, (short) blockId);
//                        }
//                    }
//                }
//            }

//            int count = 20;
//            final NBTTagList tileEntitiesList = new NBTTagList();
//            for (final TileEntity tileEntity : schematic.getTileEntities())
//            {
//                try
//                {
//                    final NBTTagCompound tileEntityTagCompound = NBTHelper.writeTileEntityToCompound(tileEntity);
//                    tileEntitiesList.appendTag(tileEntityTagCompound);
//                }
//                catch (final Exception e) {
//                final BlockPos tePos = tileEntity.getPos();
//                final int index = tePos.getX() + (tePos.getY() * schematic.getLength() + tePos.getZ()) * schematic.getWidth();
//                if (--count > 0)
//                {
//                    final IBlockState blockState = schematic.getBlockState(tePos);
//                    final Block block = blockState.getBlock();
//                    Reference.logger.error("Block {}[{}] with TileEntity {} failed to save! Replacing with bedrock...", block, block != null ? Block.REGISTRY.getNameForObject(block) : "?", tileEntity.getClass().getName(), e);
//                }
//                localBlocks[index] = (byte) Block.REGISTRY.getIDForObject(Blocks.BEDROCK);
//                localMetadata[index] = 0;
//                extraBlocks[index] = 0;
//            }
//        }

//        for (int i = 0; i<extraBlocksNibble.length; i++) {
//            if (i* 2 + 1 < extraBlocks.length) {
//                extraBlocksNibble[i] = (byte) ((extraBlocks[i * 2 + 0] << 4) | extraBlocks[i * 2 + 1]);
//            } else {
//                extraBlocksNibble[i] = (byte) (extraBlocks[i * 2 + 0] << 4);
//            }
//        }

//        final NBTTagList entityList = new NBTTagList();
//final List<Entity> entities = schematic.getEntities();
//        for (final Entity entity : entities) {
//            try {
//                final NBTTagCompound entityCompound = NBTHelper.writeEntityToCompound(entity);
//                if (entityCompound != null) {
//                    entityList.appendTag(entityCompound);
//                }
//            } catch (final Throwable t) {
//                Reference.logger.error("Entity {} failed to save, skipping!", entity, t);
//            }
//        }

//        final PreSchematicSaveEvent event = new PreSchematicSaveEvent(schematic, mappings);
//MinecraftForge.EVENT_BUS.post(event);

//final NBTTagCompound nbtMapping = new NBTTagCompound();
//        for (final Map.Entry<String, Short> entry : mappings.entrySet()) {
//            nbtMapping.setShort(entry.getKey(), entry.getValue());
//        }

//        tagCompound.setString(Names.NBT.MATERIALS, Names.NBT.FORMAT_ALPHA);
//        tagCompound.setByteArray(Names.NBT.BLOCKS, localBlocks);
//        tagCompound.setByteArray(Names.NBT.DATA, localMetadata);
//        if (extra) {
//            tagCompound.setByteArray(Names.NBT.ADD_BLOCKS, extraBlocksNibble);
//        }
//        tagCompound.setTag(Names.NBT.ENTITIES, entityList);
//        tagCompound.setTag(Names.NBT.TILE_ENTITIES, tileEntitiesList);
//        tagCompound.setTag(Names.NBT.MAPPING_SCHEMATICA, nbtMapping);
//        final NBTTagCompound extendedMetadata = event.extendedMetadata;
//        if (!extendedMetadata.hasNoTags()) {
//    tagCompound.setTag(Names.NBT.EXTENDED_METADATA, extendedMetadata);
//}

//        return true;
//}

    }
}
