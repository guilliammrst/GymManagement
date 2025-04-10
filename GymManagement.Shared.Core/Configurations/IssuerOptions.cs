﻿namespace GymManagement.Shared.Core.Configurations
{
    public class IssuerOptions
    {
        public required string Issuer { get; set; }
        public required string Audience { get; set; }
        public required string SecretKey { get; set; }
    }
}
