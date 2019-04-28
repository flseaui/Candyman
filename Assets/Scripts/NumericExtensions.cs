using System;

public static class NumericExtensions
{
    public static double ToRadians(this double val)
    {
        return 0.0174532925199433D * val;
    }
}