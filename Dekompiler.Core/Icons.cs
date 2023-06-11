using Dekompiler.Core.Statements;

namespace Dekompiler.Core;

public static class Icons
{
    public static IconInfo CaretRight = new("0 0 320 512", new ISvgChild[]
    {
        new PathInfo("M112 96L256 256 112 416l-48 0L64 96l48 0z")
    });

    public static IconInfo CaretDown = new("0 0 320 512", new ISvgChild[]
    {
        new PathInfo("M320 240L160 384 0 240l0-48 320 0 0 48z")
    });

    public static IconInfo XMark = new("0 0 320 512", new ISvgChild[]
    {
        new PathInfo(
            "M313 137c9.4-9.4 9.4-24.6 0-33.9s-24.6-9.4-33.9 0l-119 119L41 103c-9.4-9.4-24.6-9.4-33.9 0s-9.4 24.6 0 33.9l119 119L7 375c-9.4 9.4-9.4 24.6 0 33.9s24.6 9.4 33.9 0l119-119L279 409c9.4 9.4 24.6 9.4 33.9 0s9.4-24.6 0-33.9l-119-119L313 137z")
    });

    public static IconInfo VerticalGrip = new("0 0 192 512", new ISvgChild[]
    {
        new PathInfo(
            "M32 128a32 32 0 1 0 0-64 32 32 0 1 0 0 64zm0 160a32 32 0 1 0 0-64 32 32 0 1 0 0 64zM64 416A32 32 0 1 0 0 416a32 32 0 1 0 64 0zm96-288a32 32 0 1 0 0-64 32 32 0 1 0 0 64zm32 128a32 32 0 1 0 -64 0 32 32 0 1 0 64 0zM160 448a32 32 0 1 0 0-64 32 32 0 1 0 0 64z")
    });

    public static IconInfo AddFile = new("0 0 384 512", new ISvgChild[]
    {
        new PathInfo(
            "M48 448V64c0-8.8 7.2-16 16-16H224v80c0 17.7 14.3 32 32 32h80V448c0 8.8-7.2 16-16 16H64c-8.8 0-16-7.2-16-16zM64 0C28.7 0 0 28.7 0 64V448c0 35.3 28.7 64 64 64H320c35.3 0 64-28.7 64-64V154.5c0-17-6.7-33.3-18.7-45.3L274.7 18.7C262.7 6.7 246.5 0 229.5 0H64z"),
        new PathInfo(
            "M192 208c-13.3 0-24 10.7-24 24v48H120c-13.3 0-24 10.7-24 24s10.7 24 24 24h48v48c0 13.3 10.7 24 24 24s24-10.7 24-24V328h48c13.3 0 24-10.7 24-24s-10.7-24-24-24H216V232c0-13.3-10.7-24-24-24z",
            "fill-green-600")
    });

    public static IconInfo Brand = new("0 0 576 512", new ISvgChild[]
    {
        new PathInfo(
            "M64 32H96h96 32V96H192 128v96 13.3l-9.4 9.4L77.3 256l41.4 41.4 9.4 9.4V320v96h64 32v64H192 96 64V448 333.3L9.4 278.6 0 269.3V242.7l9.4-9.4L64 178.7V64 32zm448 0V64 178.7l54.6 54.6 9.4 9.4v26.5l-9.4 9.4L512 333.3V448v32H480 384 352V416h32 64V320 306.7l9.4-9.4L498.7 256l-41.4-41.4-9.4-9.4V192 96H384 352V32h32 96 32z")
    });

    public static IconInfo Assembly = new("0 0 16 16", new ISvgChild[]
    {
        new PathInfo("M16,16H0V0H16Z", "fill-none opacity-0"),
        new PathInfo("M9.5,5.5v8h-8v-8Z", "fill-[#212121] opacity-10"),
        new PathInfo("M9.5,5h-8L1,5.5v8l.5.5h8l.5-.5v-8ZM9,13H2V6H9Z", "fill-[#212121] opacity-100"),
        new PathInfo("M13.5,9.5h-2v2H10v-6L9.5,5h-6V3.5h2v-2h8Z", "fill-[#212121] opacity-10"),
        new PathInfo("M13.5,1h-8L5,1.5V3H3.5L3,3.5V5H4V4h7v7H10v1h1.5l.5-.5V10h1.5l.5-.5v-8ZM13,9H12V3.5L11.5,3H6V2h7Z",
            "fill-[#212121] opacity-75"),
    });

