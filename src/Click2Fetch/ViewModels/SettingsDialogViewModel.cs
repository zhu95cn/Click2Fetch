using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Avalonia.Styling;
using Click2Fetch.Services;
using System;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Click2Fetch.ViewModels;

public partial class SettingsDialogViewModel : ViewModelBase
{
    [ObservableProperty]
    private bool _isLightTheme;

    public ThemeVariant ActualThemeVariant => IsLightTheme ? ThemeVariant.Light : ThemeVariant.Dark;

    [ObservableProperty]
    private bool _isEnglish;

    // Localized strings
    public string L_SettingsTitle => IsEnglish ? "Settings" : "设置";
    public string L_TabPassword => IsEnglish ? "Login Password" : "登录密码";
    public string L_TabBackup => IsEnglish ? "Backup Settings" : "备份设置";
    public string L_TabExport => IsEnglish ? "Import/Export" : "导入导出";
    public string L_ChangePwdTitle => IsEnglish ? "Modify Login Password" : "修改登录密码";
    public string L_AutoBackupTitle => IsEnglish ? "Auto Backup Settings" : "自动备份设置";
    public string L_DataExportTitle => IsEnglish ? "Data Import/Export" : "数据导入导出";
    public string L_Developing => IsEnglish ? "Feature in development..." : "功能开发中...";
    public string L_Close => IsEnglish ? "Close" : "关闭";
    public string L_OldPassword => IsEnglish ? "Old Password" : "旧密码";
    public string L_NewPassword => IsEnglish ? "New Password" : "新密码";
    public string L_ConfirmNewPassword => IsEnglish ? "Confirm New Password" : "确认新密码";
    public string L_UpdatePassword => IsEnglish ? "Update Password" : "确定";
    public string L_PasswordSuccess => IsEnglish ? "Password changed successfully" : "密码修改成功";

    // Email backup localized strings
    public string L_SmtpServer => IsEnglish ? "SMTP Server" : "SMTP 服务器";
    public string L_SmtpPort => IsEnglish ? "Port" : "端口";
    public string L_SmtpEmail => IsEnglish ? "Send Email" : "发送邮箱";
    public string L_SmtpPassword => IsEnglish ? "Authorization Code" : "授权码";
    public string L_BackupTargetEmail => IsEnglish ? "Target Email" : "目标邮箱";
    public string L_SendBackup => IsEnglish ? "Send Backup" : "发送备份";
    public string L_SaveConfig => IsEnglish ? "Save Config" : "保存配置";
    public string L_BackupSuccess => IsEnglish ? "Backup sent successfully" : "备份发送成功";

    [ObservableProperty]
    private bool _isConfirmed;

    [ObservableProperty]
    private int _selectedTabIndex;

    [ObservableProperty]
    private string _currentPassword = "";

    [ObservableProperty]
    private string _newPassword = "";

    [ObservableProperty]
    private string _confirmNewPassword = "";

    [ObservableProperty]
    private string _passwordErrorMessage = "";

    [ObservableProperty]
    private bool _passwordSuccess;

    // Email backup properties
    [ObservableProperty]
    private string _smtpServer = "";

    [ObservableProperty]
    private string _smtpPort = "587";

    [ObservableProperty]
    private string _smtpEmail = "";

    [ObservableProperty]
    private string _smtpPassword = "";

    [ObservableProperty]
    private string _backupTargetEmail = "";

    [ObservableProperty]
    private string _backupMessage = "";

    [ObservableProperty]
    private bool _backupSuccess;

    [ObservableProperty]
    private bool _isSendingBackup;

    private readonly IDatabaseService _databaseService;
    private readonly IEncryptionService _encryptionService;

    public SettingsDialogViewModel(IDatabaseService databaseService, IEncryptionService encryptionService)
    {
        _databaseService = databaseService;
        _encryptionService = encryptionService;
        _ = LoadEmailSettingsAsync();
    }

    private async Task LoadEmailSettingsAsync()
    {
        try
        {
            var settings = await _databaseService.GetSettingsAsync();
            SmtpServer = settings.SmtpServer ?? "";
            SmtpPort = settings.SmtpPort.ToString();
            SmtpEmail = settings.SmtpEmail ?? "";
            SmtpPassword = settings.SmtpPassword ?? "";
            BackupTargetEmail = settings.BackupTargetEmail ?? "";
        }
        catch { }
    }

