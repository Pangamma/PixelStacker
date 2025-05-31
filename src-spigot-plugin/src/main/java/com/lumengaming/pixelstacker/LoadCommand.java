package com.lumengaming.pixelstacker;

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
import java.util.ArrayList;
import java.util.Arrays;
import java.util.HashMap;
import java.util.List;
import java.util.UUID;
import java.util.logging.Level;
import java.util.logging.Logger;
import java.util.stream.Collectors;
import org.bukkit.command.Command;
import org.bukkit.command.CommandExecutor;
import org.bukkit.command.CommandSender;
import org.bukkit.command.TabCompleter;
import org.bukkit.entity.Player;

/**
 *
 * @author prota
 */
public class LoadCommand implements CommandExecutor, TabCompleter {
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
        String url = args.length > 1 ? args[args.length-1] : "";
        
        if (url.isBlank() || req == null || !url.startsWith("http")) {
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
        
        long before = System.currentTimeMillis();
        try {
            cs.sendMessage("§8Command received. Please wait while your request finishes processing.");
            HttpResponse<byte[]> apiResponse = this.sendImageToApiForRendering(req, apiKey, downloadedImage);
            var headers = apiResponse.headers().map();
            var headerKey = headers.keySet().stream().filter(x -> x.equalsIgnoreCase("x-responsetimems")).findFirst();
            if (headerKey.isPresent()) {
                String value = headers.get(headerKey.get()).get(0);
                before = System.currentTimeMillis() - Long.parseLong(value);
            }
            
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
        long after = System.currentTimeMillis();
        
        
        
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
        
        long elapsed = after - before;
        p.sendMessage("§dYour picture was processed in "+elapsed+"ms. Paste it with //paste.");  
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
            int a = 1;
            int m = 0;
            switch (args.length) {
                case 2:
                    url = args[a++];
                    break;
                case 3:
                    req.isSideView = args[a].equalsIgnoreCase("V") || args[a].equalsIgnoreCase("vertical");
                    a++;
                    url = args[a++];
                    break;
                case 4:
                    req.isSideView = args[a].equalsIgnoreCase("V") || args[a].equalsIgnoreCase("vertical");
                    a++;
                    req.isMultiLayer = parseBoolFromArg(args[a++]);
                    url = args[a++];
                    break;
                case 6:
                    req.isSideView = args[a].equalsIgnoreCase("V") || args[a].equalsIgnoreCase("vertical");
                    a++;
                    req.isMultiLayer = parseBoolFromArg(args[a++]);
                    m = Integer.parseInt(args[a++]); if (m != -1) { req.maxWidth = m; }
                    m = Integer.parseInt(args[a++]); if (m != -1) { req.maxHeight = m; }
                    url = args[a++];
                    break;
                case 9:
                    req.isSideView = args[a].equalsIgnoreCase("V") || args[a].equalsIgnoreCase("vertical");
                    a++;
                    req.isMultiLayer = parseBoolFromArg(args[a++]);
                    m = Integer.parseInt(args[a++]); if (m != -1) { req.maxWidth = m; }
                    m = Integer.parseInt(args[a++]); if (m != -1) { req.maxHeight = m; }
                    req.rgbBucketSize = Integer.parseInt(args[a++]);
                    if (!Arrays.stream(new int[]{1, 5, 15, 17, 51}).anyMatch(i -> i == req.rgbBucketSize)) {
                        p.sendMessage("§cThe rgb bucket size provided was not a valid value.");
                        printHelp(p);
                        return null;
                    }   req.enableDithering = this.parseBoolFromArg(args[a++]);
                    req.quantizedColorCount = Integer.parseInt(args[a++]);
                    if (req.quantizedColorCount == -1) {
                        req.quantizedColorCount = null;
                    }
                    else if (!(req.quantizedColorCount > 0 && req.quantizedColorCount <= 256)) {
                        p.sendMessage("§cThe quantized color count is invalid. Please check available options.");
                        printHelp(p);
                        return null;
                    }   
                    url = args[a++];
                    break;
                default:
                    printHelp(p);
                    return null;
            }
            
            if (url == null || !url.startsWith("http")) {
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
        cs.sendMessage("§8/pixelstacker load <orientation> <url>");
        cs.sendMessage("§7/pixelstacker load <orientation> <multiLayer> <url>");
        cs.sendMessage("§8/pixelstacker load <orientation> <multiLayer> <maxWidth> <maxHeight> <url>");
        cs.sendMessage("§7/pixelstacker load <orientation> <multiLayer> <maxWidth> <maxHeight> <rgbBucketSize> <dither> <maxColors> <url>");
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

        if (req.maxHeight == null || req.maxHeight == -1) req.maxHeight = 4000;
        if (req.maxWidth == null || req.maxWidth == -1) req.maxWidth = 4000;
        queryParams.put("maxHeight", Integer.toString(req.maxHeight));
        queryParams.put("maxWidth", Integer.toString(req.maxWidth));
        if (req.quantizedColorCount != null && req.quantizedColorCount != -1) queryParams.put("quantizedColorCount", Integer.toString(req.quantizedColorCount));
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

    @Override
    public List<String> onTabComplete(CommandSender cs, Command cmnd, String string, String[] args) {
        var output = new ArrayList<String>();
        int currentArg = args.length - 1;
        switch(currentArg) {
            // /pixelstacker ___
            case 0:
                output.add("load");
                return output;
            
            // ./pixelstacker load ___
            case 1:
                output.add("<url / orientation>");
                output.add("H"); 
//                output.add("horizontal");
                output.add("V"); 
//                output.add("vertical");
                return output;
            // ./pixelstacker load <orientation> ___
            case 2:
                output.add("<url / multiLayer>");
                output.add("T"); 
                output.add("F");
//                output.add("false");output.add("true"); 
                return output;
            // ./pixelstacker load <orientation> <multiLayer> ____
            case 3:
                output.add("<url / maxWidth>");
                return output;
            // ./pixelstacker load <orientation> <multiLayer> <maxWidth> ____
            case 4:
                output.add("<maxHeight>");
                return output;
            // ./pixelstacker load <orientation> <multiLayer> <maxWidth> <maxHeight> ___
            case 5:
                output.add("<url / rgbBucketSize>");
                output.add("1"); output.add("5"); output.add("15"); output.add("17"); output.add("51");
                return output;
            // ./pixelstacker load <orientation> <multiLayer> <maxWidth> <maxHeight> <rgbBucketSize> ____
            case 6:
                output.add("<dither>");
                output.add("T"); 
//                output.add("true"); 
                output.add("F"); 
                //output.add("false");
                return output;
            // ./pixelstacker load <orientation> <multiLayer> <maxWidth> <maxHeight> <rgbBucketSize> <dither> ___
            case 7:
                output.add("<maxColors>");
                output.add("-1"); 
                output.add("2");output.add("4");output.add("8");output.add("16");
                output.add("32");output.add("64");output.add("128");output.add("256");
                return output;
            // ./pixelstacker load <orientation> <multiLayer> <maxWidth> <maxHeight> <rgbBucketSize> <dither> <maxColors> ___
            case 8:
                output.add("<url>");
                return output;
            
            default:
                return output;
        }
    }
}
