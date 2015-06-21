using System;
using System.Collections.Generic;
using Android.Accounts;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Java.Util.Jar;
using Android.Gms.Auth;
using Android.Gms.Common;
using Android.Gms.Plus;
//using System.Threading.Tasks;
using System.Threading.Tasks;
using System.Runtime.ExceptionServices;
using Java.Lang.Reflect;
using Org.Apache.Http.Util;


namespace GoogAuth
{
    [Activity(Label = "GoogAuth", MainLauncher = true, Icon = "@drawable/icon")]

    public class Main : Activity
    {

        public List<AccountItem> accountlist = new List<AccountItem>();
        public List<string> maccountlist = new List<string>();
        private int count = 1;
        private ListView lv_AccountsList;
        private static string token;
        public Account account;
        public static string CLIENT_ID = 
            //"896273265697-7dtmt76qip0k6c96us4der5ocmco6d9u@developer.gserviceaccount.com";
            "896273265697-csmen8irsvmf6qgs2bev1k2dup3pnlcn.apps.googleusercontent.com";
            //"896273265697-c3addukuu6p43jrps2iu170du1guk4g4.apps.googleusercontent.com";
            //"896273265697-mhtbubjpk54894gu2184dhv4rr6m04ka.apps.googleusercontent.com";
        public static string SCOPE = "audience:server:client_id:" + CLIENT_ID;

        public void MainActivity()
        {
            AppDomain.CurrentDomain.FirstChanceException += (object sender, FirstChanceExceptionEventArgs e) => {
                System.Diagnostics.Debug.WriteLine("FirstChanceException event raised in {0}: {1}", 
                    AppDomain.CurrentDomain.FriendlyName, 
                    e.Exception.Message);
            };
        
        }
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            Button button = FindViewById<Button>(Resource.Id.MyButton);

            //button.Click += delegate { button.Text = string.Format("{0} clicks!", count++); };
            button.Click  +=  (object sender, EventArgs e) =>
            {
                    //Task<int> sizeTask = DownloadHomepageAsync();
                    //Task<string> accountTask = GetTokenAsync();
                    //var tokenTask = await accountTask;
                    GetAccounts(sender,e);
            };
        }


            public async void GetAccounts(object sender, EventArgs e)
            {
                maccountlist.Clear();
                try
                {
                    Account[] accounts = AccountManager.Get(this).GetAccountsByType("com.google");
                foreach (Account account in accounts)
                    {
                       
                    //token =  GoogleAuthUtil.GetToken(this.ApplicationContext,account.Name,scope); //Need Plus Services to get these references...
                    token =  await GetTokenAsync(account.Name);
                    //token_task.Start();
                    //token_task.Wait();
                        if(token == null)
                    {
                            token="zzzz828398298zeer989898";
                        }
                        else{
                            
                        
                        AccountItem accountitem = new AccountItem(account.Type, account.Name,token);
                            accountlist.Add(accountitem);
                            maccountlist.Add(accountitem.name);
                            maccountlist.Add(accountitem.token);
                        }
                    }

                }
                catch (Exception)
                {

                    throw;
                }
                //return accountlist;
            lv_AccountsList = FindViewById<ListView>(Resource.Id.listView1);
            ArrayAdapter ListAdapter = new ArrayAdapter<String>(this, Android.Resource.Layout.SimpleListItem1, maccountlist); 
            lv_AccountsList.Adapter = ListAdapter;
                    
        
        }

        public Task<string> GetTokenAsync(string accountName) 
        {
            //string token_string;
            //string scope = 
            string scopesString = Scopes.PlusLogin;
            string scope = 
            //"oauth2:profile email"; //needs permission
            //"oauth2:server:client_id:"+CLIENT_ID+":api_scope:"+ Scopes.PlusMe + " " + "https://www.googleapis.com/auth/plus.login"; //needs permission
            //"oauth2:server:client_id:" + CLIENT_ID + ":api_scope:" + scopesString; //needs permission
            "oauth2:" + Scopes.PlusMe;
            //"oauth2:https://www.googleapis.com/auth/userinfo.email";

            //Need Plus Services to get these references...
            return Task.Run(()=>
            {
              try
                 {
                        var token =  GoogleAuthUtil.GetToken(this, accountName, scope);
                        return token;
                 }
                 
                    catch( UserRecoverableAuthException ex)
                    {
                        StartActivityForResult(this.Intent,55664);
                        System.Diagnostics.Debug.WriteLine("An UserRecoverableAuthException was caught!");
                        //Throwable cause = ex.getCause();
                        System.Diagnostics.Debug.WriteLine(String.Format("Invocation of {0} failed because of: {1}",ex.Source,ex.Message));
                        //activity.startActivityForResult(e.getIntent(), REQUEST_AUTHORIZATION);

                      return ex.Message;
                    }
            });
        }


   }


  



    public class AccountItem
    {
        public string type { get; set; }
        public string name { get; set; }
        public string token { get; set; }

        public AccountItem(string in_type, string in_name, string in_token)
        {
            type = in_type;
            name = in_name;
            token = in_token;

        }
    }

}

