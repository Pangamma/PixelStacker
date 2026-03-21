# Update Localization Strings

Follow these steps to add or update localized strings in PixelStacker.

## Step 1: Add or update entries in Text.resx

Open `src/PixelStacker.Resources/Text.resx` and add or modify string entries.

Each entry looks like:
```xml
<data name="SomeKey" xml:space="preserve">
  <value>Your English text here.</value>
</data>
```

Keys must not start with `$` or `>>` — those are reserved and are skipped by the generator.

## Step 2: Verify en.json reflects the current Text.resx

The source of truth for English strings is `Text.resx`. The generator syncs it into:
```
src/PixelStacker.Resources/Localization/en.json
```

The generator will:
- **Add** keys present in `Text.resx` but missing from `en.json`
- **Update** keys whose values have changed in `Text.resx`
- **Remove** keys present in `en.json` that no longer exist in `Text.resx`

You do not need to edit `en.json` manually — the test in Step 3 handles it.

## Step 3: Run the translation generator test

Run the following test to sync `en.json` and translate any new/changed keys into all supported locales:

- **Project:** `PixelStacker.CodeGenerator`
- **Class:** `JsonResxMaker`
- **Method:** `Text_Translate`
- **Category:** `Generators`

This test requires an OpenAI API key stored in user secrets under `OPENAI_API_KEY`. It uses `gpt-4o-mini` to translate new or updated keys.

**What it does:**
1. Reads all keys from `Text.resx`
2. Syncs `en.json` (adds new keys, updates changed values, removes stale keys)
3. For each supported locale, translates only the keys that are missing or newly changed
4. Writes the updated locale JSON files (sorted alphabetically by key)
5. Touches `ResxContainer.resx` so MSBuild re-embeds the updated locale files on the next build

**Supported output locales:**

| Locale | Language |
|--------|----------|
| `da`   | Danish |
| `de`   | German |
| `es`   | Spanish |
| `fr`   | French |
| `it`   | Italian |
| `ja`   | Japanese |
| `ko`   | Korean |
| `nl`   | Dutch |
| `sv`   | Swedish |
| `zh`   | Chinese (Simplified) |
| `zu`   | Zulu |

Locale JSON files live in:
```
src/PixelStacker.Resources/Localization/<lang>.json
```

## Why touching ResxContainer.resx matters

The locale JSON files are embedded into the assembly via `ResXFileRef` entries in `ResxContainer.resx`. MSBuild's `GenerateResource` task uses timestamp comparison — if `ResxContainer.resx` itself hasn't changed, the task is skipped even when the referenced JSON files are newer. The generator touches `ResxContainer.resx` after writing all locale files to ensure incremental builds pick up the changes.

## That's it

No other files need to be touched. On the next build, the updated locale JSON files will be re-embedded into the assembly automatically.
