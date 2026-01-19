using System.Globalization;
using Avalonia.Data.Converters;
using Click2Fetch.Models;

namespace Click2Fetch.Converters;

/// <summary>
/// Converts AttributeType to icon emoji
/// </summary>
public class TypeToIconConverter : IValueConverter
{
    public static readonly TypeToIconConverter Instance = new();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is AttributeType type)
        {
            return type switch
            {
                AttributeType.Username => "👤",
                AttributeType.Password => "🔑",
                AttributeType.Url => "🔗",
                AttributeType.ApiKey => "🗝️",
                AttributeType.Email => "📧",
                AttributeType.Port => "🔌",
                AttributeType.Host => "🖥️",
                AttributeType.Application => "💻",
                AttributeType.Text => "📝",
                _ => "📋"
            };
        }
        return "📋";
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// Converts AttributeType to action text
/// </summary>
public class TypeToActionConverter : IValueConverter
{
    public static readonly TypeToActionConverter Instance = new();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is AttributeType type)
        {
            return type switch
            {
                AttributeType.Url => "Click to open in browser",
                _ => "Click to copy"
            };
        }
        return "点击复制";
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// Converts AttributeType to boolean (true if URL)
/// </summary>
public class TypeToUrlConverter : IValueConverter
{
    public static readonly TypeToUrlConverter Instance = new();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value is AttributeType type && type == AttributeType.Url;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// Converts visibility state to icon
/// </summary>
public class VisibilityIconConverter : IValueConverter
{
    public static readonly VisibilityIconConverter Instance = new();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool isVisible)
        {
            return isVisible ? "👁️" : "👁️‍🗨️";
        }
        return "👁️‍🗨️";
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// Converts first time setup state to button text
/// </summary>
public class SetupButtonTextConverter : IValueConverter
{
    public static readonly SetupButtonTextConverter Instance = new();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool isFirstTimeSetup)
        {
            return isFirstTimeSetup ? "创建并解锁" : "解锁";
        }
        return "解锁";
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// Converts copied state to text
/// </summary>
public class CopiedTextConverter : IValueConverter
{
    public static readonly CopiedTextConverter Instance = new();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool isCopied)
        {
            return isCopied ? "✓ 已复制" : "点击复制";
        }
        return "点击复制";
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// Converts copied state to color
/// </summary>
public class CopiedColorConverter : IValueConverter
{
    public static readonly CopiedColorConverter Instance = new();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool isCopied)
        {
            return isCopied ? Avalonia.Media.Brush.Parse("#10B981") : Avalonia.Media.Brush.Parse("#6B7280");
        }
        return Avalonia.Media.Brush.Parse("#6B7280");
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// Converts language state to display text
/// </summary>
public class LangToggleConverter : IValueConverter
{
    public static readonly LangToggleConverter Instance = new();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool isEnglish)
        {
            return isEnglish ? "中文" : "EN";
        }
        return "EN";
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// Converts language state to column index (0=EN, 1=中文)
/// </summary>
public class LangColumnConverter : IValueConverter
{
    public static readonly LangColumnConverter Instance = new();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool isEnglish)
        {
            return isEnglish ? 0 : 1;
        }
        return 1;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// Converts language state to active text
/// </summary>
public class LangTextConverter : IValueConverter
{
    public static readonly LangTextConverter Instance = new();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool isEnglish)
        {
            return isEnglish ? "EN" : "中文";
        }
        return "中文";
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// Converts theme state to name
/// </summary>
public class ThemeNameConverter : IValueConverter
{
    public static readonly ThemeNameConverter Instance = new();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool isLight)
        {
            return isLight ? "浅色主题" : "深色主题";
        }
        return "深色主题";
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// Theme color converters
/// </summary>
public class ThemeBgConverter : IValueConverter
{
    public static readonly ThemeBgConverter Instance = new();
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        bool isLight = value is bool b && b;
        return Avalonia.Media.Brush.Parse(isLight ? "#F5F5F7" : "#0A0A0C");
    }
    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotImplementedException();
}

