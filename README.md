# Click2Fetch

> **The missing control panel for your digital assets.**
>
> **你的数字资产控制台：不再翻找，一触即达。**

![Platform](https://img.shields.io/badge/platform-Windows-0078D6?style=flat-square)
![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?style=flat-square)
![License](https://img.shields.io/badge/license-MIT-green?style=flat-square)
![Build](https://img.shields.io/badge/build-passing-brightgreen?style=flat-square)

[English](#-introduction) | [中文介绍](#-中文介绍)

---

## 📖 Introduction

**Click2Fetch** is not just another password manager; it is an **Operations Panel** designed for Developers, DevOps, and Geeks.

We all have to manage dozens of login details: URLs, Usernames, Passwords, API Keys, Database Connection Strings, and Server IPs. Storing them in Excel is insecure, and browser password managers can't handle non-web attributes well.

**Click2Fetch** turns your data into a **Control Board**. Every attribute becomes a button. **Click to copy, click to navigate.** No server required, your data is yours.

---

## 🇨🇳 中文介绍

**Click2Fetch** 不仅仅是一个密码管理器，它是专为开发者、运维人员和极客打造的**“信息控制台”**。

我们每天都要处理大量的登录信息：网址、账号、密码、API Key、数据库连接串、服务器 IP 等等。记在 Excel 里不安全，浏览器的自动填充又搞不定非网页类的场景。

**Click2Fetch** 将你的这些信息转化为一个**可视化操作面板**。每一个属性都是一个按钮。**点击即复制，点击即跳转**。不需要任何第三方服务器，数据完全由你掌控。

---

## ✨ Key Features | 核心亮点

### ⚡ Panel-Based Interaction | 面板式交互
Forget the boring lists. Customize attributes for each entry (e.g., `RedisAuth`, `Intranet IP`, `Admin URL`). They are automatically rendered as action buttons.
- **One-Click Fetch:** Click any button to fetch its value to the clipboard immediately.
- **Auto Navigation:** If the value is a URL, clicking it opens your default browser.

告别枯燥的列表。为每个条目自定义属性（如：`Redis密码`、`内网IP`、`后台地址`），它们会自动渲染为**操作按钮**。
- **一键获取：** 点击按钮，内容直达剪贴板。
- **自动导航：** 如果是 URL，点击直接唤起默认浏览器。

### ☁️ Serverless Email Sync | 无服务器邮箱同步
**The coolest feature.** No need to trust a third-party cloud or host a complex server.
- Uses **your own Email** (via SMTP/IMAP) as secure cloud storage.
- Data is encrypted locally, packed as an attachment, and sent to yourself.
- **Version Control:** Your email history is your data backup history.

**这是本项目最酷的功能。** 不需要信任第三方云服务，也不需要自己搭建服务器。
- 利用**你自己的邮箱**作为云存储。
- 数据变动时，自动打包加密并通过 SMTP 发送给自己。
- **天然的版本控制**：邮箱的历史邮件就是你数据的历史快照，误删随时找回！

### 🔒 Bank-Grade Security | 银行级安全
- **AES-256 Encryption:** Your data is encrypted locally using AES-256.
- **Argon2id Hashing:** Master password is protected against brute-force attacks.
- **Clipboard Clearing:** Clipboard is automatically cleared after 30 seconds.
- **Auto-Lock:** App locks itself after N minutes of inactivity.

- **AES-256 加密：** 数据在本地经过高强度加密存储。
- **Argon2id 哈希：** 抵御暴力破解。
- **剪贴板焚毁：** 复制的内容在 30 秒后自动清除，防止历史记录泄露。
- **自动锁定：** 无操作 N 分钟后自动锁定界面。

---

## 📸 Screenshots | 截图预览

<!-- Place your screenshots in a 'docs/images' folder or upload to issue to get a url -->
<!-- ![Dashboard](docs/images/dashboard-preview.png) -->


<img width="2404" height="1660" alt="image" src="https://github.com/user-attachments/assets/c49c25fc-f9b7-4614-a54c-94d3b6f40d94" />
<img width="2404" height="1660" alt="image" src="https://github.com/user-attachments/assets/6c88135f-0e3e-4f07-a5df-dee30abacbf8" />

---

## 🛠️ Getting Started | 快速开始

### Installation (安装)
1. Go to the [Releases](../../releases) page.
2. Download the latest `.exe` file (Single-file, no installation needed).
3. Run it and set your **Master Password**.

1. 前往 [Releases](../../releases) 页面。
2. 下载最新的 `.exe` 文件（单文件绿色版，无需安装）。
3. 运行并设置你的 **主密码**。

### Configuration (配置)
To enable cloud sync:
1. Go to **Settings** -> **Sync**.
2. Enter your Email and SMTP/IMAP details.
3. **Note:** Use an **App Password** provided by your email provider, not your login password.

开启云同步：
1. 进入 **设置** -> **同步**。
2. 填写你的邮箱地址和 SMTP/IMAP 信息。
3. **注意：** 密码处请填写邮箱服务商提供的 **App Password (应用专用密码)**，而非你的邮箱登录密码。

---

## 🗺️ Roadmap | 路线图

- [x] Panel-style Attribute Buttons (面板式属性按钮)
- [x] AES-256 Local Encryption (AES-256 本地加密)
- [x] Clipboard Auto-Clear (剪贴板自动清除)
- [x] **Email Backup & Sync** (邮箱备份与同步)
- [ ] Fuzzy Search & Tags (模糊搜索与标签)
- [ ] TOTP Support (2FA 动态验证码集成)
- [ ] **Enterprise Edition**: Self-hosted Docker server for teams (企业版：支持团队协作的 Docker 服务端)

---

## 🤝 Contributing | 贡献

Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

欢迎提交 PR。如果你觉得这个工具提高了你的效率，请点亮右上角的 ⭐️ **Star**！

---

## 📜 License

[MIT](LICENSE) © 2026 Click2Fetch Team

**Disclaimer:** This software is provided "as is", without warranty of any kind. Please keep your Master Password safe; lost passwords cannot be recovered.

**免责声明：** 本软件按“原样”提供。请务必保管好您的主密码，一旦丢失将无法找回数据。