    public static IconInfo Module = new("0 0 16 16", new ISvgChild[]
    {
        new PathInfo("M16,16H0V0H16Z", "fill-none opacity-0"),
        new GInfo(new[]
        {
            new PathInfo("M2.5,9.5v5h12v-5Z", "fill-[#6936aa] opacity-10"),
            new PathInfo("M14.5,9H2.5L2,9.5v5l.5.5h12l.5-.5v-5ZM14,14H3V10H14Z", "fill-[#6936aa] opacity-100"),
        }, "opacity-75"),
        new PathInfo("M14.5,2.5v5h-5v-5Zm-12,5h5v-5h-5Z", "fill-[#6936aa] opacity-10"),
        new PathInfo("M14.5,2h-5L9,2.5v5l.5.5h5l.5-.5v-5ZM14,7H10V3h4ZM2.5,2,2,2.5v5l.5.5h5L8,7.5v-5L7.5,2ZM7,7H3V3H7Z",
            "fill-[#6936aa] opacity-100"),
    });

    public static IconInfo Type = new IconInfo("0 0 16 16", new ISvgChild[]
    {
        new PathInfo("M16,16H0V0H16Z", "fill-none opacity-0"),
        new PathInfo("M5.5.5l3,3-5,5-3-3Zm10,6-2-2-3,3,2,2Zm-5,7,2,2,3-3-2-2Z", "fill-[#996f00] opacity-10"),
        new PathInfo(
            "M10.146,7.854l2,2h.708l3-3V6.146l-2-2h-.708L11.293,6H6.707L8.854,3.854V3.146l-3-3H5.146l-5,5v.708l3,3h.708L5.707,7H8v5.5l.5.5h1.793l-.147.146v.708l2,2h.708l3-3v-.708l-2-2h-.708L11.293,12H9V7h1.293l-.147.146ZM3.5,7.793,1.207,5.5,5.5,1.207,7.793,3.5Zm10,3.414L14.793,12.5,12.5,14.793,11.207,13.5Zm0-6L14.793,6.5,12.5,8.793,11.207,7.5Z",
            "fill-[#996f00] opacity-100")
    });

    public static IconInfo ValueType = new IconInfo("0 0 16 16", new ISvgChild[]
    {
        new PathInfo("M16,16H0V0H16Z", "fill-none opacity-0"),
        new GInfo(new[]
        {
            new PathInfo("M4.5,9.5v3h-3v-3Zm10,0v3h-3v-3Z", "fill-[#005dba] opacity-10"),
            new PathInfo(
                "M14.5,9h-3l-.5.5v3l.5.5h3l.5-.5v-3ZM14,12H12V10h2ZM1.5,9,1,9.5v3l.5.5h3l.5-.5v-3L4.5,9ZM4,12H2V10H4Z",
                "fill-[#005dba] opacity-100")
        }, "opacity-75"),
        new PathInfo("M14.5,4.5v3H1.5v-3Z", "fill-[#005dba] opacity-10"),
        new PathInfo("M14.5,4H1.5L1,4.5v3l.5.5h13l.5-.5v-3ZM14,7H2V5H14Z", "fill-[#005dba] opacity-100")
    });

