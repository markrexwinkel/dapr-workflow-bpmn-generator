using Rex.Bpmn.Abstractions.Model;
using System.Globalization;

namespace Rex.Bpmn.Drawing;

class Path
{
    public static readonly Path EventMessage = new Path
    {
        D = p => ((FormattableString)$"m {p.mx},{p.my} l 0,{p.e["y1"]} l {p.e["x1"]},0 l 0,-{p.e["y1"]} z l {p.e["x0"]},{p.e["y0"]} l {p.e["x0"]},-{p.e["y0"]}").ToString(CultureInfo.InvariantCulture),
        Height = 36,
        Width = 36,
        HeightElements = new[] { 6f, 14f },
        WidthElements = new[] { 10.5f, 21f }
    };

    public static readonly Path EventSignal = new Path
    {
        D = p => ((FormattableString)$"M {p.mx},{p.my} l {p.e["x0"]},{p.e["y0"]} l -{p.e["x1"]},0 Z").ToString(CultureInfo.InvariantCulture),
        Height = 36,
        Width = 36,
        HeightElements = new[] { 18f },
        WidthElements = new[] { 10f, 20f }
    };

    public static readonly Path EventEscalation = new Path
    {
        D = p => ((FormattableString)$"M {p.mx},{p.my} l {p.e["x0"]},{p.e["y0"]} l -{p.e["x0"]},-{p.e["y1"]} l -{p.e["x0"]},{p.e["y1"]} Z").ToString(CultureInfo.InvariantCulture),
        Height = 36,
        Width = 36,
        HeightElements = new[] { 20f, 7f },
        WidthElements = new[] { 8f }
    };

    public static readonly Path EventConditional = new Path
    {
        D = p => ((FormattableString)$"M {p.mx},{p.my} m {p.e["x0"]},{p.e["y0"]} l {p.e["x1"]},0 l 0,{p.e["y2"]} l -{p.e["x1"]},0 Z ").ToString(CultureInfo.InvariantCulture) +
                 ((FormattableString)$"M {p.mx},{p.my} m {p.e["x2"]},{p.e["y3"]} l {p.e["x0"]},0 ").ToString(CultureInfo.InvariantCulture) +
                 ((FormattableString)$"M {p.mx},{p.my} m {p.e["x2"]},{p.e["y4"]} l {p.e["x0"]},0 ").ToString(CultureInfo.InvariantCulture) +
                 ((FormattableString)$"M {p.mx},{p.my} m {p.e["x2"]},{p.e["y5"]} l {p.e["x0"]},0 ").ToString(CultureInfo.InvariantCulture) +
                 ((FormattableString)$"M {p.mx},{p.my} m {p.e["x2"]},{p.e["y6"]} l {p.e["x0"]},0 ").ToString(CultureInfo.InvariantCulture) +
                 ((FormattableString)$"M {p.mx},{p.my} m {p.e["x2"]},{p.e["y7"]} l {p.e["x0"]},0 ").ToString(CultureInfo.InvariantCulture) +
                 ((FormattableString)$"M {p.mx},{p.my} m {p.e["x2"]},{p.e["y8"]} l {p.e["x0"]},0 ").ToString(CultureInfo.InvariantCulture),
        Height = 36,
        Width = 36,
        HeightElements = new[] { 8.5f, 14.5f, 18f, 11.5f, 14.5f, 17.5f, 20.5f, 23.5f, 26.5f },
        WidthElements = new[] { 10.5f, 14.5f, 12.5f }
    };

    public static readonly Path EventLink = new Path
    {
        D = p => ((FormattableString)$"m {p.mx},{p.my} 0,{p.e["y0"]} -{p.e["x1"]},0 0,{p.e["y1"]} {p.e["x1"]},0 0,{p.e["y0"]} {p.e["x0"]},-{p.e["y2"]} -{p.e["x0"]},-{p.e["y2"]} z").ToString(CultureInfo.InvariantCulture),
        Height = 36,
        Width = 36,
        HeightElements = new[] { 4.4375f, 6.75f, 7.8125f },
        WidthElements = new[] { 9.84375f, 13.5f }
    };

