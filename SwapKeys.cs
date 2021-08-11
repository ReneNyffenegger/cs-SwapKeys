// vi: foldmarker={{{,}}} foldmethod=marker
//
// V0.2
//
using System;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;            // Use for Application.Run

using TQ84;

namespace TQ84 {

   public class SwapKeys {

      private static Win32.INPUT[] inputs;
      private static int sizeOfInputs_1;

      public static void go() {

         inputs = new Win32.INPUT[1];

         inputs[0].type             = 1; // 1 = INPUT_KEYBOARD;
         inputs[0].U.ki.wScan       = 0;
         inputs[0].U.ki.dwFlags     =           0;
         inputs[0].U.ki.time        = (int)     0;
         inputs[0].U.ki.dwExtraInfo = (UIntPtr) 0;

         sizeOfInputs_1 = System.Runtime.InteropServices.Marshal.SizeOf(inputs[0]);

         TQ84.Win32.Hook.KeyboardLL(keyboardEvent);
         Application.Run();
      }

      private static void replaceVKWith(Win32.VirtualKey vk) {
         inputs[0].U.ki.wVk = vk;
//       if (Win32.Hook.SendInput(1, inputs, System.Runtime.InteropServices.Marshal.SizeOf(inputs[0])) != 1)
         if (Win32.Hook.SendInput(1, inputs, sizeOfInputs_1) != 1)
         {
             Console.WriteLine("SendInput() did not return 1");
         }
      }

      public static bool keyboardEvent(IntPtr wParam, Win32.Hook.KBDLLHOOKSTRUCT kbd) {

          if (wParam == (IntPtr) 0x0101 /* WM_KEYUP */ || wParam == (IntPtr) 0x0105 /* WM_SYSKEYUP*/ ) {
              inputs[0].U.ki.dwFlags = Win32.KEYEVENTF.KEYUP;
          }
          else {
              inputs[0].U.ki.dwFlags = 0;
          }

          if (  ( kbd.flags & Win32.Hook.KBDLLHOOKSTRUCTFlags.LLKHF_INJECTED ) == 0 ) {

               if (kbd.vkCode == (uint) Win32.VirtualKey.CAPITAL) {
                   replaceVKWith(Win32.VirtualKey.ESCAPE);
                   return true;
               }

               if (kbd.vkCode == (uint) Win32.VirtualKey.ESCAPE) {
                   replaceVKWith(Win32.VirtualKey.CAPITAL);
                   return true;
               }

               if (kbd.vkCode == (uint) Win32.VirtualKey.RWIN) {
                   replaceVKWith(Win32.VirtualKey.LCONTROL);
                   Console.WriteLine("RWIN");
                   return true;
               }

           //
           //  2021-08-11 / V0.2: Removing this line seemed to generally improve typing speed and reduce Null pointer exception errors.
           //
           //  Console.WriteLine("scan: " + kbd.scanCode + ", vk = " + kbd.vkCode + ", wParam = " + wParam);
           //
               if (kbd.vkCode == (uint) Win32.VirtualKey.SNAPSHOT) { // Prt Scr
                  Win32.Hook.Stop();
                  Application.ExitThread();

                //  Never reached:
                    return true;
              }
           }

           return false;
      }
   }
}
