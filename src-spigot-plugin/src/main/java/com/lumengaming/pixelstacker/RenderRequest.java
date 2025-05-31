package com.lumengaming.pixelstacker;

/**
 *
 * @author prota
 */
public class RenderRequest {
    public String format = "schem2";
    public Boolean isSideView = true;
    public Boolean isMultiLayer = false;
    public Boolean enableDithering = false;
    public Integer maxHeight = 500;
    public Integer maxWidth = 500;
    public Integer rgbBucketSize = null;
    public Integer quantizedColorCount = null;
}