    public static readonly Path EventError = new Path
    {
        D = p => ((FormattableString)$"m {p.mx},{p.my} {p.e["x0"]},-{p.e["y0"]} {p.e["x1"]},-{p.e["y1"]} {p.e["x2"]},{p.e["y2"]} {p.e["x3"]},-{p.e["y3"]} -{p.e["x4"]},{p.e["y4"]} -{p.e["x5"]},-{p.e["y5"]} z").ToString(CultureInfo.InvariantCulture),
        Height = 36,
        Width = 36,
        HeightElements = new[] { 0.023f, 8.737f, 8.151f, 16.564f, 10.591f, 8.714f },
        WidthElements = new[] { 0.085f, 6.672f, 6.97f, 4.273f, 5.337f, 6.636f }
    };

    public static readonly Path EventCancel45 = new Path
    {
        D = p => ((FormattableString)$"m {p.mx},{p.my} -{p.e["x1"]},0 0,{p.e["x0"]} {p.e["x1"]},0 0,{p.e["y1"]} {p.e["x0"]},0 ").ToString(CultureInfo.InvariantCulture) +
                 ((FormattableString)$"0,-{p.e["y1"]} {p.e["x1"]},0 0,-{p.e["y0"]} -{p.e["x1"]},0 0,-{p.e["y1"]} -{p.e["x0"]},0 z").ToString(CultureInfo.InvariantCulture),
        Height = 36,
        Width = 36,
        HeightElements = new[] { 4.75f, 8.5f },
        WidthElements = new[] { 4.75f, 8.5f }
    };

    public static readonly Path EventCompensation = new Path
    {
        D = p => ((FormattableString)$"m {p.mx},{p.my} {p.e["x0"]},-{p.e["y0"]} 0,{p.e["y1"]} z m {p.e["x1"]},-{p.e["y2"]} {p.e["x2"]},-{p.e["y3"]} 0,{p.e["y1"]} -{p.e["x2"]},-{p.e["y3"]} z").ToString(CultureInfo.InvariantCulture),
        Height = 36,
        Width = 36,
        HeightElements = new[] { 6.5f, 13f, 0.4f, 6.1f },
        WidthElements = new[] { 9f, 9.3f, 8.7f }
    };

    public static readonly Path EventTimerWH = new Path
    {
        D = p => ((FormattableString)$"M {p.mx},{p.my} l {p.e["x0"]},-{p.e["y0"]} m -{p.e["x0"]},{p.e["y0"]} l {p.e["x1"]},{p.e["y1"]} ").ToString(CultureInfo.InvariantCulture),
        Height = 36,
        Width = 36,
        HeightElements = new[] { 10f, 2f },
        WidthElements = new[] { 0f, 0f }
    };

    public static readonly Path EventTimerLine = new Path
    {
        D = p => ((FormattableString)$"M {p.mx},{p.my} m {p.e["x0"]},{p.e["y0"]} l -{p.e["x1"]},{p.e["y1"]} ").ToString(CultureInfo.InvariantCulture),
        Height = 36,
        Width = 36,
        HeightElements = new[] { 10f, 3f },
        WidthElements = new[] { 0f, 0f }
    };

    public static readonly Path EventMultiple = new Path
    {
        D = p => ((FormattableString)$"m {p.mx},{p.my} {p.e["x1"]},-{p.e["y0"]} {p.e["x1"]},{p.e["y0"]} -{p.e["x0"]},{p.e["y1"]} -{p.e["x2"]},0 z").ToString(CultureInfo.InvariantCulture),
        Height = 36,
        Width = 36,
        HeightElements = new[] { 6.28099f, 12.56199f },
        WidthElements = new[] { 3.1405f, 9.42149f, 12.56198f }
    };

    public static readonly Path EventParallelMultiple = new Path
    {
        D = p => ((FormattableString)$"m {p.mx},{p.my} {p.e["x0"]},0 0,{p.e["y1"]} {p.e["x1"]},0 0,{p.e["y0"]} -{p.e["x1"]},0 0,{p.e["y1"]} ").ToString(CultureInfo.InvariantCulture) +
                 ((FormattableString)$"-{p.e["x0"]},0 0,-{p.e["y1"]} -{p.e["x1"]},0 0,-{p.e["y0"]} {p.e["x1"]},0 z").ToString(CultureInfo.InvariantCulture),
        Height = 36,
        Width = 36,
        HeightElements = new[] { 2.56228f, 7.68683f },
        WidthElements = new[] { 2.56228f, 7.68683f }
    };

