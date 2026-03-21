# Add a New Material to PixelStacker

Follow these steps to add a new cubic block material to PixelStacker.

## Step 1: Verify the texture exists

Check that a 16×16 PNG for the block exists in:
```
src/PixelStacker.Resources/Images/Textures/x16/<texture_name>.png
```

If it's missing, run the `ImageTextureUpdater.TestZipUpdate()` test (category: `Tools`) to extract textures from the Minecraft JAR, or add the PNG manually.

For blocks with distinct top vs. side textures (logs, pillars, etc.), there may be two files (e.g. `tuff.png` and `tuff_top.png`).

## Step 2: Add the Material entry

Open `src/PixelStacker.Logic/Model/Materials.cs` and add a new `Material(...)` call inside the appropriate category group.

**Constructor signature:**
```csharp
new Material(minMcVersion, isAdvanced, category, pixelStackerID, label,
             Textures.GetBitmap("top_texture_name"),
             Textures.GetBitmap("side_texture_name"),
             $"minecraft:block_id",
             $"minecraft:block_id",
             "minecraft:legacy_id_or_empty_string")
```

**Field reference:**

| Parameter | Notes |
|---|---|
| `minMcVersion` | Must be one of the allowed strings: `NEW`, `1.7`, `1.8`, `1.9`, `1.10`, `1.11`, `1.12`, `1.13`, `1.14`, `1.15`, `1.16`, `1.17`, `1.19`, `1.20`, `1.21.5` — note there is no `1.18`, `1.21`, etc. Use the closest valid version at or after when the block was added. |
| `isAdvanced` | `true` to hide the block behind the "Advanced" toggle in the UI |
| `category` | Group label shown in the material picker (e.g. `"Bricks"`, `"Other"`, `"Ores (Solid)"`) |
| `pixelStackerID` | Short ALL_CAPS unique key (e.g. `"POLISHED_TUFF"`) |
| `label` | Human-readable display name |
| `top_texture_name` | Filename without `.png` — used for the top-down view |
| `side_texture_name` | Filename without `.png` — used for the side view (same as top for uniform blocks) |
| `minecraft:block_id` | Modern Java Edition block ID (arg 7 = top orientation, arg 8 = side orientation) |
| legacy ID | Schematica/legacy ID if applicable, otherwise `""` |

**Example (Polished Tuff, uniform texture, no legacy ID):**
```csharp
new Material("1.21.5", false, "Other", "POLISHED_TUFF", "Polished Tuff",
    Textures.GetBitmap("polished_tuff"),
    Textures.GetBitmap("polished_tuff"),
    $"minecraft:polished_tuff",
    $"minecraft:polished_tuff",
    ""),
```

**Example (Deepslate, distinct top/side, with blockstate axis):**
```csharp
new Material("1.17", false, "Other", "DEEPSLATE", "Deepslate",
    Textures.GetBitmap("deepslate_top"),
    Textures.GetBitmap("deepslate"),
    $"minecraft:deepslate",
    $"minecraft:deepslate",
    ""),
```

## Step 3: Regenerate the material palette

Run the following test to regenerate `materialPalette.json`:

- **Project:** `PixelStacker.CodeGenerator`
- **Class:** `MaterialPaletteGenerator`
- **Method:** `GenerateMaterialCombinationPalette()`
- **Category:** `Generators`

This updates `src/PixelStacker.Resources/Files/materialPalette.json`, which is the pre-computed combination index loaded at startup. It also touches `Data.resx` so MSBuild re-embeds the updated JSON into the assembly on the next build — without this touch, incremental builds would leave the old data embedded.

## That's it

No other files need to be touched for a standard cubic block with a uniform texture. The material will appear in the block picker filtered by its `minMcVersion` and `isAdvanced` flags.