    // Theme colors for dialog
    public IBrush DialogBg => Brush.Parse(IsLightTheme ? "#F5F5F7" : "#0A0A0C");
    public IBrush DialogCardBg => Brush.Parse(IsLightTheme ? "#FFFFFF" : "#16161A");
    public IBrush DialogBorder => Brush.Parse(IsLightTheme ? "#E5E7EB" : "#252530");
    public IBrush DialogTextPrimary => Brush.Parse(IsLightTheme ? "#1F2937" : "#F9FAFB");
    public IBrush DialogTextSecondary => Brush.Parse(IsLightTheme ? "#6B7280" : "#9CA3AF");
    public IBrush DialogInputBg => Brush.Parse(IsLightTheme ? "#FFFFFF" : "#1C1C22");
    public IBrush DialogCaretBrush => Brush.Parse(IsLightTheme ? "#1F2937" : "#F9FAFB");
    public IBrush DialogButtonBg => Brush.Parse(IsLightTheme ? "#3B82F6" : "#8B5CF6");
    public IBrush DialogSecondaryButtonBg => Brush.Parse(IsLightTheme ? "#FFFFFF" : "#1C1C22");
    public IBrush DialogSecondaryButtonBorder => Brush.Parse(IsLightTheme ? "#D1D5DB" : "#2D2D38");
    public IBrush DialogTabBg => Brush.Parse(IsLightTheme ? "#F3F4F6" : "#1C1C22");
    public IBrush DialogTabSelectedBg => Brush.Parse(IsLightTheme ? "#FFFFFF" : "#252530");

    partial void OnIsLightThemeChanged(bool value)
    {
        OnPropertyChanged(nameof(DialogBg));
        OnPropertyChanged(nameof(DialogCardBg));
        OnPropertyChanged(nameof(DialogBorder));
        OnPropertyChanged(nameof(DialogTextPrimary));
        OnPropertyChanged(nameof(DialogTextSecondary));
        OnPropertyChanged(nameof(DialogInputBg));
        OnPropertyChanged(nameof(DialogCaretBrush));
        OnPropertyChanged(nameof(DialogButtonBg));
        OnPropertyChanged(nameof(DialogSecondaryButtonBg));
        OnPropertyChanged(nameof(DialogSecondaryButtonBorder));
        OnPropertyChanged(nameof(DialogTabBg));
        OnPropertyChanged(nameof(DialogTabSelectedBg));
        OnPropertyChanged(nameof(ActualThemeVariant));
    }

    partial void OnIsEnglishChanged(bool value)
    {
        OnPropertyChanged(nameof(L_SettingsTitle));
        OnPropertyChanged(nameof(L_TabPassword));
        OnPropertyChanged(nameof(L_TabBackup));
        OnPropertyChanged(nameof(L_TabExport));
        OnPropertyChanged(nameof(L_ChangePwdTitle));
        OnPropertyChanged(nameof(L_AutoBackupTitle));
        OnPropertyChanged(nameof(L_DataExportTitle));
        OnPropertyChanged(nameof(L_Developing));
        OnPropertyChanged(nameof(L_Close));
        OnPropertyChanged(nameof(L_OldPassword));
        OnPropertyChanged(nameof(L_NewPassword));
        OnPropertyChanged(nameof(L_ConfirmNewPassword));
        OnPropertyChanged(nameof(L_UpdatePassword));
        OnPropertyChanged(nameof(L_PasswordSuccess));
        OnPropertyChanged(nameof(L_SmtpServer));
        OnPropertyChanged(nameof(L_SmtpPort));
        OnPropertyChanged(nameof(L_SmtpEmail));
        OnPropertyChanged(nameof(L_SmtpPassword));
        OnPropertyChanged(nameof(L_BackupTargetEmail));
        OnPropertyChanged(nameof(L_SendBackup));
        OnPropertyChanged(nameof(L_SaveConfig));
        OnPropertyChanged(nameof(L_BackupSuccess));
    }

    public Action? CloseAction { get; set; }

    [RelayCommand]
    private void SelectTab(string index)
    {
        if (int.TryParse(index, out int tabIndex))
        {
            SelectedTabIndex = tabIndex;
        }
    }