    public static readonly Path GatewayExclusive = new Path
    {
        D = p => ((FormattableString)$"m {p.mx},{p.my} {p.e["x0"]},{p.e["y0"]} {p.e["x1"]},{p.e["y0"]} {p.e["x2"]},0 {p.e["x4"]},{p.e["y2"]} ").ToString(CultureInfo.InvariantCulture) +
                 ((FormattableString)$"{p.e["x4"]},{p.e["y1"]} {p.e["x2"]},0 {p.e["x1"]},{p.e["y3"]} {p.e["x0"]},{p.e["y3"]}").ToString(CultureInfo.InvariantCulture) +
                 ((FormattableString)$"{p.e["x3"]},0 {p.e["x5"]},{p.e["y1"]} {p.e["x5"]},{p.e["y2"]} {p.e["x3"]},0 z").ToString(CultureInfo.InvariantCulture),
        Height = 17.5f,
        Width = 17.5f,
        HeightElements = new[] { 8.5f, 6.5312f, -6.5312f, -8.5f },
        WidthElements = new[] { 6.5f, -6.5f, 3f, -3f, 5f, -5f }
    };

    public static readonly Path GatewayParallel = new Path
    {
        D = p => ((FormattableString)$"m {p.mx},{p.my} 0,{p.e["y1"]} -{p.e["x1"]},0 0,{p.e["y0"]} {p.e["x1"]},0 0,{p.e["y1"]} {p.e["x0"]},0 ").ToString(CultureInfo.InvariantCulture) +
                 ((FormattableString)$"0,-{p.e["y1"]} {p.e["x1"]},0 0,-{p.e["y0"]} -{p.e["x1"]},0 0,-{p.e["y1"]} -{p.e["x0"]},0 z").ToString(CultureInfo.InvariantCulture),
        Height = 30f,
        Width = 30f,
        HeightElements = new[] { 5f, 12.5f },
        WidthElements = new[] { 5f, 12.5f }
    };

    public static readonly Path GatewayEventBased = new Path
    {
        D = p => ((FormattableString)$"m {p.mx},{p.my} {p.e["x0"]},{p.e["y0"]} {p.e["x0"]},{p.e["y1"]} {p.e["x1"]},{p.e["y2"]} {p.e["x2"]},0 z").ToString(CultureInfo.InvariantCulture),
        Height = 11f,
        Width = 11f,
        HeightElements = new[] { -6f, 6f, 12f, -12f },
        WidthElements = new[] { 9f, -3f, -12f }
    };

    public static readonly Path GatewayComplex = new Path
    {
        D = p => ((FormattableString)$"m {p.mx},{p.my} 0,{p.e["y0"]} -{p.e["x0"]},-{p.e["y1"]} -{p.e["x1"]},{p.e["y2"]} {p.e["x0"]},{p.e["y1"]} -{p.e["x2"]},0 0,{p.e["y3"]} ").ToString(CultureInfo.InvariantCulture) +
                 ((FormattableString)$"{p.e["x2"]},0  -{p.e["x0"]},{p.e["y1"]} l {p.e["x1"]},{p.e["y2"]} {p.e["x0"]},-{p.e["y1"]} 0,{p.e["y0"]} {p.e["x3"]},0 0,-{p.e["y0"]} {p.e["x0"]},{p.e["y1"]} ").ToString(CultureInfo.InvariantCulture) +
                 ((FormattableString)$"{p.e["x1"]},-{p.e["y2"]} -{p.e["x0"]},-{p.e["y1"]} {p.e["x2"]},0 0,-{p.e["y3"]} -{p.e["x2"]},0 {p.e["x0"]},-{p.e["y1"]} -{p.e["x1"]},-{p.e["y2"]} ").ToString(CultureInfo.InvariantCulture) +
                 ((FormattableString)$"-{p.e["x0"]},{p.e["y1"]} 0,-{p.e["y0"]} -{p.e["x3"]},0 z").ToString(CultureInfo.InvariantCulture),
        Height = 17.125f,
        Width = 17.125f,
        HeightElements = new[] { 4.875f, 3.4375f, 2.125f, 3f },
        WidthElements = new[] { 3.4375f, 2.125f, 4.875f, 3f }
    };

