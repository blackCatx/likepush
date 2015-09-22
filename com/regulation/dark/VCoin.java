package com.regulation.dark;

import java.security.MessageDigest;
import java.security.NoSuchAlgorithmException;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Date;
import java.util.HashMap;
import java.util.Iterator;
import java.util.List;
import java.util.Map;
import java.util.concurrent.ExecutionException;
import java.util.concurrent.TimeUnit;
import java.util.concurrent.TimeoutException;

import org.json.JSONException;
import org.json.JSONObject;

import android.content.Context;
import android.text.format.Time;
import android.util.Log;
import android.util.Pair;

import com.android.volley.AuthFailureError;
import com.android.volley.Request;
import com.android.volley.RequestQueue;
import com.android.volley.toolbox.RequestFuture;
import com.android.volley.toolbox.StringRequest;
import com.android.volley.toolbox.Volley;


public class VCoin {
	public class TradeRequest {
		public String version;
		public String signMethod;
		public String cpId;
		public String cpKey;
		public String appId;
		public String cpOrderNumber;
		public String notifyUrl;
		public int orderAmount;
		public String orderTitle;
		public String orderDesc;
		public String extInfo;
		public TradeRequest() {
		}
	}
	
	public class TradeResponse {
		public int respCode;
		public String respMsg;
		public String signMethod;
		public String signature;
		public String accessKey;
		public String orderNumber;
		public int orderAmount;
		public TradeResponse() {
		}
	}
	
	public static final int NETWORK_ERROR = -1;
	public static final int RESPONSE_DATA_ERROR = -2;
	
	public static TradeResponse trade(Context ctx, String url, TradeRequest args) {
		VCoin x = new VCoin();
		Http httpc = x.new Http();
		String signature = "";
	
		Date currentTime = new Date();
		SimpleDateFormat formatter = new SimpleDateFormat("yyyyMMddHHmmss");
		String orderTime = formatter.format(currentTime);
		
		httpc.url(url)
				.param("appId", args.appId)
				.param("cpId", args.cpId)
				.param("cpOrderNumber", args.cpOrderNumber)
				.param("extInfo", args.extInfo)
				.param("notifyUrl", args.notifyUrl)
				.param("orderAmount", Integer.valueOf(args.orderAmount).toString())
				.param("orderDesc", args.orderDesc)
				.param("orderTime", orderTime)
				.param("orderTitle", args.orderTitle)
				.param("version", args.version)
				;
		
		signature = httpc.encodeValues(httpc.params());
		signature = md5(signature+"&"+md5(args.cpKey));
		
		httpc
		.param("signature", signature)
		.param("signMethod", args.signMethod);

		TradeResponse resp = x.new TradeResponse();
		Pair<String,Boolean> ret = httpc.post(ctx);
		
		if (!ret.second) {
			resp.respCode = NETWORK_ERROR;
			return resp;
		}

		try {
            JSONObject jsonRsp = new JSONObject(ret.first);
            resp.respCode = jsonRsp.getInt("respCode");
            resp.respMsg = jsonRsp.getString("respMsg");
            resp.signMethod = jsonRsp.getString("signMethod");
            resp.signature = jsonRsp.getString("signature");
            resp.accessKey = jsonRsp.getString("accessKey");
            resp.orderNumber = jsonRsp.getString("orderNumber");
            resp.orderAmount = jsonRsp.getInt("orderAmount");
        } catch (JSONException e) {
        	Log.w("parse json", e.toString());
            resp.respCode = RESPONSE_DATA_ERROR;
            return resp;
        }
		
		return resp;
	}
	
	public static String md5(String s) {
        try {
            MessageDigest digester = MessageDigest.getInstance("MD5");
            digester.update(s.getBytes());
            byte[] digest = digester.digest();
    		StringBuffer buff = new StringBuffer();
    		for (byte b : digest) {
    			buff.append(String.format("%02x", b & 0xff));
    		}
            return buff.toString();
        } catch (NoSuchAlgorithmException e) {
            Log.w("md5(String)", "No MD5 engine");
            return s;
        }
    }

