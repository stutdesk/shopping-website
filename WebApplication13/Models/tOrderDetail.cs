//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApplication13.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    
    public partial class tOrderDetail
    {
        public int fId { get; set; }
        [DisplayName("訂單編號")]
        public string fOrderGuid { get; set; }
        [DisplayName("會員帳號")]
        public string fUserId { get; set; }
        [DisplayName("產品編號")]
        public string fPId { get; set; }
        [DisplayName("產品名稱")]
        public string fName { get; set; }
        [DisplayName("價格")]
        public Nullable<int> fPrice { get; set; }
        [DisplayName("下定數量")]
        public Nullable<int> fQty { get; set; }
        [DisplayName("訂單狀態")]
        public string fIsApproved { get; set; }
        [DisplayName("訂單日期")]
        public Nullable<System.DateTime> fdate { get; set; }
    }
}
