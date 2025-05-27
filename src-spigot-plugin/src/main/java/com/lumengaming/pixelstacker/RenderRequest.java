package com.lumengaming.pixelstacker;

/**
 *
 * @author prota
 */
public class RenderRequest {
    public String format = "schem2";
    public boolean isSideView = true;
    public boolean isMultiLayer = false;
    public boolean enableDithering = false;
    public Integer maxHeight = 256;
    public Integer maxWidth = 256;
    public Integer rgbBucketSize = null;
    public Integer quantizedColorCount = null;
}
