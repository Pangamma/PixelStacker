package com.lumengaming.pixelstacker;

import com.google.gson.ExclusionStrategy;
import com.google.gson.FieldAttributes;
import com.google.gson.Gson;
import com.google.gson.GsonBuilder;
import java.io.File;
import java.io.IOException;
import java.lang.reflect.Modifier;
import java.nio.charset.Charset;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.StandardOpenOption;
import java.util.logging.Level;
import java.util.logging.Logger;

/**
 *
 * @author prota
 */
public class ConfigHelper {
    public static File DATA_FOLDER;
    
    private static Charset getCharset(){
        Charset cs = Charset.defaultCharset();
        if (Charset.isSupported("UTF-16")){
            cs = Charset.forName("UTF-16");
        }
        return cs;
    }
    
    public static String LoadJson(String fileName){
        File file = new File(DATA_FOLDER, fileName);
        Path path = file.toPath();
        
        String json = null;
        try {
            if (!file.exists()){
                file.createNewFile();
            }
            
            byte[] data = Files.readAllBytes(path);
            json = new String(data, getCharset());
        } catch (IOException ex) {
            Logger.getLogger(ConfigHelper.class.getName()).log(Level.SEVERE, "Failed to load config '"+fileName+"'", ex);
        }
        return json;
    }
    
    public static boolean SaveJson(Object o, String fileName){
        try {
            File file = new File(DATA_FOLDER, fileName);
            Path path = file.toPath();
            
            if (!file.exists()){
                file.createNewFile();
            }
            
            String json = getGson().toJson(o, o.getClass());
            Files.write(path, json.getBytes(getCharset()), StandardOpenOption.CREATE, StandardOpenOption.TRUNCATE_EXISTING, StandardOpenOption.WRITE);
            return true;
        } catch (IOException ex) {
            Logger.getLogger(ConfigHelper.class.getName()).log(Level.SEVERE, "Failed to save config '"+fileName+"'", ex);
            return false;
        }
    }

    public static Gson getGson() {
        return new GsonBuilder()
            .setPrettyPrinting()
            .serializeNulls()
            .disableHtmlEscaping()
            .setExclusionStrategies(new ExclusionStrategy() {
            @Override
            public boolean shouldSkipField(FieldAttributes fa) {
                return fa.hasModifier(Modifier.TRANSIENT);
            }

            @Override
            public boolean shouldSkipClass(Class<?> arg0) {
                return false;
            }
        }).create();
    }
}
