using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using CombatSystem.DataStructures;

namespace CombatSystem.Interfaces
{
    public interface ICombatSystem
    {
        Dictionary<string, ComboConfiguration> CombatAttacksConfiguration { get; }
        IComboConfiguration StartCombo(string comboName);
    }
}
