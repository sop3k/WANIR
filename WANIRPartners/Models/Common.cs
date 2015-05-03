using NHibernate.Util;

namespace WANIRPartners.Models
{
    public class ObjectPopulator<T>
    {
        public virtual void Populate<TFrom>(TFrom source)
        {
            var fromProps = typeof(T).GetProperties();
            foreach (var prop in fromProps)
            {
                if (source.GetType().HasProperty(prop.Name))
                {
                    var fromProp = source.GetType().GetProperty(prop.Name);
                    prop.SetValue(this, fromProp.GetValue(source));
                }
            }
        }
    }
}