    public static readonly Path DataObjectPath = new Path
    {
        D = p => ((FormattableString)$"m 0,0 {p.e["x1"]},0 {p.e["x0"]},{p.e["y0"]} 0,{p.e["y1"]} -{p.e["x2"]},0 0,-{p.e["y2"]} {p.e["x1"]},0 0,{p.e["y0"]} {p.e["x0"]},0").ToString(CultureInfo.InstalledUICulture),
        Height = 61f,
        Width = 51f,
        HeightElements = new[] { 10f, 50f, 60f },
        WidthElements = new[] { 10f, 40f, 50f, 60f }
    };

    public static readonly Path DataObjectCollectionPath = new Path
    {
        D = p => ((FormattableString)$"m {p.mx}, {p.my} m  0 15  l 0 -15 m  4 15  l 0 -15 m  4 15  l 0 -15").ToString(CultureInfo.InstalledUICulture),
        Height = 61f,
        Width = 51f,
        HeightElements = new[] { 12f },
        WidthElements = new[] { 1f, 6f, 12f, 15f }
    };

    public static readonly Path DataArrow = new Path
    {
        D = p => "m 5,9 9,0 0,-3 5,5 -5,5 0,-3 -9,0 z",
        Height = 61f,
        Width = 51f,
        HeightElements = new float[0],
        WidthElements = new float[0]
    };

    public static readonly Path DataStore = new Path
    {
        D = p => ((FormattableString)$"m {p.mx},{p.my} ").ToString(CultureInfo.InstalledUICulture) +
                 ((FormattableString)$"l  0,{p.e["y2"]} ").ToString(CultureInfo.InstalledUICulture) +
                 ((FormattableString)$"c  {p.e["x0"]},{p.e["y1"]} {p.e["x1"]},{p.e["y1"]}  {p.e["x2"]},0 ").ToString(CultureInfo.InstalledUICulture) +
                 ((FormattableString)$"l  0,-{p.e["y2"]} ").ToString(CultureInfo.InstalledUICulture) +
                 ((FormattableString)$"c -{p.e["x0"]},-{p.e["y1"]} -{p.e["x1"]},-{p.e["y1"]} -{p.e["x2"]},0").ToString(CultureInfo.InstalledUICulture) +
                 ((FormattableString)$"c  {p.e["x0"]},{p.e["y1"]} {p.e["x1"]},{p.e["y1"]}  {p.e["x2"]},0 ").ToString(CultureInfo.InstalledUICulture) +
                 ((FormattableString)$"m  -{p.e["x2"]},{p.e["y0"]}").ToString(CultureInfo.InstalledUICulture) +
                 ((FormattableString)$"c  {p.e["x0"]},{p.e["y1"]} {p.e["x1"]},{p.e["y1"]} {p.e["x2"]},0 ").ToString(CultureInfo.InstalledUICulture) +
                 ((FormattableString)$"m  -{p.e["x2"]},{p.e["y0"]}").ToString(CultureInfo.InstalledUICulture) +
                 ((FormattableString)$"c  {p.e["x0"]},{p.e["y1"]} {p.e["x1"]},{p.e["y1"]}  {p.e["x2"]},0").ToString(CultureInfo.InstalledUICulture),
        Height = 61f,
        Width = 61f,
        HeightElements = new[] { 7f, 10f, 45f },
        WidthElements = new[] { 2f, 58f, 60f }
    };

    public static readonly Path TextAnnotation = new Path
    {
        D = p => ((FormattableString)$"m {p.mx}, {p.my} m 10,0 l -10,0 l 0,{p.e["y0"]} l 10,0").ToString(CultureInfo.InvariantCulture),
        Height = 30f,
        Width = 10f,
        HeightElements = new[] { 30f },
        WidthElements = new[] { 10f }
    };

