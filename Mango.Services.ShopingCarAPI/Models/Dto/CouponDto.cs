﻿namespace Mango.Services.ShopingCarAPI.Models.Dto
{
    public class CouponDto
    {
        public int CouponId { get; set; }
        public required string CouponCode { get; set; }
        public double DsicountAmount { get; set; }
        public int MinAmount { get; set; }
    }
}
