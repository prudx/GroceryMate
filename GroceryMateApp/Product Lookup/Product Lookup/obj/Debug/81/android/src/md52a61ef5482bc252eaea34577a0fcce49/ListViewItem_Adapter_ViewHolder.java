package md52a61ef5482bc252eaea34577a0fcce49;


public class ListViewItem_Adapter_ViewHolder
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("GroceryMate.Resources.adapters.ListViewItem_Adapter+ViewHolder, GroceryMate", ListViewItem_Adapter_ViewHolder.class, __md_methods);
	}


	public ListViewItem_Adapter_ViewHolder ()
	{
		super ();
		if (getClass () == ListViewItem_Adapter_ViewHolder.class)
			mono.android.TypeManager.Activate ("GroceryMate.Resources.adapters.ListViewItem_Adapter+ViewHolder, GroceryMate", "", this, new java.lang.Object[] {  });
	}

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
