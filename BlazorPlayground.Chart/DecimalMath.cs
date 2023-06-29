﻿namespace BlazorPlayground.Chart;

public static class DecimalMath {
    public static decimal Pow(decimal x, int y) {
        var result = 1M;
        
        if (y < 0) {
            y *= -1;
            x = 1 / x;
        }
        
        while (y > 0) {
            if ((y & 1) != 0) {
                result *= x;
            }
            y >>= 1;
            x *= x;
        }

        return result;
    }

    public static decimal FloorToScale(decimal value, decimal scale)
        => Math.Floor(value / scale) * scale;

    public static decimal CeilingToScale(decimal value, decimal scale)
        => Math.Ceiling(value / scale) * scale;
}