namespace Demo.Common;

/// <summary>
/// Prosty pasek postępu w konsoli, aktualizowany w tej samej linii.
/// </summary>
public sealed class ConsoleProgressBar : IDisposable
{
    private const char FilledChar = '█';
    private const char EmptyChar  = '░';
    private static readonly char[] Partial = { ' ', '▏', '▎', '▍', '▌', '▋', '▊', '▉' };

    private readonly string _label;
    private readonly int _barWidth;
    private readonly bool _usePartialBlocks;
    private int _lastWriteLength;
    private bool _completed;

    public ConsoleProgressBar(string label = "Postęp", int barWidth = 40, bool usePartialBlocks = false)
    {
        if (barWidth <= 0)
            throw new ArgumentOutOfRangeException(nameof(barWidth), "Szerokość paska musi być dodatnia.");

        _label = string.IsNullOrWhiteSpace(label) ? "Postęp" : label.Trim();
        _barWidth = barWidth;
        _usePartialBlocks = usePartialBlocks;
    }

    /// <summary>
    /// Uaktualnia pasek do podanego postępu (0-1).
    /// </summary>
    public void Report(double progress)
    {
        if (_completed)
            return;

        var value = Math.Clamp(progress, 0d, 1d);
        string bar;

        if (_usePartialBlocks)
        {
            var totalBars = value * _barWidth;
            var full = (int)Math.Floor(totalBars);
            var fraction = totalBars - full;
            var partialIndex = (int)Math.Round(fraction * Partial.Length);
            if (partialIndex >= Partial.Length)
            {
                full = Math.Min(full + 1, _barWidth);
                partialIndex = 0;
            }

            bar = new string(FilledChar, full)
                  + (partialIndex > 0 && full < _barWidth ? Partial[partialIndex] : string.Empty)
                  + new string(EmptyChar, _barWidth - full - (partialIndex > 0 && full < _barWidth ? 1 : 0));
        }
        else
        {
            var filled = (int)Math.Round(value * _barWidth, MidpointRounding.AwayFromZero);
            bar = new string(FilledChar, filled) + new string(EmptyChar, _barWidth - filled);
        }

        var text = $"{_label} [{bar}] {value * 100,5:0.0}%";
        WriteLineInPlace(text);
    }

    /// <summary>
    /// Zamyka pasek (100%) i przechodzi do nowej linii.
    /// </summary>
    public void Complete()
    {
        if (_completed)
            return;

        Report(1);
        _completed = true;
        Console.WriteLine();
        _lastWriteLength = 0;
    }

    public void Dispose()
    {
        Complete();
    }

    private void WriteLineInPlace(string text)
    {
        // Dopisuje spacje, by nadpisać krótszy poprzedni tekst.
        var padding = Math.Max(0, _lastWriteLength - text.Length);
        Console.Write("\r" + text + new string(' ', padding));
        _lastWriteLength = text.Length;
    }
}