    public static readonly Path MarkerSubProcess = new Path
    {
        D = p => ((FormattableString)$"m {p.mx},{p.my} m 7,2 l 0,10 m -5,-5 l 10,0").ToString(CultureInfo.InvariantCulture),
        Height = 10f,
        Width = 10f,
        HeightElements = new float[0],
        WidthElements = new float[0]
    };

    public static readonly Path MarkerParallel = new Path
    {
        D = p => ((FormattableString)$"m {p.mx},{p.my} m 3,2 l 0,10 m 3,-10 l 0,10 m 3,-10 l 0,10").ToString(CultureInfo.InvariantCulture),
        Height = 10f,
        Width = 10f,
        HeightElements = new float[0],
        WidthElements = new float[0]
    };

    public static readonly Path MarkerSequential = new Path
    {
        D = p => ((FormattableString)$"m {p.mx},{p.my} m 0,3 l 10,0 m -10,3 l 10,0 m -10,3 l 10,0").ToString(CultureInfo.InvariantCulture),
        Height = 10f,
        Width = 10f,
        HeightElements = new float[0],
        WidthElements = new float[0]
    };

    public static readonly Path MarkerCompensation = new Path
    {
        D = p => ((FormattableString)$"m {p.mx},{p.my} 7,-5 0,10 z m 7.1,-0.3 6.9,-4.7 0,10 -6.9,-4.7 z").ToString(CultureInfo.InvariantCulture),
        Height = 10f,
        Width = 21f,
        HeightElements = new float[0],
        WidthElements = new float[0]
    };

    public static readonly Path MarkerLoop = new Path
    {
        D = p => ((FormattableString)$"m {p.mx},{p.my} c 3.526979,0 6.386161,-2.829858 6.386161,-6.320661 0,-3.490806 -2.859182,-6.320661 ").ToString(CultureInfo.InvariantCulture) +
                                      "-6.386161,-6.320661 -3.526978,0 -6.38616,2.829855 -6.38616,6.320661 0,1.745402 " +
                                      "0.714797,3.325567 1.870463,4.469381 0.577834,0.571908 1.265885,1.034728 2.029916,1.35457 " +
                                      "l -0.718163,-3.909793 m 0.718163,3.909793 -3.885211,0.802902",
        Height = 13.9f,
        Width = 13.7f,
        HeightElements = new float[0],
        WidthElements = new float[0]
    };

    public static readonly Path MarkerAdHoc = new Path
    {
        D = p => ((FormattableString)$"m {p.mx},{p.my} m 0.84461,2.64411 c 1.05533,-1.23780996 2.64337,-2.07882 4.29653,-1.97997996 2.05163,0.0805 ").ToString(CultureInfo.InvariantCulture) +
                                      "3.85579,1.15803 5.76082,1.79107 1.06385,0.34139996 2.24454,0.1438 3.18759,-0.43767 0.61743,-0.33642 " +
                                      "1.2775,-0.64078 1.7542,-1.17511 0,0.56023 0,1.12046 0,1.6807 -0.98706,0.96237996 -2.29792,1.62393996 " +
                                      "-3.6918,1.66181996 -1.24459,0.0927 -2.46671,-0.2491 -3.59505,-0.74812 -1.35789,-0.55965 " +
                                      "-2.75133,-1.33436996 -4.27027,-1.18121996 -1.37741,0.14601 -2.41842,1.13685996 -3.44288,1.96782996 z",
        Height = 4f,
        Width = 15f,
        HeightElements = new float[0],
        WidthElements = new float[0]
    };

    public static readonly Path TaskTypeSend = new Path
    {
        D = p => ((FormattableString)$"m {p.mx},{p.my} l 0,{p.e["y1"]} l {p.e["x1"]},0 l 0,-{p.e["y1"]} z l {p.e["x0"]},{p.e["y0"]} l {p.e["x0"]},-{p.e["y0"]}").ToString(CultureInfo.InvariantCulture),
        Height = 14f,
        Width = 21f,
        HeightElements = new[] { 6f, 14f },
        WidthElements = new[] { 10.5f, 21f }
    };