    public static IconInfo Enum = new IconInfo("0 0 16 16", new ISvgChild[]
    {
        new PathInfo("M16,16H0V0H16Z", "fill-none opacity-0"),
        new GInfo(new[]
        {
            new PathInfo("M14.5,2.5v6H9V7H6.5V2.5Z", "fill-[#005dba] opacity-10"),
            new PathInfo("M12.5,4V5h-4V4Zm-4,3h4V6h-4ZM15,2.5v6l-.5.5H10V8h4V3H7V7H6V2.5L6.5,2h8Z", "fill-[#005dba]")
        }, "opacity-75"),
        new PathInfo("M9.5,7h-8L1,7.5v6l.5.5h8l.5-.5v-6Z", "fill-[#005dba]"),
        new PathInfo("M8,11H3V10H8Z", "fill-[#ffffff]")
    });

    public static IconInfo Delegate = new IconInfo("0 0 16 16", new ISvgChild[]
    {
        new PathInfo("M16,16H0V0H16Z", "fill-none opacity-0"),
        new PathInfo("M14.5,4.5v9H1.5v-9Z", "fill-[#6936aa] opacity-10"),
        new PathInfo("M14.5,4H1.5L1,4.5v9l.5.5h13l.5-.5v-9ZM12,5v8H4V5ZM2,5H3v8H2Zm12,8H13V5h1Z", "fill-[#6936aa]"),
        new PathInfo("M11,2.5V4H10V3H6V4H5V2.5L5.5,2h5Z", "fill-[#212121]")
    });

    public static IconInfo Interface = new IconInfo("0 0 16 16", new ISvgChild[]
    {
        new PathInfo("M16,16H0V0H16Z", "fill-none opacity-0"),
        new GInfo(new[]
        {
            new PathInfo("M8.5,7V8h-4V7Z", "fill-[#005dba]")
        }, "opacity-75"),
        new PathInfo("M4.5,7.5a2,2,0,1,1-2-2A2,2,0,0,1,4.5,7.5Zm10,0a3,3,0,1,1-3-3A3,3,0,0,1,14.5,7.5Z",
            "fill-[#005dba] opacity-10"),
        new PathInfo(
            "M2.5,5A2.5,2.5,0,1,0,5,7.5,2.5,2.5,0,0,0,2.5,5Zm0,4A1.5,1.5,0,1,1,4,7.5,1.5,1.5,0,0,1,2.5,9Zm9-5A3.5,3.5,0,1,0,15,7.5,3.5,3.5,0,0,0,11.5,4Zm0,6A2.5,2.5,0,1,1,14,7.5,2.5,2.5,0,0,1,11.5,10Z",
            "fill-[#005dba]")
    });

    public static IconInfo AssemblyReference = new IconInfo("0 0 16 16", new ISvgChild[]
    {
        new PathInfo("M16,16H0V0H16Z", "fill-none opacity-0"),
        new GInfo(new[]
        {
            new PathInfo("M9,7V8H5V7Z", "fill-[#212121] opacity-75")
        }, "opacity-75"),
        new PathInfo("M4.5,5.5v4H.5v-4Z", "fill-[#212121] opacity-10"),
        new PathInfo("M4.5,10H.5L0,9.5v-4L.5,5h4l.5.5v4ZM1,9H4V6H1Z", "fill-[#212121]"),
        new PathInfo("M15.5,4.5v6h-6v-6Z", "fill-[#212121] opacity-10"),
        new PathInfo("M15.5,11h-6L9,10.5v-6L9.5,4h6l.5.5v6ZM10,10h5V5H10Z", "fill-[#212121]")
    });