	class Http {
	    private final static int _DEFAULT_TIMEOUT = 30;
	    private final Map<String,String> _EMPTY_MAP = new HashMap<String,String>();
	    private Map<String,String> m_headers = new HashMap<String,String>();
	    private List<Pair<String, String>> m_params = new ArrayList<Pair<String, String>>();
	    private String m_url = "";
	    private int m_timeout = _DEFAULT_TIMEOUT;
	    
	    public List<Pair<String, String>> params() {
	    	return m_params;
	    }

	    public Http clear() {
	        m_headers.clear();
	        m_params.clear();
	        m_url = "";
	        m_timeout = _DEFAULT_TIMEOUT;
	        return this;
	    }

	    public Http header(String key,String value) {
	        this.m_headers.put(key, value);
	        return this;
	    }

	    public Http header(Pair<String,String> kv) {
	        this.m_headers.put(kv.first, kv.second);
	        return this;
	    }

	    public Http param(String key,String value) {
	        this.m_params.add(Pair.create(key, value));
	        return this;
	    }

	    public Http param(Pair<String,String> kv) {
	        this.m_params.add(kv);
	        return this;
	    }

	    public Http headers(Map<String,String> headers_) {
	        this.m_headers = headers_;
	        return this;
	    }

	    public Http url(String url_) {
	        this.m_url = url_;
	        return this;
	    }

	    public Http timeout(int timeout_) {
	        this.m_timeout = timeout_;
	        return this;
	    }

	    public Pair<String,Boolean> get(Context ctx) {
	        return request(ctx, Request.Method.GET, m_timeout, m_url, m_params, m_headers);
	    }

	    public Pair<String,Boolean> post(Context ctx) {
	        return request(ctx, Request.Method.POST, m_timeout, m_url, m_params, m_headers);
	    }
	    
	    public String encodeValues(final List<Pair<String, String>> params) {
	    	String ret = "";
	    	Iterator<Pair<String, String>> iter = params.iterator();
    		while(iter.hasNext()) {
    			Pair<String, String> kv = iter.next();
    			ret += kv.first + "=" + kv.second;
    			if (iter.hasNext()) {
    				ret += "&";
    			}
	        }
    		return ret;
	    }
	    
	    private Map<String, String> list2map(final List<Pair<String, String>> params) {
	    	HashMap<String, String> map = new HashMap<String, String>();
	    	Iterator<Pair<String, String>> iter = params.iterator();
    		while(iter.hasNext()) {
    			Pair<String, String> kv = iter.next();
    			map.put(kv.first, kv.second);
	        }
	    	return map;
	    }

	    public Pair<String,Boolean> request(Context ctx,
	                                                int method,
	                                                int timeout,
	                                                String url,
	                                                final List<Pair<String, String>> params,
	                                                final Map<String,String> headers)
	    {
	    	if (method==Request.Method.GET) {
	    		url += "?" + encodeValues(params);
	    	}
	        final Map<String, String> __params = (method==Request.Method.GET? _EMPTY_MAP: list2map(params));
	        
	        RequestFuture<String> future = RequestFuture.newFuture();
	        StringRequest req = new StringRequest(method, url, future, future) {
	            @Override
	            protected Map<String, String> getParams() {
	                return __params;
	            }
	            @Override
	            public Map<String, String> getHeaders() throws AuthFailureError {
	                return headers;
	            }
	        };
	        RequestQueue mRequestQueue = Volley.newRequestQueue(ctx);
	        mRequestQueue.add(req);
	        try {
	            String response = future.get(timeout, TimeUnit.SECONDS);
	            return Pair.create(response, true);
	        } catch (InterruptedException e) {
	        	Log.w("http.request", e.toString());
	        } catch (ExecutionException e) {
	        	Log.w("http.request", e.toString());
	        } catch (TimeoutException e) {
	        	Log.w("http.request", e.toString());
		    }
	        return Pair.create("", false);
	    }
	    
	}
}
