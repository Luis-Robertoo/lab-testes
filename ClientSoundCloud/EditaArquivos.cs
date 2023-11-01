using NAudio.Lame;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace ClientSoundCloud;

public class EditaArquivos
{
    public static IEnumerable<string>? ExtrairLinksDosTrechos(string playlist)
    {
        if (playlist is null) return null;

        var trechos = playlist.Split("#EXTINF").ToList().Select(trecho =>
        {
            var comecoLink = trecho.IndexOf("h");

            if (comecoLink == -1) return string.Empty;

            return trecho.Substring(comecoLink, trecho.Length - comecoLink).Replace("\n#EXT-X-ENDLIST", "");
        });

        return trechos.ToList();
    }

    public static void ConcatenaTrechos(List<string> caminhoArquivos, string caminhoBase)
    {
        var lista = new List<AudioFileReader>();

        foreach (var caminhoArquivo in caminhoArquivos)
        {
            if (File.Exists(caminhoArquivo))
            {
                var reader = new AudioFileReader(caminhoArquivo);
                lista.Add(reader);
            }
        }

        var nome = $"{Guid.NewGuid()}.wav";

        var playlist = new ConcatenatingSampleProvider(lista);
        var saida = Path.Combine(caminhoBase, "concluido", "tudojunto5.wav");

        WaveFileWriter.CreateWaveFile16(saida, playlist);

        var audio = new AudioFileReader(saida);
        var wave32 = new Wave32To16Stream(audio);
        var mp3Writer = new LameMP3FileWriter(Path.Combine(caminhoBase, "concluido", "tremdasonze.mp3"), wave32.WaveFormat, 128);
        wave32.CopyTo(mp3Writer);

        wave32.Close();
        mp3Writer.Close();

    }

    public static string MesclaTrechos(List<string> caminhoArquivos, string caminhoBase)
    {
        var mixer = new WaveMixerStream32
        {
            AutoStop = true
        };

        foreach (var caminhoArquivo in caminhoArquivos)
        {
            if (File.Exists(caminhoArquivo))
            {
                var reader = new Mp3FileReader(caminhoArquivo);

                var waveStream = WaveFormatConversionStream.CreatePcmStream(reader);
                var channel = new WaveChannel32(waveStream)
                {

                    Volume = 0.5f
                };

                mixer.AddInputStream(channel);
            }
        }

        var saida = Path.Combine($"{caminhoBase}", "concluido", "tudoJunto.mp3");

        var wave32 = new Wave32To16Stream(mixer);
        var mp3Writer = new LameMP3FileWriter(saida, wave32.WaveFormat, 128);
        wave32.CopyTo(mp3Writer);

        wave32.Close();
        mp3Writer.Close();

        return saida;
    }


}
