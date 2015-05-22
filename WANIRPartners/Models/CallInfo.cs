using System;

namespace WANIRPartners.Models
{
    public class CallInfo : ObjectPopulator<CallInfo>
    {
        public CallInfo()
        {
            FirstDate = null;
            SecondDate = null;
            MeetingDate = null;
            OfferDate = null;
        }

        virtual public int Id { get; set; }
       
        #region FirstCall
        virtual public string FirstCallee { get; set; }
        virtual public string FirstInfo { get; set; }
        virtual public DateTime? FirstDate { get; set; }
        virtual public bool Offer { get; set; }
        virtual public DateTime? OfferDate { get; set; }
        #endregion

        #region SecondCall
        virtual public string SecondCallee { get; set; }
        virtual public string SecondInfo { get; set; }
        virtual public DateTime? SecondDate { get; set; }
        virtual public string Undecided { get; set; }
        virtual public string ResignationReason { get; set; }
        virtual public bool Meeting { get; set; }
        #endregion

        #region Meeting
        virtual public string MeetingPerson{ get; set; }
        virtual public DateTime? MeetingDate { get; set; }
        virtual public bool ActiveProject { get; set; }
        virtual public string ProjectWriter{ get; set; }
        virtual public string MeetingInfo { get; set; }
        #endregion

        virtual public Project Project { get; set; }
        virtual public Partner Partner { get; set; }
    }
}
