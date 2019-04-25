package md5fa4cf85b4275368a1fe83c4fc1d27cb1;


public class ReceiptActivity
	extends android.app.Activity
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"";
		mono.android.Runtime.register ("GroceryMate.ReceiptActivity, GroceryMate", ReceiptActivity.class, __md_methods);
	}


	public ReceiptActivity ()
	{
		super ();
		if (getClass () == ReceiptActivity.class)
			mono.android.TypeManager.Activate ("GroceryMate.ReceiptActivity, GroceryMate", "", this, new java.lang.Object[] {  });
	}


	public void onCreate (android.os.Bundle p0)
	{
		n_onCreate (p0);
	}

	private native void n_onCreate (android.os.Bundle p0);

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
