using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using PSParams;

[System.Serializable]
public class StoreItemsParams{
    public string[] prices;
    public string[] tittles;
    public string[] descs;
	public string[] ids;
}
public class StoreListener :  PS_SingletonBehaviour<StoreListener> 
	{



		public void Init(){
			Debug.Log("ストア　初期化");
			if(IsStoreAvaillable()){
				PS_Plugin.Instance.OnStoreInitComplete(true);
				return;
			}
			this.items.prices=new string[AppData.IAP_SKUs.Length];
			this.items.tittles=new string[AppData.IAP_SKUs.Length];
			this.items.descs=new string[AppData.IAP_SKUs.Length];

			for(int i=0;i<this.items.prices.Length;i++){
				this.items.prices[i]="--";
				this.items.tittles[i]="--";
				this.items.descs[i]="--";
			}

			for(int i=0;i<AppData.IAP_SKUs.Length;i++){
				AndroidInAppPurchaseManager.Client.AddProduct(AppData.IAP_SKUs[i]);
			}


			AndroidInAppPurchaseManager.ActionProductPurchased += OnProductPurchased;
			AndroidInAppPurchaseManager.ActionProductConsumed += OnProductConsumed;
			AndroidInAppPurchaseManager.ActionBillingSetupFinished += OnBillingConnected;
			AndroidInAppPurchaseManager.Client.Connect();

		}
		

        public delegate void Callback_purchasFailledEvent(string s);
        public delegate void Callback_purchasedEvent(string s);
        public StoreItemsParams items;

        
		public bool isConsumableProduct(string skusName){

			int i=Array.IndexOf(AppData.IAP_SKUs,skusName);
			if(i<AppData.IAP_Comsumable.Length){
				return AppData.IAP_Comsumable[i];
			}else{
				Debug.LogError("isConsumableProduct sukus is over isConsumable Length");
				return true;
			}
		}

		public string GetPrice(string skusName){

			int i=Array.IndexOf(items.ids,skusName);
			if(i<items.ids.Length){
				return items.prices[i];
			}else{
				Debug.LogError("isConsumableProduct sukus is over isConsumable Length");
				return "--";
			}
		}
	    public bool isDebugLog=false;
		//購入の成功時
		public  event Callback_purchasedEvent purchasedEvent;
		//購入時のキャンセル時
		public  event Callback_purchasFailledEvent purchasFailledEvent;


		//--------------------------------------
		//  PUBLIC METHODS
		//--------------------------------------
		public void ClearAllEventListeners(){
			if(purchasedEvent!=null)purchasedEvent=null;
			if(purchasFailledEvent!=null)purchasFailledEvent=null;
		}

		bool isInvoking=false;
		public void PurchaseProduct(string id,Callback_purchasedEvent purchasedEvent,Callback_purchasFailledEvent purchasFailledEvent){
			ClearAllEventListeners();
			this.purchasedEvent=purchasedEvent;
			this.purchasFailledEvent=purchasFailledEvent;

		    if(!isInvoking && AndroidInAppPurchaseManager.Client.IsConnected){
				isInvoking=true;
				AndroidInAppPurchaseManager.Client.Purchase (id);
			}else{
                Debug.LogError("PurchaseProduct GPGにコネクトされていない");
				
			}
		}


        public bool IsStoreAvaillable(){

            if(!isInvoking && AndroidInAppPurchaseManager.Client.IsConnected){
                Debug.Log("ストアにコネクトされていない");
                return false;
            }

        if( this.items.prices.Length<=0 || this.items.prices[0]=="--"){
                Debug.Log("ストアアイテムがクエリされていない");
                return false;
            }
            return true;
        }
		

		
		
		void consume(string SKU) {
			AndroidInAppPurchaseManager.Client.Consume (SKU);
		}


        //Init 1
		void OnBillingConnected(BillingResult result) {
		
			AndroidInAppPurchaseManager.ActionBillingSetupFinished -= OnBillingConnected;

            if(result.IsSuccess  &&  !result.IsFailure) {
			    if(PS_Plugin.Instance.isDebugMode && isDebugLog)Debug.Log ("ストア　コネクト: Success"+result.IsSuccess );
				//Store connection is Successful. Next we loading product and customer purchasing details
				AndroidInAppPurchaseManager.ActionRetrieveProducsFinished += OnRetrieveProductsFinised;
				AndroidInAppPurchaseManager.Client.RetrieveProducDetails();

    		} else{
                if(PS_Plugin.Instance.isDebugMode && isDebugLog)Debug.LogError ("ストア　コネクト: "+result.IsSuccess );
                PS_Plugin.Instance.OnStoreInitComplete(false);
    		}
			
		}


        //Init ２
		void OnRetrieveProductsFinised(BillingResult result) {
			AndroidInAppPurchaseManager.ActionRetrieveProducsFinished -= OnRetrieveProductsFinised;

              if(result.IsSuccess &&  !result.IsFailure) {
			    if(PS_Plugin.Instance.isDebugMode && isDebugLog)Debug.Log ("OnRetrieveProductsFinised  Success");
           
			} else {
			    if(PS_Plugin.Instance.isDebugMode && isDebugLog)Debug.LogError ("OnRetrieveProductsFinised "+result.IsSuccess );
                 PS_Plugin.Instance.OnStoreInitComplete(false);
				return;
			}
			
			foreach(GoogleProductTemplate p in AndroidInAppPurchaseManager.Client.Inventory.Products) {
				if(PS_Plugin.Instance.isDebugMode && isDebugLog)Debug.Log("プロダクトのリストをロードしました: " + p.Title);
				if(PS_Plugin.Instance.isDebugMode && isDebugLog)Debug.Log( p.SKU + "\n" );

                OnItemsQuery(p.SKU,p.LocalizedPrice.ToString(),p.Description,p.Title);

				if(isConsumableProduct(p.SKU)){
					if(AndroidInAppPurchaseManager.Client.Inventory.IsProductPurchased(p.SKU)) {
						if(PS_Plugin.Instance.isDebugMode && isDebugLog)Debug.Log("consume 忘れ " +p.SKU);
                       consume(p.SKU);
					}
				}else{
				    if(AndroidInAppPurchaseManager.Client.Inventory.IsProductPurchased(p.SKU)) {
						if(PS_Plugin.Instance.isDebugMode && isDebugLog)Debug.Log("リストア　 " +p.SKU);
						OnPurchased(p.SKU,true);
					}
				}
			}
            PS_Plugin.Instance.OnStoreInitComplete(true);

		}

		

		

        void OnItemsQuery(string id,string price,string desc,string tittle){
			int i=0;
			i=Array.IndexOf(AppData.IAP_SKUs,id);
			if(i>=0){
				this.items.ids[i]=id;
                this.items.prices[i]= price;
                this.items.tittles[i]=tittle;
                this.items.descs[i]=desc;
			}else{
				if(PS_Plugin.Instance.isDebugMode && isDebugLog)if(PS_Plugin.Instance.isDebugMode && isDebugLog)Debug.LogError( "not match id for this items"+id );
			}

		}




		void OnProductPurchased(BillingResult result)
		{
			
		if(PS_Plugin.Instance.isDebugMode && isDebugLog)Debug.Log( "OnProductPurchased: "+result.IsSuccess);
			

			if(result.Purchase==null || !result.IsSuccess){
				if(PS_Plugin.Instance.isDebugMode && isDebugLog)Debug.Log( "OnProductPurchased null result: ");
				OnPurchased("",true);
			}else{
			
				if(PS_Plugin.Instance.isDebugMode && isDebugLog){
					if(PS_Plugin.Instance.isDebugMode && isDebugLog)Debug.Log( "OnProductPurchased: "+result.Purchase.SKU);
				}
				if(result.Purchase.State==GooglePurchaseState.PURCHASED){
					if(isConsumableProduct(result.Purchase.SKU)){
						if(PS_Plugin.Instance.isDebugMode && isDebugLog)Debug.Log( "consume first ");
						consume(result.Purchase.SKU);
					}else{
						OnPurchased(result.Purchase.SKU,false);
					}
						
				}else if(result.Purchase.State==GooglePurchaseState.CANCELED){
						OnPurchased(result.Purchase.SKU,true);
				}
			}
             isInvoking=false;
		}
		



        //消費された時に呼ばれる
		void OnProductConsumed(BillingResult result) {

		    if(PS_Plugin.Instance.isDebugMode && isDebugLog)Debug.LogError ("OnProductConsumed "+result.IsSuccess );

			if(result.IsSuccess) {
				OnPurchased(result.Purchase.SKU,false);
			} else {
                if(purchasFailledEvent!=null)purchasFailledEvent("購入完了も、消費に失敗　再起動せよ");
			}

		}


        //まず　Cosumeして成功したら呼ばれる
       void OnPurchased(string id,bool isCancel){
            if(PS_Plugin.Instance.isDebugMode && isDebugLog)Debug.Log( "OnPurchased "+id+" "+isCancel);

            if(isCancel){
                if(purchasFailledEvent!=null)purchasFailledEvent(id);
            }else{
         
					if(id==AppData.IAP_SKUs[0]){
						//ここで購入を
						DataManager.Instance.gameData.gameTickets+=5;
						DataManager.Instance.SaveAll();
                     }
					if(id==AppData.IAP_SKUs[1]){
						//ここで購入を
						DataManager.Instance.gameData.gameTickets=-100;
						DataManager.Instance.SaveAll();

					}
					if(id=="ship1sc"){
						DataManager.Instance.SetShipPurchase(0,3,true);
						//0 3
					}else if(id=="ship2sc"){
						//1 3
						DataManager.Instance.SetShipPurchase(1,3,true);
					}else if(id=="ship4sc"){
						//3 3
						DataManager.Instance.SetShipPurchase(3,3,true);
					}else if(id=="ship6pan"){
						//5 2
						DataManager.Instance.SetShipPurchase(5,2,true);
					}else if(id=="ship6sak"){
						//5 4
						DataManager.Instance.SetShipPurchase(5,4,true);
					}else if(id=="ship7met"){
						//6 2
						DataManager.Instance.SetShipPurchase(6,2,true);
					}else if(id=="ship7gld"){
						//6 3
						DataManager.Instance.SetShipPurchase(6,3,true);
					}else if(id=="ship8gld"){
						//7 3
						DataManager.Instance.SetShipPurchase(7,3,true);
					}else if(id=="ship8sc"){
						//7 4
						DataManager.Instance.SetShipPurchase(7,4,true);
					}
					

					if(purchasedEvent!=null){
						purchasedEvent(id);
					}else{
						GameObject go=GameObject.FindGameObjectWithTag("TicketSetter");
						if(go){
							GameTicketSetter setter=go.GetComponent<GameTicketSetter>();
							if(setter){
								setter.UpdateTickets();
							}
						}

						
					}
        	}
            ClearAllEventListeners();
        }



		
}

