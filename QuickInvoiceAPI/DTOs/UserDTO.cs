﻿namespace QuickInvoiceAPI.DTOs
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string UserName { get; set; } = String.Empty;
        public string? AccessToken { get; set; }
    }
}