/*
 *  This code belongs to 
 * https://www.insecure.ws/2010/03/19/control-rc-aircrafts-from-your-computer-even-if-you-use-windows
 *  
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX;
using SlimDX.DirectSound;

namespace RC_Control
{
    class SoundCapture
    {
        SlimDX.DirectSound.DirectSoundCapture captureDevice = new SlimDX.DirectSound.DirectSoundCapture();
        SlimDX.Multimedia.WaveFormat waveFormat = new SlimDX.Multimedia.WaveFormat();

        public int GetAmplitude()
        {
            waveFormat.FormatTag = SlimDX.Multimedia.WaveFormatTag.Pcm;

            waveFormat.BitsPerSample = 16;
            waveFormat.BlockAlignment = (short)((waveFormat.BitsPerSample / 8));
            waveFormat.Channels = 1;
            waveFormat.SamplesPerSecond = 44100;
            waveFormat.AverageBytesPerSecond = waveFormat.SamplesPerSecond * waveFormat.BlockAlignment * waveFormat.Channels;

            SlimDX.DirectSound.CaptureBufferDescription bufferDescription = new SlimDX.DirectSound.CaptureBufferDescription();
            bufferDescription.BufferBytes = 8192;
            bufferDescription.Format = waveFormat;
            bufferDescription.WaveMapped = false;

            SlimDX.DirectSound.CaptureBuffer buffer = new SlimDX.DirectSound.CaptureBuffer(captureDevice, bufferDescription);
            buffer.Start(true);

            short[] samples = new short[5000];
            int max = 0;
            for (int i = 0; i < 1000; i++)
            {
                buffer.Read<short>(samples, 0, true);
                max = samples.Max();
                if (max != 0)
                    break;

            }

            buffer.Stop();
            buffer.Dispose();
            captureDevice.Dispose();

            return max;
        }
    }
}
