package com.regulation.dark;

import javax.xml.parsers.SAXParserFactory;

import org.cocos2dx.lib.Cocos2dxActivity;
import org.cocos2dx.lib.Cocos2dxLuaJavaBridge;
import org.xml.sax.Attributes;
import org.xml.sax.InputSource;
import org.xml.sax.SAXException;
import org.xml.sax.XMLReader;
import org.xml.sax.helpers.DefaultHandler;

import android.content.res.AssetManager;
import android.content.res.Resources;
import android.util.Log;

public class XmlHeadler extends DefaultHandler {
	private String strTag = "";
	private static String strmd , strad, strsd;
	public static Cocos2dxActivity stcontext;
	static private int luaFunction  = 0;
	private static String strDevToken;
	public static void setContext(Cocos2dxActivity context_) {
			 
		  stcontext = context_;
		 
	 }

	@Override
	public void characters(char[] ch, int start, int length)
			throws SAXException {
		// TODO Auto-generated method stub		
		if (strTag.equals("md")) {
			strmd =  new String(ch, start, length);
			strTag = "";
			Log.d("xmlxml", strmd);
		} else if (strTag.equals("ad")) {
			strad =  new String(ch, start, length);
			strTag = "";
			Log.d("xmlxml", strad);
		} else if (strTag.equals("sd")) {
			strsd =  new String(ch, start, length);
			strTag = "";
			Log.d("xmlxml", strsd);
		} else {
			
		}
		
	}

	@Override
	public void endDocument() throws SAXException {
		// TODO Auto-generated method stub
		Log.d("xmlxml", "endDocument");
		setXmlData(strmd, strad, strsd);
	}

	@Override
	public void endElement(String uri, String localName, String qName)
			throws SAXException {
		// TODO Auto-generated method stub
		Log.d("xmlxml", "endElement");
	}

	@Override
	public void startDocument() throws SAXException {
		// TODO Auto-generated method stub
		Log.d("xmlxml", "startDocument");
	
	}

	@Override
	public void startElement(String uri, String localName, String qName,
			Attributes attributes) throws SAXException {
		// TODO Auto-generated method stub
			
		if (localName.equals("string")) {
			for(int i=0; i< attributes.getLength(); i++) {
				if 	(attributes.getValue(i).equals("md")) {
					strTag = attributes.getValue(i);
				} else if (attributes.getValue(i).equals("sd")) {
					strTag = attributes.getValue(i);
				} else if (attributes.getValue(i).equals("ad")) {
					strTag = attributes.getValue(i);
				} else {
					strTag = "";
				}
				Log.d("xmlxml", attributes.getLocalName(i) + "=" + attributes.getValue(i));
			}		
					
		}
			
	}
	public static int getXmlData(final int luaCallbackFunction){
		Log.d("xmlxml", "getXmlData!!!........");
	   	try{
	   	luaFunction = luaCallbackFunction;
	   				
    	SAXParserFactory factory = SAXParserFactory.newInstance();
    	XMLReader reader = factory.newSAXParser().getXMLReader();
    	reader.setContentHandler( new XmlHeadler());
    	Resources res = stcontext.getResources();
    	AssetManager  am = res.getAssets();
    	reader.parse( new InputSource(am.open("BossSDKGameConfig.xml")));
    	}catch(Exception e){
    		Log.e("xmlxml", "getXmlData!!!.......Exception."+ e.toString());
    		e.printStackTrace();
    	}

    	return 0;
    }	
	
	public static int setXmlData(final String md,final String ad,final String sd) {
		final String strParm = "{\"md\":\""+ md +"\","+ "\"sd\":\"" + sd + "\"," + "\"ad\":\"" + ad  +"\"}" ;
//		Log.d("xmlxml", "setXmlData!!!........" + strParm);
		stcontext.runOnGLThread(new Runnable() {
                    @Override
                    public void run() {
//                    	Log.d("xmlxml", "setXmlData!!!.......runrunrunrun.");
                        Cocos2dxLuaJavaBridge.callLuaFunctionWithString(luaFunction, strParm );
                        Cocos2dxLuaJavaBridge.releaseLuaFunction(luaFunction);
                    }
            	 });

		return 0;
	}
	
	public static void setDevToken(String token){
		strDevToken = token;
//		Log.d("dark", "注册成功，设备token为：" + strDevToken);
	}
	public static String getDevToken(){
//		if (strDevToken == null){
//			Log.d("dark", " strDevToken == null，设备token为：" + strDevToken);
//			return "0";
//		}
//		return strDevToken;
//		
		return "0";
	}
	public static int sendMessage(String title, String content,String year, String hour, String min){
		Log.d("dark", title + "xxxxxx" + content);
//		XGPushManager.clearLocalNotifications(stcontext);
//		XGLocalMessage xMsg = new XGLocalMessage();
//		xMsg.setTitle("黑暗纪元");
//		xMsg.setContent(content);
//		if(!year.equals("")){
//			Log.d("dark", year + "xxxxxx" + hour + "xxx" + min);
//			xMsg.setDate(year);
//			xMsg.setHour(hour);
//			xMsg.setMin(min);
//		}
//
//		XGPushManager.addLocalNotification(stcontext, xMsg);
		return 0;
	}
	
	public static int clearXgMessage(){
		Log.d("dark",  " clear  xxxxxx");
//		XGPushManager.clearLocalNotifications(stcontext);
		return 0;
	}
	
}