    [RelayCommand]
    private async Task ChangePasswordAsync()
    {
        PasswordErrorMessage = "";
        PasswordSuccess = false;

        if (string.IsNullOrEmpty(CurrentPassword) || string.IsNullOrEmpty(NewPassword) || string.IsNullOrEmpty(ConfirmNewPassword))
        {
            PasswordErrorMessage = IsEnglish ? "Please fill in all fields" : "请填写所有字段";
            return;
        }

        if (NewPassword != ConfirmNewPassword)
        {
            PasswordErrorMessage = IsEnglish ? "New passwords do not match" : "新密码不匹配";
            return;
        }

        if (NewPassword.Length < 8)
        {
            PasswordErrorMessage = IsEnglish ? "Password must be at least 8 characters" : "密码至少需要8个字符";
            return;
        }

        try
        {
            var settings = await _databaseService.GetSettingsAsync();
            var currentSalt = Convert.FromBase64String(settings.PasswordSalt ?? "");
            if (!_encryptionService.VerifyPassword(CurrentPassword, currentSalt, settings.PasswordHash ?? ""))
            {
                PasswordErrorMessage = IsEnglish ? "Incorrect current password" : "当前密码错误";
                return;
            }

            // Update password hash
            var newSalt = _encryptionService.GenerateSalt();
            var newHash = _encryptionService.HashPassword(NewPassword, newSalt);
            
            settings.PasswordHash = newHash;
            settings.PasswordSalt = Convert.ToBase64String(newSalt);
            
            await _databaseService.SaveSettingsAsync(settings);
            
            // Re-derive session key with new password
            var newSessionKey = _encryptionService.DeriveKey(NewPassword, newSalt);
            _encryptionService.SetSessionKey(newSessionKey);

            PasswordSuccess = true;
            CurrentPassword = "";
            NewPassword = "";
            ConfirmNewPassword = "";
        }
        catch (Exception ex)
        {
            PasswordErrorMessage = $"Error: {ex.Message}";
        }
    }

    [RelayCommand]
    private async Task SaveEmailConfigAsync()
    {
        BackupMessage = "";
        BackupSuccess = false;

        try
        {
            var settings = await _databaseService.GetSettingsAsync();
            settings.SmtpServer = SmtpServer;
            settings.SmtpPort = int.TryParse(SmtpPort, out int port) ? port : 587;
            settings.SmtpEmail = SmtpEmail;
            settings.SmtpPassword = SmtpPassword;
            settings.BackupTargetEmail = BackupTargetEmail;
            await _databaseService.SaveSettingsAsync(settings);
            
            BackupMessage = IsEnglish ? "Configuration saved" : "配置已保存";
            BackupSuccess = true;
        }
        catch (Exception ex)
        {
            BackupMessage = $"Error: {ex.Message}";
        }
    }

    [RelayCommand]
    private async Task SendBackupAsync()
    {
        BackupMessage = "";
        BackupSuccess = false;

        if (string.IsNullOrEmpty(SmtpServer) || string.IsNullOrEmpty(SmtpEmail) || 
            string.IsNullOrEmpty(SmtpPassword) || string.IsNullOrEmpty(BackupTargetEmail))
        {
            BackupMessage = IsEnglish ? "Please fill in all email settings" : "请填写所有邮箱配置";
            return;
        }

        IsSendingBackup = true;
        try
        {
            // Get all entries for backup
            var entries = await _databaseService.GetAllEntriesAsync();
            var backupData = new StringBuilder();
            backupData.AppendLine("Click2Fetch Backup");
            backupData.AppendLine($"Date: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            backupData.AppendLine($"Total Entries: {entries.Count}");
            backupData.AppendLine(new string('=', 50));
            
            foreach (var entry in entries)
            {
                backupData.AppendLine($"\nTitle: {entry.Title}");
                backupData.AppendLine($"Notes: {entry.Notes ?? ""}");
                backupData.AppendLine($"Created: {entry.CreatedAt}");
                backupData.AppendLine($"Modified: {entry.LastModified}");
                if (entry.Attributes != null)
                {
                    backupData.AppendLine("Attributes:");
                    foreach (var attr in entry.Attributes)
                    {
                        backupData.AppendLine($"  - {attr.Key}: {(attr.IsSensitive ? "[ENCRYPTED]" : attr.Value)}");
                    }
                }
                backupData.AppendLine(new string('-', 30));
            }

            // Send email
            var port = int.TryParse(SmtpPort, out int p) ? p : 587;
            using var client = new SmtpClient(SmtpServer, port)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(SmtpEmail, SmtpPassword)
            };

            var message = new MailMessage(SmtpEmail, BackupTargetEmail)
            {
                Subject = $"Click2Fetch Backup - {DateTime.Now:yyyy-MM-dd}",
                Body = backupData.ToString(),
                IsBodyHtml = false
            };

            await client.SendMailAsync(message);
            
            BackupMessage = IsEnglish ? "Backup sent successfully" : "备份发送成功";
            BackupSuccess = true;
        }
        catch (Exception ex)
        {
            BackupMessage = $"Error: {ex.Message}";
        }
        finally
        {
            IsSendingBackup = false;
        }
    }

    [RelayCommand]
    private void Confirm()
    {
        IsConfirmed = true;
        CloseAction?.Invoke();
    }

    [RelayCommand]
    private void Cancel()
    {
        IsConfirmed = false;
        CloseAction?.Invoke();
    }
}
