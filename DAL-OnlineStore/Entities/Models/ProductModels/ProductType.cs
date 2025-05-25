using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_OnlineStore.Entities.Models.ProductModels
{

    //جدول الأنواع(Types):

//هذا الجدول سيحتوي على أنواع المنتجات المختلفة
//(في البداية سيكون لديك نوع واحد وهو "تيشيرت"
//، ولكن مع التوسع المستقبلي سيكون لديك أنواع أخرى
//مثل "قمصان"، "بناطيل"، "فساتين"، إلخ)
    public class ProductType
    {
        public int TypeId { get; set; }
        public string TypeName { get; set; } = null!;

        public IList<Product>? Products { get; set;} 
    }
}
