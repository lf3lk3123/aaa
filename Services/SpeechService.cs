using System;
using System.Speech.Synthesis;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Services
{
    public class SpeechService : IDisposable
    {
        private readonly SpeechSynthesizer _synthesizer;
        private bool _isEnabled;

        public SpeechService()
        {
            _synthesizer = new SpeechSynthesizer();
            _synthesizer.SetOutputToDefaultAudioDevice();
            _isEnabled = true;

            // 設置語音參數
            _synthesizer.Rate = 0; // 正常語速
            _synthesizer.Volume = 80; // 音量 80%

            // 嘗試設置中文語音
            try
            {
                var voices = _synthesizer.GetInstalledVoices();
                foreach (var voice in voices)
                {
                    if (voice.VoiceInfo.Culture.Name.Contains("zh") || 
                        voice.VoiceInfo.Name.Contains("Chinese"))
                    {
                        _synthesizer.SelectVoice(voice.VoiceInfo.Name);
                        break;
                    }
                }
            }
            catch
            {
                // 如果沒有中文語音，使用默認語音
            }
        }

        public bool IsEnabled
        {
            get => _isEnabled;
            set => _isEnabled = value;
        }

        public void Speak(string text)
        {
            if (!_isEnabled || string.IsNullOrWhiteSpace(text))
                return;

            try
            {
                _synthesizer.SpeakAsync(text);
            }
            catch (Exception ex)
            {
                // 記錄錯誤但不中斷程序
                System.Diagnostics.Debug.WriteLine($"語音播報錯誤: {ex.Message}");
            }
        }

        public async Task SpeakAsync(string text)
        {
            if (!_isEnabled || string.IsNullOrWhiteSpace(text))
                return;

            try
            {
                await Task.Run(() => _synthesizer.Speak(text));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"語音播報錯誤: {ex.Message}");
            }
        }

        public void Stop()
        {
            try
            {
                _synthesizer.SpeakAsyncCancelAll();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"停止語音播報錯誤: {ex.Message}");
            }
        }

        // 預定義的語音提示
        public void SpeakLowStockAlert(string itemName, int currentStock)
        {
            var message = $"注意！{itemName} 庫存不足，目前庫存 {currentStock} 件";
            Speak(message);
        }

        public void SpeakSaleComplete(string itemName, int quantity, double amount)
        {
            var message = $"銷售完成！{itemName} {quantity} 件，金額 {amount:F0} 元";
            Speak(message);
        }

        public void SpeakPurchaseComplete(string itemName, int quantity)
        {
            var message = $"進貨完成！{itemName} {quantity} 件已入庫";
            Speak(message);
        }

        public void SpeakWelcome()
        {
            Speak("歡迎使用進銷存管理系統");
        }

        public void SpeakError(string errorMessage)
        {
            Speak($"錯誤：{errorMessage}");
        }

        public void Dispose()
        {
            _synthesizer?.Dispose();
        }
    }
}