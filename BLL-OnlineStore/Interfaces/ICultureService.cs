using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_OnlineStore.Interfaces
{
    public interface ICultureService
    {
        string CurrentCulture { get; }
        void SetCulture(string culture);
    }

}
