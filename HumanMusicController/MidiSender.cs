using System;
using Melanchall.DryWetMidi.Multimedia;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Common;

namespace HumanMusicController
{
    public class MidiSender : IDisposable
    {
        private readonly string midiPortName;
        private readonly OutputDevice outputDevice;
        private int lastPlayedNote = 0;

        public MidiSender(string midiPortName)
        {
            this.midiPortName = midiPortName;
            outputDevice = OutputDevice.GetByName(midiPortName);
        }

        public void SendMidiMsg(int heartrate)
        {
            var key = heartrate - 40;
            if (lastPlayedNote == 0)
            {
                outputDevice.SendEvent(new NoteOnEvent(new SevenBitNumber((byte)key), new SevenBitNumber((byte)key)));
                lastPlayedNote = key;
            }
            else
            {
                outputDevice.SendEvent(new NoteOffEvent(new SevenBitNumber((byte)lastPlayedNote), new SevenBitNumber((byte)lastPlayedNote)));
                lastPlayedNote = key;
                outputDevice.SendEvent(new NoteOnEvent(new SevenBitNumber((byte)key), new SevenBitNumber((byte)key)));
            }
        }

        public void Dispose()
        {
            outputDevice.Dispose();
        }
    }
}