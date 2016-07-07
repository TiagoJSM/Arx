using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonInterfaces.Controllers
{
    public interface IAnimationController
    {
        bool IsCurrentAnimationOver { get; }
    }
}