    public static readonly Path TaskTypeScript = new Path
    {
        D = p => ((FormattableString)$"m {p.mx},{p.my} c 9.966553,-6.27276 -8.000926,-7.91932 2.968968,-14.938 l -8.802728,0 ").ToString(CultureInfo.InvariantCulture) +
                                      "c -10.969894,7.01868 6.997585,8.66524 -2.968967,14.938 z " +
                                      "m -7,-12 l 5,0 " +
                                      "m -4.5,3 l 4.5,0 " +
                                      "m -3,3 l 5,0" +
                                      "m -4,3 l 5,0",
        Height = 15f,
        Width = 12.6f,
        HeightElements = new[] { 6f, 14f },
        WidthElements = new[] { 10.5f, 21f }
    };

    public static readonly Path TaskTypeUser1 = new Path
    {
        D = p => ((FormattableString)$"m {p.mx},{p.my} c 0.909,-0.845 1.594,-2.049 1.594,-3.385 0,-2.554 -1.805,-4.62199999 ").ToString(CultureInfo.InvariantCulture) +
                                      "-4.357,-4.62199999 -2.55199998,0 -4.28799998,2.06799999 -4.28799998,4.62199999 0,1.348 " +
                                      "0.974,2.562 1.89599998,3.405 -0.52899998,0.187 -5.669,2.097 -5.794,4.7560005 v 6.718 " +
                                      "h 17 v -6.718 c 0,-2.2980005 -5.5279996,-4.5950005 -6.0509996,-4.7760005 z" +
                                      "m -8,6 l 0,5.5 m 11,0 l 0,-5"
    };

    public static readonly Path TaskTypeUser2 = new Path
    {
        D = p => ((FormattableString)$"m {p.mx},{p.my} m 2.162,1.009 c 0,2.4470005 -2.158,4.4310005 -4.821,4.4310005 ").ToString(CultureInfo.InvariantCulture) +
                                      "-2.66499998,0 -4.822,-1.981 -4.822,-4.4310005 "
    };

    public static readonly Path TaskTypeUser3 = new Path
    {
        D = p => ((FormattableString)$"m {p.mx},{p.my} m -6.9,-3.80 c 0,0 2.25099998,-2.358 4.27399998,-1.177 2.024,1.181 4.221,1.537 ").ToString(CultureInfo.InvariantCulture) +
                                      "4.124,0.965 -0.098,-0.57 -0.117,-3.79099999 -4.191,-4.13599999 -3.57499998,0.001 " +
                                      "-4.20799998,3.36699999 -4.20699998,4.34799999 z"
    };

    public static readonly Path TaskTypeManual = new Path
    {
        D = p => ((FormattableString)$"m {p.mx},{p.my} c 0.234,-0.01 5.604,0.008 8.029,0.004 0.808,0 1.271,-0.172 1.417,-0.752 0.227,-0.898 ").ToString(CultureInfo.InvariantCulture) +
                                      "-0.334,-1.314 -1.338,-1.316 -2.467,-0.01 -7.886,-0.004 -8.108,-0.004 -0.014,-0.079 0.016,-0.533 0,-0.61 " +
                                      "0.195,-0.042 8.507,0.006 9.616,0.002 0.877,-0.007 1.35,-0.438 1.353,-1.208 0.003,-0.768 -0.479,-1.09 " +
                                      "-1.35,-1.091 -2.968,-0.002 -9.619,-0.013 -9.619,-0.013 v -0.591 c 0,0 5.052,-0.016 7.225,-0.016 " +
                                      "0.888,-0.002 1.354,-0.416 1.351,-1.193 -0.006,-0.761 -0.492,-1.196 -1.361,-1.196 -3.473,-0.005 " +
                                      "-10.86,-0.003 -11.0829995,-0.003 -0.022,-0.047 -0.045,-0.094 -0.069,-0.139 0.3939995,-0.319 " +
                                      "2.0409995,-1.626 2.4149995,-2.017 0.469,-0.4870005 0.519,-1.1650005 0.162,-1.6040005 -0.414,-0.511 " +
                                      "-0.973,-0.5 -1.48,-0.236 -1.4609995,0.764 -6.5999995,3.6430005 -7.7329995,4.2710005 -0.9,0.499 " +
                                      "-1.516,1.253 -1.882,2.19 -0.37000002,0.95 -0.17,2.01 -0.166,2.979 0.004,0.718 -0.27300002,1.345 " +
                                      "-0.055,2.063 0.629,2.087 2.425,3.312 4.859,3.318 4.6179995,0.014 9.2379995,-0.139 13.8569995,-0.158 " +
                                      "0.755,-0.004 1.171,-0.301 1.182,-1.033 0.012,-0.754 -0.423,-0.969 -1.183,-0.973 -1.778,-0.01 " +
                                      "-5.824,-0.004 -6.04,-0.004 10e-4,-0.084 0.003,-0.586 10e-4,-0.67 z"
    };