public class ThemeSidebarBgConverter : IValueConverter
{
    public static readonly ThemeSidebarBgConverter Instance = new();
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        bool isLight = value is bool b && b;
        return Avalonia.Media.Brush.Parse(isLight ? "#FFFFFF" : "#0D0D10");
    }
    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotImplementedException();
}

public class ThemeCardBgConverter : IValueConverter
{
    public static readonly ThemeCardBgConverter Instance = new();
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        bool isLight = value is bool b && b;
        return Avalonia.Media.Brush.Parse(isLight ? "#FFFFFF" : "#16161A");
    }
    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotImplementedException();
}

public class ThemeBorderConverter : IValueConverter
{
    public static readonly ThemeBorderConverter Instance = new();
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        bool isLight = value is bool b && b;
        return Avalonia.Media.Brush.Parse(isLight ? "#E5E7EB" : "#252530");
    }
    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotImplementedException();
}

public class ThemeTextPrimaryConverter : IValueConverter
{
    public static readonly ThemeTextPrimaryConverter Instance = new();
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        bool isLight = value is bool b && b;
        return Avalonia.Media.Brush.Parse(isLight ? "#1F2937" : "#F9FAFB");
    }
    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotImplementedException();
}

public class ThemeTextSecondaryConverter : IValueConverter
{
    public static readonly ThemeTextSecondaryConverter Instance = new();
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        bool isLight = value is bool b && b;
        return Avalonia.Media.Brush.Parse(isLight ? "#6B7280" : "#9CA3AF");
    }
    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotImplementedException();
}

public class ThemeAccentConverter : IValueConverter
{
    public static readonly ThemeAccentConverter Instance = new();
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        bool isLight = value is bool b && b;
        return Avalonia.Media.Brush.Parse(isLight ? "#3B82F6" : "#8B5CF6");
    }
    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotImplementedException();
}

public class ThemeInputBgConverter : IValueConverter
{
    public static readonly ThemeInputBgConverter Instance = new();
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        bool isLight = value is bool b && b;
        return Avalonia.Media.Brush.Parse(isLight ? "#FFFFFF" : "#1C1C22");
    }
    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotImplementedException();
}

public class ThemeSelectedItemBgConverter : IValueConverter
{
    public static readonly ThemeSelectedItemBgConverter Instance = new();
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        bool isLight = value is bool b && b;
        return Avalonia.Media.Brush.Parse(isLight ? "#EBF5FF" : "#1C1C24");
    }
    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotImplementedException();
}

/// <summary>
/// Converts bool to border brush for theme selection
/// </summary>
public class ThemeSelectedBorderConverter : IValueConverter
{
    public static readonly ThemeSelectedBorderConverter Instance = new();
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        bool isSelected = value is bool b && b;
        string color = parameter as string ?? "#8B5CF6";
        return Avalonia.Media.Brush.Parse(isSelected ? color : "#2D2D38");
    }
    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotImplementedException();
}

/// <summary>
/// Converts theme state to column index (0=Light, 1=Dark)
/// </summary>
public class ThemeColumnConverter : IValueConverter
{
    public static readonly ThemeColumnConverter Instance = new();
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool isLight)
        {
            return isLight ? 0 : 1;
        }
        return 1;
    }
    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotImplementedException();
}

/// <summary>
/// Converts theme state to icon
/// </summary>
public class ThemeIconConverter : IValueConverter
{
    public static readonly ThemeIconConverter Instance = new();
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool isLight)
        {
            return isLight ? "☀️" : "🌙";
        }
        return "🌙";
    }
    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotImplementedException();
}

/// <summary>
/// Converts theme state to short text
/// </summary>
public class ThemeTextShortConverter : IValueConverter
{
    public static readonly ThemeTextShortConverter Instance = new();
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool isLight)
        {
            return isLight ? "浅" : "深";
        }
        return "深";
    }
    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotImplementedException();
}

/// <summary>
/// Converts int to bool by comparing with parameter
/// </summary>
public class IntEqualConverter : IValueConverter
{
    public static readonly IntEqualConverter Instance = new();
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is int intValue && parameter is string paramStr && int.TryParse(paramStr, out int param))
        {
            return intValue == param;
        }
        return false;
    }
    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotImplementedException();
}
