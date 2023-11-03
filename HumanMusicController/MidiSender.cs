using System;
using Melanchall.DryWetMidi.Multimedia;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Common;

namespace HumanMusicController
{
    public class MidiSender : IDisposable
    {
        private readonly OutputDevice outputDevice;


        private int lastPlayedNote = 0;

        public MidiSender(string midiPortName)
        {
            outputDevice = OutputDevice.GetByName(midiPortName);
        }

        public void SendMidiMsg(int key)
        {
            //To make the note played relate more to the music, hold it until the next one arrives
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