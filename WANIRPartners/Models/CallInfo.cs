using System;
using WANIRPartners.Utils;

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
        virtual public string OfferStr { get { return Const.Convert(Offer); } }
        virtual public DateTime? OfferDate { get; set; }
        virtual public String FirstDateStr
        {
            get
            {
                if (!FirstDate.HasValue)
                    return String.Empty;
                return FirstDate.Value.Date.ToShortDateString();
            }
        }
        #endregion

        #region SecondCall
        virtual public string SecondCallee { get; set; }
        virtual public string SecondInfo { get; set; }
        virtual public DateTime? SecondDate { get; set; }
        virtual public string Undecided { get; set; }
        virtual public string ResignationReason { get; set; }
        virtual public bool Meeting { get; set; }
        virtual public string MeetingStr 
        {
            get { return Const.Convert(Meeting); }
            set { Meeting = Const.ConvertToBool(value).Value; }
        }
        virtual public String SecondDateStr
        {
            get
            {
                if (!SecondDate.HasValue)
                    return String.Empty;
                return SecondDate.Value.Date.ToShortDateString();
            }
        }
        #endregion

        #region Meeting
        virtual public string MeetingPerson{ get; set; }
        virtual public DateTime? MeetingDate { get; set; }
        virtual public bool ActiveProject { get; set; }
        virtual public string ActiveProjectStr 
        { 
            get { return Const.Convert(ActiveProject); }
            set { ActiveProject = Const.ConvertToBool(value).Value; }
        }
        virtual public string ProjectWriter{ get; set; }
        virtual public string MeetingInfo { get; set; }
        #endregion

        virtual public Project Project { get; set; }
        virtual public Partner Partner { get; set; }
    }
}
