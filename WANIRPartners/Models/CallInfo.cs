using System;

namespace WANIRPartners.Models
{
    public class CallInfo : ObjectPopulator<CallInfo>
    {
        virtual public int Id { get; set; }
        virtual public string Callee { get; set; }
        virtual public string Info { get; set; }
        virtual public Project Project { get; set; }
        virtual public Partner Partner { get; set; }
    }
}
