using System;
using System.Speech.Synthesis;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Services
{
    public class SpeechService : IDisposable
    {
        private readonly SpeechSynthesizer? _synthesizer;
        private bool _isEnabled;
        private bool _isAvailable;
        private bool _disposed = false;

        public SpeechService()
        {
            try
            {
                _synthesizer = new SpeechSynthesizer();
                _synthesizer.SetOutputToDefaultAudioDevice();
                _isEnabled = true;
                _isAvailable = true;

                // 設置語音參數
                _synthesizer.Rate = 0; // 正常語速
                _synthesizer.Volume = 80; // 音量 80%

                // 嘗試設置中文語音
                SetChineseVoice();
            }
            catch (Exception ex)
            {
                _isAvailable = false;
                _isEnabled = false;
                System.Diagnostics.Debug.WriteLine($"語音服務初始化失敗: {ex.Message}");
            }
        }

        private void SetChineseVoice()
        {
            if (_synthesizer == null) return;

            try
            {
                var voices = _synthesizer.GetInstalledVoices();
                foreach (var voice in voices)
                {
                    if (voice.VoiceInfo.Culture.Name.Contains("zh") || 
                        voice.VoiceInfo.Name.Contains("Chinese") ||
                        voice.VoiceInfo.Name.Contains("中文"))
                    {
                        _synthesizer.SelectVoice(voice.VoiceInfo.Name);
                        System.Diagnostics.Debug.WriteLine($"已選擇語音: {voice.VoiceInfo.Name}");
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"設置中文語音失敗: {ex.Message}");
            }
        }

        public bool IsEnabled
        {
            get => _isEnabled && _isAvailable;
            set => _isEnabled = value;
        }

        public bool IsAvailable => _isAvailable;

        public void Speak(string text)
        {
            if (!IsEnabled || string.IsNullOrWhiteSpace(text) || _synthesizer == null)
                return;

            try
            {
                _synthesizer.SpeakAsync(text);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"語音播報錯誤: {ex.Message}");
                _isAvailable = false;
            }
        }

        public async Task SpeakAsync(string text)
        {
            if (!IsEnabled || string.IsNullOrWhiteSpace(text) || _synthesizer == null)
                return;

            try
            {
                await Task.Run(() => _synthesizer.Speak(text));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"語音播報錯誤: {ex.Message}");
                _isAvailable = false;
            }
        }

        public void Stop()
        {
            if (_synthesizer == null) return;

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
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    Stop();
                    _synthesizer?.Dispose();
                }
                _disposed = true;
            }
        }

        ~SpeechService()
        {
            Dispose(false);
        }
    }
}