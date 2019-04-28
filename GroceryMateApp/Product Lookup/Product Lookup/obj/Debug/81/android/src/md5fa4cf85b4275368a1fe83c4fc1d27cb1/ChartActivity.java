package md5fa4cf85b4275368a1fe83c4fc1d27cb1;


public class ChartActivity
	extends android.app.Activity
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"n_LaunchStoreAnalysis:(Landroid/view/View;)V:__export__\n" +
			"n_LaunchItemAnalysis:(Landroid/view/View;)V:__export__\n" +
			"";
		mono.android.Runtime.register ("GroceryMate.ChartActivity, GroceryMate", ChartActivity.class, __md_methods);
	}


	public ChartActivity ()
	{
		super ();
		if (getClass () == ChartActivity.class)
			mono.android.TypeManager.Activate ("GroceryMate.ChartActivity, GroceryMate", "", this, new java.lang.Object[] {  });
	}


	public void onCreate (android.os.Bundle p0)
	{
		n_onCreate (p0);
	}

	private native void n_onCreate (android.os.Bundle p0);


	public void LaunchStoreAnalysis (android.view.View p0)
	{
		n_LaunchStoreAnalysis (p0);
	}

	private native void n_LaunchStoreAnalysis (android.view.View p0);


	public void LaunchItemAnalysis (android.view.View p0)
	{
		n_LaunchItemAnalysis (p0);
	}

	private native void n_LaunchItemAnalysis (android.view.View p0);

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
