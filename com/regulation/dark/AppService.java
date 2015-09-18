package com.regulation.dark;

import java.util.Timer;
import java.util.TimerTask;

import android.app.Service;
import android.content.Intent;
import android.os.IBinder;
import android.util.Log;

public class AppService extends Service {
	private static final String TAG = "DarkService";
	public native void jstartKeepAlive(String code, String json);
	static private Timer timer;
	static int count = 0;
	/**
	 * @param args
	 */
	public static void main(String[] args) {
		// TODO Auto-generated method stub

	}

	@Override
	public IBinder onBind(Intent arg0) {
		// TODO Auto-generated method stub
		return null;
	}
	@Override
	public void onCreate() {
		Log.v(TAG, "onCreate");
		
	}

	@Override
	public void onDestroy() {
		Log.v(TAG, "onDestroy");
		timer.cancel();
	}

	@Override
	public void onStart(Intent intent, int startId) {
		Log.v(TAG, "onStart");
		timer = new Timer(true);
		startKeepAlive();
		
	}
	
	public void startKeepAlive(){
		count = 0;
		TimerTask task = new TimerTask(){
			@Override
			public void run() {
				// TODO Auto-generated method stub
				Log.v(TAG, "run -xxx-- startKeepAlive");
				jstartKeepAlive("0","0");
				if (count >= 20){
					count = 0;
					timer.cancel();
					stopSelf();
				}else{
					count = count + 1;
				}
				
			}  
 
		}; 
		
		timer.schedule(task, 1000, 25000);
		
	}
		
	
}
