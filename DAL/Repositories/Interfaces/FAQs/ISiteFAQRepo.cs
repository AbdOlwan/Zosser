using DAL.Entities.Models.FaqModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Interfaces.FAQs
{
    public interface ISiteFAQRepo : IBaseFAQRepo<SiteFAQ>
    {
        // يمكن إضافة طرق خاصة بـ SiteFAQ هنا
    }
}
