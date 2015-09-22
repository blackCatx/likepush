package com.regulation.dark;

import org.cocos2dx.lib.Cocos2dxActivity;
import org.json.JSONException;
import org.json.JSONObject;

import android.content.Intent;
import android.os.Bundle;
import android.util.Base64;
import android.util.Log;

import com.bbk.payment.PaymentActionDetailsInit;
import com.bbk.payment.PaymentActivity;
import com.regulation.dark.VCoin.TradeRequest;
import com.regulation.dark.VCoin.TradeResponse;
import com.vivo.account.base.accounts.OnVivoAccountChangedListener;
import com.vivo.account.base.accounts.VivoAccountManager;
import com.vivo.account.base.activity.LoginActivity;

public class SdkGames {
	private static Cocos2dxActivity context; 
	
	private static String TAG = "SdkGames";
	public static void setContext(Cocos2dxActivity context_) {			 
		context = context_;		 
	}
	
    public static native void loginCallBack(int code, String json);
    public static native void logoutCallBack(int code, String json);
    public static native void nativeInitResultCallback(int code, String json);
    
    public static void init(String payInfo){


        nativeInitResultCallback(0, "");
    }


	public static void login(){
		Log.i("SdkO4game", "SdkO4game login------");

     // try{
                    
        //     int strPid = 30;
        //     String strCode = "0";
        //     JSONObject jsonLogin = new JSONObject();          //创建JSONObject对象

        //     jsonLogin.put("pid", strPid);
        //     jsonLogin.put("code", strCode);

        //     jsonLogin.put("sdk", "");                   
        //     jsonLogin.put("uid", (String)userInfo.get("userID"));
        //     jsonLogin.put("userName", (String)userInfo.get("userName"));
        //     String strMsg = Base64.encodeToString(((String)userInfo.get("accesstoken")).getBytes(), Base64.DEFAULT);

        //     jsonLogin.put("token", strMsg);
        //     Log.i("SdkGames","SdkGames login : " + jsonLogin.toString());

        //     GlobalParam.hwBuoy.showSmallWindow(context);
        //     loginCallBack(0, jsonLogin.toString());
        
        // }catch (Exception e) {
        //   e.printStackTrace();
        // }

      //   loginCallBack(0, jsonLogin.toString());
		
		Intent loginIntent = new Intent(context, LoginActivity.class);
		context.startActivity (loginIntent);

		VivoAccountManager mVivoAccountManager;
		mVivoAccountManager = VivoAccountManager.getInstance(context);
				
		OnVivoAccountChangedListener mOnVivoAccountChangedListener = new OnVivoAccountChangedListener() {
				@Override
				public void onAccountLogin(String name, String openid, String authtoken) {
					// TODO Auto-generated method stub
				//	nameVal.setText(name);
				//	openidVal.setText(openid);
				//	authtokenVal.setText(authtoken);
					
					try{
						
						int strPid = 48;


						 
			             String strCode = "0";
			             JSONObject jsonLogin = new JSONObject();          //创建JSONObject对象
	
			             jsonLogin.put("pid", strPid);
			             jsonLogin.put("code", strCode);
	
			             jsonLogin.put("sdk", "");                   
			             jsonLogin.put("uid", openid);
			             jsonLogin.put("userName", name);
			             String strMsg = Base64.encodeToString(authtoken.getBytes(), Base64.DEFAULT);
	
			             jsonLogin.put("token", strMsg);
			             Log.i("SdkGames","SdkGames login : " + jsonLogin.toString());
	
	
			             loginCallBack(0, jsonLogin.toString());
							
			             new PaymentActionDetailsInit(context, "b1de7be9299ad61b808f8900d391800f");
						

					}catch (Exception e) {
						e.printStackTrace();
					}
					
					Log.d(TAG, "name="+name+", openid="+openid+", authtoken="+authtoken);
				}
				
				@Override
				public void onAccountRemove(boolean isRemoved) {
					// TODO Auto-generated method stub
//					if(isRemoved){
//						Log.d(TAG, "remove success");
//					}
				}
		@Override
				public void onAccountLoginCancled() {
					Log.d(TAG, "onAccountLoginCancled");
					// TODO Auto-generated method stub
					
				}
			};

		mVivoAccountManager.registeListener(mOnVivoAccountChangedListener);
		
		
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
     		String payId = jsonPay.getString("payId");
     		String roleId = jsonPay.getString("roleId");
     		String extInfo = String.format("zoneId=%s,roleId=%s,payId=%s", zoneId, roleId, payId);


            String orderId = jsonPay.getString("orderId");
            String level = jsonPay.getString("roleLevel");
            String roleName = jsonPay.getString("roleName");
            int money = jsonPay.getInt("money");
            String payName = jsonPay.getString("payName");

            
  
        
       // 	TradeRequest tradeReq = new TradeRequest("1.0.0", "MD5", cpId, "", appId, orderId, "", payCount, payName, "", "");
            VCoin vcoin = new VCoin();
            TradeRequest tradeReq = vcoin.new TradeRequest();
            tradeReq.version = "1.0.0";
            tradeReq.signMethod = "MD5";
            tradeReq.cpId = "20150915165009826712";
            tradeReq.cpKey = "8acce93025fad986c71afb8c0ad81484";
            tradeReq.appId = "b1de7be9299ad61b808f8900d391800f";
            tradeReq.cpOrderNumber = orderId;
            tradeReq.notifyUrl = "http://ccyldyjpay.jinglungame.net:29002/v1/vivo/payback";
            tradeReq.orderAmount = money;
            tradeReq.orderTitle = payName;
            tradeReq.orderDesc = payName;
            tradeReq.extInfo = extInfo;
            TradeResponse traderesponse = VCoin.trade(context, "https://pay.vivo.com.cn/vcoin/trade", tradeReq);
            

            
            if (traderesponse.respCode == 200){
                
                Bundle localBundle = new Bundle();
                localBundle.putString("transNo", traderesponse.orderNumber);// 交易流水号，由订单推送接口返回
                localBundle.putString("accessKey", traderesponse.accessKey);// 由订单推送接口返回
                localBundle.putString("productName", payName);//商品名称
                localBundle.putString("productDes", payName);//商品描述
                localBundle.putLong("price", traderesponse.orderAmount);//价格,单位为分（1000即10.00元）
                localBundle.putString("appId", "b1de7be9299ad61b808f8900d391800f");// appid为vivo开发者平台中生成的App ID
                
                localBundle.putString("blance", "100钻石");
                localBundle.putString("vip", "vip0");
                localBundle.putInt("level", Integer.parseInt(level));
                localBundle.putString("party", "No.1");
                localBundle.putString("roleId", roleId);
                localBundle.putString("roleName", roleName);
                localBundle.putString("serverName", zoneId);
                localBundle.putString("extInfo", extInfo);
                localBundle.putBoolean("logOnOff", true);

                Intent target = new Intent(context, PaymentActivity.class);
                target.putExtra("payment_params", localBundle);
                context.startActivityForResult(target, 1);

            }
   

             
         } catch (JSONException e) {
             e.printStackTrace();
         }
		
		
		
		
	}
	
	
	public static void submitExtendData(String info){
		
	}
	

	public static void userSwitch() {
		String KEY_SWITCH_ACCOUNT = "switchAccount";
		Intent swithIntent = new Intent(context, LoginActivity.class);
		swithIntent.putExtra(KEY_SWITCH_ACCOUNT, true);
		context.startActivity (swithIntent);
	}
	
	public static void logout(){
		Log.i("SdkO4game", "SdkO4game logout------");



		VivoAccountManager.vivoAccountreportPlayTimeInfo(context);
		
		try {
			JSONObject jobj = new JSONObject();
			logoutCallBack(0, jobj.toString());
		} catch (Exception e) {
			e.printStackTrace();
		}
			
	}
	
}
