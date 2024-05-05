//----------------------------------------------------------------------------
// name: r.ck ('r' is for "receiver")
// desc: OpenSoundControl (OSC) receiver example
// note: launch with s.ck (or another sender)
//----------------------------------------------------------------------------

// the patch
// our patch: sine oscillator -> dac
SinOsc s => dac;

30 => int freq;
100:ms => dur beat;

spork ~ ReceiveOscMessage();
// infinite time loop
while( true )
{
    // randomly choose frequency from 30 to 1000
    freq => s.freq;

    // advance time by 100 millisecond
    beat => now;
}

fun void ReceiveOscMessage()
{
    while (true)
    {
        // create our OSC receiver
        OscIn oin;
        // create our OSC message
        OscMsg msg;

        4560 => oin.port;
        // create an address in the receiver, expect an int and a float
        oin.addAddress( "/speed/gy" );
        // wait for event to arrive
        oin => now;

        // grab the next message from the queue. 
        while( oin.recv(msg) )
        {
            // fetch the first data element as int
            msg.getInt(0)::ms => beat;
        }
    }
}