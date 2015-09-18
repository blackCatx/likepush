package com.regulation.dark;

import org.cocos2dx.lib.Cocos2dxActivity;
import org.cocos2dx.lib.Cocos2dxLuaJavaBridge;
import org.json.JSONException;
import org.json.JSONObject;

import android.util.Log;

public class SdkGames {
	private static Cocos2dxActivity context; 
	public static void setContext(Cocos2dxActivity context_) {			 
		context = context_;		 
	}
	
//	public static native void loginCallBack(int code, String json);
	
	public static void login(){
		Log.i("SdkO4game", "SdkO4game login------");

			
				
	}
	
	public static void pay(String payInfo){
		
		 try {
             JSONObject jsonPay = new JSONObject(payInfo);          //创建JSONObject对象
     		/*
             * 
             * tbjson["money"] = tostring(tonumber(payData.rmb)*100)
                tbjson["roleId"] = tostring(roleId)
                tbjson["zoneId"] = tostring(zoneid)
                tbjson["payCount"] = payCount
                tbjson["payId"] = tostring(payData.id)
                tbjson["payName"] = tostring(payData.yuanbao .. strtb.localstring(strtb.strDiamond))
                tbjson["roleName"] = tostring(roleName)
                tbjson["roleLevel"] = tostring(roleLevel)
                tbjson["userId"] = tostring(uid)
                tbjson["account"] = tostring(account)
                local tbExt = {}
                tbExt["role_id"] = tostring(roleId)
                tbExt["pay_id"] = tostring(payData.id)
                print(" tbExt ....", json.encode(tbExt))
                tbjson["ext"] = json.encode(tbExt)
                tbjson["orderId"]  = tostring(sn)
                */

     		String zoneId = jsonPay.getString("zoneId");
     		String userId = jsonPay.getString("account");
     		String payId = jsonPay.getString("payId");
     		String roleId = jsonPay.getString("roleId");
     		int npayId = jsonPay.getInt("payId");
     		String extInfo = roleId + "@" + payId;
     		final String productId;
            int nPayId = Integer.valueOf(payId);
             
        
             switch (nPayId){
             	case 14001:
             		productId = "ldmmdloedv1_120_2";
             		break;
             	case 14002:       	             		
             		productId = "ldmmdloedv1_600_10";
             		break;
             	case 14003:
             		productId = "ldmmdloedv1_1200_20";
             		break;
             	case 14004:
             		productId = "ldmmdloedv1_3000_50";
             		break;
             	case 14005:
             		productId = "ldmmdloedv1_6000_100";
             		break;
             	case 14006:
             		productId = "ldmmdloedv1_9000_150";
             		break;

             	case 14007:
             		productId = "ldmmdloedv1_300_6";            		
             		break;
             	case 14008:
             		productId = "ldmmdloedv1_1560_26";
             		break;
 		  		default:
 		  			productId = "ldmmdloedv1_120_2";
 		  			break;
             		
            	 
             }
             

             
         } catch (JSONException e) {
             e.printStackTrace();
         }
		
		
		
	}
	
}
