using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Product_Lookup.Model
{
    public class RootObject : IEnumerable<Uk>
    {
        public Uk uk { get; set; }

        public IEnumerator<Uk> GetEnumerator()
        {
            throw new Exception("IEnumerator first one");
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}