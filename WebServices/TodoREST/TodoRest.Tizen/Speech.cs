﻿using TodoREST;
using Tizen.Uix.Tts;
using System.Threading.Tasks;


namespace TodoRest.Tizen
{
    class Speech : ITextToSpeech
    {
        public Task Prepare(TtsClient speaker)
        {
            TaskCompletionSource<bool> source = new TaskCompletionSource<bool>();
            speaker.StateChanged += (s, e) =>
            {
                if (e.Current == State.Ready)
                {
                    source.SetResult(true);
                }
            };

            if (speaker.CurrentState == State.Created)
                speaker.Prepare();

            return source.Task;
        }

        public async void Speak(string text)
        {
            TtsClient speaker = new TtsClient();

            if (speaker.CurrentState == State.Created)
                await Prepare(speaker);

            if (speaker.CurrentState == State.Ready)
            {
                speaker.AddText(text, speaker.DefaultVoice.Language, (int)speaker.DefaultVoice.VoiceType, speaker.GetSpeedRange().Normal);
                speaker.Play();
            }
        }
    }
}