    public static IconInfo Namespace = new IconInfo("0 0 16 16", new ISvgChild[]
    {
        new PathInfo("M16,16H0V0H16Z", "fill-none opacity-0"),
        new PathInfo(
            "M4.9,2,5,2v.9l-.092,0a.94.94,0,0,0-.745.332,1.784,1.784,0,0,0-.229,1.025V6.23a1.741,1.741,0,0,1-.888,1.786c.57.2.888.819.888,1.8v1.89a3.406,3.406,0,0,0,.053.646,1.071,1.071,0,0,0,.164.417.7.7,0,0,0,.29.24A1.246,1.246,0,0,0,4.9,13.1l.1,0V14l-.1,0a2.079,2.079,0,0,1-1.478-.538,2.187,2.187,0,0,1-.5-1.577v-2a1.948,1.948,0,0,0-.249-1.079A.914.914,0,0,0,1.963,8.4L1.875,8.39V7.61L1.962,7.6c.645-.07.959-.541.959-1.438V4.132a2.206,2.206,0,0,1,.5-1.586A2.062,2.062,0,0,1,4.9,2ZM14.03,7.6c-.64-.063-.951-.533-.951-1.438V4.132a2.2,2.2,0,0,0-.5-1.591A2.069,2.069,0,0,0,11.1,2L11,2v.9l.093,0A1.357,1.357,0,0,1,11.531,3a.693.693,0,0,1,.29.218A1.174,1.174,0,0,1,12,3.622a2.779,2.779,0,0,1,.064.644V6.23c0,.95.319,1.559.892,1.754a1.8,1.8,0,0,0-.892,1.833v1.89a3.181,3.181,0,0,1-.061.675,1.07,1.07,0,0,1-.178.416.671.671,0,0,1-.29.219,1.291,1.291,0,0,1-.442.081l-.1,0V14l.1,0a2.069,2.069,0,0,0,1.477-.538,2.184,2.184,0,0,0,.5-1.577v-2c0-.929.311-1.415.952-1.485l.087-.009V7.609Z",
            "fill-[#212121]")
    });

    public static IconInfo Method = new IconInfo("0 0 16 16", new ISvgChild[]
    {
        new PathInfo("M16,16H0V0H16Z", "fill-none opacity-0"),
        new GInfo(new[]
        {
            new PathInfo("M13.5,5.045v7L8,14.545l-5.5-2.5v-7l5.5-3Z", "fill-[#6936aa] opacity-10"),
            new PathInfo(
                "M14,5.045v7l-.293.455L8.207,15H7.793l-5.5-2.5L2,12.045v-7l.261-.439.032.894L3,5.821v5.9l4.5,2.045,0-5.9.3.135h.414l.288-.131,0,5.9L13,11.723v-5.9l.707-.321.032-.894Z",
                "fill-[#6936aa]"),
        }, "opacity-75"),
        new PathInfo("M13.5,5.045,8,7.545l-5.5-2.5,5.5-3Z", "fill-[#6936aa] opacity-10"),
        new PathInfo("M8.239,1.606H7.761l-5.5,3,.032.894L7.793,8h.414l5.5-2.5.032-.894ZM8,7,3.619,5,8,2.614,12.381,5Z",
            "fill-[#6936aa]"),
    });

    public static IconInfo Property = new IconInfo("0 0 16 16", new ISvgChild[]
    {
        new PathInfo("M16,16H0V0H16Z", "fill-none opacity-0"),
        new GInfo(new[]
        {
            new PathInfo(
                "M10.242,7.167c-1.722,1.75-5.418,5.45-7.191,7.1-.861.75-2.076-.651-1.418-1.4,1.671-1.75,5.418-5.449,7.14-7.1a1.078,1.078,0,0,1,.659-.25A1.026,1.026,0,0,1,10.242,7.167Z",
                "fill-[#212121] opacity-10"),
            new PathInfo(
                "M10.828,5.938a1.508,1.508,0,0,0-1.4-.921,1.628,1.628,0,0,0-1.005.388c-1.714,1.643-5.473,5.355-7.17,7.131a1.444,1.444,0,0,0,.187,1.973,1.5,1.5,0,0,0,1.056.47,1.352,1.352,0,0,0,.892-.346C5.254,12.9,9.051,9.09,10.61,7.506A1.45,1.45,0,0,0,10.828,5.938Zm-.942.878C8.344,8.384,4.561,12.178,2.723,13.89a.4.4,0,0,1-.571-.087C2,13.653,1.86,13.365,2,13.212c1.673-1.753,5.417-5.448,7.076-7.044a.575.575,0,0,1,.361-.151.513.513,0,0,1,.48.322A.465.465,0,0,1,9.886,6.816Z",
                "fill-[#212121]"),
        }, "opacity-75"),
        new PathInfo(
            "M14.974,5a4,4,0,0,1-8,0,3.961,3.961,0,0,1,4-4,4.19,4.19,0,0,1,1.441.241L9.534,4.2l2.24,2.241,2.96-2.882A4.19,4.19,0,0,1,14.974,5Z",
            "fill-[#212121]"),
    });

