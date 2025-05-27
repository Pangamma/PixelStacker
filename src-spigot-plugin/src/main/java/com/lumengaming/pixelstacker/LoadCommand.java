package com.lumengaming.pixelstacker;

import com.google.common.io.Files;
import com.google.gson.Gson;
import com.google.gson.JsonArray;
import com.google.gson.JsonObject;
import com.google.gson.JsonParser;
import com.sk89q.worldedit.LocalSession;
import com.sk89q.worldedit.bukkit.WorldEditPlugin;
import com.sk89q.worldedit.extent.clipboard.Clipboard;
import com.sk89q.worldedit.extent.clipboard.io.ClipboardFormats;
import com.sk89q.worldedit.extent.clipboard.io.ClipboardFormat;
import com.sk89q.worldedit.extent.clipboard.io.ClipboardReader;
import com.sk89q.worldedit.session.ClipboardHolder;
import org.bukkit.Bukkit;
import java.io.*;
import java.io.IOException;
import java.net.URI;
import java.net.URISyntaxException;
import java.net.URLConnection;
import java.net.URLEncoder;
import java.net.http.HttpClient;
import java.net.http.HttpRequest;
import java.net.http.HttpRequest.BodyPublishers;
import java.net.http.HttpResponse;
import java.nio.charset.StandardCharsets;
import java.util.Arrays;
import java.util.HashMap;
import java.util.UUID;
import java.util.logging.Level;
import java.util.logging.Logger;
import java.util.stream.Collectors;
import org.bukkit.command.Command;
import org.bukkit.command.CommandExecutor;
import org.bukkit.command.CommandSender;
import org.bukkit.entity.Player;

/**
 *
 * @author prota
 */
public class LoadCommand implements CommandExecutor {
    private final PixelStackerMain plugin;

