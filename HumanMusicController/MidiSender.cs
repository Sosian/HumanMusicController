using System;
using Melanchall.DryWetMidi.Multimedia;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Common;

namespace HumanMusicController
{
    public class MidiSender : IDisposable
    {
        private readonly OutputDevice outputDevice;

        private List<int> listOfHeartrates = new List<int>();

        private int lastPlayedNote = 0;

        public MidiSender(string midiPortName)
        {
            outputDevice = OutputDevice.GetByName(midiPortName);
        }

        public void SendMidiMsg(int heartrate)
        {
            listOfHeartrates.Add(heartrate);

            var key = heartrate - 40;
            if (lastPlayedNote == 0)
            {
                outputDevice.SendEvent(new NoteOnEvent(new SevenBitNumber((byte)key), new SevenBitNumber((byte)key)));
                lastPlayedNote = key;
            }
            else if (LastXItemsAreNotTheSame(listOfHeartrates, 5))
            {
                outputDevice.SendEvent(new NoteOffEvent(new SevenBitNumber((byte)lastPlayedNote), new SevenBitNumber((byte)lastPlayedNote)));
                lastPlayedNote = key;
                outputDevice.SendEvent(new NoteOnEvent(new SevenBitNumber((byte)key), new SevenBitNumber((byte)key)));
            }
        }

        private bool LastXItemsAreNotTheSame(IEnumerable<int> listOfItems, int numberOfItems)
        {
            return listOfItems.Reverse().Take(numberOfItems).Distinct().Count() != 1;
        }

        public void Dispose()
        {
            outputDevice.Dispose();
        }
    }
}