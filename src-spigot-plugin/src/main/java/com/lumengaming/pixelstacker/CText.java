package com.lumengaming.pixelstacker;


import net.md_5.bungee.api.ChatColor;
import net.md_5.bungee.api.chat.BaseComponent;
import net.md_5.bungee.api.chat.ClickEvent;
import net.md_5.bungee.api.chat.HoverEvent;
import net.md_5.bungee.api.chat.TextComponent;

/**
 * @author Taylor Love (Pangamma)
 */
public class CText{

    public static BaseComponent[] legacy(String s){
        return TextComponent.fromLegacyText(s);
    }
	
	public static String toLegacyString(BaseComponent[] orig){
        return TextComponent.toLegacyText(orig);
    }
    
	public static BaseComponent[] stripColors(BaseComponent[] orig){
        String txt = TextComponent.toPlainText(orig);
        return legacy(txt);
    }
        
    /** Merges many arrays of base components.
     * @param oL list of arrays
     * @return  **/
    public static BaseComponent[] merge(BaseComponent[]... oL){
        
        int maxLen = 0;
        for(BaseComponent[] bc : oL){
            maxLen += bc.length;
        }
        
        BaseComponent[] n = new BaseComponent[maxLen];
        int i = 0;
        
        for(BaseComponent[] bcc : oL)
            for(BaseComponent bc : bcc)
                n[i++] = bc;
        
        return n;
    }
    /** Iterates through all the base components and sets an event on them.
     * @param bcs
     * @param e **/
    public static void applyEvent(BaseComponent[] bcs,HoverEvent e){
        for (BaseComponent bc : bcs){
            bc.setHoverEvent(e);
        }
    }
    /** Iterates through all the base components and sets an event on them.
     * @param bcs
     * @param e **/
    public static void applyEvent(BaseComponent[] bcs,ClickEvent e){
        for (BaseComponent bc : bcs){
            bc.setClickEvent(e);
        }
    }
	
    /** Iterates through all the base components and sets an event on them.
     * @param bcs
     * @return  **/
    public static BaseComponent[] clone(BaseComponent[] bcs){
		BaseComponent[] clone = new BaseComponent[bcs.length];
        for (int i = 0; i < bcs.length;i++){
			clone[i] = bcs[i].duplicate();
        }
		return clone;
    }
	
    /** Iterates through all the base components and sets an event on them.
     * @param bcs
     * @param c **/
    public static void applyColor(BaseComponent[] bcs, net.md_5.bungee.api.ChatColor c){
        for (BaseComponent bc : bcs){
            bc.setColor(c);
        }
    }
    
    /**
     * Creates hover text easily.
     * @param displayText
     * @param hoverText
     * @return 
     */
    public static BaseComponent[] hoverText(String displayText,String hoverText){
        BaseComponent[] txt = CText.legacy(displayText);
        CText.applyEvent(txt, new HoverEvent(HoverEvent.Action.SHOW_TEXT, CText.legacy(hoverText)));
        return txt;
    }
    
    /**
     * Creates hover text easily. Click event suggests the command.
     * @param displayText
     * @param hoverText
     * @param commandText
     * @return 
     */
    public static BaseComponent[] hoverTextSuggest(String displayText,String hoverText,String commandText){
        BaseComponent[] txt = CText.legacy(displayText);
        CText.applyEvent(txt, new HoverEvent(HoverEvent.Action.SHOW_TEXT, CText.legacy(hoverText)));
        CText.applyEvent(txt, new ClickEvent(ClickEvent.Action.SUGGEST_COMMAND, commandText));
        return txt;
    }    
    
    /**
     * Creates hover text easily. Click event runs the command.
     * @param displayText
     * @param hoverText
     * @param commandText
     * @return 
     */
    public static BaseComponent[] hoverTextForce(String displayText,String hoverText,String commandText){
        BaseComponent[] txt = CText.legacy(displayText);
        CText.applyEvent(txt, new HoverEvent(HoverEvent.Action.SHOW_TEXT, CText.legacy(hoverText)));
        CText.applyEvent(txt, new ClickEvent(ClickEvent.Action.RUN_COMMAND, commandText));
        return txt;
    }

    public static String colorize(String colorCodes, String input){
		if (colorCodes == null || colorCodes.isEmpty()){ return input; }
		if (input == null || input.isEmpty()){ return input; }
		
		StringBuilder sb = new StringBuilder();
		int i = 0;
		for(char c : input.toCharArray()){
			i++;
			i %= colorCodes.length();
			char cc = colorCodes.charAt(i);
			sb.append("§").append(cc).append(c);
		}
		return sb.toString();
	}

    public static String stripColors(String input) {
        return ChatColor.stripColor(input);
    }
}