    public static IconInfo Event = new IconInfo("0 0 16 16", new ISvgChild[]
    {
        new PathInfo("M16,16H0V0H16Z", "fill-none opacity-0"),
        new PathInfo("M12.5,6.5l-8,8H3l3.5-6H3l4-7h5.387L8,6.5Z", "fill-[#996f00] opacity-25"),
        new PathInfo(
            "M12.5,6H9.1l3.659-4.17L12.387,1H7l-.434.252-4,7L3,9H5.629L2.568,14.248,3,15H4.5l.354-.146,8-8ZM4.293,14H3.871L6.932,8.752,6.5,8H3.862L7.29,2h3.993L7.624,6.17,8,7h3.293Z",
            "fill-[#996f00]"),
    });

    public static IconInfo EnumItem = new IconInfo("0 0 16 16", new ISvgChild[]
    {
        new PathInfo("M16,16H0V0H16Z", "fill-none opacity-0"),
        new GInfo(new[]
        {
            new PathInfo("M14.5,2.5v6H9V7H6.5V2.5Z", "fill-[#005dba] opacity-10"),
            new PathInfo("M12.5,4V5h-4V4Zm-4,3h4V6h-4ZM15,2.5v6l-.5.5H10V8h4V3H7V7H6V2.5L6.5,2h8Z", "fill-[#005dba]"),
        }, "opacity-75"),
        new PathInfo("M9.5,7h-8L1,7.5v6l.5.5h8l.5-.5v-6Z", "fill-[#005dba]"),
        new PathInfo("M8,11H3V10H8Z", "fill-[#ffffff]"),
    });

    public static IconInfo Field = new IconInfo("0 0 16 16", new ISvgChild[]
    {
        new PathInfo("M16,16H0V0H16Z", "fill-none opacity-0"),
        new GInfo(new[]
        {
            new PathInfo("M14.5,5.5V10l-9,5-4-4V6.5l9-5Z", "fill-[#005dba] opacity-10"),
            new PathInfo(
                "M14.854,5.146l-4-4-.6-.083-9,5L1,6.5V11l.146.354,4,4,.6.083,9-5L15,10V5.5ZM14,9.706,6,14.15V10.5H5v3.293l-3-3v-4L10.413,2.12,14,5.707Z",
                "fill-[#005dba]"),
        }, "opacity-0.75"),
        new PathInfo("M14.5,5.5l-9,5-4-4,9-5Z", "fill-[#005dba] opacity-10"),
        new PathInfo(
            "M10.854,1.146l-.6-.083-9,5-.111.791,4,4,.6.083,9-5,.111-.791ZM5.587,9.88,2.322,6.615,10.413,2.12l3.265,3.265Z",
            "fill-[#005dba]"),
    });
    
    public static IconInfo Constant = new IconInfo("0 0 16 16", new ISvgChild[]
    {
        new PathInfo("M16,16H0V0H16Z", "fill-none opacity-0"),
        new PathInfo("M3,13V4h9v9H3Z", "fill-[#212121] opacity-10"),
        new PathInfo("M12.5,3H2.5L2,3.5v10l.5.5h10l.5-.5V3.5ZM12,13H3V4h9Z", "fill-[#212121] opacity-100"),
        new PathInfo("M9.991,8h-5V7h5Zm0,1h-5v1h5Z", "fill-[#005dba] opacity-100")
    });


    public record struct IconInfo(string ViewBox, ISvgChild[] PathInfos);

    public interface ISvgChild
    {
    }

    public record struct PathInfo(string Data, string? Class = null) : ISvgChild;

    public record struct GInfo(PathInfo[] PathInfos, string? Class = null) : ISvgChild; // :/
}