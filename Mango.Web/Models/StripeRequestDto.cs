﻿namespace Mango.Web.Models
{
    public class StripeRequestDto
    {
        public string? StripeSessionUrl { get; set; }
        public string? StripeSessionId { get; set; }
        public string ApprovedURL { get; set; }
        public string CancelURL { get; set; }
        public OrderHeaderDto OrderHeader { get; set; }
    }
}