    public static readonly Path TaskTypeInstantiatingSend = new Path
    {
        D = p => ((FormattableString)$"m {p.mx},{p.my} l 0,8.4 l 12.6,0 l 0,-8.4 z l 6.3,3.6 l 6.3,-3.6").ToString(CultureInfo.InvariantCulture)
    };

    public static readonly Path TaskTypeService = new Path
    {
        D = p => ((FormattableString)$"m {p.mx},{p.my} v -1.71335 c 0.352326,-0.0705 0.703932,-0.17838 1.047628,-0.32133 ").ToString(CultureInfo.InvariantCulture) +
                                      "0.344416,-0.14465 0.665822,-0.32133 0.966377,-0.52145 l 1.19431,1.18005 1.567487,-1.57688 " +
                                      "-1.195028,-1.18014 c 0.403376,-0.61394 0.683079,-1.29908 0.825447,-2.01824 l 1.622133,-0.01 " +
                                      "v -2.2196 l -1.636514,0.01 c -0.07333,-0.35153 -0.178319,-0.70024 -0.323564,-1.04372 " +
                                      "-0.145244,-0.34406 -0.321407,-0.6644 -0.522735,-0.96217 l 1.131035,-1.13631 -1.583305,-1.56293 " +
                                      "-1.129598,1.13589 c -0.614052,-0.40108 -1.302883,-0.68093 -2.022633,-0.82247 l 0.0093,-1.61852 " +
                                      "h -2.241173 l 0.0042,1.63124 c -0.353763,0.0736 -0.705369,0.17977 -1.049785,0.32371 -0.344415,0.14437 " +
                                      "-0.665102,0.32092 -0.9635006,0.52046 l -1.1698628,-1.15823 -1.5667691,1.5792 1.1684265,1.15669 " +
                                      "c -0.4026573,0.61283 -0.68308,1.29797 -0.8247287,2.01713 l -1.6588041,0.003 v 2.22174 " +
                                      "l 1.6724648,-0.006 c 0.073327,0.35077 0.1797598,0.70243 0.3242851,1.04472 0.1452428,0.34448 " +
                                      "0.3214064,0.6644 0.5227339,0.96066 l -1.1993431,1.19723 1.5840256,1.56011 1.1964668,-1.19348 " +
                                      "c 0.6140517,0.40346 1.3028827,0.68232 2.0233517,0.82331 l 7.19e-4,1.69892 h 2.226848 z " +
                                      "m 0.221462,-3.9957 c -1.788948,0.7502 -3.8576,-0.0928 -4.6097055,-1.87438 -0.7521065,-1.78321 " +
                                      "0.090598,-3.84627 1.8802645,-4.59604 1.78823,-0.74936 3.856881,0.0929 4.608987,1.87437 " +
                                      "0.752106,1.78165 -0.0906,3.84612 -1.879546,4.59605 z"
    };

    public static readonly Path TaskTypeServiceFill = new Path
    {
        D = p => ((FormattableString)$"m {p.mx},{p.my} c -1.788948,0.7502 -3.8576,-0.0928 -4.6097055,-1.87438 -0.7521065,-1.78321 ").ToString(CultureInfo.InvariantCulture) +
                                      "0.090598,-3.84627 1.8802645,-4.59604 1.78823,-0.74936 3.856881,0.0929 4.608987,1.87437 " +
                                      "0.752106,1.78165 -0.0906,3.84612 -1.879546,4.59605 z"
    };

