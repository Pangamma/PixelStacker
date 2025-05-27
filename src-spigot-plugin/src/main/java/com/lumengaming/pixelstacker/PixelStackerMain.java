package com.lumengaming.pixelstacker;

import org.bukkit.Bukkit;
import org.bukkit.plugin.java.JavaPlugin;
import java.io.IOException;
import java.util.UUID;
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
