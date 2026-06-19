using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine.UI;

namespace NewAPIMigrator.Runtime.UnityEngine.UI
{
    public class InputField_
    {
        public static InputField.SubmitEvent get_onEndEdit(InputField self)
        {
            return Unsafe.As<InputField.SubmitEvent>(self.onEndEdit);
        }
        public static void set_onEndEdit(InputField self, InputField.SubmitEvent ev)
        {
            self.onEndEdit = Unsafe.As<InputField.EndEditEvent>(ev);
        }
    }
}
