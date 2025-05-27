package com.lumengaming.pixelstacker;

import com.google.common.io.Files;
import com.google.gson.Gson;
import com.sk89q.worldedit.LocalSession;
import com.sk89q.worldedit.bukkit.WorldEditPlugin;
import com.sk89q.worldedit.extent.clipboard.Clipboard;
import com.sk89q.worldedit.extent.clipboard.io.ClipboardFormats;
import com.sk89q.worldedit.extent.clipboard.io.ClipboardFormat;
import com.sk89q.worldedit.extent.clipboard.io.ClipboardReader;
import com.sk89q.worldedit.session.ClipboardHolder;
import org.bukkit.Bukkit;
import org.bukkit.plugin.java.JavaPlugin;
import java.io.*;
import java.io.IOException;
import java.net.URI;
import java.net.URLEncoder;
import java.net.http.HttpClient;
import java.net.http.HttpRequest;
import java.net.http.HttpResponse;
import java.nio.charset.StandardCharsets;
import java.util.UUID;
import java.util.logging.Level;
import java.util.logging.Logger;
import org.bukkit.command.Command;
import org.bukkit.command.CommandSender;
import org.bukkit.entity.Player;
import org.bukkit.event.HandlerList;

/**
 *
 * @author prota
 */
public class PixelStackerMain extends JavaPlugin {

    public static void main(String[] args) throws IOException, InterruptedException {
    }
    
    @Override
    public void onEnable() {
        if (!Bukkit.getServer().getPluginManager().getPlugin(this.getName()).getDataFolder().exists()) {
            Bukkit.getServer().getPluginManager().getPlugin(this.getName()).getDataFolder().mkdir();
        }
        
        ConfigHelper.DATA_FOLDER = Bukkit.getServer().getPluginManager().getPlugin("PixelStacker").getDataFolder();

        PixelStackerConfig.Load();
        if ("YOUR_API_KEY_HERE".equals(PixelStackerConfig.Get().ApiKey)) {
            UUID uuid = UUID.randomUUID();
            PixelStackerConfig.Get().ApiKey = uuid.toString();
        }
        PixelStackerConfig.Save();
        
        this.getCommand("pixelstacker").setExecutor(new LoadCommand(this));
    }

    @Override
    public void onDisable() {
        HandlerList.unregisterAll(this);
        Bukkit.getScheduler().cancelTasks(this);
        this.getServer().getServicesManager().unregisterAll(this);
    }
}
