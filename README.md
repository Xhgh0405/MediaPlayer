# HW5_MediaPlayer

## 專案簡介

`HW5_MediaPlayer` 是一個使用 **C# Windows Forms** 製作的多媒體播放器。程式使用 Windows Media Player ActiveX 控制項播放影片，支援瀏覽檔案、播放、暫停、停止、播放倍速與循環播放功能。

本版本已內建老師提供的 `Dog.wmv`，程式啟動後會自動載入預設影片並停在開頭，按下「播放」即可開始播放。

---

## 功能特色

- 可播放 WMV、MP4、AVI 等影片檔
- 內建預設影片 `sample_media/Dog.wmv`
- 支援播放、暫停、停止
- 支援播放倍速：0.5x、1.0x、1.5x、2.0x
- 支援循環播放
- 使用 `AxWindowsMediaPlayer` 作為影片播放元件
- 已加入循環播放補強機制，影片結束後會自動回到開頭重播

---

## 開發環境

| 項目 | 內容 |
|---|---|
| 開發工具 | Visual Studio |
| 程式語言 | C# |
| 專案類型 | Windows Forms App |
| 目標框架 | .NET Framework 4.7.2 |
| 播放元件 | Windows Media Player ActiveX |
| 作業系統 | Windows |

---

## 專案結構

```text
HW5_MediaPlayer/
├── HW5_MediaPlayer.sln
├── HW5_MediaPlayer.csproj
├── Program.cs
├── frmMediaPlayer.cs
├── Properties/
│   └── AssemblyInfo.cs
├── sample_media/
│   └── Dog.wmv
├── .gitignore
└── README.md
```

---

## 執行說明

### 方法一：使用 Visual Studio 執行

1. 下載或 clone 本專案到電腦。
2. 使用 Visual Studio 開啟 `HW5_MediaPlayer.sln`。
3. 確認上方執行設定為 `Debug / Any CPU`。
4. 按下 Visual Studio 上方的「開始」按鈕，或按 `F5` 執行程式。
5. 程式啟動後會自動載入 `sample_media/Dog.wmv`，按下「播放」即可播放。

---

## 操作說明

| 按鈕 / 選項 | 功能 |
|---|---|
| 瀏覽 | 選擇要播放的影片檔 |
| 播放 | 播放目前載入的影片 |
| 暫停 | 暫停目前播放中的影片 |
| 停止 | 停止播放並回到停止狀態 |
| 播放倍速 | 調整影片播放速度 |
| 循環播放 | 影片結束後自動重新播放 |

---
## 執行截圖
<img width="1067" height="683" alt="image" src="https://github.com/user-attachments/assets/da4f9812-7ede-4a5d-8563-3f8482403bca" />

---

