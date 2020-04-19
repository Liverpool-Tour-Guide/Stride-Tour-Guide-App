using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Plugin.SimpleAudioPlayer;

namespace StrideApp.Views
{
    public class AudioPlayer
    {
        public bool audioPlaying { get; set; }
        public string audioName { get; set; }

        ISimpleAudioPlayer audio;

        Stream GetStreamFromFile(string filename)
        {
            var assembly = typeof(App).GetTypeInfo().Assembly;

            var stream = assembly.GetManifestResourceStream("StrideApp.POPULATED_DATA_STRIDE." + filename);

            return stream;
        }

        public void startAudio(string audioName)
        {
            var stream = GetStreamFromFile(audioName);
            audio = CrossSimpleAudioPlayer.Current;
            audio.Load(stream);
            audio.Play();
            audioPlaying = true;
        }

        public void pauseAudio()
        {
            audio.Pause();
            audioPlaying = false;
        }

        public void playAudio()
        {
            audio.Play();
            audioPlaying = true;
        }
    }
}
