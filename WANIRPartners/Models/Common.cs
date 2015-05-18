using NHibernate.Util;

namespace WANIRPartners.Models
{
    public class ObjectPopulator<T> 
        where T : class
    {
        public ObjectPopulator()
        {
            _this = null;
        }

        public ObjectPopulator(T obj)
        {
            _this = obj;
        }

        public virtual void Populate<TFrom>(TFrom source)
        {
            var fromProps = typeof(T).GetProperties();
            foreach (var prop in fromProps)
            {
                if (source.GetType().HasProperty(prop.Name))
                {
                    var fromProp = source.GetType().GetProperty(prop.Name);
                    if(_this != null)
                        prop.SetValue(_this, fromProp.GetValue(source));
                    else
                        prop.SetValue(this, fromProp.GetValue(source));
                }
            }
        }

        private T _this;
    }
}
