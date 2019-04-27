package md5fa4cf85b4275368a1fe83c4fc1d27cb1;


public class MainActivity
	extends android.support.v7.app.AppCompatActivity
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"n_onCreateOptionsMenu:(Landroid/view/Menu;)Z:GetOnCreateOptionsMenu_Landroid_view_Menu_Handler\n" +
			"n_LoginUser:(Landroid/view/View;)V:__export__\n" +
			"n_StartReceipts:(Landroid/view/View;)V:__export__\n" +
			"n_StartCharts:(Landroid/view/View;)V:__export__\n" +
			"";
		mono.android.Runtime.register ("GroceryMate.MainActivity, GroceryMate", MainActivity.class, __md_methods);
	}


	public MainActivity ()
	{
		super ();
		if (getClass () == MainActivity.class)
			mono.android.TypeManager.Activate ("GroceryMate.MainActivity, GroceryMate", "", this, new java.lang.Object[] {  });
	}


	public void onCreate (android.os.Bundle p0)
	{
		n_onCreate (p0);
	}

	private native void n_onCreate (android.os.Bundle p0);


	public boolean onCreateOptionsMenu (android.view.Menu p0)
	{
		return n_onCreateOptionsMenu (p0);
	}

	private native boolean n_onCreateOptionsMenu (android.view.Menu p0);


	public void LoginUser (android.view.View p0)
	{
		n_LoginUser (p0);
	}

	private native void n_LoginUser (android.view.View p0);


	public void StartReceipts (android.view.View p0)
	{
		n_StartReceipts (p0);
	}

	private native void n_StartReceipts (android.view.View p0);


	public void StartCharts (android.view.View p0)
	{
		n_StartCharts (p0);
	}

	private native void n_StartCharts (android.view.View p0);

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
