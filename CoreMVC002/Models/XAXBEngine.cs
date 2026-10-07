namespace CoreMVC002.Models
{
    public class XAXBEngine
    {
        // 祕密數字
        public string Secret { get; set; }
        // 目前猜測
        public string Guess { get; set; }
        // 目前結果 (例如 "4A0B")
        public string Result { get; set; }

        // 歷史紀錄，每次猜測與結果的字串 (例如 "1234 => 4A0B")
        public List<string> History { get; set; }

        // 猜測次數 (等於 History.Count)
        public int Attempts => History?.Count ?? 0;

        private static readonly Random _rng = new Random();

        public XAXBEngine()
        {
            Secret = GenerateSecretNumber();
            Guess = null;
            Result = null;
            History = new List<string>();
        }

        public XAXBEngine(string secretNumber)
        {
            Secret = secretNumber ?? GenerateSecretNumber();
            Guess = null;
            Result = null;
            History = new List<string>();
        }

        // 產生一個隨機、不重複的 4 位數字字串
        private string GenerateSecretNumber()
        {
            var digits = Enumerable.Range(0, 10).Select(d => d.ToString()).ToList();
            // 隨機抽 4 個
            var chosen = new List<string>();
            for (int i = 0; i < 4; i++)
            {
                int idx = _rng.Next(digits.Count);
                chosen.Add(digits[idx]);
                digits.RemoveAt(idx);
            }
            return string.Join("", chosen);
        }

        // 重置遊戲: 重新產生 Secret 並清除歷史與狀態
        public void Reset()
        {
            Secret = GenerateSecretNumber();
            Guess = null;
            Result = null;
            History = new List<string>();
        }

        // 計算 A (位置與數字都相同)
        public int numOfA(string guessNumber)
        {
            if (string.IsNullOrEmpty(guessNumber) || string.IsNullOrEmpty(Secret)) return 0;
            int a = 0;
            int len = Math.Min(guessNumber.Length, Secret.Length);
            for (int i = 0; i < len; i++)
            {
                if (guessNumber[i] == Secret[i]) a++;
            }
            return a;
        }

        // 計算 B (數字存在但位置不同)
        public int numOfB(string guessNumber)
        {
            if (string.IsNullOrEmpty(guessNumber) || string.IsNullOrEmpty(Secret)) return 0;
            // 計算每個數字在 secret 和 guess 中的出現次數，取最小值後扣掉 A
            var freqSecret = new int[10];
            var freqGuess = new int[10];
            for (int i = 0; i < Secret.Length && i < 4; i++)
            {
                if (char.IsDigit(Secret[i])) freqSecret[Secret[i] - '0']++;
            }
            for (int i = 0; i < guessNumber.Length && i < 4; i++)
            {
                if (char.IsDigit(guessNumber[i])) freqGuess[guessNumber[i] - '0']++;
            }
            int common = 0;
            for (int d = 0; d < 10; d++)
            {
                common += Math.Min(freqSecret[d], freqGuess[d]);
            }
            int a = numOfA(guessNumber);
            return Math.Max(0, common - a);
        }

        // 判斷是否結束 (4A0B)
        public bool IsGameOver(string guessNumber)
        {
            return numOfA(guessNumber) == 4;
        }

        // 執行一次猜測：設定 Guess、計算 Result、加入 History，並回傳 Result
        public string MakeGuess(string guessNumber)
        {
            Guess = guessNumber;
            int a = numOfA(guessNumber);
            int b = numOfB(guessNumber);
            Result = $"{a}A{b}B";
            History.Add($"{guessNumber} => {Result}");
            return Result;
        }

    }

}
