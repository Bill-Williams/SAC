using SAC.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAC.Domain.TypeConverters
{
    public class AspNetRoleTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
            {
                return true;
            }
            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string)
            {
                var db = SacContext.GetCurrentContext();
                
                //var c = db.Roles.AsNoTracking().FirstOrDefault(u => u.Id == (string)value);
                var c = db.Roles.FirstOrDefault(u => u.Id == (string)value);

                if (null != c)
                {
                    return c;
                }
                
            }
            return base.ConvertFrom(context, culture, value);
        }
    }
}
