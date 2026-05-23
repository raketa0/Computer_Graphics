using NAudio.Wave;
using OpenTK.Audio.OpenAL;

namespace SeaGame.Services;

public unsafe class AudioService
{
    public int LoadSound(string path)
    {
        int buffer =
            AL.GenBuffer();

        using WaveFileReader wave =
            new(path);

        byte[] data =
            new byte[wave.Length];

        wave.Read(
            data,
            0,
            data.Length);

        ALFormat format =
            wave.WaveFormat.Channels == 1
            ? ALFormat.Mono16
            : ALFormat.Stereo16;

        fixed (byte* ptr = data)
        {
            AL.BufferData(
                buffer,
                format,
                ptr,
                data.Length,
                wave.WaveFormat.SampleRate);
        }

        return buffer;
    }

    public int CreateSource(
        int buffer,
        bool loop = false)
    {
        int source =
            AL.GenSource();

        AL.Source(
            source,
            ALSourcei.Buffer,
            buffer);

        AL.Source(
            source,
            ALSourceb.Looping,
            loop);

        return source;
    }

    public void Play(int source)
    {
        AL.SourcePlay(source);
    }
}