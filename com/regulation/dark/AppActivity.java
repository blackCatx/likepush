/****************************************************************************
Copyright (c) 2008-2010 Ricardo Quesada
Copyright (c) 2010-2012 cocos2d-x.org
Copyright (c) 2011      Zynga Inc.
Copyright (c) 2013-2014 Chukong Technologies Inc.
 
http://www.cocos2d-x.org

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
****************************************************************************/
package com.regulation.dark;

import org.cocos2dx.lib.Cocos2dxActivity;

import android.content.Intent;
import android.os.Bundle;
import android.util.Log;
import android.view.KeyEvent;



public class AppActivity extends Cocos2dxActivity {
	int REQUEST_CODE =1;

	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		Log.i("csdk", "app init------");


		SdkGames.setContext(this);
		XmlHeadler.setContext(this);


		Log.v("TAG", "onCreate");


 		
	} 
	
	@Override
	protected void onNewIntent(Intent intent) {
		super.onNewIntent(intent);		

	}
	
	static {
		
		System.loadLibrary("cocos2dlua");
	}
	
	public void onStop() {   
    	super.onStop();
	   //test
    }
    
    public void onDestroy() {    
	    super.onDestroy();
		
	}
    
    public void onResume() {    
    	super.onResume();    
		
	}
    
    public void onPause() {    
    	super.onPause();    
		
    }
    
    public void onRestart() {    
    	super.onRestart();    
		
    }
    
    public boolean onKeyDown(int keyCoder,KeyEvent event)
    {
 
    	if( keyCoder == KeyEvent.KEYCODE_BACK){

        }else if(keyCoder == KeyEvent.KEYCODE_BACK ){
        	
        }
        return false;      
    }
    
    protected void onActivityResult(int requestCode, int resultCode, Intent data) {
    	super.onActivityResult(requestCode, resultCode, data);
/////////////////////////login/////////////////////////////////////////	
    	if (resultCode == RESULT_OK && requestCode == REQUEST_CODE) {
    	}
    	if (resultCode == RESULT_CANCELED && requestCode == REQUEST_CODE) {
    	}
    	if(data==null) {
    		Log.d("user null", "user login fail");
    		return;
    	}

    	////////////////////////////////////

    /////test
        ///test222

    }
}
