﻿using System;

namespace EVA.Core.Currencies
{ 
    public static class CurrencyExtensions
    {
        public static decimal RoundFor(this decimal amount, string currencyID)
        {
            return Math.Round(amount, 2, MidpointRounding.AwayFromZero);
        }
    }
}
