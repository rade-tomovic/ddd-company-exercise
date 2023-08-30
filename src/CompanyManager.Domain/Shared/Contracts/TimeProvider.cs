﻿namespace CompanyManager.Domain.Shared.Contracts;

public static class TimeProvider
{
    private static DateTime? _customDate;

    public static DateTime Now => _customDate ?? DateTime.UtcNow;

    public static void Set(DateTime customDate) => _customDate = customDate;

    public static void Reset() => _customDate = null;
}