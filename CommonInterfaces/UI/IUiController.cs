using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonInterfaces.UI
{
    public interface IUiController
    {
        void TurnOn();
        void TurnOff();
        void Toggle();
    }
}
