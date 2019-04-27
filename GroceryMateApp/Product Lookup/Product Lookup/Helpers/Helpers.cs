using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using EDMTDialog;
using Plugin.CurrentActivity;
using AlertDialog = Android.Support.V7.App.AlertDialog;

namespace GroceryMate.Helpers
{
    public static class Helper
    {
        public enum AlertType { Error, Load, Info }
        public static AlertDialog dialog;

        //Method to close current keyboard field (usually called at the end of a button press)
        public static void CloseKeyboard()
        {
            Activity currentActivity = CrossCurrentActivity.Current.Activity; //get current activity

            View view = currentActivity.CurrentFocus;
            InputMethodManager imm = (InputMethodManager)currentActivity.GetSystemService(Context.InputMethodService);
            imm.HideSoftInputFromWindow(view.WindowToken, 0);
        }

        //Alert handler to improve code cleanliness
        public static AlertDialog CreateAlert(AlertType type, string alertMessage, string alertTitle)
        {
            Activity currentActivity = CrossCurrentActivity.Current.Activity; //get current activity

            if (type == AlertType.Error)
            {
                AlertDialog.Builder dialogConnection = new AlertDialog.Builder(currentActivity);
                dialog = dialogConnection.Create();
                dialog.SetTitle(alertTitle);
                dialog.SetMessage(alertMessage);
            }
            else if (type == AlertType.Load)
            {
                dialog = new EDMTDialogBuilder()
                    .SetContext(CrossCurrentActivity.Current.AppContext)
                    .SetMessage(alertMessage)
                    .Build();
            }
            else if (type == AlertType.Info)
            {
                AlertDialog.Builder dialogConnection = new AlertDialog.Builder(currentActivity);

                dialogConnection.SetPositiveButton("OK", (senderAlert, args) => {
                    dialogConnection.Dispose();
                });
                dialog = dialogConnection.Create();
                dialog.SetTitle(alertTitle);
                dialog.SetMessage(alertMessage);
                dialog.SetCanceledOnTouchOutside(true);
            }

            dialog.Show();

            return dialog;
        }
    }

    public interface INativeFont
    {
        float GetNativeSize(float size);
    }

    public class NativeFont : INativeFont
    {
        public float GetNativeSize(float size)
        {
            var displayMetrics = Android.App.Application.Context.Resources.DisplayMetrics;
            return TypedValue.ApplyDimension(ComplexUnitType.Dip, size, displayMetrics);
        }
    }
}