package com.lumengaming.pixelstacker;

import com.google.gson.Gson;
import com.google.gson.annotations.SerializedName;
import java.io.File;
import java.util.logging.Level;
import java.util.logging.Logger;
import org.bukkit.Bukkit;

/**
 *
 * @author prota
 */
public class PixelStackerConfig {
    
    //<editor-fold defaultstate="collapsed" desc="Properties">
    @SerializedName("api-key")
    public String ApiKey = "YOUR_API_KEY_HERE";
    
    @SerializedName("defaults")
    public RenderRequest Defaults = new RenderRequest();
    //</editor-fold>
    
    //<editor-fold defaultstate="collapsed" desc="CORE methods">
    public static File DATA_FOLDER = Bukkit.getServer().getPluginManager().getPlugin("PixelStacker").getDataFolder()
            ;
    private static final String fileName = "config.json";
    /** 
     * Java sucks compared to C#. I miss my properties!
     * 
     * @return Options
     */
    public static PixelStackerConfig Get(){
        if (_get == null){
            _get = PixelStackerConfig.Load();
        }
        return _get;
    }
    private static PixelStackerConfig _get = null;
    
    public static PixelStackerConfig Save(){
        PixelStackerConfig o = Get();
        ConfigHelper.SaveJson(o, fileName);
        return _get;
    }
    
    public static PixelStackerConfig Load(){
        String json = ConfigHelper.LoadJson(fileName);
        try{
            Gson gson = new Gson();
            if (json != null){
                PixelStackerConfig fromJson = gson.fromJson(json, PixelStackerConfig.class);
                _get = fromJson;
            }
        }catch(Exception ex){
            Logger.getLogger(ConfigHelper.class.getName()).log(Level.SEVERE, "Failed to load config '"+fileName+"'", ex);
        }
        if (_get == null){
            _get = new PixelStackerConfig();
        }
        return _get; 
    }


    public PixelStackerConfig() {
    }
    //</editor-fold>

}
