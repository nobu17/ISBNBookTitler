using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BookTitleGetter.OpenBDData
{
    [DataContract]
    public class HanmotoDataRoot
    {
        [DataMember(Name = "onix")]
        public HanmotoData HanmotoData { get; set; }

        [DataMember(Name = "summary")]
        public Summary Summary { get; set; }
    }
    #region onix

    [DataContract]
    public class HanmotoData
    {
        [DataMember(Name = "DescriptiveDetail")]
        public DescriptiveDetail DescriptiveDetail { get; set; }


        [DataMember(Name = "PublishingDetail")]
        public PublishingDetail PublishingDetail { get; set; }
    }

    #region DescriptiveDetail

    [DataContract]
    public class DescriptiveDetail
    {
        [DataMember(Name = "TitleDetail")]
        public TitleDetail TitleDetail { get; set; }

        [DataMember(Name = "ProductFormDetail")]
        public string ProductFormDetail { get; set; }

        [DataMember(Name = "Contributor")]
        public List<PersonInfo> Contributor { get; set; }

        [DataMember(Name = "Subject")]
        public List<Subject> Subjects { get; set; }
    }

    #region TitleDetail
    [DataContract]
    public class TitleDetail
    {
        [DataMember(Name = "TitleElement")]
        public TitleElement TitleElement { get; set; }
    }
    [DataContract]
    public class TitleElement
    {
        [DataMember(Name = "TitleText")]
        public ContentText TitleText { get; set; }
        [DataMember(Name = "Subtitle")]
        public ContentText Subtitle { get; set; }
    }
    [DataContract]
    public class ContentText
    {
        [DataMember(Name = "content")]
        public string Content { get; set; }
    }

    #endregion

    #region Contributor

    [DataContract]
    public class PersonInfo
    {
        [DataMember(Name = "SequenceNumber")]
        public string SequenceNumber { get; set; }

        [DataMember(Name = "PersonName")]
        public ContentText PersonName { get; set; }
    }

    #endregion

    #region subject
    [DataContract]
    public class Subject
    {
        [DataMember(Name = "SubjectSchemeIdentifier")]
        public string SubjectSchemeIdentifier { get; set; }

        [DataMember(Name = "SubjectCode")]
        public string SubjectCode { get; set; }
    }

    #endregion

    #endregion

    #region PublishingDetail

    [DataContract]
    public class PublishingDetail
    {
        [DataMember(Name = "Imprint")]
        public Imprint Imprint { get; set; }

        [DataMember(Name = "PublishingDate")]
        public List<PublishingDate> PublishingDates { get; set; }
    }


    [DataContract]
    public class Imprint
    {
        [DataMember(Name = "ImprintName")]
        public string ImprintName { get; set; }
    }

    [DataContract]
    public class PublishingDate
    {
        [DataMember(Name = "PublishingDateRole")]
        public string PublishingDateRole { get; set; }

        [DataMember(Name = "Date")]
        public string Date { get; set; }
    }

    #endregion

    #endregion

    #region summary

    [DataContract]
    public class Summary
    {
        [DataMember(Name = "publisher")]
        public string Publisher { get; set; }

        [DataMember(Name = "pubdate")]
        public string Pubdate { get; set; }

        [DataMember(Name = "title")]
        public string Title { get; set; }

        [DataMember(Name = "author")]
        public string Author { get; set; }
    }

    #endregion
}
