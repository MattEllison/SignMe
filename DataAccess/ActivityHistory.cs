//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataAccess
{
    using System;
    using System.Collections.Generic;
    
    public partial class ActivityHistory
    {
        public int Id { get; set; }
        public System.DateTime InsertDate { get; set; }
        public int StatusID { get; set; }
        public int DocumentID { get; set; }
        public int UserID { get; set; }
    
        public virtual Document Document { get; set; }
        public virtual DocumentStatu DocumentStatu { get; set; }
    }
}