    public LoadCommand(PixelStackerMain plugin) {
        this.plugin = plugin;
    }
    
    
    @Override
    public boolean onCommand(CommandSender cs, Command cmnd, String string, String[] args) {
        if (cs == null) {
            return false; // Silence IDE warnings.
        }
        
        if (!(cs instanceof Player)) {
            cs.sendMessage("Only players can use this command.");
            return false;
        }
        
        WorldEditPlugin worldEdit = (WorldEditPlugin) Bukkit.getPluginManager().getPlugin("WorldEdit");
        if (worldEdit == null) {
            cs.sendMessage("§cWorldEdit not found.");
            return true;
        }
        
        Player p = (Player) cs;
        RenderRequest req = this.parseCommandArguments(p, args);
        String url = args.length > 1 ? args[1] : "";
        if (url.isBlank() || req == null) {
            return true;
        }
        
        byte[] downloadedImage;
        try {
            downloadedImage = downloadImageToBytes(url);
        } catch (IOException | InterruptedException ex) {
            Logger.getLogger(LoadCommand.class.getName()).log(Level.SEVERE, null, ex);
            p.sendMessage("§cUnable to load image at: "+url);
            return true;
        }
        
        String apiKey = PixelStackerConfig.Get().ApiKey;
        byte[] apiResponseData;
        
        try {
            HttpResponse<byte[]> apiResponse = this.sendImageToApiForRendering(req, apiKey, downloadedImage);
            apiResponseData = apiResponse.body();
            
            if (apiResponse.statusCode() != 200) {
                String errorJson = new String(apiResponseData, StandardCharsets.UTF_8);
                String errorMsg = extractFirstErrorMessage(errorJson);
                if (errorMsg != null) {
                    p.sendMessage("§cPixelStacker API request failed: "+errorMsg);
                    return true;
                } else {
                    p.sendMessage("§cPixelStacker API request failed. See console for more details.");
                    Logger.getLogger(LoadCommand.class.getName()).log(Level.SEVERE, errorJson);
                    return true;
                }
            }
        } catch (IOException | InterruptedException | URISyntaxException ex) {
            Logger.getLogger(LoadCommand.class.getName()).log(Level.SEVERE, null, ex);
            p.sendMessage("§cA problem occurred when reaching the PixelStacker API. See console for more details.");
            return true;
        }
        
        
        
        // Loading worldedit plugin now...
        ClipboardFormat format = ClipboardFormats.findByAlias("sponge.2");
        if (format == null) {
            Logger.getLogger(LoadCommand.class.getName()).log(Level.SEVERE, "Clipboard format not supported.");
                p.sendMessage("§cPixelStacker command failed. Clipboard format not supported.");
            return true;
        }
        
        try {
            InputStream inputStream = new ByteArrayInputStream(apiResponseData);
            ClipboardReader reader = format.getReader(inputStream);
            Clipboard clipboard = reader.read();
            LocalSession session = worldEdit.getSession(p);
            ClipboardHolder clipboardHolder = new ClipboardHolder(clipboard);
            session.setClipboard(clipboardHolder);
            
        } catch (IOException ex) {
            Logger.getLogger(LoadCommand.class.getName()).log(Level.SEVERE, null, ex);
        }
        
        p.sendMessage("§aYour picture has been loaded. Paste it with //paste.");  
        Gson gson = new Gson();
        String jsonInputString = gson.toJson(req, RenderRequest.class);      
        p.sendMessage("§8" + jsonInputString);  
        return true;
    }

    
    private RenderRequest parseCommandArguments(Player p, String[] args) {
        var req = new RenderRequest();
        RenderRequest def = PixelStackerConfig.Get().Defaults;

        req.enableDithering = def.enableDithering;
        req.format = def.format;
        req.isMultiLayer = def.isMultiLayer;
        req.isSideView = def.isSideView;
        req.maxHeight = def.maxHeight;
        req.maxWidth = def.maxWidth;
        req.quantizedColorCount = def.quantizedColorCount;
        req.rgbBucketSize = def.rgbBucketSize;

        try {
            if (!args[0].equalsIgnoreCase("load")) {
                printHelp(p);
                return null;
            }
            
            if (!(p.isOp() || p.hasPermission("pixelstacker.load") || p.hasPermission("pixelstacker.*"))) {
                p.sendMessage("§cyou are not OP and you lack the 'pixelstacker.load' permission.");
                return null;
            }
            
            String url;
            switch(args.length) {
                case 9:
                    req.quantizedColorCount = Integer.parseInt(args[8]);
                    if (req.quantizedColorCount == -1) {
                        req.quantizedColorCount = null;
                    }
                    else if (!(req.quantizedColorCount > 0 && req.quantizedColorCount <= 256)) {
                        p.sendMessage("§cThe quantized color count is invalid. Please check available options.");
                        printHelp(p);
                        return null;
                    }
                    req.enableDithering = this.parseBoolFromArg(args[7]);
//                case 7:
                    req.rgbBucketSize = Integer.parseInt(args[6]);
                    if (!Arrays.stream(new int[]{1, 5, 15, 17, 51}).anyMatch(i -> i == req.rgbBucketSize)) {
                        p.sendMessage("§cThe rgb bucket size provided was not a valid value.");
                        printHelp(p);
                        return null;
                    }
                case 6:
                    req.isMultiLayer = this.parseBoolFromArg(args[5]);
//                case 5:
                    req.maxHeight = Integer.parseInt(args[4]);
                    req.maxWidth = Integer.parseInt(args[3]);
                    req.isSideView = args[2].equalsIgnoreCase("V") || args[2].equalsIgnoreCase("vertical");
                case 2: 
                    url = args[1];
                    break;
                default:
                    printHelp(p);
                    return null;
            }
                    
            return req;
                
        } catch (ArrayIndexOutOfBoundsException | NumberFormatException aioe) {
            printHelp(p);
        }
        
        return null;
    }
    
    private Boolean parseBoolFromArg(String arg) {
        if (arg == null) return null;
        if ("true".equalsIgnoreCase(arg) || "1".equals(arg) || "T".equalsIgnoreCase(arg)) return true;
        if ("false".equalsIgnoreCase(arg) || "0".equals(arg) || "F".equalsIgnoreCase(arg)) return false;
        return null;
    }
    
    public void printHelp(CommandSender cs) {
        cs.sendMessage("§cInvalid syntax. Try following this syntax.");
        cs.sendMessage("§7/pixelstacker load <url>");
//        cs.sendMessage("§c/pixelstacker load <url> <orientation> <maxWidth> <maxHeight>");
        cs.sendMessage("§8/pixelstacker load <url> <orientation> <maxWidth> <maxHeight> <multiLayer>");
//        cs.sendMessage("§c/pixelstacker load <url> <orientation> <maxWidth> <maxHeight> <multiLayer> <rgbBucketSize>");
        cs.sendMessage("§7/pixelstacker load <url> <orientation> <maxWidth> <maxHeight> <multiLayer> <rgbBucketSize> <dither> <maxColors>");
        cs.sendMessage("§cAvailable arguments with acceptable values:");
        cs.spigot().sendMessage(CText.hoverText("§7- orientation: H, Horizontal, V, Vertical", "Controls the orientation of the build."));
        cs.spigot().sendMessage(CText.hoverText("§8- maxWidth: [4 ... 4000]", "200 is good for most use cases."));
        cs.spigot().sendMessage(CText.hoverText("§7- maxHeight: [4 ... 4000]", "200 is good for most use cases."));
        cs.spigot().sendMessage(CText.hoverText("§8- multiLayer: True, T, 1, False, F, 0", "If true, stained glass panes will be used."));
        cs.spigot().sendMessage(CText.hoverText("§7- rgbBucketSize: 1, 5, 15, 17, 51", "Low numbers = more accurate, high numbers = faster"));
        cs.sendMessage("§8- dither: True, T, 1, False, F, 0");
        cs.spigot().sendMessage(CText.hoverText("§7- maxColors: -1 or [1 ... 256]", "Set to -1 for no limit."));
    }

