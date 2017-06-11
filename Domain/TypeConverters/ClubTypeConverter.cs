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
    public class ClubTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string)
                || sourceType == typeof(Guid))
            {
                return true;
            }
            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string)
            {
                using (var db = new SacContext())
                {
                    var c = db.Clubs.AsNoTracking().FirstOrDefault(u => u.Id.ToString() == (string)value);

                    if (null != c)
                    {
                        return c;
                    }
                }
            }
            if (value is Guid)
            {
                using (var db = new SacContext())
                {
                    var c = db.Clubs.AsNoTracking().FirstOrDefault(u => u.Id == (Guid)value);

                    if (null != c)
                    {
                        return c;
                    }
                }
            }
            return base.ConvertFrom(context, culture, value);
        }
    }
}
