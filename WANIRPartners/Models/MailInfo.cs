using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WANIRPartners.Models
{
    public class MailInfo : ObjectPopulator<MailInfo>
    {
        public MailInfo()
        {
            Date = null;
        }

        virtual public int Id { get; set; }
        virtual public DateTime? Date { get; set; }
        virtual public string Subject { get; set; }
       
        virtual public Project Project { get; set; }
        virtual public Partner Partner { get; set; }
    }
}