    public static byte[] downloadImageToBytes(String imageUrl) throws IOException, InterruptedException {
        HttpClient client = HttpClient.newHttpClient();

        HttpRequest request = HttpRequest.newBuilder()
                .uri(URI.create(imageUrl))
                .GET()
                .build();

        HttpResponse<byte[]> response = client.send(request, HttpResponse.BodyHandlers.ofByteArray());

        if (response.statusCode() == 200) {
            return response.body();
        } else {
            throw new IOException("Failed to download image, HTTP response code: " + response.statusCode());
        }
    }
    
    public static HttpResponse<byte[]> sendImageToApiForRendering(RenderRequest req, String apiKey, byte[] imageBytes) throws IOException, InterruptedException, URISyntaxException {
        String endpoint = "https://taylorlove.info/projects/pixelstacker/api/Render/ByFileAdvanced";
        HashMap<String, String> queryParams = new HashMap<>();
        queryParams.put("format", req.format);
        queryParams.put("enableDithering", Boolean.toString(req.enableDithering));
        queryParams.put("isMultiLayer", Boolean.toString(req.isMultiLayer));
        queryParams.put("isSideView", Boolean.toString(req.isSideView));
        if (req.maxHeight != null) queryParams.put("maxHeight", Integer.toString(req.maxHeight));
        if (req.maxWidth != null) queryParams.put("maxWidth", Integer.toString(req.maxWidth));
        if (req.quantizedColorCount != null) queryParams.put("quantizedColorCount", Integer.toString(req.quantizedColorCount));
        if (req.rgbBucketSize != null) queryParams.put("rgbBucketSize", Integer.toString(req.rgbBucketSize));
        
        String query = queryParams.entrySet()
            .stream()
            .map(e -> String.format("%s=%s",
                e.getKey(),
                URLEncoder.encode(e.getValue(), StandardCharsets.UTF_8)))
            .collect(Collectors.joining("&"));
            String boundary = "Boundary-" + UUID.randomUUID();

        var byteArrayOutputStream = new ByteArrayOutputStream();
        try (java.io.PrintWriter writer = new PrintWriter(new OutputStreamWriter(byteArrayOutputStream, StandardCharsets.UTF_8), true)) {
            InputStream is = new ByteArrayInputStream(imageBytes);
            String contentType = URLConnection.guessContentTypeFromStream(is);
            is.close();
            if (contentType == null) {
                contentType = "application/octet-stream"; // default fallback
            }
            
            writer.append("--").append(boundary).append("\r\n");
            writer.append("Content-Disposition: form-data; name=\"File\"; filename=\"").append("File").append("\"\r\n");
            writer.append("Content-Type: ").append(contentType).append("\r\n\r\n");
            writer.flush();
            
            byteArrayOutputStream.write(imageBytes);
            byteArrayOutputStream.flush();
            
            writer.append("\r\n--").append(boundary).append("--\r\n");
        }

    
        URI uri = new URI(endpoint + "?"+ query);
        System.out.println("URL: "+uri.toString());
        HttpClient client = HttpClient.newHttpClient();
        HttpRequest request = HttpRequest.newBuilder()
                .uri(uri)
                .header("Content-Type", "multipart/form-data; boundary="+boundary)
                .header("Accept", "*/*")
                .header("X-API-KEY", apiKey)
                .header("X-PIXELSTACKER-API-KEY", apiKey)
                .POST(BodyPublishers.ofByteArray(byteArrayOutputStream.toByteArray()))
                .build();

        HttpResponse<byte[]> response = client.send(request, HttpResponse.BodyHandlers.ofByteArray());
        return response;
    }

    public static String extractFirstErrorMessage(String jsonResponse) {
        JsonObject jsonObject = JsonParser.parseString(jsonResponse).getAsJsonObject();
        if (jsonObject.has("errors")) {
            JsonArray errors = jsonObject.getAsJsonArray("errors");
            if (errors.size() > 0) {
                return errors.get(0).getAsString();
            }
        }
        return null; // or a default message
    }
}
