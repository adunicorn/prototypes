//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OldIssuingService.EF
{
    using System;
    using System.Collections.Generic;
    
    public partial class Card
    {
        public string ID { get; set; }
        public string CardHolderId { get; set; }
        public string EmbossingLine { get; set; }
    
        public virtual CardHolder CardHolder { get; set; }
    }
}