    public static readonly Path TaskTypeBusinessRuleHeader = new Path
    {
        D = p => ((FormattableString)$"m {p.mx},{p.my} 0,4 20,0 0,-4 z").ToString(CultureInfo.InvariantCulture)
    };

    public static readonly Path TaskTypeBusinessRuleMain = new Path
    {
        D = p => ((FormattableString)$"m {p.mx},{p.my} 0,12 20,0 0,-12 z").ToString(CultureInfo.InvariantCulture) +
                                      "m 0,8 l 20,0 " +
                                      "m -13,-4 l 0,8"
    };

    public static readonly Path MessageFlowMarker = new Path
    {
        D = p => ((FormattableString)$"m {p.mx},{p.my} m -10.5 ,-7 l 0,14 l 21,0 l 0,-14 z l 10.5,6 l 10.5,-6").ToString(CultureInfo.InvariantCulture)
    };



    public struct PathParam
    {
        public float mx;
        public float my;
        public Dictionary<string, float> e;
    }

    public Func<PathParam, string> D { get; set; }
    public float[] HeightElements { get; set; }
    public float[] WidthElements { get; set; }
    public float Height { get; set; }
    public float Width { get; set; }

    public struct ScaleParams
    {
        public Point AbsolutePosition { get; set; }
        public float ContainerWidth { get; set; }
        public float ContainerHeight { get; set; }
        public Point Position { get; set; }
        public float XScaleFactor { get; set; }
        public float YScaleFactor { get; set; }
    };

    public string GetScaledPath(float scaleFactor, Bounds bounds, float mx, float my)
    {
        return GetScaledPath(new ScaleParams
        {
            AbsolutePosition = new Point { X = bounds.X, Y = bounds.Y },
            ContainerHeight = (float)bounds.Height,
            ContainerWidth = (float)bounds.Width,
            Position = new Point { X = mx, Y = my },
            XScaleFactor = scaleFactor,
            YScaleFactor = scaleFactor
        });
    }

    public string GetScaledPath(float scaleFactor, float x, float y, float w, float h, float mx, float my)
    {
        return GetScaledPath(new ScaleParams
        {
            AbsolutePosition = new Point { X = x, Y = y },
            ContainerHeight = h,
            ContainerWidth = w,
            Position = new Point { X = mx, Y = my },
            XScaleFactor = scaleFactor,
            YScaleFactor = scaleFactor
        });
    }

    public string GetScaledPath(Point p)
    {
        return GetScaledPath((float)p.X, (float)p.Y);
    }

    public string GetScaledPath(float mx, float my)
    {
        return GetScaledPath(new ScaleParams
        {
            AbsolutePosition = new Point { X = mx, Y = my }
        });
    }

    public string GetScaledPath(ScaleParams param)
    {
        // positioning
        // compute the start point of the path
        float mx = 0f;
        float my = 0f;

        if (param.AbsolutePosition != null)
        {
            mx = (float)param.AbsolutePosition.X;
            my = (float)param.AbsolutePosition.Y;
        }

        if (param.Position != null)
        {
            mx += param.ContainerWidth * (float)param.Position.X;
            my += param.ContainerHeight * (float)param.Position.Y;
        }

        var coordinates = new Dictionary<string, float>();
        if (param.Position != null)
        {
            // path
            var heightRatio = (param.ContainerHeight / Height) * param.YScaleFactor;
            var widthRatio = (param.ContainerWidth / Width) * param.XScaleFactor;

            // Apply height ration
            for (var heightIndex = 0; heightIndex < HeightElements.Length; heightIndex++)
            {
                coordinates[$"y{heightIndex}"] = HeightElements[heightIndex] * heightRatio;
            }

            // Apply width ratio
            for (var widthIndex = 0; widthIndex < WidthElements.Length; widthIndex++)
            {
                coordinates[$"x{widthIndex}"] = WidthElements[widthIndex] * widthRatio;
            }
        }

        // Apply value to raw path
        var path = D(new PathParam { mx = mx, my = my, e = coordinates });
        return path;
    }

}
